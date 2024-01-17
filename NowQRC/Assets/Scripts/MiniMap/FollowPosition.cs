using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField]
    private Transform playerCamera;
    private int conversionScale;

    // Start is called before the first frame update
    void Start()
    {
        conversionScale = 80;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localPosition = new Vector3(Mathf.Clamp(playerCamera.localPosition.x, -0.1f * conversionScale, 0.1f * conversionScale), 
            Mathf.Clamp(playerCamera.localPosition.z, -0.1f * conversionScale, 0.1f * conversionScale), 
            0); // -0.1f: Left/Lower Boundary of MiniMap ; 0.1f: Right/Upper Boundary of MiniMap
        transform.localPosition /= conversionScale; // First unify the coordinate system with the model-origin's Only Then set this scale
    }
}
