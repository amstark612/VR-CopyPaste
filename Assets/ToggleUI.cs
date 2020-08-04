using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private OVRInput.Controller controller;

    void Start()
    {
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool currentState = UI.activeSelf;
        
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
        {
            if (!currentState)
            {
                // get user's current position
                Vector3 userPosition = player.transform.position;

                // set UI position
                // change y value and rotation later
                UI.transform.position = new Vector3(userPosition.x, UI.transform.position.y, userPosition.z + 1.5f);

                // show UI
                UI.SetActive(true);
            }

            else
            {
                UI.SetActive(false);
            }
        }

    }
}
