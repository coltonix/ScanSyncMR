using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareTransform_Simple : MonoBehaviour
{
    [SerializeField]
    private Transform ReceiveFromSource; // Either this
    [SerializeField]
    private Transform SendToTarget; // Or this
    [SerializeField]
    private bool isSharingActive;
    [SerializeField]
    private bool SharePosition; // Relative Location
    [SerializeField]
    private bool ShareRotation;
    [SerializeField]
    private bool ShareScale;
    private Vector3 targetLocation;
    


    // Update is called once per frame
    void Update()
    {
        if (isSharingActive)
        {
            SendTransform(SharePosition, ShareRotation, ShareScale);
            //ReceiveTransform(SharePosition, ShareRotation, ShareScale);
        }
    }
 
        public void SetSharingActive(bool status) // True or false
    {
        isSharingActive = status;
    }

    public void ShareSourceTransform(int status) // (from 000 to 111)
    {
            SharePosition = (status / 100 == 1);
            ShareRotation = (status / 10 == 1);
            ShareScale = (status % 10 == 1);
    }

    private void SendTransform(bool bpos, bool brot, bool bscal) 
    {
        if (bpos)
        {
            // set target position
            SendToTarget.localPosition = transform.localPosition;
        }
        if (brot)
        {
            // set target rotation
            SendToTarget.localRotation = transform.localRotation;
        }
        if (bscal)
        {
            // set target scale
            SendToTarget.localScale = transform.localScale;
        }
    }

    private void ReceiveTransform(bool bpos, bool brot, bool bscal)
    {
        if (bpos)
        {
            // set target position
            transform.localPosition = ReceiveFromSource.localPosition;
        }
        if (brot)
        {
            // set target rotation
            transform.localRotation = ReceiveFromSource.localRotation;
        }
        if (bscal)
        {
            // set target scale
            transform.localScale = ReceiveFromSource.localScale;
        }
    }
}
