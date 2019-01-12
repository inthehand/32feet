//-----------------------------------------------------------------------
// <copyright file="DevicePicker.iOS.cs" company="In The Hand Ltd">
//   Copyright (c) 2018 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using ExternalAccessory;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Devices.Enumeration
{
    partial class DevicePicker
    {
        internal static EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static DevicePicker s_current;
        internal EASession _device;
        
        private Task<DeviceInformation> DoPickSingleDeviceAsync()
        {
            s_current = this;

            // TODO: register EAAccessoryManager notifications so handle can be triggered when device picked
            EAAccessoryManager.SharedAccessoryManager.RegisterForLocalNotifications();
            EAAccessoryManager.SharedAccessoryManager.AddObserver(EAAccessoryManager.DidConnectNotification, global::Foundation.NSKeyValueObservingOptions.New, (c) => {
                global::System.Diagnostics.Debug.WriteLine(c.NewValue.ToString());
            });
            EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPicker(null, (e)=> {
                if (e != null)
                {
                    global::System.Diagnostics.Debug.WriteLine(e.Code);
                }
            });

             return Task.Run<DeviceInformation>(() =>
            {                
                s_handle.WaitOne();

                return Task.FromResult<DeviceInformation>(null);
            });
        }

        

    }
    
}