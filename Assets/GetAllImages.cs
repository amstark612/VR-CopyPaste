using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetAllImages : MonoBehaviour
{


    // panel sizes as set in inspector
    int panelWidth = 676;
    int panelPadding = 8;
    int panelSpacing = 12;
    float numCols = 6.0f; // float for calculation purposes later

    public string[] fileNames;
    public GameObject newThumbnailPrefab;
    public GameObject panel;
    Texture2D myImage;
    
    // directory path
    string downloadLocation = "C:/Users/REUstudent.CSEL-KH1254-17/Documents/Projects/VRCutPaste/ar-cutpaste/server/";

    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("ImageMenuPanel");

        fileNames = Directory.GetFiles(downloadLocation, "*.png");

        // set panel height according to number of rows
        int numRows = (int)Mathf.Ceil(fileNames.Length / numCols);
        // I know this is really, really ugly
        int panelHeight = ((numRows * 100) + ((numRows - 1) * panelSpacing) + (2 * panelPadding));
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);

        foreach (string name in fileNames)
        {
            CreateThumbnail(name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //private void GetImageNames(string[] fileNames)
    //{

    //    // put all image names (PNG format, lowercase) in directory into array
    //    fileNames = Directory.GetFiles(downloadLocation, "*.png");

    //    UnityEngine.Debug.Log("-----------IMAGE NAMES-----------");
    //    foreach (string name in fileNames)
    //    {
    //        UnityEngine.Debug.Log(name);
    //    }

    //    UnityEngine.Debug.Log("Number of images: " + fileNames.Length);
    //}

    private void CreateThumbnail (string filename)
    {
        StartCoroutine(GetTexture());

        IEnumerator GetTexture()
        {
            string url = "file:///" + filename;

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


            // create new blank thumbnail button
            GameObject newThumbnail = Instantiate(newThumbnailPrefab);

            // set object name to name of downloaded pic (minus file path)
            newThumbnail.name = filename.Substring(downloadLocation.Length);

            // set parent so it snaps to grid
            newThumbnail.transform.SetParent(panel.transform, false);

            // convert downloaded pic to sprite
            Sprite webSprite = SpriteFromTexture2D(myImage);

            // assign downloaded pic to thumbnail
            newThumbnail.transform.Find("Image").GetComponent<Image>().sprite = webSprite;

        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }


}
