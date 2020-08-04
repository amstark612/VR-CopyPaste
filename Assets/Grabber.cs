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
        if (other.GetComponent<Grabbable>())
        {
            collidingObject = other.gameObject;
        }
    }

    void Update()
    {
        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController);
        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, thisController);

        if (collidingObject)
        {
            Grabbable grabbable = collidingObject.GetComponent<Grabbable>();

            if (grabbing && bimanualInteraction() && !grabbable.bimanualInteracting)
            {
                grabbable.grabbedByBoth = true;
                Debug.Log("grabbing && BimanualInteraction() && !grabbable.bimanualInteracting");
            }

            if (!grabbing && handTrigger > grabBegin)
            {
                Grab();
                Debug.Log("!grabbing && handTrigger > grabBegin");
            }
            
            else if (grabbing && handTrigger < grabEnd)
            {
                Drop();
                Debug.Log("grabbing && handTrigger < grabEnd");
            }
        }

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
                    // change color
                    DrawLaser(hit.point);

                    raycastGrabbing = handTrigger > grabBegin ? true : false;

                    if (raycastGrabbing)
                    {
                        // pick up object
                        Debug.Log("raycastGrabbing, should be picking up target");
                        raycastObject = hit.transform.gameObject;
                        raycastObject.transform.parent = this.transform;
                    }
                }

                else if (raycastGrabbing && handTrigger > grabBegin)
                {
                    Debug.Log("raycastGrabbing, should be drawing laser to object");

                    player.EnableLinearMovement = false;
                    player.EnableRotation = false;

                    if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, thisController))
                    {
                        raycastObject.transform.position += transform.forward * Time.deltaTime * 2.0f;
                        DrawLaser(raycastObject.transform.position);
                    }

                    else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, thisController))
                    {
                        raycastObject.transform.position -= transform.forward * Time.deltaTime * 2.0f;
                        DrawLaser(raycastObject.transform.position);
                    }

                    else
                    {
                        DrawLaser(raycastObject.transform.position);
                    }
                }

                else if (raycastGrabbing && handTrigger < grabEnd)
                {
                    Debug.Log("should be dropping");

                    player.EnableLinearMovement = true;
                    player.EnableRotation = true;

                    raycastObject.transform.parent = null;
                    raycastObject = null;
                    raycastGrabbing = false;
                }

            }

            else if (laser.enabled && indexTrigger < grabEnd)
            {
                laser.enabled = false;
                Debug.Log("laser.enabled && indexTrigger < grabEnd");
            }
        }
    }

    private void Grab()
    {
        collidingObject.transform.SetParent(this.transform);

        grabbing = true;
    }
    
    private bool bimanualInteraction()
    {
        Grabber other = otherControllerGameObject.GetComponent<Grabber>();

        return other.grabbing && collidingObject == other.collidingObject;
    }

    private void Drop()
    {
        // do I need to change this? what if it was parented by something else originally?
        collidingObject.transform.SetParent(null);
        collidingObject = null;

        grabbing = false;
    }

    private void DrawLaser(Vector3 endpoint)
    {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, endpoint);
    }

}