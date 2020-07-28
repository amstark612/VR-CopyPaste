using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeTexture : MonoBehaviour, IPointerClickHandler
{
    // for whatever object the GetImage script is attached to and is holding the downloaded pic
    public GameObject button;
    GetImage getImageScript;
    public Texture newImage;


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        button = GameObject.Find("Button_ChangeTexture"); // find the object holding the downloaded pic
        getImageScript = button.GetComponent<GetImage>();
        newImage = getImageScript.myImage;  // downloaded pic

        UnityEngine.Debug.Log(newImage.width);
        UnityEngine.Debug.Log(newImage.height);

        // assign downloaded pic to this object's texture
        GetComponent<Renderer>().material.mainTexture = newImage;
    }

}
