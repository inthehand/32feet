//-----------------------------------------------------------------------
// <copyright file="DevicePicker.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-18 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
#if !UNITY
using System.Threading.Tasks;
#endif
#if WINDOWS_APP
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DevicePicker
    {
#if WINDOWS_PHONE_APP
        private DevicePickerDialog _dialog;
#elif WINDOWS_APP
        private Popup _popup;
#endif

#if WINDOWS_UWP || WIN32
        private IDevicePicker _picker;
#else
        private DevicePickerFilter _filter = new DevicePickerFilter();
        
#endif

        /// <summary>
        /// Creates a <see cref="DevicePicker"/> object.
        /// </summary>
        public DevicePicker()
        {
#if WIN32
            if (Type.GetType("Windows.Devices.Enumeration.DevicePicker, Windows, ContentType=WindowsRuntime") != null)
            {
                _picker = new DevicePickerUap();
            }
            else
            {
                _picker = new DevicePickerWin32();
            }
#elif WINDOWS_UWP
            _picker = new DevicePickerUap();
#endif
        }




        // <summary>
        // Indicates that the device picker was light dismissed by the user.
        // Light dismiss happens when the user clicks somewhere other than the picker UI and the picker UI disappears. 
        // On Windows Phone this indicates the Back button was pressed.
        // </summary>
        //internal event TypedEventHandler<DevicePicker, object> DevicePickerDismissed;

        // raises the DevicePickerDismissed event
        //internal void OnDevicePickerDismissed()
        //{
        //    DevicePickerDismissed?.Invoke(this, null);
        //}

        // <summary>
        // Indicates that the user selected a device in the picker.
        // </summary>
        //internal event EventHandler<DeviceSelectedEventArgs> DeviceSelected;

        // Raises the DeviceSelected event
        //internal void OnDeviceSelected(DeviceInformation device)
        //{
        //    DeviceSelected?.Invoke(this, new DeviceSelectedEventArgs() { SelectedDevice = device });
        //}

            /*
#if !UNITY
#if !WINDOWS_UWP
        private DevicePickerAppearance _appearance = new DevicePickerAppearance();
#endif


        /// <summary>
        /// Gets the colors of the picker.
        /// </summary>
        /// <value>The color of the picker.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
        /// </remarks>
        public DevicePickerAppearance Appearance
        {
            get
            {
#if WINDOWS_UWP || WIN32
                return _picker.Appearance;
#else
                return _appearance;
#endif
            }
        }
#endif
     */   

            
        /// <summary>
        /// Gets the filter used to choose what devices to show in the picker.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public DevicePickerFilter Filter
        {
            get
            {
#if WINDOWS_UWP || WIN32
                return _picker.Filter;
#else
                return _filter;
#endif
            }
        }

        /// <summary>
        /// Shows the picker UI and returns the selected device; does not require you to register for an event.
        /// </summary>
        /// <returns></returns>
/*
public Task<DeviceInformation> PickSingleDeviceAsync()
{
    return PickSingleDeviceAsync(new Rect(0,0,1,1), Placement.Default);
}

/// <summary>
/// Shows the picker UI and returns the selected device; does not require you to register for an event.
/// </summary>
/// <param name="selection">The rectangle from which you want the picker to fly out.
/// <para>Ignored on Phone platforms as dialog is presented full-screen and Windows Desktop where it is centered.</para></param>
/// <returns></returns>
public Task<DeviceInformation> PickSingleDeviceAsync(Rect selection)
{
    return PickSingleDeviceAsync(selection, Placement.Default);
}

/// <summary>
/// Shows the picker UI and returns the selected device; does not require you to register for an event.
/// </summary>
/// <param name="selection">The rectangle from which you want the picker to fly out.
/// <para>Ignored on Phone platforms as dialog is presented full-screen and Windows Desktop where it is centered.</para></param>
/// <param name="placement">The edge of the rectangle from which you want the picker to fly out.
/// <para>Ignored on Phone platforms as dialog is presented full-screen and Windows Desktop where it is centered.</para></param>
/// <returns></returns>
*/
#if !UNITY
        public async Task<DeviceInformation> PickSingleDeviceAsync()
        {
#if WINDOWS_UWP || WIN32
            return await _picker.PickSingleDeviceAsync();
#elif __ANDROID__ || __IOS__
            return await DoPickSingleDeviceAsync();

#elif WINDOWS_PHONE_APP
            _dialog = new DevicePickerDialog(this);
            Windows.UI.Xaml.Controls.ContentDialogResult result = await _dialog.ShowAsync();
            return _dialog.SelectedDevice;

#elif WINDOWS_APP
            _popup = new Popup();
            DevicePickerControl dpc = new DevicePickerControl(this, _popup);
            _popup.Child = dpc;
            _popup.HorizontalOffset = selection.X + selection.Width;
            _popup.VerticalOffset = selection.Y;
            _popup.IsLightDismissEnabled = true;
            _popup.IsOpen = true;
            return await Task.Run<DeviceInformation>(() => { return dpc.WaitForSelection(); });

#else
            return null;
#endif
        }
#else
        public DeviceInformation PickSingleDevice()
        {
            return _picker.PickSingleDevice();
        }
#endif
    }
}