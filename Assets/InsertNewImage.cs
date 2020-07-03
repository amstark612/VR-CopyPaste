using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using Boo.Lang.Environments;

public class InsertNewImage : MonoBehaviour
{
    public GameObject newSpritePrefab;
    Texture2D myImage;

    void Start()
    {
        //transform.parent.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        StartCoroutine(GetTexture());

        IEnumerator GetTexture()
        {
            string url = "file:///" + GetAllImages.downloadLocation + transform.parent.name;

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // Send request and wait
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                UnityEngine.Debug.Log("Error while receiving " + www.error);
            }

            else
            {
                myImage = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
            }


            // get player's current position
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // set spawn position of new image ahead of player
            Vector3 spawnPosition = new Vector3(playerPosition.x, playerPosition.y, (playerPosition.z + 5));
            // does the new vector3 need to be destroyed?????

            // create new sprite
            GameObject newSprite = Instantiate(newSpritePrefab, spawnPosition, Quaternion.identity);
            SpriteRenderer renderer = newSprite.GetComponent<SpriteRenderer>();
            Sprite webSprite = SpriteFromTexture2D(myImage); // convert downloaded pic to sprite
            renderer.sprite = webSprite;                     // set new gameobject's sprite to downloaded pic

            // set new gameobject's box collider dimensions proportional to image dimensions
            float size = 0.01f; // 1 unit in Unity = 100 pixels
            newSprite.GetComponent<BoxCollider>().size = new Vector3(myImage.width * size, myImage.height * size, 0.2f);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}