// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (iOS)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using ExternalAccessory;
using Foundation;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothDevicePicker
    {
        NSObject connectionObserver;

        private async Task<BluetoothDeviceInfo> DoPickSingleDeviceAsync()
        {
            EAAccessoryManager.SharedAccessoryManager.RegisterForLocalNotifications();

            TaskCompletionSource<EAAccessory> tcs = new TaskCompletionSource<EAAccessory>();

            connectionObserver = EAAccessoryManager.Notifications.ObserveDidConnect((s, e) =>
            {
                tcs.SetResult(e.Selected);
            });

            /*var observer2 = EAAccessoryManager.Notifications.ObserveDidDisconnect((s, e) =>
            {
               // tcs.SetResult(e.Accessory);
            });*/

            try
            {
                await EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPickerAsync(null);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                EAAccessoryManager.SharedAccessoryManager.UnregisterForLocalNotifications();
                return null;
            }

            EAAccessory accessory = null;
            if (EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories.GetLength(0) > 0)
            {
                accessory = EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories[0];
            }
            else
            {
                accessory = await tcs.Task;
            }
            
            EAAccessoryManager.SharedAccessoryManager.UnregisterForLocalNotifications();
            connectionObserver.Dispose();

            return accessory;
        }
    }
}