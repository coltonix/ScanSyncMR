// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.SampleQRCodes {
    [RequireComponent(typeof(SpatialGraphNodeTracker))]
    public class QRCode : MonoBehaviour {
//#if UNITY_WSA
        public Microsoft.MixedReality.QR.QRCode qrCode;

        [SerializeField]
        private GameObject qrCodeCube;
        [SerializeField]
        private TextMeshPro resultText;

        public float PhysicalSize { get; private set; }

        private long lastTimeStamp = 0;

        private string prevQRData; /* --------------------- #ColMOD --------------------- */

        // Use this for initialization
        void Start() {
            PhysicalSize = 0.1f;

            if (qrCode == null) {
                throw new System.Exception("QR Code Empty");
            }

            PhysicalSize = qrCode.PhysicalSideLength;

            qrCodeCube = gameObject.transform.Find("Cube").gameObject;
            resultText = GameObject.Find("ResultText").GetComponent<TextMeshPro>();

            Debug.Log("Id= " + qrCode.Id + "NodeId= " + qrCode.SpatialGraphNodeId + " PhysicalSize = " + PhysicalSize + " TimeStamp = " + qrCode.SystemRelativeLastDetectedTime.Ticks + " QRVersion = " + qrCode.Version + " QRData = " + qrCode.Data);
        }

        void UpdatePropertiesDisplay() {
            // Update properties that change
            if (qrCode != null && lastTimeStamp != qrCode.SystemRelativeLastDetectedTime.Ticks) {
                PhysicalSize = qrCode.PhysicalSideLength;
                resultText.text = qrCode.Data;
                /* --------------------- #ColMOD ---------------------O */
                GlobalVariables.SharedInstance.isQRDataUpdated = prevQRData == qrCode.Data; // singleton: GlobalVariables.cs
                if (!GlobalVariables.SharedInstance.isQRDataUpdated) // so as to fetch file ONLY ONCE for every QR code
                {
                    GlobalVariables.SharedInstance.currentQRData = qrCode.Data; // singleton: GlobalVariables.cs
                    prevQRData = qrCode.Data;
                    GlobalVariables.SharedInstance.isQRDataUpdated = true;  // singleton: GlobalVariables.cs
                }
                /* --------------------- #ColMOD ---------------------1 */

                Debug.Log("Id= " + qrCode.Id + "NodeId= " + qrCode.SpatialGraphNodeId + " PhysicalSize = " + PhysicalSize + " TimeStamp = " + qrCode.SystemRelativeLastDetectedTime.Ticks + " Time = " + qrCode.LastDetectedTime.ToString("MM/dd/yyyy HH:mm:ss.fff"));

                qrCodeCube.transform.localPosition = new Vector3(PhysicalSize / 2.0f, PhysicalSize / 2.0f, 0.0f);
                qrCodeCube.transform.localScale = new Vector3(PhysicalSize, PhysicalSize, 0.0005f);
                lastTimeStamp = qrCode.SystemRelativeLastDetectedTime.Ticks;
            }
        }

        // Update is called once per frame
        void Update() {
            UpdatePropertiesDisplay();
        }
//#endif
    }
}