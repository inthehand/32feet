//-----------------------------------------------------------------------
// <copyright file="GattValueChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic or Descriptor value.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed class GattValueChangedEventArgs
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs _args;

        private GattValueChangedEventArgs(Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            _args = args;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs(GattValueChangedEventArgs args)
        {
            return args._args;
        }

        public static implicit operator GattValueChangedEventArgs(Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            return new GattValueChangedEventArgs(args);
        }

#elif __ANDROID__ || __UNIFIED__
        private byte[] _value;
        private DateTimeOffset _timestamp;
      
        internal GattValueChangedEventArgs(byte[] value, DateTimeOffset timestamp)
        {
            _value = value;
            _timestamp = timestamp;
        }
#endif

        /// <summary>
        /// Gets the new Characteristic Value.
        /// </summary>
        public byte[] CharacteristicValue
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _args.CharacteristicValue.ToArray();
#elif __ANDROID__ || __UNIFIED__
                return _value;
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Gets the time at which the system was notified of the Characteristic Value change.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _args.Timestamp;
#elif __ANDROID__ || __UNIFIED__
                return _timestamp;
#else
                return DateTimeOffset.MinValue;
#endif
            }
        }
    }
}