//-----------------------------------------------------------------------
// <copyright file="DevicePickerAppearance.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.UI;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents the appearance of a device picker.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed class DevicePickerAppearance
    {
#if WINDOWS_UWP
        private Windows.Devices.Enumeration.DevicePickerAppearance _appearance;

        private DevicePickerAppearance(Windows.Devices.Enumeration.DevicePickerAppearance appearance)
        {
            _appearance = appearance;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePickerAppearance(DevicePickerAppearance a)
        {
            return a._appearance;
        }

        public static implicit operator DevicePickerAppearance(Windows.Devices.Enumeration.DevicePickerAppearance a)
        {
            return new DevicePickerAppearance(a);
        }
#else
        internal DevicePickerAppearance()
        {
#if WINDOWS_UWP
            _appearance = new Windows.Devices.Enumeration.DevicePickerAppearance();
#elif WINDOWS_PHONE_APP
            AccentColor = ((Windows.UI.Xaml.Media.SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources["PhoneAccentBrush"]).Color;
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneBackgroundColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["PhoneForegroundColor"];
#elif WINDOWS_APP
            AccentColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorControlAccentColor"];
            BackgroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowColor"];
            ForegroundColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemColorWindowTextColor"];
#endif
        }
#endif

        /// <summary>
        /// Gets and sets the accent color of the picker UI.
        /// </summary>
        public Color AccentColor { get; set; }

        /// <summary>
        /// Gets and sets the background color of the picker UI.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets and sets the foreground color of the picker UI.
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets and sets the accent color of the picker UI.
        /// </summary>
        public Color SelectedAccentColor { get; set; }

        /// <summary>
        /// Gets and sets the background color of the picker UI.
        /// </summary>
        public Color SelectedBackgroundColor { get; set; }

        /// <summary>
        /// Gets and sets the foreground color of the picker UI.
        /// </summary>
        public Color SelectedForegroundColor { get; set; }

        private string _title;
        /// <summary>
        /// The title of the picker UI.
        /// </summary>
        /// <remarks>For Windows Desktop apps this is used as the info text below the device list.</remarks>
        public string Title
        {
            get
            {
#if WINDOWS_UWP
                return _appearance.Title;
#else
                return _title;
#endif
            }
            set
            {
#if WINDOWS_UWP
                _appearance.Title = value;
#else
                _title = value;
#endif
            }
        }
    }
}