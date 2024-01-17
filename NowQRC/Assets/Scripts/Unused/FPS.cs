using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    //public TMPro.TMP_Text titleText;
    public TextMeshPro textmeshPro;

    private float deltaTime;
    private float _fps;
    private string fps;

    // Update is called once per frame
    void Update()
    {
        setTextboxText();
    }

    private void setTextboxText()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        _fps = 1.0f / deltaTime;
        fps = "FPS:" + Mathf.Ceil(_fps).ToString();
        
        textmeshPro.SetText(fps);
    }
}
