using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    public float FixedScale;
    public bool MoveModeOn;
    public bool isMoveHorizontal;
    public GameObject orb;
    //public List<GameObject> parents;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        //parents = new List<GameObject> ();
    }

    // Update is called once per frame
    void Update()
    {
        MoveModePositionUpdate(MoveModeOn);

    }

    public void SetMoveMode(bool ModeOn)
    {
        MoveModeOn = ModeOn;
    }

    public void SetMoveHorizontal(bool bIsMoveHorizontal)
    {
        isMoveHorizontal = bIsMoveHorizontal;
    }

    private void MoveModePositionUpdate(bool ModeOn)
    {
        originalScale = transform.localScale;
        if (ModeOn)
        {
            
            originalPosition = transform.position;
            if (isMoveHorizontal) // Move in XoZ-plane
            {
                transform.SetParent(orb.transform, true);
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
            }
            else // Move along Y-axis
            {
                transform.SetParent(orb.transform, true);
                transform.position = new Vector3(originalPosition.x, transform.position.y, originalPosition.z);
            }
            // transform.localScale = originalScale;
            transform.localScale = new Vector3(FixedScale / orb.transform.localScale.x, FixedScale / orb.transform.localScale.y, FixedScale / orb.transform.localScale.z);
        }
        else
        {
            transform.SetParent(null);
            transform.localScale = new Vector3(FixedScale / originalScale.x, FixedScale / originalScale.y, FixedScale / originalScale.z);
        }
        
    }

    
}
