using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowTarget : MonoBehaviour
{
    /*[SerializeField]
    private GameObject model;*/
    [SerializeField]
    private Transform targetToFollow;
    [Tooltip("To control the distance and position relative to the target")]
    [SerializeField]
    private Vector3 positionOffset;
    [Tooltip("To determine which of (x, y, z) should be followed/interpolated, say (0, 1, 0) to only follow the target in y-axis direction")]
    [SerializeField]
    private Vector3 positionMask = Vector3.one;
    [SerializeField]
    private bool useLerp;

    private Vector3 followPosition;
    private Vector3 currentPosition;
    //private float pmx, pmy, pmz, checksum;

/*    // Start is called before the first frame update
    void Start()
    {
        //transform.position = targetToFollow.position;
    }*/


    void LateUpdate()
    {
        if (targetToFollow)
        {
            followPosition = targetToFollow.position + positionOffset;
            currentPosition = transform.position;
            /*pmx = positionMask.x;
            pmy = positionMask.y;
            pmz = positionMask.z;
            checksum = pmx + pmy + pmz;
            if (checksum > 3 || checksum < 0)
            {
                Debug.LogWarningFormat("FollowTarget.cs: Vector3 positionMask values are invalid...\nWill Not Follow");
            }
            else
            {
            }*/
            if (!useLerp)
            {
                transform.position = new Vector3(positionMask.x == 1 ? followPosition.x : currentPosition.x,
                                                positionMask.y == 1 ? followPosition.y : currentPosition.y,
                                                positionMask.z == 1 ? followPosition.z : currentPosition.z);
            }
            else
            {
                transform.position = new Vector3(Mathf.Lerp(currentPosition.x, followPosition.x, positionMask.x),
                                                Mathf.Lerp(currentPosition.y, followPosition.y, positionMask.y),
                                                Mathf.Lerp(currentPosition.z, followPosition.z, positionMask.z));
            }
            
        }
    }
}
