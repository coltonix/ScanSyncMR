using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    /* Whether the assets have been loaded; 
     * Should be `false` by default
     */
    public static bool isLoaded;

    [SerializeField]
    private AssetReference AssetRefModel;
    [SerializeField]
    private AssetLabelReference AssetLabelRef; // this goes in place of 'key' in Addressables.LoadAssetAsync<GameObject>(key)
    //private GameObject model;

    private TextMeshPro resultText;

    //AsyncOperationHandle<GameObject> opHandle;

    void Awake()
    {
        Debug.Log("Initializing addressables...");
        Addressables.InitializeAsync(); 
        //Addressables.InitializeAsync().Completed += AddressablesManager_Completed;
        Debug.Log("Initialized addressables!!!");

        //Application.targetFrameRate = 60; // Set FPS // #TODO: Move to new script SceneManager.cs
        resultText = GameObject.Find("ResultText2").GetComponent<TextMeshPro>();

    }

    /* Load Addressable by Asset Reference
     * Called when Addressables is done initialized
     */
    private void AddressablesManager_Completed(AsyncOperationHandle<IResourceLocator> handle)
    {
        isLoaded = false;
        /*AssetRef_model.InstantiateAsync().Completed += (go) =>
        {
            model = go.Result;
            ObjectPool.SharedInstance.objectToPool = model;
        };*/
        // Load Model Asset to `objectToPool`
        AssetRefModel.LoadAssetAsync<GameObject>().Completed += (go) =>
        {
            ObjectPool.SharedInstance.objectToPool = go.Result; // singleton: ObjectPool.cs
                                                                //Debug.LogWarningFormat("Model name: {0}", ObjectPool.SharedInstance.objectToPool.name);
            if (ObjectPool.SharedInstance.objectToPool)
            {
                isLoaded = true;
                Debug.Log("Model assigned!");
            }
        };

        // Warning: Don't Destroy Loaded Asset in this function, wait until this entire function AND LoadBlahBLah<T>.Completed += Blabla is completed

    }

    public void FetchFile_Test(string key = "")
    {

        Debug.Log("FetchFile_Test() starting");
        FetchFile(key);
        Debug.Log("FetchFile_Test() finished");
    }

    /* Load Addressable Asset by string path
     */
    public void FetchFile(string key = "") // key: Addressble Asset's address. E.g., under Addresables Groups: "Assets/_ProcessedModels/FBX/LRT-2222(S)-RC.fbx"
    {
        if (key == "") // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs1736?f1url=%3FappId%3Droslyn%26k%3Dk(CS1736)
        {
            //yield return StartCoroutine(WaitforCondition());
            
            key = GlobalVariables.SharedInstance.currentQRData; // singleton: GlobalVariables.cs
        }
        resultText.text = "Found model at:\n" + key;

        GlobalVariables.SharedInstance.LoadingVisualToggle(true); // singleton: GlobalVariables.cs

        isLoaded = false;
        Addressables.LoadAssetAsync<GameObject>(key).Completed += (opHandle) =>
        {
            resultText.text += "\nLoadAssetAsync(key) Completed!";
            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                resultText.text += "\nLoadAssetAsync(key) Success!!";
                ObjectPool.SharedInstance.objectToPool = opHandle.Result; // singleton: ObjectPool.cs
                                                                          //Debug.LogWarningFormat("Model name: {0}", ObjectPool.SharedInstance.objectToPool.name);
                if (ObjectPool.SharedInstance.objectToPool)
                {
                    isLoaded = true;
                    Debug.Log("Model assigned!!");
                    resultText.text += "\nModel assigned: \n" + ObjectPool.SharedInstance.objectToPool.name;
                }
            }
        };

        
    }

    IEnumerator WaitforCondition()
    {
        while (GlobalVariables.SharedInstance.isQRDataUpdated != true)
        {
            yield return null;
        }
    }

    /*    
        void Start()
        {
            StartCoroutine(WaitToDestroy());
        }

        IEnumerator WaitToDestroy()
        {
            yield return new WaitForSeconds(10);

            if (AssetRefModel != null)
            {
                OnDestroy();
                Debug.Log("ARM DESTROYED!");
            }
        }
    */
    // Release memory
    public void OnDestroy()
    {
        //AssetRef_model.ReleaseInstance(model);
        //AssetRefModel.ReleaseAsset();
        resultText.SetText("Asset Released");
        //Addressables.Release(opHandle);
    }

}
