using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    ObjectManager objectManager;

    private GameObject thisObject = null;   // object to be scaled
    private Transform originalParent;

    [SerializeField] private float scaleIncrement = 0.05f;

    // empty object that will be used to scale and rotate actual object
    private GameObject emptyObject;

    private float lastDist;

    private BoxCollider collider;

    [HideInInspector] public bool grabbedByBoth = false;

    // this becomes active the frame AFTER object is initially grabbed by both
    [HideInInspector] public bool bimanualInteracting = false;

    void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        // where to put this? parent of object may change after start and before scaling?
        originalParent = transform.parent;
    }

    void Update()
    {
        if (grabbedByBoth && !bimanualInteracting)
        {
            thisObject = gameObject;
            collider = thisObject.GetComponent<BoxCollider>();

            emptyObject = new GameObject();
            emptyObject.transform.position = thisObject.transform.position;
            emptyObject.transform.localScale = thisObject.transform.localScale;

            // parent actual object to empty object
            thisObject.transform.SetParent(emptyObject.transform);

            lastDist = objectManager.GetControllerDistance();

            bimanualInteracting = true;
        }

        else if (grabbedByBoth && bimanualInteracting)   // grabbedByBoth must be true if bimanualInteracting
        {
            float currentDist = objectManager.GetControllerDistance();
            float newScale = currentDist / lastDist;

            Vector3 midpoint = objectManager.GetControllerMidpoint();

            emptyObject.transform.localScale *= newScale;
            collider.size *= newScale;

            // keep object between controllers
            emptyObject.transform.position = midpoint;

            lastDist = currentDist;
        }

        else if (!grabbedByBoth && bimanualInteracting)
        {
            gameObject.transform.parent = originalParent;
            Destroy(emptyObject);
            thisObject = null;

            // resetting just in case...dunno if this is really necessary
            lastDist = 0.0f;

            bimanualInteracting = false;
        }
    }
}
