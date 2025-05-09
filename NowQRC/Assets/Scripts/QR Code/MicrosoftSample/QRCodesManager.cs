﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#if UNITY_WSA
using Microsoft.MixedReality.QR;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.SampleQRCodes {
    public static class QRCodeEventArgs {
        public static QRCodeEventArgs<TData> Create<TData>(TData data) {
            return new QRCodeEventArgs<TData>(data);
        }
    }

    [Serializable]
    public class QRCodeEventArgs<TData> : EventArgs {
        public TData Data { get; private set; }

        public QRCodeEventArgs(TData data) {
            Data = data;
        }
    }

    public class QRCodesManager : Singleton<QRCodesManager> {
//#if UNITY_WSA
        [Tooltip("Determines if the QR codes scanner should be automatically started.")]
        public bool AutoStartQRTracking = true;

        public bool IsTrackerRunning { get; private set; }

        public bool IsSupported { get; private set; }

        public event EventHandler<bool> QRCodesTrackingStateChanged;
        public event EventHandler<QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode>> QRCodeAdded;
        public event EventHandler<QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode>> QRCodeUpdated;
        public event EventHandler<QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode>> QRCodeRemoved;

        private System.Collections.Generic.SortedDictionary<System.Guid, Microsoft.MixedReality.QR.QRCode> qrCodesList = new SortedDictionary<System.Guid, Microsoft.MixedReality.QR.QRCode>();

        private QRCodeWatcher qrTracker;
        private bool capabilityInitialized = false;
        private QRCodeWatcherAccessStatus accessStatus;
        private System.Threading.Tasks.Task<QRCodeWatcherAccessStatus> capabilityTask;

        private TextMeshPro resultText;


        public System.Guid GetIdForQRCode(string qrCodeData) {
            lock (qrCodesList) {
                foreach (var ite in qrCodesList) {
                    if (ite.Value.Data == qrCodeData) {
                        return ite.Key;
                    }
                }
            }
            return new System.Guid();
        }

        public System.Collections.Generic.IList<Microsoft.MixedReality.QR.QRCode> GetList() {
            lock (qrCodesList) {
                return new List<Microsoft.MixedReality.QR.QRCode>(qrCodesList.Values);
            }
        }
        /*protected void Awake() {

        }*/

        // Use this for initialization
        async protected virtual void Start() {
            IsSupported = QRCodeWatcher.IsSupported();
            capabilityTask = QRCodeWatcher.RequestAccessAsync();
            accessStatus = await capabilityTask;
            capabilityInitialized = true;

            resultText = GameObject.Find("ResultText2").GetComponent<TextMeshPro>();
        }

        private void SetupQRTracking() {
            try {
                qrTracker = new QRCodeWatcher();
                IsTrackerRunning = false;
                qrTracker.Added += QRCodeWatcher_Added;
                qrTracker.Updated += QRCodeWatcher_Updated;
                qrTracker.Removed += QRCodeWatcher_Removed;
                qrTracker.EnumerationCompleted += QRCodeWatcher_EnumerationCompleted;
            } catch (Exception ex) {
                Debug.Log("QRCodesManager : exception starting the tracker " + ex.ToString());
                resultText.text = "QRCodesManager : exception starting the tracker " + ex.ToString();
            }

            if (AutoStartQRTracking)
            {
                resultText.SetText("Calling StartQRTracking()");
                Debug.Log("-------------------------------------------------- StartQRTracking() - 3 -----------------------------------------------------------");
                StartQRTracking();
                Debug.Log("-------------------------------------------------- StartQRTracking() - 4 -----------------------------------------------------------");
            }
            else
            {
                resultText.SetText("Auto off : Calling StopQRTracking()");
                StopQRTracking();
            }
            
        }

        public void ToggleAutoStartQRTracking(bool bToggledOn)
        {
            AutoStartQRTracking = bToggledOn;
        }

        public void StartQRTracking() {
            Debug.Log("-------------------------------------------------- StartQRTracking() - 0 -----------------------------------------------------------");
            if (qrTracker != null && !IsTrackerRunning) {
                Debug.Log("QRCodesManager starting QRCodeWatcher");
                try {
                    qrTracker.Start();
                    IsTrackerRunning = true;
                    QRCodesTrackingStateChanged?.Invoke(this, true);
                } catch (Exception ex) {
                    Debug.Log("QRCodesManager starting QRCodeWatcher Exception:" + ex.ToString());
                }
            }
            Debug.Log("-------------------------------------------------- StartQRTracking() - 1 -----------------------------------------------------------");
            resultText.text += "\nCalled StartQRTracking()";
        }

        public void StopQRTracking() {
            resultText.text += "\nCalling StopQRTracking()";
            Debug.Log("-------------------------------------------------- StopQRTracking() - 0 -----------------------------------------------------------");
            if (IsTrackerRunning) {
                IsTrackerRunning = false;
                if (qrTracker != null) {
                    qrTracker.Stop();
                    qrCodesList.Clear();
                }

                var handlers = QRCodesTrackingStateChanged;
                if (handlers != null) {
                    handlers(this, false);
                }
            }
            Debug.Log("-------------------------------------------------- StopQRTracking() - 1 -----------------------------------------------------------");
            resultText.text += "\nCalled StopQRTracking()";
        }

        private void QRCodeWatcher_Removed(object sender, QRCodeRemovedEventArgs args) {
            Debug.Log("QRCodesManager QRCodeWatcher_Removed");

            bool found = false;
            lock (qrCodesList) {
                if (qrCodesList.ContainsKey(args.Code.Id)) {
                    qrCodesList.Remove(args.Code.Id);
                    found = true;
                }
            }
            if (found) {
                var handlers = QRCodeRemoved;
                if (handlers != null) {
                    handlers(this, QRCodeEventArgs.Create(args.Code));
                }
            }
        }

        private void QRCodeWatcher_Updated(object sender, QRCodeUpdatedEventArgs args) {
            Debug.Log("QRCodesManager QRCodeWatcher_Updated");

            bool found = false;
            lock (qrCodesList) {
                if (qrCodesList.ContainsKey(args.Code.Id)) {
                    found = true;
                    qrCodesList[args.Code.Id] = args.Code;
                }
            }
            if (found) {
                var handlers = QRCodeUpdated;
                if (handlers != null) {
                    handlers(this, QRCodeEventArgs.Create(args.Code));
                }
            }
        }

        private void QRCodeWatcher_Added(object sender, QRCodeAddedEventArgs args) {
            Debug.Log("QRCodesManager QRCodeWatcher_Added");

            lock (qrCodesList) {
                qrCodesList[args.Code.Id] = args.Code;
            }
            var handlers = QRCodeAdded;
            if (handlers != null) {
                handlers(this, QRCodeEventArgs.Create(args.Code));
            }
        }

        private void QRCodeWatcher_EnumerationCompleted(object sender, object e) {
            Debug.Log("QRCodesManager QrTracker_EnumerationCompleted");
        }

        private void Update() {
            if (qrTracker == null && capabilityInitialized && IsSupported) {
                if (accessStatus == QRCodeWatcherAccessStatus.Allowed) {
                    SetupQRTracking();
                } else {
                    Debug.Log("Capability access status : " + accessStatus);
                    resultText.text = "Capability access status : " + accessStatus;
                }
            }
        }
//#endif
    }
}