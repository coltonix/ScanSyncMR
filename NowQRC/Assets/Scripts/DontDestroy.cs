using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    private static Dictionary<string, GameObject> DontDestroyObjs = new Dictionary<string, GameObject>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        

        foreach (GameObject obj in objs)
        {
            if (!DontDestroyObjs.ContainsKey(obj.name))
            {
                DontDestroyObjs[obj.name] = obj;
                DontDestroyOnLoad(obj);
            }
            else
            {
                if (obj != DontDestroyObjs[obj.name]) // so it won't delete the original game object but the duplcates only
                    Destroy(obj);
            }
        }

        DontDestroyOnLoad(GameObject.Find("WorldLockingViz"));
        DontDestroyOnLoad(GameObject.Find("SpongyWorldAnchorRoot"));
        //DontDestroyOnLoad(GameObject.FindObjectOfType(SpongyAnchorNull));
    }

}
