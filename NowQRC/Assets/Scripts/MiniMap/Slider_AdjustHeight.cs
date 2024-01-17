using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class Slider_AdjustHeight : MonoBehaviour
{
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private Slider slider;

    private Vector3 originalPosition;

    // Update is called once per frame
    void Update()
    {
        AdjustDistance();
    }

    private void AdjustDistance()
    {
        originalPosition = transform.localPosition;
        transform.localPosition = new Vector3(originalPosition.x, slider.Value, originalPosition.z);
    }

    
}
