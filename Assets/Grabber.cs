using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public OVRInput.Controller thisController;
    public OVRInput.Controller otherController;
    public GameObject otherControllerGameObject;

    public Transform DeltaTransform { get; private set; }
    
    GameObject collidingObject = null;
    bool grabbing = false;

    private static float grabBegin = 0.1f;
    private static float grabEnd = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        // find out if colliding object is grabbable
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable)
        {
            collidingObject = other.gameObject;
        }
    }

    void Update()
    {
        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController);
        
        if (collidingObject)
        {
            if (bimanualInteraction())
            {
                collidingObject.GetComponent<Grabbable>().grabbedByBoth = true;

                Transform lastTransform = collidingObject.transform;

                CalculateDeltaTransform();
            }

            if (!grabbing && handTrigger > grabBegin)
            {
                Grab();
                grabbing = true;
            }
            
            else if (grabbing && handTrigger < grabEnd)
            {
                Drop();
                grabbing = false;
            }
        }
    }

    private void Grab()
    {
        //if (bimanualInteraction())
        //{
        //    collidingObject.GetComponent<Grabbable>().grabbedByBoth = true;
        //}

        //else
        //{
        //    PickUp();
        //}

        collidingObject.transform.SetParent(this.transform);
    }
    
    private bool bimanualInteraction()
    {
        Grabber other = otherControllerGameObject.GetComponent<Grabber>();

        return other.grabbing && collidingObject == other.collidingObject;
    }

    //private void PickUp()
    //{
    //    collidingObject.transform.SetParent(this.transform);
    //}

    private void Drop()
    {
        // do I need to change this? what if it was parented by something else originally?
        collidingObject.transform.SetParent(null);
        collidingObject = null;
    }

    private Transform CalculateDeltaTransform()
    {
        return;
    }
}