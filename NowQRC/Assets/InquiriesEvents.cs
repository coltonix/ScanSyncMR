using Microsoft.MixedReality.SampleQRCodes;
using MixedReality.Toolkit.UX;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class InquiriesEvents : MonoBehaviour
{
    [SerializeField]
    private PressableButton btn_Rescan;
    [SerializeField]
    private PressableButton btn_Confirm;

/*    private void Awake()
    {
        btn_Rescan = transform.Find("Btn_Rescan").GetComponent<PressableButton>();
        btn_Confirm = transform.Find("Btn_Confirm").GetComponent<PressableButton>();
    }*/

    void Awake()
    {
        btn_Confirm.OnClicked.AddListener(btn_ConfirmedEvents);
        /*btn_Confirm.OnClicked.AddListener(() => transform.root.GetComponent<QRCodesManager>().StopQRTracking());
        btn_Confirm.OnClicked.AddListener(() => gameObject.SetActive(false));*/

        btn_Rescan.OnClicked.AddListener(() => gameObject.SetActive(false));
    }

    private void btn_ConfirmedEvents()
    {
        //GameObject.Find("AddressablesManager").GetComponent<AddressablesManager>().OnDestroy();
        GameObject.Find("AddressablesManager").GetComponent<AddressablesManager>().FetchFile(""); //FetchFile("Assets/_ProcessedModels/FBX/LRT-2222(S)-RC-Simplygon.fbx")
                                                                                                  //GameObject.Find("Btn_ScanStartEnd")?.GetComponent<PressableButton>().ForceSetToggled(false);
                                                                                                  //GameObject.Find("QRMenu")?.SetActive(false);
        GameObject.Find("HololensMrQrReaderSample").GetComponent<QRCodesManager>().StopQRTracking(); // works; //GameObject.Find("HololensMrQrReaderSample").GetComponent<QRCodesManager>().ToggleAutoStartQRTracking(false);
        gameObject.SetActive(false);
    }
}

    