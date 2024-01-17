using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    private static GlobalVariables instance; // use of Singleton pattern
    public static GlobalVariables SharedInstance 
    { 
        get 
        { 
            return instance; 
        } 
    }

    // Global Varables
    public bool isGettable; // Are all variables initialized in Start()? (Should be 'false' by default)

    [Tooltip("Map to RealWorld Conversion Scale")]
    public int conversionScale;
    [Tooltip("MRTK Sliders (Order Does Not Matter, but Name Does)")]
    public List<Slider> sliders = new List<Slider>();

    [Tooltip("Is Traditional Control mode enabled? (Should be 'false' by default)")]
    public bool MapEnabled;

    [Tooltip("The string data from the lastest scanned QR code (Value is assigned in QRCode.cs)")]
    public string currentQRData;
    public bool isQRDataUpdated;

    [Tooltip("The Loading animation to be played, from Confirming QRCode until Models are loaded.")]
    [SerializeField]
    private GameObject LoadingVisual;

    private void Awake()
    {
        if (instance != null && instance != this) // Destroy previous instance to only use the new one instead
        {
            Destroy(instance);
        }
        instance = this;
        //DontDestroyOnLoad(SharedInstance);
    }

    // Start is called before the first frame update
    void Start()
    {
        conversionScale = 3000;

        // Same as BoundsControl's Min Max Scale Constraint Component Values
        Slider sliderScale = sliders.SingleOrDefault(slider => slider.gameObject.name == "SliderBase_Scale"); // Get Reference of SliderBase_Scale
        sliderScale.MaxValue = 2;
        sliderScale.MinValue = 0.09f;

        MapEnabled = false;

        isGettable = true;
    }

    public void LoadingVisualToggle(bool bToggleOn)
    {
        LoadingVisual.GetComponentInChildren<Animator>().enabled = bToggleOn; // Turn it false to improve performance 
        LoadingVisual.SetActive(bToggleOn);
    }
}
