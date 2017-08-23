//-----------------------------------------------------------------------
// <copyright file="BluetoothClassOfDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Provides functionality to determine the Bluetooth Class Of Device (Bluetooth COD) information for a device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    /// <seealso cref="InTheHand.Net.Bluetooth.ClassOfDevice"/>
    public sealed partial class BluetoothClassOfDevice
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private Windows.Devices.Bluetooth.BluetoothClassOfDevice _classOfDevice;
                
        private BluetoothClassOfDevice(Windows.Devices.Bluetooth.BluetoothClassOfDevice classOfDevice)
        {
            _classOfDevice = classOfDevice;
        }

        public static implicit operator Windows.Devices.Bluetooth.BluetoothClassOfDevice(BluetoothClassOfDevice classOfDevice)
        {
            return classOfDevice._classOfDevice;
        }

        public static implicit operator BluetoothClassOfDevice(Windows.Devices.Bluetooth.BluetoothClassOfDevice classOfDevice)
        {
            return new BluetoothClassOfDevice(classOfDevice);
        }
#else
        private uint _rawValue;

        internal BluetoothClassOfDevice(uint rawValue)
        {
            _rawValue = rawValue;
        }
#endif

        /// <summary>
        /// Creates a BluetoothClassOfDevice object from a raw integer value representing the Major Class, Minor Class and Service Capabilities of the device.
        /// </summary>
        /// <param name="rawValue">The raw integer value from which to create the BluetoothClassOfDevice object.</param>
        /// <returns>A BluetoothClassOfDevice object.</returns>
        public static BluetoothClassOfDevice FromRawValue(uint rawValue)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Devices.Bluetooth.BluetoothClassOfDevice.FromRawValue(rawValue);
#else
            return new Bluetooth.BluetoothClassOfDevice(rawValue);
#endif
        }

        /// <summary>
        /// Gets the Bluetooth Class Of Device information, represented as an integer value.
        /// </summary>
        /// <value>The Bluetooth Class Of Device information, represented as a raw integer value.</value>
        public uint RawValue
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _classOfDevice.RawValue;
#else
                return _rawValue;
#endif
            }
        }

        /// <summary>
        /// Gets the Major Class code of the Bluetooth device.
        /// </summary>
        /// <value>One of the enumeration values that specifies the device's Major Class code.</value>
        public BluetoothMajorClass MajorClass
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (BluetoothMajorClass)((int)_classOfDevice.MajorClass);
#else
                return (BluetoothMajorClass)((_rawValue & 0x01f00) >> 8);
#endif
            }
        }

        /// <summary>
        /// Gets the Minor Class code of the Bluetooth device.
        /// </summary>
        /// <value>One of the enumeration values that specifies the device's Minor Class code.</value>
        public BluetoothMinorClass MinorClass
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (BluetoothMinorClass)((int)_classOfDevice.MinorClass);
#else
                return (BluetoothMinorClass)((_rawValue & 0xfc) >> 2);

#endif
            }
        }

        /// <summary>
        /// Gets the service capabilities of the device.
        /// </summary>
        public BluetoothServiceCapabilities ServiceCapabilities
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return (BluetoothServiceCapabilities)((int)_classOfDevice.ServiceCapabilities);
#else
                return (BluetoothServiceCapabilities)((_rawValue & 0xffe00) >>13);
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as BluetoothClassOfDevice;
            if (other != null && this.RawValue == other.RawValue)
                return true;

            return false;
        }

        /// <summary>
        /// Returns the Major/Minor device class as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
#if PCL
            // PCL is missing GetFields() but it doesn't matter since the PCL is only a placeholder
            return MinorClass.ToString();
#else
            string majorClassName = MajorClass.ToString();

            if (MajorClass != BluetoothMajorClass.Miscellaneous)
            {
                if(MinorClass == BluetoothMinorClass.Uncategorized)
                {
                    // return just the Major class
                    return majorClassName;
                }

#if WINDOWS_PHONE_APP
                foreach (FieldInfo fi in typeof(BluetoothMinorClass).GetRuntimeFields())
#else
                foreach (FieldInfo fi in typeof(BluetoothMinorClass).GetFields())
#endif
                {
                    string minorName = fi.Name;
                    if (minorName.StartsWith(majorClassName))
                    {
                        if ((int)fi.GetValue(null) == (int)MinorClass)
                        {
                            // return the correct Minor class
                            return minorName;
                        }
                    }
                }
            }

            // if it doesn't match a known major/minor class return the raw value
            return RawValue.ToString("X8");
#endif
        }
    }
}