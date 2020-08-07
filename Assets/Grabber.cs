using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField]
    private ObjectManager objectManager;

    [SerializeField]
    private OVRPlayerController player;
    [SerializeField]
    private OVRInput.Controller thisController;
    [SerializeField]
    private OVRInput.Controller otherController;
    [SerializeField]
    private GameObject otherControllerGameObject;

    private LineRenderer laser;
    [SerializeField]
    private float maxLength = 100.0f;
    
    GameObject collidingObject = null;
    GameObject raycastObject = null;
    bool grabbing = false;
    bool raycastGrabbing = false;

    [Tooltip("Grip trigger thresholds for picking up objects, with some hysteresis")]
    [SerializeField] private static float grabBegin = 0.55f;
    [Tooltip("Grip trigger thresholds for picking up objects, with some hysteresis")]
    [SerializeField] private static float grabEnd = 0.35f;

    void Start()
    {
        laser = GetComponent<LineRenderer>();    
    }

    void OnTriggerEnter(Collider other)
    {
        bool alreadyGrabbing = grabbing || raycastGrabbing;

        if (!alreadyGrabbing && other.GetComponent<Grabbable>())
        {
            collidingObject = other.gameObject;
        }
    }

    void Update()
    {
        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController);
        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, thisController);

        // "normal"? interaction ------------------------------------
        if (collidingObject)
        {
            Grabbable grabbable = collidingObject.GetComponent<Grabbable>();
            
            // bimanualInteraction() checks that both controllers are grabbing same object
            // grabbable.bimanualInteracting is true AFTER the first intial frame when both controllers grab object
            if (grabbing && bimanualInteraction() && !grabbable.bimanualInteracting)
            {
                grabbable.grabbedByBoth = true;
            }

            if (!grabbing && handTrigger > grabBegin)
            {
                Grab();
            }
            
            else if (grabbing && handTrigger < grabEnd)
            {
                Drop();
            }
        }
        // ----------------------------------------------------------

        // ray interaction ------------------------------------------
        else
        {
            if (indexTrigger > grabBegin)
            {
                RaycastHit hit;
                bool hitTarget = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);

                laser.enabled = true;
                DrawLaser(transform.position + transform.forward * maxLength);

                if (!raycastGrabbing && hitTarget)
                {
                    Debug.Log("!raycastGrabbing && hitTarget");
                    // add change line renderer color here (hover color)
                    DrawLaser(hit.point);

                    raycastGrabbing = handTrigger > grabBegin ? true : false;

                    if (raycastGrabbing)
                    {
                        // add change line renderer color here (select color)
                        Debug.Log("raycastGrabbing, should be picking up target");
                        raycastObject = hit.transform.gameObject;
                        raycastObject.transform.parent = this.transform;
                    }
                }

                else if (raycastGrabbing && handTrigger > grabBegin)
                {
                    Debug.Log("raycastGrabbing, should be drawing laser to object");

                    // ensure controller input is used to move object, not player
                    player.EnableLinearMovement = false;
                    player.EnableRotation = false;

                    // pushing/pulling object farther/closer
                    if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, thisController))
                    {
                        raycastObject.transform.position += transform.forward * Time.deltaTime * 2.0f;
                    }

                    else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, thisController))
                    {
                        raycastObject.transform.position -= transform.forward * Time.deltaTime * 2.0f;
                    }

                    DrawLaser(raycastObject.transform.position);
                }

                else if (raycastGrabbing && (handTrigger < grabEnd || indexTrigger < grabEnd))
                {
                    Debug.Log("should be dropping");
                    Drop();
                }

            }

            else if (laser.enabled && indexTrigger < grabEnd)
            {
                Debug.Log("laser.enabled && indexTrigger < grabEnd");
                Drop();
                laser.enabled = false;
            }
        }
        // ----------------------------------------------------------
    }

    private void Grab()
    {
        collidingObject.transform.SetParent(this.transform);

        grabbing = true;
    }
    
    // checks if both controllers are grabbing AND grabbing same object
    private bool bimanualInteraction()
    {
        Grabber other = otherControllerGameObject.GetComponent<Grabber>();

        return other.grabbing && collidingObject == other.collidingObject;
    }

    private void Drop()
    {
        if (raycastGrabbing)
        {
            player.EnableLinearMovement = true;
            player.EnableRotation = true;

            // do I need to change this? what if it was parented by something else originally?
            raycastObject.transform.parent = null;
            raycastObject = null;
            raycastGrabbing = false;
        }

        else if (grabbing)
        {
            // do I need to change this? what if it was parented by something else originally?
            collidingObject.transform.SetParent(null);
            collidingObject = null;
            grabbing = false;
        }
    }

    private void DrawLaser(Vector3 endpoint)
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, endpoint);
    }

}