using System.Collections;
using System.IO;
using System.Security;
using UnityEngine;
using UnityEngine.Networking;
using File = System.IO.File;

public class ModelDownload : MonoBehaviour
{
    [SerializeField]
    private string localFilePath;
    private bool isLoaded;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Download());
    }



    private IEnumerator Download()
    {
        isLoaded = false;

        // Example code to download the file using UnityWebRequest
        UnityWebRequest www = UnityWebRequest.Get("https://colton.nyc3.digitaloceanspaces.com");
        yield return www.SendWebRequest();

        bool isNetworkError = www.result == UnityWebRequest.Result.ConnectionError;
        bool isHttpError = www.result == UnityWebRequest.Result.ProtocolError;

        if (isNetworkError || isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            localFilePath = Application.streamingAssetsPath; // + "/Downloaded";
            File.WriteAllBytes(localFilePath, www.downloadHandler.data);

            isLoaded = true;
        }
    }

    public string modelFileName = "YourModelFileName.obj";

    private IEnumerator Import()
    {
        while (!isLoaded)
        {
            yield return null;
        }
        if (File.Exists(localFilePath))
        {
            //.ImportFile(localFilePath); // This function imports the OBJ file
        }
        else
        {
            Debug.LogError("Model file not found: " + localFilePath);
        }
    }
}
