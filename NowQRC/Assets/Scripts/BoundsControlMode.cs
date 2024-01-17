using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

public class BoundsControlMode : MonoBehaviour
{
    [SerializeField]
    private Transform BoundsControl;

    private Vector3 prevPos;
    private Vector3 posOffset;
    private float rotOffset;
    private float scaOffset;
    private float BCScale;

    // To sync the values with the sliders on Trad Control Mode
    float mapYMax;
    float mapYMin;
    float mapXMax;
    float mapXMin;
    float mapZMax;
    float mapZMin;
    private int mapConvScale;

    private Slider slider; // Temporary Slider to store selected slider | See usage in method SliderValue(string sliderName, string memberName, float sliderValue = float.NaN) 


    // Start is called before the first frame update
    void Start()
    {
        prevPos = BoundsControl.localPosition;
        rotOffset = 360f;
        BCScale = BoundsControl.localScale.x; // 0.15f is the original scale of the BoundsControl cube

        StartCoroutine(WaitForCondition());

    }

    IEnumerator WaitForCondition()
    {
       // Wait for isGettable to become true
        yield return new WaitUntil( () => GlobalVariables.SharedInstance.isGettable ); // singleton: GlobalVariables.cs
        // Continue work When isGettable == true
        mapConvScale = GlobalVariables.SharedInstance.conversionScale;

        // To sync the values with the sliders on Trad Control Mode
        mapYMax = SliderValue("MapSliderY", "MaxValue");
        mapYMin = SliderValue("MapSliderY", "MinValue");
        mapXMax = SliderValue("MapSliderX", "MaxValue");
        mapXMin = SliderValue("MapSliderX", "MinValue");
        mapZMax = SliderValue("MapSliderZ", "MaxValue");
        mapZMin = SliderValue("MapSliderZ", "MinValue");
    }

    // Update is called once per frame
    void Update()
    {
        posOffset = BoundsControl.localPosition - prevPos;

        scaOffset = BoundsControl.localScale.x / BCScale;
    }
   
    

    void LateUpdate()
    {
        // Move
        UpdatePosition();

        if (!GlobalVariables.SharedInstance.MapEnabled) // singleton: GlobalVariables.cs
        {
            // Rotate
            SliderValue("SliderBase_Rotate-X", "Value", BoundsControl.localEulerAngles.x % rotOffset);
            SliderValue("SliderBase_Rotate-Y", "Value", BoundsControl.localEulerAngles.y % rotOffset);
            SliderValue("SliderBase_Rotate-Z", "Value", BoundsControl.localEulerAngles.z % rotOffset);

            // Scale
            SliderValue("SliderBase_Scale", "Value", scaOffset);
           
        }
        else // When on Traditional Mode (MiniMap)
        {
            // Rotate the Bounds Control Cube Accordingly
            BoundsControl.localEulerAngles = transform.localEulerAngles;
            // Scale the Bounds Control Cube  Accordingly
            BoundsControl.localScale = transform.localScale * BCScale;
            // Nope, we don't want to move the Bounds Control Cube on this mode
        }

    }

    private void UpdatePosition()
    {
        if (posOffset.magnitude != 0)
        {
            transform.localPosition += posOffset * 10; // 10 is the magnitude of how changing the BoundsControl cube's pos can change the model's pos #TODO: Make Variable

            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, mapXMin * mapConvScale, mapXMax * mapConvScale),
                Mathf.Clamp(transform.localPosition.y, mapYMin, mapYMax),
                Mathf.Clamp(transform.localPosition.z, mapZMin * mapConvScale, mapZMax * mapConvScale)
                );

        }
        prevPos = BoundsControl.localPosition;
    }

    private float SliderValue(string sliderName, string memberName, float sliderValue = float.NaN)
    {
        slider = GlobalVariables.SharedInstance.sliders.SingleOrDefault(slider => slider.gameObject.name == sliderName); // singleton: GlobalVariables.cs
        if (!float.IsNaN(sliderValue)) // if sliderValue is passed, Update respective Slider's selected member value
        {
            switch (memberName)
            {
                case "Value":
                    slider.Value = sliderValue;
                    break;
                case "MinValue":
                    slider.MinValue = sliderValue;
                    break;
                case "MaxValue":
                    slider.MaxValue = sliderValue;
                    break;
            }

        }

        return memberName switch
        {
            "Value" => slider.Value,
            "MinValue" => slider.MinValue,
            "MaxValue" => slider.MaxValue,
            _ => 0f // Default value when `memberName` is not recognized
        };

        /*
         * Slider slider = GlobalVariables.SharedInstance.sliders.SingleOrDefault(slider => slider.gameObject.name == sliderName); // singleton: GlobalVariables.cs
         * if (!float.IsNaN(sliderValue))
        {
            slider?.GetType()?.GetField(memberName)?.SetValue(this, sliderValue);
        }
        return (float)(slider?.GetType()?.GetField(memberName)?.GetValue(this)); */

    }
}
