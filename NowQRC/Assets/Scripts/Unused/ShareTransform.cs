using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareTransform : MonoBehaviour
{
    private bool isMoving;
    private bool isRotating;
    private bool isScaling;


    [SerializeField]
    [Tooltip("Array of Transform family that will be managed by this controller.")]
    private List<Transform> family = new List<Transform>();


    /*--
    /// <summary>
    /// List of Transform family that will be managed by this controller.
    /// </summary>
    public List<Transform> TransformFamily
    {
        get => family;
        set
        {
            if (value != null && family != value)
            {
                if (family != null)
                {
                    // Destroy all listeners on previous toggleList
                    //RemoveSelectionListeners();
                }

                // Set new list
                family = value;

                //int index = Mathf.Clamp(CurrentIndex, 0, family.Count - 1);
                //SetSelection(index);
            }
        }
    }
    --*/

    [SerializeField]
    [Tooltip("Currently selected index in the Transform Family, its transform will be propagated to other transform family members;" +
        "\ndefault is 0")]
    private int currentIndex;

    /*-------
    /// <summary>
    /// The current index in the array of family
    /// </summary>
    public int CurrentIndex
    {
        get => currentIndex;
        //set => SetSelection(value);
    }
    -------*/

    private void Start()
    {
        isMoving = true;
        isRotating = false;

        // If we don't already have any family member listed, we scan for family
        // in our direct children.
        if (family == null || family.Count == 0)
        {
            // Make sure our family are not null.
            family ??= new List<Transform>();

            /*-----------------
            // Find some family!
            foreach (Transform child in transform)
            {
                var childTransform = child.GetComponent<Transform>();

                if (childTransform != null)
                {
                    family.Add(childTransform);
                }                
            }
            if (TransformFamily != null)
            {

                // Force set initial selection in the toggle collection at start
                if (CurrentIndex >= 0 && CurrentIndex < TransformFamily.Count)
                {
                    //SetSelection(CurrentIndex, true);
                    //TransformFamily[CurrentIndex].ForceSetToggled(true); // Some operations
                }
            }
            ------------------------*/
            Debug.Log("Family: \n" + family);
            //Debug.LogWarning("Transform Family: \n" + TransformFamily);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowParent();
        RotateObj();
      
    }

    /** ----------------------------- **/

    public void IsMoving(bool state)
    {
        isMoving = state;
    }

    public void IsRotating(bool state)
    {
        isRotating = state;
    }

    public void IsScaling(bool state)
    {
        isScaling = state;
    }

    public void SetActiveTransformFamilyMember(int familyMemberIndex)
    { 
        currentIndex = familyMemberIndex;
    }

    private void UpdateTransformFamily(string updateMode)
    {
        if (updateMode == "position")
        {
            for (int i = 1; i < family.Count; i++)
            {
                if (i != currentIndex)
                {
                    family[i].localPosition = family[currentIndex].localPosition;
                }
            }
        }
        if (updateMode == "rotation")
        {
            Vector3 targetRotation = transform.eulerAngles;

            for (int i = 1; i < family.Count; i++)
            {
                family[i].rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
            }
        }
        if (updateMode == "scale")
        {
            Vector3 targetScale = transform.localScale;

            for (int i = 1; i < family.Count; i++)
            {
                family[i].localScale = targetScale;
            }
        }
    }

    private void FollowParent()
    {

        if (isMoving)
        {
            //var originalScale = transform.localScale;
            //----> float FixedScale = 1;
            Vector3 originalPosition = transform.position;

            if (transform.parent != null)
            {
                //transform.parent = null;
            }


            if (currentIndex == 1) // Move on XoZ-plane
            {
                //Debug.Log("family[currentIndex]: " + family[currentIndex]);
                transform.SetParent(family[currentIndex], true);
                //transform.SetParent(new GameObject("TestParent_0").transform);
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
            }
            if (currentIndex == 2) // Move along Y-axis
            {
                //Debug.Log("family[currentIndex]: " + family[currentIndex]);
                transform.SetParent(family[currentIndex], true);
                //transform.SetParent(new GameObject("TestParent_1").transform);
                transform.position = new Vector3(originalPosition.x, transform.position.y, originalPosition.z);
            }
            // transform.localScale = originalScale;
            //----> transform.localScale = new Vector3(FixedScale / family[currentIndex].localScale.x, FixedScale / family[currentIndex].localScale.y, FixedScale / family[currentIndex].localScale.z);

            UpdateTransformFamily("position");
        }
        else
        {
            //transform.SetParent(family[0], true); // index 0 or others ?????????? What's happening???????????????????????????????????????????????????????????????????????????????????????????????
            //transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }


    }

    private void RotateObj()
    {
        if (isRotating)
        {
            UpdateTransformFamily("rotation");
        }

    }

    private void ScaleObj()
    {
        if (isScaling)
        {
            //UpdateTransformFamily("scale"); // No need to now. Besides, it's spinning so much
        }

    }
}
