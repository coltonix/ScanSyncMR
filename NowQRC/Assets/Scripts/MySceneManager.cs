using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    [Tooltip("Drag a Game Object whose GPU Enhancing you want to Enable")]
    [SerializeField]
    private GameObject objToEnableGPUI;

    [Tooltip("Player's transform to offset the positions of the Objects to be Reset")]
    [SerializeField]
    private Vector3 offset; // Player (Main Camera)'s position 
    private Transform inspectMini;
    private Transform model_origin_mini;

/*    private void Awake()
    {
        objToEnableGPUI = GameObject.Find("MainMenu");
        //EnableGPUIntancing();

        Vector3 offset = GameObject.Find("Main Camera").transform.position;
        // Caching references
        inspectMini = GameObject.Find("InspectMini").transform;
        model_origin_mini = GameObject.Find("model_origin_mini").transform;
    }*/

    /* Enable GPU Instancing for all materials* of the entire hierarchy** of the GameObject objToEnableGPUI
     * Only need to be run ONCE bc modifications to .shareMaterials are project-wide.
     *  */
    private void EnableGPUIntancing()
    {
        // Get Reference 
        MeshRenderer[] mrs = objToEnableGPUI.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            for (int i = 0; i < mr.sharedMaterials.Length; i++)
            {
                mr.sharedMaterials[i].enableInstancing = true;
            }
            Debug.Log(mr.gameObject.name);
            
        }

    }

    public void SceneReset()
    {
        /*// InspectMini
        inspectMini.gameObject.SetActive(true);
        inspectMini.position = new Vector3(-0.004f, 1.3f, 0.8f) + offset;
        inspectMini.rotation = Quaternion.identity;
        inspectMini.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        DetachFromParent(model_origin_mini);
        inspectMini.gameObject.SetActive(false);

        // MapOrigin_RealWorld
        GameObject.Find("MapOrigin_RealWorld").SetActive(false);

        // MainMenu
        objToEnableGPUI.SetActive(false);
        objToEnableGPUI.transform.position = new Vector3(-0.004f, 1.3f, 0.8f) + offset;
        objToEnableGPUI.transform.rotation = Quaternion.identity;
        objToEnableGPUI.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        objToEnableGPUI.SetActive(false);*/

        Resources.UnloadUnusedAssets();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    }

    private void DetachFromParent(Transform _child)
    {
        // #TODO: Save World Position, Rotation, Scale first??

        _child.SetParent(null, true);
        _child.position = Vector3.zero + offset;
        _child.rotation = Quaternion.identity;
        _child.localScale = Vector3.one;
    }

    private void DetachChild(Transform _parent, string childName)
    {
        _parent.GetChild(0).SetParent(null, true); // #TODO: destroy child
    }

}
