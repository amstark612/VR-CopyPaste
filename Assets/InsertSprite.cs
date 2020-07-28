using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InsertSprite : MonoBehaviour
{

    public GameObject newSpritePrefab;

    public void TaskOnClick()
    {
        // get player's current position
        Vector3 userPosition = GameObject.Find("OVRPlayerController").transform.position;

        // set spawn position of new image ahead of player
        Vector3 spawnPosition = new Vector3(userPosition.x, userPosition.y, (userPosition.z + 5));
        // does the new vector3 need to be destroyed?????

        // get thumbnail image
        Sprite myImage = GetComponent<Image>().sprite;

        // create new sprite
        GameObject newObject = Instantiate(newSpritePrefab, spawnPosition, Quaternion.identity);

        SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
        // assign thumbnail sprite to new object
        spriteRenderer.sprite = myImage;

        // set box collider size to image size
        newObject.GetComponent<BoxCollider>().size = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0.1f);
    }
}
