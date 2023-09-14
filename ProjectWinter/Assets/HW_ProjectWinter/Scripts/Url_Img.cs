using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Url_Img : MonoBehaviour
{
    public string url = "https://projectwinter.co/presskit/logos_and_icons/WinterLogo_Blue_Large.png";
    RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTexture(url));
        rawImage = GetComponent<RawImage>();
    }


    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GetTexture(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;
        }
    }
}