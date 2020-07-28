//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Scaler : MonoBehaviour
//{
//    // Grip trigger thresholds for picking up objects, with some hysteresis.
//    public static float grabBegin = 0.55f;
//    public static float grabEnd = 0.35f;

//    private GameObject thisObject = null; // object to be scaled

//    private Transform originalParent;

//    // empty parent object that will be used to scale and rotate actual object
//    private GameObject parentObject;

//    private Vector3 LInitialPosition;
//    private Vector3 RInitialPosition;
//    private float initialDistance;

//    private Vector3 LCurrentPosition;
//    private Vector3 RCurrentPosition;

//    private Vector3 initialScale;
//    private Vector3 colliderInitialScale;

//    private BoxCollider collider;

//    private bool grabbedByBoth = false;

//    void Start()
//    {
//        // move to OnTriggerEnter after writing rotation
//        originalParent = gameObject.transform.parent;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        // detect whether both controllers are grasping
//        grabbedByBoth = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > grabBegin && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > grabBegin;


//        if (!grabbedByBoth) { return; }

//        else
//        {

//            UnityEngine.Debug.Log("Grabbed by both");
//            thisObject = gameObject;
//            collider = thisObject.GetComponent<BoxCollider>();

//            LInitialPosition = GameObject.FindWithTag("LController").transform.position;
//            RInitialPosition = GameObject.FindWithTag("RController").transform.position;

//            initialDistance = Vector3.Distance(LInitialPosition, RInitialPosition);

//            Vector3 midpoint = Vector3.Lerp(LInitialPosition, RInitialPosition, 0.5f);

//            parentObject = new GameObject();                        // create empty gameobject
//            parentObject.transform.position = thisObject.transform.position;
//            parentObject.transform.localScale = thisObject.transform.localScale;
//            thisObject.transform.SetParent(parentObject.transform); // parent actual object to it

//            initialScale = parentObject.transform.localScale;
//            colliderInitialScale = collider.size;

//        }
//    }

//    // move to OnTriggerStay after writing rotation? maybe?
//    void Update()
//    {
//        grabbedByBoth = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > grabBegin && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > grabBegin;

//        if (!thisObject) { return; }

//        else if (thisObject && grabbedByBoth)
//        {

//            LCurrentPosition = GameObject.FindWithTag("LController").transform.position;
//            RCurrentPosition = GameObject.FindWithTag("RController").transform.position;

//            float currentDistance = Vector3.Distance(LCurrentPosition, RCurrentPosition);
//            float newScale = currentDistance / initialDistance;

//            Vector3 midpoint = Vector3.Lerp(LCurrentPosition, RCurrentPosition, 0.5f);

//            parentObject.transform.localScale = initialScale * newScale;
//            collider.size = colliderInitialScale * newScale;

//            // check to see if controller velocities are available
//            // if so set speed according to average?
//            parentObject.transform.position = Vector3.MoveTowards(parentObject.transform.position, midpoint, 1.0f * Time.deltaTime);

//            //thisObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

//        }

//        else
//        {
//            // move to OnTriggerExit after writing rotation, maybe?
//            gameObject.transform.parent = originalParent;
//            Destroy(parentObject);
//            thisObject = null;
//        }
//    }

//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public static float grabBegin = 0.01f; //0.55f
    public static float grabEnd = 0.01f; //0.35f

    private GameObject thisObject = null; // object to be scaled

    private Transform originalParent;

    // empty parent object that will be used to scale and rotate actual object
    private GameObject parentObject;

    private Vector3 LInitialPosition;
    private Vector3 RInitialPosition;
    private float initialDistance;

    private Vector3 LCurrentPosition;
    private Vector3 RCurrentPosition;

    private Vector3 initialScale;

    private bool grabbedByBoth = false;

    void Start()
    {
        // move to OnTriggerEnter after writing rotation
        originalParent = gameObject.transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        // detect whether both controllers are grasping
        grabbedByBoth = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > grabBegin && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > grabBegin;

        if (!grabbedByBoth) { return; }

        else
        {

            UnityEngine.Debug.Log("Grabbed by both");
            thisObject = gameObject;

            LInitialPosition = GameObject.FindWithTag("LController").transform.position;
            RInitialPosition = GameObject.FindWithTag("RController").transform.position;

            initialDistance = Vector3.Distance(LInitialPosition, RInitialPosition);

            Vector3 midpoint = Vector3.Lerp(LInitialPosition, RInitialPosition, 0.5f);

            parentObject = new GameObject();                        // create empty gameobject
            parentObject.transform.position = thisObject.transform.position;
            parentObject.transform.localScale = thisObject.transform.localScale;
            thisObject.transform.SetParent(parentObject.transform); // parent actual object to it

            initialScale = parentObject.transform.localScale;
        }
    }

    // move to OnTriggerStay after writing rotation? maybe?
    void Update()
    {
        grabbedByBoth = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > grabBegin && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > grabBegin;

        if (!thisObject) { return; }

        else if (thisObject && grabbedByBoth)
        {

            LCurrentPosition = GameObject.FindWithTag("LController").transform.position;
            RCurrentPosition = GameObject.FindWithTag("RController").transform.position;

            float currentDistance = Vector3.Distance(LCurrentPosition, RCurrentPosition);
            float newScale = currentDistance / initialDistance;

            Vector3 midpoint = Vector3.Lerp(LCurrentPosition, RCurrentPosition, 0.5f);

            parentObject.transform.localScale = initialScale * newScale;

            // check to see if controller velocities are available
            // if so set speed according to average?
            parentObject.transform.position = Vector3.MoveTowards(parentObject.transform.position, midpoint, 1.0f * Time.deltaTime);

            //thisObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        }

        else
        {
            // move to OnTriggerExit after writing rotation, maybe?
            gameObject.transform.parent = originalParent;
            Destroy(parentObject);
            thisObject = null;
        }
    }

}