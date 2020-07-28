using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [HideInInspector] public bool grabbedByBoth = false;

    void Update()
    {
        if (grabbedByBoth) 
        {
            UnityEngine.Debug.Log("Grabbed by both!");
            
            // get last delta transform of both controllers
                // new transform * inverse of last transform
                // so need last transform and new transform

            // get average of two delta transforms

            // multiply by object transform

        }
    }
}
