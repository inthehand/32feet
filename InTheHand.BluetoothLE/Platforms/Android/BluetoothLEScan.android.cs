//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth.LE;
using Android.Runtime;
using System.Collections.Generic;
using System.Diagnostics;

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
        private readonly BluetoothLEScanOptions _options;
        private readonly BluetoothLeScanner _scanner;
        private readonly Callback _callback;

        internal BluetoothLEScan(BluetoothLEScanOptions options, BluetoothLeScanner scanner)
        {
            _options = options;
            if (options != null)
            {
                _filters = options.Filters;
            }

            //var settings = new ScanSettings.Builder().SetScanMode(ScanMode.LowLatency).Build();

            _callback = new Callback(this);
            _scanner = scanner;
            scanner.StartScan(_callback);
            Active = true;
        }

        public static implicit operator BluetoothLeScanner(BluetoothLEScan scan)
        {
            return scan._scanner;
        }

        private bool PlatformKeepRepeatedDevices
        {
            get
            {
                if (_options == null)
                    return true;

                return _options.KeepRepeatedDevices;
            }
        }

        private bool PlatformAcceptAllAdvertisements
        {
            get
            {
                if (_options == null)
                    return true;

                return _options.AcceptAllAdvertisements;
            }
        }

        private void PlatformStop()
        {
            _scanner.StopScan(_callback);
        }

        private sealed class Callback : ScanCallback
        {
            private readonly BluetoothLEScan _scan;

            internal Callback(BluetoothLEScan owner)
            {
                _scan = owner;
            }

            public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
            {
                Debug.WriteLine($"BluetoothLEScan.OnScanResult {result.Device} {result.Rssi} {result.TxPower} {result.ScanRecord}");
                Bluetooth.OnAdvertisementReceived(result);
            }

            public override void OnBatchScanResults(IList<ScanResult> results)
            {
                Debug.WriteLine("BluetoothLEScan.OnBatchScanResults");
            }

            public override void OnScanFailed(ScanFailure errorCode)
            {
                Debug.WriteLine($"BluetoothLEScan.OnScanFailed {errorCode}");
            }
        }
    }
}
