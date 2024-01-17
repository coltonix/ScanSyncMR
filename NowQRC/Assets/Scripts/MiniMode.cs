using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;

public class MiniMode : MonoBehaviour
{
    private GameObject model;
    public bool animRotate;

    private Quaternion targetRotation;
    private Vector3 originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        animRotate = false;
        StartCoroutine(WaitForCondition());
    }

    // Update is called once per frame
    void Update()
    {
        if (animRotate)
        {
            AnimRotate();
        }
    }


    IEnumerator WaitForCondition()
    {
        while (!transform.Find("model_origin_mini").gameObject)
        {
            // Wait until there's a child
            yield return null;
        }
        // Continue work When there's a child
        model = transform.Find("model_origin_mini").gameObject;
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale /= 500;
        InitializeColliderSize();
        /*model.AddComponent<BoxCollider>();
        // Add ObjectManipulator to this
        model.AddComponent<ObjectManipulator>();
        // Add BoundsControl to this
        model.AddComponent<BoundsControl>();
        // Add MinMaxScaleConstraint to this
        model.AddComponent<MinMaxScaleConstraint>();
        // Add BoxCollider to this
        this.AddComponent<BoxCollider>();*/
    }

    // Modify BoxCollider Size to fit model(child)
    private void InitializeColliderSize()
    {
        model.AddComponent<BoxCollider>();
        model.GetComponent<BoxCollider>().size = model.transform.localScale;
    }    

    // Function to Enable/Disable Auto-rotation "Animation"
    public void EnableAnimRotate(bool state)
    {
        animRotate = state;
    }

    private void AnimRotate()
    {
        originalRotation = model.transform.localEulerAngles;
        targetRotation = Quaternion.Euler(originalRotation.x, originalRotation.y + 1f, originalRotation.z);
        model.transform.localRotation = targetRotation;
    }
}
