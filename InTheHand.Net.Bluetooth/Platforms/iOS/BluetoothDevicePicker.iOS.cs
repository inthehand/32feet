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
using UIKit;

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
                await EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPickerAsync(null);//, new Action<NSError>((e) =>

                /* {
                     System.Diagnostics.Debug.WriteLine(e.Code);

                     switch(e.Code)
                     {
                         case (long)EABluetoothAccessoryPickerError.Cancelled:
                             tcs.SetCanceled();
                             break;

                         case (long)EABluetoothAccessoryPickerError.Failed:
                         case (long)EABluetoothAccessoryPickerError.NotFound:
                             tcs.SetException(new Exception(e.Code.ToString()));
                             break;

                         default:
                             tcs.SetResult(EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories[0]);
                             break;
                     }
                 }));
             });*/

                if (EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories.Length > 0)
                {
                    tcs.SetResult(EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories[0]);
                }
                else
                {
                    tcs.SetResult(null);
                }
            }
            catch (NSErrorException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                switch(ex.Code)
                {
                    case (long)EABluetoothAccessoryPickerError.AlreadyConnected:
                        tcs.SetResult(EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories[0]);
                        break;

                    case (long)EABluetoothAccessoryPickerError.Cancelled:
                        tcs.SetCanceled();
                        break;

                    case (long)EABluetoothAccessoryPickerError.Failed:
                    case (long)EABluetoothAccessoryPickerError.NotFound:
                        tcs.SetException(ex);
                        break;
                }
            }
            finally
            {
                EAAccessoryManager.SharedAccessoryManager.UnregisterForLocalNotifications();
                connectionObserver.Dispose();
            }
            
            return await tcs.Task;
        }
    }
}
