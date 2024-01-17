using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class Slider_Rotate : MonoBehaviour
{
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private List<Slider> sliders = new List<Slider>();

    Quaternion targetRotation;



    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        /*// Auto Spin
        Vector3 originalRotation = transform.localEulerAngles;
        targetRotation = Quaternion.Euler(originalRotation.x + sliders[0].Value, originalRotation.y + sliders[1].Value, originalRotation.z + sliders[2].Value);
        transform.localRotation = targetRotation;*/

        targetRotation = Quaternion.Euler(sliders[0].Value, sliders[1].Value, sliders[2].Value);
        transform.localRotation = targetRotation;
    }

        
}
