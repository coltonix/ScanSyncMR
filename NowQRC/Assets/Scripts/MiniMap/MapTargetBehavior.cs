using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class MapTargetBehavior : MonoBehaviour
{
    [SerializeField]
    private bool SlidersEnabled;
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private Slider sliderX;
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private Slider sliderZ;
    
    [SerializeField]
    [Tooltip("MRTK Slider")]
    private Slider sliderY; // Height

    [SerializeField]
    private Transform realWorldTarget;

    [SerializeField]
    private int conversionScale; // Map coordinates <=> Real World Coordinates

    private Transform temp; // Temporary Transform of realWorldTarget | See in LateUpdate()

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCondition());
    }

    IEnumerator WaitForCondition()
    {
        while (!GlobalVariables.SharedInstance.isGettable) // singleton: GlobalVariables.cs
        {
            // Wait for isGettable to become true
            yield return null;
        }
        // Continue work When isGettable == true
        conversionScale = GlobalVariables.SharedInstance.conversionScale; // singleton: GlobalVariables.cs
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!GlobalVariables.SharedInstance.MapEnabled) // Update Position Changes from Bounds Control Mode // singleton: GlobalVariables.cs
        {
            temp = realWorldTarget.transform;
            temp.localPosition /= conversionScale;
            /*transform.localPosition = new Vector3(temp.localPosition.x, temp.localPosition.z, 0);
            MoveByDragging();*/
            sliderX.Value = temp.localPosition.x;
            sliderZ.Value = temp.localPosition.z;
            MoveBySliders();

            sliderY.Value = temp.localPosition.y * conversionScale; // cancel the conversion bc there was no conversion for Y

            GlobalVariables.SharedInstance.MapEnabled = true; // singleton: GlobalVariables.cs
        }
        else // "Traditional Control > Move mode" is active
        {
            if (SlidersEnabled) // Drag Slider to Move Model
            {
                MoveBySliders();
            }
            else // Drag Map Target to Move Model
            {
                MoveByDragging();
            }

            UpdateTargetPosition();
        }

    }

    public void InformMapStatus(bool state)
    {
        GlobalVariables.SharedInstance.MapEnabled = state; // singleton: GlobalVariables.cs
    }

    public void EnableSliders(bool state)
    {
        SlidersEnabled = state;
    }

    private void MoveBySliders()
    {
        transform.localPosition = new Vector3(sliderX.Value, sliderZ.Value, 0);
    }

    private void MoveByDragging()
    {
        float maxX = 0.1f;
        float maxY = 0.1f;

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, -maxX, maxX),
            Mathf.Clamp(transform.localPosition.y, -maxY, maxY),
            0
        );

        transform.localPosition = clampedPosition;
        sliderX.Value = clampedPosition.x;
        sliderZ.Value = clampedPosition.y;
    }

    private void UpdateTargetPosition()
    {
        realWorldTarget.localPosition = new Vector3(transform.localPosition.x * conversionScale, sliderY.Value, transform.localPosition.y * conversionScale);
    }
}
