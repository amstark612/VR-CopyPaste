using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{
    public Texture myImage; // Will hold image downloaded from URL

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        StartCoroutine(GetTexture());

        IEnumerator GetTexture()
        {
            //string url = "file:///C:/Users/REUstudent.CSEL-KH1254-17/OneDrive%20-%20MNSCU/Coursework/REU/Projects/ar-cutpaste/server/water.jpg";
            string url = "file:///C:/Users/REUstudent.CSEL-KH1254-17/Documents/Projects/VRCutPaste/ar-cutpaste/server/cut_current.png";

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // Send request and wait
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                UnityEngine.Debug.Log("Error while receiving " + www.error);
            }

            else
            {
                myImage = ((DownloadHandlerTexture)www.downloadHandler).texture;

            }
        }
    }
}