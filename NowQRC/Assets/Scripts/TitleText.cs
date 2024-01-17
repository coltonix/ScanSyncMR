using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleText : MonoBehaviour
{
    //public TMPro.TMP_Text titleText;
    public TextMeshPro textmeshPro;
    public Transform modelOrigin;
    private string modelName;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCondition());
    }

    void Update()
    {
        setTextboxText();
    }

    IEnumerator WaitForCondition()
    {
        while (!ObjectPool.SharedInstance.isGettable) // singleton: ObjectPool.cs
        {
            // Wait for isGettable to become true
            yield return null;
        }
        // Continue work When isGettable == true
        
        setTextboxText();
    }

    private void initializeModelName()
    {
        //string modelName = modelOrigin.GetChild(0).gameObject.name; // this might run before modelOrgin gets its child => out of bound error
        if (ObjectPool.SharedInstance.objectToPool == null)
        {
            Debug.Log("OTP DESTROYED!");
        }
        else
        {
            modelName = ObjectPool.SharedInstance.objectToPool.name; // singleton: ObjectPool.cs
            modelName = modelName.Split("(Clone)", StringSplitOptions.RemoveEmptyEntries)[0]; // Remove "(Clone)" at the end of the name string
        }
        
    }

    private void setTextboxText()
    {
        initializeModelName();

        textmeshPro.SetText(modelName);
        //Debug.LogWarningFormat("TitleText.cs: Received Model Name : {0}",modelName);
    }
}
