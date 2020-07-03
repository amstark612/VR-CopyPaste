using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject UI;

    void Start()
    {
        transform.SetParent(GameObject.Find("RightControllerAnchor").transform);
    }
    // Update is called once per frame
    void Update()
    {
        bool currentState = UI.activeSelf;
        
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (!currentState)
            {
                UI.SetActive(true);
            }

            else
            {
                UI.SetActive(false);
            }
        }

    }
}
