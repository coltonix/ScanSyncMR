using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance; // use of Singleton pattern
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    public bool isGettable = false; // Are all variables initialized in Start()? (Should be 'false' by default)

    
    [SerializeField]
    private GameObject MainMenu;
    

    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this) // Destroy previous instance to only use the new one instead
        {
            Destroy(SharedInstance);
        }
        SharedInstance = this;
        // DontDestroyOnLoad(SharedInstance);

        //GlobalVariables.SharedInstance.LoadingVisualToggle(false); // Disabled to improve performance // singleton: GlobalVariables.cs
        MainMenu.SetActive(false);
        Debug.Log("=============== ObjectPool.cs Awake() ==================");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("=============== ObjectPool.cs StartCoroutine ==================");
        StartCoroutine(WaitForCondition());
    }

    IEnumerator WaitForCondition()
    {
        while (!objectToPool)
        {
            // Wait for objectToPool to be assigned in AddressablesManager.cs
            yield return null;
        }
        // Continue work When objectToPool is assigned a GameObject
        Coroutine CreateOTP = StartCoroutine(PreprocessObjectToPool()); // Start Preprocessing AND THEN Instantiating...

        // Do the following while CreateOTP is running...

        Debug.Log("=============== yield return CreateOTP; 0 ==================");
        yield return CreateOTP; // Wait for CreateOTP to terminate
        Debug.Log("=============== yield return CreateOTP; 1 ==================");

        GlobalVariables.SharedInstance.LoadingVisualToggle(false); // Disabled to improve performance // singleton: GlobalVariables.cs
        MainMenu.SetActive(true);
        isGettable = true;
    }

    // To improve model rendering performance
    private IEnumerator PreprocessObjectToPool()
    {
        // Get Reference | This works when the entire model hierarchy has only 1 <MeshRenderer> component
        MeshRenderer meshRenderer = objectToPool.GetComponentInChildren<MeshRenderer>();
        MeshFilter meshFilter = objectToPool.GetComponentInChildren<MeshFilter>();

        if (meshRenderer != null)
        {
            // Enable GPU Instancing for all materials on the model
            Material[] modelMaterials = meshRenderer.sharedMaterials; // Get Reference | Modifying `.sharedMaterials` even changes the materials in the project
            for (int i = 0; i < modelMaterials.Length; i++)
            {
                modelMaterials[i].shader = Shader.Find("Graphics Tools/Standard"); // Change shader to "Mobile/VertexLit" (better than "Mobile/Diffuse") https://docs.unity3d.com/Manual/shader-Performance.html
                modelMaterials[i].enableInstancing = true;
            }

            // Turn off shadows
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
        }
        else
        {
            Debug.LogWarning(this.name + ".cs : objectToPool and its children do not have one or just one <MeshRenderer> component, but it requires ONE exactly.");
        }

        if (meshFilter != null)
        {
            meshFilter.sharedMesh.UploadMeshData(true); // Frees up system memory copy of mesh data 
            /* It must be noted than when a Mesh is not readable, 
             * it only lives in the GPU memory and NOT in the CPU memory. 
             * Therefore, to make it "readable", 
             * one must pull back the data from the GPU. 
             * https://forum.unity.com/threads/reading-meshes-at-runtime-that-are-not-enabled-for-read-write.950170/ 
             * https://docs.unity3d.com/ScriptReference/Mesh-isReadable.html
             * https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Mesh.UploadMeshData.html
             */
            //Debug.LogFormat("SharedMesh's Read/Write Enabled : " + meshFilter.sharedMesh.isReadable);
        }

        Debug.Log("=============== PreprocessObjectToPool() ==================");

        // ----------- Preprocessing is done, now instantiate the objects ----------- 
        yield return StartCoroutine(InstantiatePooledObjects());
    }

    private IEnumerator InstantiatePooledObjects()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        // Instantiate set amount of objects, make inactive, then store them to the list (all before runtime)
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            //tmp = CombineMeshes(tmp); 
            //Debug.LogWarningFormat("-- Loop {0}: Instantiated {1}", i, tmp.name);
            
            /*if (i == 0)
            {
                Mesh mesh = tmp.GetComponentInChildren<MeshFilter>().sharedMesh;

                for (int j = 0; j < mesh.subMeshCount; j++)
                {
                    Debug.LogWarningFormat("Submesh {0}: {1}", j, mesh.GetSubMesh(j).bounds.center);
                }
            }*/
           
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        Debug.Log("=============== InstantiatePooledObjects() ==================");
        // ----------- Instantiation is completed, now go back to next line atm in IEnumerator WaitForCondition() ----------- 
        yield break;
    }

    /*    // Optimize performance (https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html)
        private GameObject CombineMeshes(GameObject model) // #TODO: Solve issue: Too many materials
        {
            MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();
            List<CombineInstance> combines = new List<CombineInstance>();
            List<Material> materials = new List<Material>(); // KurtFixed: handle materials... I mean, they're kind of important!

            for (int i = 0; i < meshFilters.Length; i++)
            {
                // KurtFixed: we gotta ignore ourselves or our count would be off! [Note: only when `model` itself has the <MeshFilter> component]
                if (meshFilters[i] == model.GetComponent<MeshFilter>())
                {
                    continue;
                }

                MeshRenderer mr = meshFilters[i].GetComponent<MeshRenderer>(); // KurtFixed: tally up the materials, since each mesh could have multiple
                for (int j = 0; j < mr.materials.Length; j++)
                {
                    CombineInstance combine = new CombineInstance
                    {
                        // Once mesh property is queried, link to the original shared mesh is lost and 
                        // MeshFilter.sharedMesh property becomes an alias to mesh. If you want 
                        // to avoid this automatic mesh duplication, use MeshFilter.sharedMesh instead.
                        mesh = meshFilters[i].sharedMesh,
                        subMeshIndex = j,
                        //transform = meshFilters[i].transform.localToWorldMatrix
                        transform = model.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix // Combine initial meshes in a space of root GameObject
                    };
                    meshFilters[i].gameObject.SetActive(false);

                    combines.Add(combine);

                    materials.Add(mr.materials[j]);
                }
            }
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // To increase the max vertex count (65535) of UInt16 index format.
            mesh.CombineMeshes(combines.ToArray(), false);
            model.AddComponent<MeshFilter>().sharedMesh = mesh;
            model.SetActive(true);
            model.AddComponent<MeshRenderer>().materials = materials.ToArray(); // KurtFixed: inject the original materials

            return model;
        }*/

    // Allow other scripts to get desired object and set it to active
    public GameObject GetPooledObjectByIndex(int index)
    {
        /*
         * In our case,
         *      index = 0 => Model for "Edit" & "Inspect/1:1" purposes
         *      index = 1 => Model for "Inspect/Mini" purpose
         */
        return pooledObjects[index];
           
    }

    // Allow other scripts to get first inactive object in the hierarchy and set it to active
    public GameObject GetPooledObjectBasedOnActivity()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    
}
