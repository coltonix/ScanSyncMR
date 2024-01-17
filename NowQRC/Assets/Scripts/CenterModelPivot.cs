using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterModelPivot : MonoBehaviour
{
    public int modelIndex;
    private GameObject model;
    //private List<GameObject> models = new List<GameObject> ();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCondition());
    }


    IEnumerator WaitForCondition()
    {
        while (!ObjectPool.SharedInstance.isGettable) // singleton: ObjectPool.cs
        {
            // Wait for isGettable to become true
            yield return null;
        }
        // Continue work When isGettable == true
        HookModelCenter();
    }

    // Assign the model center to _this_ very pivot point
    public void HookModelCenter()
    {
        Debug.Log(this.name + ": Object Pool is gettable");
        // Get a model from the object pool
        model = ObjectPool.SharedInstance.GetPooledObjectByIndex(modelIndex); // singleton: ObjectPool.cs
        if (model)
        {
            Debug.Log(this.name + " successfully got a model.");
            model.SetActive(true);
        }
        else
        {
            Debug.Log(this.name + " cannot get a model.");
        }


        /*// Positioning the 3D model at the center of the camera's view ---- When the model contains children each with its own mesh
        MeshFilter[] meshes = model.GetComponentsInChildren<MeshFilter>();
        MeshFilter middleMesh = meshes[meshes.Length / 2];
        model.transform.position -= middleMesh.transform.rotation * middleMesh.sharedMesh.bounds.center;*/

        // This GameObject must first exist in the highest hierarchy (with model being in the same level of hierarchy)
        model.transform.SetParent(transform, true);

        // Make inactive until either one Control mode is turned on OR until Inspect/1:1 is toggled on. 
        SetGameObjetActive(false);
    }

    public void SetControlInactive()
    {
        transform.SetParent(null, true);

        SetGameObjetActive(false);
    }

    /*Assign to parameter[modelCenterPivot]:
     *  [OrbControl > Pivot-Model-Center] For "Edit/Traditional Control" mode -- Deprecated
     *  [BoundsControl > Pivot-Move-Distance] For "Edit/Bounds Control" mode
     *  [InspectMini] For "Inspect/Mini" (Scale-Disabled Bounds Control)
    */
    public void SetControlActive(Transform modelCenterPivot)
    {
        transform.SetParent(modelCenterPivot, true);
        // Now you can rotate/scale the model by rotating/scaling modelCenterPivot

        SetGameObjetActive(true);
    }

    public void SetGameObjetActive(bool state)
    {
        transform.gameObject.SetActive(state);
    }

}
