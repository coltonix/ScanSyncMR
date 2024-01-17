using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class Slider_Scale : MonoBehaviour
{
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private Slider slider;



    // Update is called once per frame
    void Update()
    {
        ScaleObject();
    }

    public void ScaleObject()
    {
        Vector3 Scale = new Vector3(slider.Value, slider.Value, slider.Value);
        transform.localScale = Scale;
    }

    
}
