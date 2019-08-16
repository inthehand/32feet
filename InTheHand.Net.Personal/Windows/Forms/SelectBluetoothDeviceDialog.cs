// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Windows.Forms.SelectBluetoothDeviceDialog
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the Microsoft Public License (Ms-PL) - see License.txt

#if ! NO_WINFORMS

//
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth.Msft;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides a form to select an available Bluetooth device.
    /// </summary>
    public class SelectBluetoothDeviceDialog : CommonDialog
    {
        #region Delegate defns
        internal delegate bool PFN_DEVICE_CALLBACK(IntPtr pvParam, ref BLUETOOTH_DEVICE_INFO pDevice);
        #endregion

#if WinXP
        private BLUETOOTH_SELECT_DEVICE_PARAMS dialogWin32;
#endif
        private readonly ISelectBluetoothDevice dialog;
        private BluetoothDeviceInfo device;
        bool _ShowDiscoverableOnly;
        Predicate<BluetoothDeviceInfo> _deviceFilter;
        readonly PFN_DEVICE_CALLBACK _msftFilterProxy;


        /// <summary>
        /// Initializes an instance of the <see cref="SelectBluetoothDeviceDialog"/> class.
        /// </summary>
        public SelectBluetoothDeviceDialog()
            : this(false)
        {
        }

        internal SelectBluetoothDeviceDialog(bool forceCustomDialog)
        {

#if WinXP
                _msftFilterProxy = MsftFilterProxy;
                dialogWin32 = new BLUETOOTH_SELECT_DEVICE_PARAMS();
                dialogWin32.Reset();
                dialog = dialogWin32;
#else
                Debug.Fail("Should use custom dialog on non-Win32!");
#endif

            ClassOfDevices = new System.Collections.Generic.List<ClassOfDevice>();
        }

        /// <summary>
        /// Resets the properties of the <see cref="SelectBluetoothDeviceDialog"/> to their default values.
        /// </summary>
        public override void Reset()
        {
            this.DeviceFilter = null;
            dialog.Reset();
        }


        /// <summary>
        /// Specifies a common dialog box.
        /// </summary>
        /// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
        /// <returns>true if the dialog box was successfully run; otherwise, false.</returns>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            AssertWindowsFormsThread();
            if (this._ShowDiscoverableOnly) {
                // ShowDiscoverableOnly is not supported by the Microsoft stack on desktop Windows.
                return false;
            }
            //
            if ((object)dialogWin32 != (object)dialog) {
                dialogWin32 = (BLUETOOTH_SELECT_DEVICE_PARAMS)dialog;
            }
            //set parent window
            dialogWin32.hwndParent = hwndOwner;

            //set class of device filters
            dialogWin32.SetClassOfDevices(ClassOfDevices.ToArray());

            bool success = NativeMethods.BluetoothSelectDevices(ref dialogWin32);

            if (success) {
                if (dialogWin32.cNumDevices > 0) {
                    device = new BluetoothDeviceInfo(new WindowsBluetoothDeviceInfo(dialogWin32.Device));
                }

                bool freed = NativeMethods.BluetoothSelectDevicesFree(ref dialogWin32);
            }

            return success;
        }

        private static void AssertWindowsFormsThread()
        {
            var hasMl = Application.MessageLoop;
            if (!hasMl) {
                Trace.WriteLine("WARNING: The SelectBluetoothDeviceDialog needs to be called on the UI thread.");
                Debug.Fail("The SelectBluetoothDeviceDialog needs to be called on the UI thread.");
            }
        }


        /// <summary>
        /// If TRUE, invokes the Add New Device Wizard.
        /// </summary>
        /// <remarks>Supported only on Windows XP/Vista with Microsoft stack.</remarks>
        public bool AddNewDeviceWizard
        {
            get
            {
                return dialog.AddNewDeviceWizard;
            }
            set
            {
                dialog.AddNewDeviceWizard = value;
            }
        }



        /// <summary>
        /// If TRUE, skips the Services page in the Add New Device Wizard.
        /// </summary>
        /// <remarks>Supported only on Windows XP/Vista with Microsoft stack.</remarks>
        public bool SkipServicesPage
        {
            get
            {
                return dialog.SkipServicesPage;
            }
            set
            {
                dialog.SkipServicesPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the information text.
        /// </summary>
        /// <remarks></remarks>
        public string Info
        {
            get
            {
                return dialog.Info;
            }
            set
            {
                dialog.Info = value;
            }
        }

        /// <summary>
        /// Array of class of devices to find.
        /// </summary>
        /// <remarks>Clear the collection to return all devices.</remarks>
        public System.Collections.Generic.List<ClassOfDevice> ClassOfDevices
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the selected Bluetooth device.
        /// </summary>
        public BluetoothDeviceInfo SelectedDevice
        {
            get
            {
                return device;
            }
        }



        /// <summary>
        /// If TRUE, authenticated devices are shown in the picker.
        /// </summary>
        public bool ShowAuthenticated
        {
            get
            {
                return dialog.ShowAuthenticated;
            }
            set
            {
                dialog.ShowAuthenticated = value;
            }
        }


        /// <summary>
        /// If TRUE, remembered devices are shown in the picker.
        /// </summary>
        public bool ShowRemembered
        {
            get
            {
                return dialog.ShowRemembered;
            }
            set
            {
                dialog.ShowRemembered = value;
            }
        }

        /// <summary>
        /// If TRUE, unknown devices are shown in the picker.
        /// </summary>
        public bool ShowUnknown
        {
            get
            {
                return dialog.ShowUnknown;
            }
            set
            {
                dialog.ShowUnknown = value;
            }
        }

        /// <summary>
        /// If TRUE, forces authentication before returning.
        /// </summary>
        /// <remarks></remarks>
        public bool ForceAuthentication
        {
            get
            {
                return dialog.ForceAuthentication;
            }
            set
            {
                dialog.ForceAuthentication = value;
            }
        }

        /// <summary>
        /// Set a function that will be called for each device
        /// that returns whether to include the device in the list or not.
        /// </summary>
        /// -
        /// <value>The function to call for each device.
        /// The function should returns <c>true</c> if the device is to be included or <c>false</c> if not.
        /// Pass <c>null</c> to the property to clear the filter function.
        /// </value>
        /// -
        /// <remarks>
        /// <para>The callback method is called for each device as it is 
        /// being added to the dialog box.  If the function returns <c>false</c> it 
        /// won't be added, otherwise it will be added and displayed. The 
        /// information about each device is provided as a <see cref="T:InTheHand.Net.Sockets.BluetoothDeviceInfo"/>
        /// instance which will contain all the information about the device 
        /// that the discovery process knows and will also include any 
        /// information from the remembered/authenticated/paired devices. 
        /// Note that prior to Bluetooth v2.1 a separate query has to be 
        /// carried out to find whether the device also has a name, so unless 
        /// both devices are v2.1 or later then it&apos;s likely that the 
        /// name won't be included in the first discovery. 
        /// <see href="http://32feet.codeplex.com/wikipage?title=DeviceName%20and%20Discovery"/>
        /// </para>
        /// </remarks>
        /// -
        /// <example>
        ///     '......
        ///     Dim dlg As New InTheHand.Windows.Forms.SelectBluetoothDeviceDialog()
        ///     dlg.DeviceFilter = AddressOf FilterDevice
        ///     Dim rslt As DialogResult = dlg.ShowDialog()
        ///     '...... 
        ///
        /// Shared Function FilterDevice(ByVal dev As BluetoothDeviceInfo) As Boolean
        ///     Dim rslt As DialogResult = MessageBox.Show("Include this device " &amp; dev.DeviceAddress.ToString &amp; " " &amp; dev.DeviceName, "FilterDevice", MessageBoxButtons.YesNo)
        ///     Dim ret As Boolean = (DialogResult.Yes = rslt)
        ///     Return ret
        /// End Function
        /// </example>
        public Predicate<BluetoothDeviceInfo> DeviceFilter
        {
            //public void SetDeviceFilter(Predicate<BluetoothDeviceInfo> filter)
            get { return this._deviceFilter; }
            set
            {
                this._deviceFilter = value;
                dialog.SetFilter(value, _msftFilterProxy);
            }
        }

        private bool MsftFilterProxy(IntPtr state, ref BLUETOOTH_DEVICE_INFO dev)
        {
            if (_deviceFilter == null) {
                return true;
            }
            return _deviceFilter(new BluetoothDeviceInfo(new WindowsBluetoothDeviceInfo(dev)));
        }


#endif
    }
}