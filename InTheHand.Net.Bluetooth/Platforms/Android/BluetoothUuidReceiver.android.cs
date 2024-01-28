// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothUuidReceiver (Android)
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    internal class BluetoothUuidReceiver : BroadcastReceiver
    {
        private TaskCompletionSource<IEnumerable<Guid>> _tcs;

        // contractual obligation - do not use
        public BluetoothUuidReceiver() { }

        public BluetoothUuidReceiver(TaskCompletionSource<IEnumerable<Guid>> taskCompletion)
        {
            _tcs = taskCompletion;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == BluetoothDevice.ActionUuid)
            {
                // process uuids;
                if (intent.Extras.ContainsKey(BluetoothDevice.ExtraUuid))
                {
                    var list = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                    if (list == null)
                        return;

                    List<Guid> uuids = new List<Guid>();
                    foreach (var item in list)
                    {
                        var uuid = item as ParcelUuid;
                        var g = Guid.Parse(uuid.ToString());
                        if (g != BluetoothService.BluetoothBase)
                        {
                            uuids.Add(g);
                        }

                    }

                    _tcs.SetResult(uuids.AsReadOnly());
                }
            }
        }
    }
}