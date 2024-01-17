using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowYRotation : MonoBehaviour
{
    public Transform TargetToFollow;
    Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles = new Vector3(0, 0, -TargetToFollow.eulerAngles.y);
        targetRotation = Quaternion.Euler(0, 0, -TargetToFollow.eulerAngles.y);
        transform.localRotation = targetRotation;
    }
}
