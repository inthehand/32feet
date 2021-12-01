// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.BluetoothAddress
// 
// Copyright (c) 2003-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;

namespace InTheHand.Net
{
    /// <summary>
    /// Represents a Bluetooth device address.
    /// </summary>
    public partial class BluetoothAddress : IComparable<BluetoothAddress>, IEquatable<BluetoothAddress>, IFormattable, ICloneable
    {
        private readonly ulong _address;

        /// <summary>
        /// Provides a null Bluetooth address.
        /// </summary>
        public static readonly BluetoothAddress None = new BluetoothAddress(0UL);

        /// <summary>
        /// Initializes a new instance of the BluetoothAddress class with the specified address.
        /// </summary>
        /// <param name="address">Int64 representation of the address.</param>
        [Obsolete("Use unsigned long for widest compatibility", false)]
        public BluetoothAddress(long address)
        {
            _address = (ulong)address;
        }

        /// <summary>
        /// Initializes a new instance of the BluetoothAddress class with the specified address.
        /// </summary>
        /// <param name="address">UInt64 representation of the address.</param>
        public BluetoothAddress(ulong address)
        {
            _address = address;
        }

        /// <summary>
        /// Initializes a new instance of the BluetoothAddress class with the specified address.
        /// </summary>
        /// <param name="addressBytes">Address as 6 byte array.</param>
        public BluetoothAddress(byte[] addressBytes)
        {
            byte[] raw = addressBytes;

            if (addressBytes.Length < 8)
            {
                raw = new byte[8];
                addressBytes.CopyTo(raw, 0);
            }

            _address = BitConverter.ToUInt64(raw, 0);
        }

        /// <summary>
        /// Defines an implicit conversion of a BluetoothAddress to a <see cref="ulong"/>,
        /// </summary>
        public static implicit operator ulong(BluetoothAddress address)
        {
            return address._address;
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="ulong"/> to a BluetoothAddress,
        /// </summary>
        public static implicit operator BluetoothAddress(ulong address)
        {
            return new BluetoothAddress(address);
        }

        /// <summary>
        /// Returns the value as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return BitConverter.GetBytes(_address);
        }

        internal byte[] ToSixByteArray()
        {
            byte[] bytes = new byte[6];
            Buffer.BlockCopy(ToByteArray(), 0, bytes, 0, 6);
            return bytes;
        }

        /// <summary>
        /// Returns the Bluetooth address as a long integer.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use unsigned long for widest compatibility", false)]
        public long ToInt64()
        {
            return (long)_address;
        }

        /// <summary>
        /// Returns the Bluetooth address as an unsigned long integer.
        /// </summary>
        /// <returns></returns>
        public ulong ToUInt64()
        {
            return _address;
        }

        /// <summary>
        /// Compares two BluetoothAddress instances for equality.
        /// </summary>
        /// <param name="obj">The BluetoothAddress to compare with the current instance.</param>
        /// <returns>true if obj is a BluetoothAddress and equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            BluetoothAddress bta = obj as BluetoothAddress;

            if (bta != null)
            {
                return _address == bta._address;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => _address.GetHashCode();

        public int CompareTo(BluetoothAddress other)
        {
            if (other != null)
            {
                return _address.CompareTo(other._address);
            }

            return -1;
        }

        /// <summary>
        /// Returns a String representation of the value of this BluetoothAddress instance, according to the provided format specifier.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of this address. 
        /// The format parameter can be "N", "C", or "P". 
        /// If format is null or the empty string (""), "N" is used.</param>
        /// <param name="formatProvider">Ignored.</param>
        /// <returns>A String representation of the value of this BluetoothAddress.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }

        /// <summary>
        /// Returns a String representation of the value of this BluetoothAddress instance, according to the provided format specifier.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of this address. 
        /// The format parameter can be "N", "C", or "P". 
        /// If format is null or the empty string (""), "N" is used.</param>
        /// <returns>A String representation of the value of this BluetoothAddress.</returns>
        public string ToString(string format)
        {
            string separator;

            if (string.IsNullOrEmpty(format))
            {
                separator = string.Empty;
            }
            else
            {
                switch (format.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "8":
                    case "N":
                        separator = string.Empty;
                        break;
                    case "C":
                        separator = ":";
                        break;
                    case "P":
                        separator = ".";
                        break;
                    default:
                        throw new FormatException("Invalid format specified - must be either \"N\", \"C\", \"P\", \"\" or null.");
                }
            }

            byte[] data = ToByteArray();

            System.Text.StringBuilder result = new System.Text.StringBuilder(18);

            if (format == "8")
            {
                result.Append(data[7].ToString("X2") + separator);
                result.Append(data[6].ToString("X2") + separator);
            }

            result.Append(data[5].ToString("X2") + separator);
            result.Append(data[4].ToString("X2") + separator);
            result.Append(data[3].ToString("X2") + separator);
            result.Append(data[2].ToString("X2") + separator);
            result.Append(data[1].ToString("X2") + separator);
            result.Append(data[0].ToString("X2"));

            return result.ToString();
        }

        public override string ToString()
        {
            return ToString("N");
        }

        /// <summary>
        /// Converts the string representation of an address to it's <see cref="BluetoothAddress"/> equivalent.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="bluetoothString">A string containing an address to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="BluetoothAddress"/> equivalent to the address contained in s, if the conversion succeeded, or null (Nothing in Visual Basic) if the conversion failed.
        /// The conversion fails if the s parameter is null or is not of the correct format.</param>
        /// <returns>true if s is a valid Bluetooth address; otherwise, false.</returns>
        public static bool TryParse(string bluetoothString, out BluetoothAddress result)
        {
            Exception ex = ParseInternal(bluetoothString, out result);
            if (ex != null) return false;
            else return true;
        }

        /// <summary>
        /// Converts the string representation of a Bluetooth address to a new <see cref="BluetoothAddress"/> instance.
        /// </summary>
        /// <param name="bluetoothString">A string containing an address to convert.</param>
        /// <returns>New <see cref="BluetoothAddress"/> instance.</returns>
        /// <remarks>Address must be specified in hex format optionally separated by the colon or period character e.g. 000000000000, 00:00:00:00:00:00 or 00.00.00.00.00.00.</remarks>
        /// <exception cref="T:System.ArgumentNullException">bluetoothString is null.</exception>
        /// <exception cref="T:System.FormatException">bluetoothString is not a valid Bluetooth address.</exception>
        public static BluetoothAddress Parse(string bluetoothString)
        {
            Exception ex = ParseInternal(bluetoothString, out BluetoothAddress result);
            if (ex != null) throw ex;
            else return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Returned to caller.")]
        static Exception ParseInternal(string bluetoothString, out BluetoothAddress result)
        {
            const Exception Success = null;
            result = null;

            if (string.IsNullOrEmpty(bluetoothString))
            {
                return new ArgumentNullException(nameof(bluetoothString));
            }

            if (bluetoothString.IndexOf(":", StringComparison.Ordinal) > -1)
            {
                // assume address in standard hex format 00:00:00:00:00:00

                // check length
                if (bluetoothString.Length != 17)
                {
                    return new FormatException("bluetoothString is not a valid Bluetooth address.");
                }

                try
                {
                    byte[] babytes = new byte[8];
                    // split on colons
                    string[] sbytes = bluetoothString.Split(':');
                    for (int ibyte = 0; ibyte < 6; ibyte++)
                    {
                        // parse hex byte in reverse order
                        babytes[ibyte] = byte.Parse(sbytes[5 - ibyte], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                    result = new BluetoothAddress(babytes);
                    return Success;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else if (bluetoothString.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                // assume address in uri hex format 00.00.00.00.00.00
                // check length
                if (bluetoothString.Length != 17)
                {
                    return new FormatException("bluetoothString is not a valid Bluetooth address.");
                }

                try
                {
                    byte[] babytes = new byte[8];
                    // split on periods
                    string[] sbytes = bluetoothString.Split('.');
                    for (int ibyte = 0; ibyte < 6; ibyte++)
                    {
                        // parse hex byte in reverse order
                        babytes[ibyte] = byte.Parse(sbytes[5 - ibyte], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                    result = new BluetoothAddress(babytes);
                    return Success;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
            {
                // assume specified as long integer
                if ((bluetoothString.Length < 12) | (bluetoothString.Length > 16))
                {
                    return new FormatException("bluetoothString is not a valid Bluetooth address.");
                }
                try
                {
                    result = new BluetoothAddress(ulong.Parse(bluetoothString, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                    return Success;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }

        public bool Equals(BluetoothAddress other)
        {
            if (other is null)
                return false;

            return _address == other._address;
        }

        public static bool operator ==(BluetoothAddress x, BluetoothAddress y)
        {
            if ((x is null) && (y is null))
            {
                return true;
            }

            if ((x is object) && (y is object))
            {
                if (x._address == y._address)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an indication whether the values of two specified <see cref="BluetoothAddress"/> objects are not equal.
        /// </summary>
        /// -
        /// <param name="x">A <see cref="BluetoothAddress"/> or <see langword="null"/>.</param>
        /// <param name="y">A <see cref="BluetoothAddress"/> or <see langword="null"/>.</param>
        /// -
        /// <returns><c>true</c> if the value of the two instance is different;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(BluetoothAddress x, BluetoothAddress y)
        {
            return !(x == y);
        }

        internal static Guid HostToNetworkOrder(Guid hostGuid)
        {
            byte[] guidBytes = hostGuid.ToByteArray();

            BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt32(guidBytes, 0))).CopyTo(guidBytes, 0);
            BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(guidBytes, 4))).CopyTo(guidBytes, 4);
            BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BitConverter.ToInt16(guidBytes, 6))).CopyTo(guidBytes, 6);

            return new Guid(guidBytes);
        }

        public object Clone()
        {
            return new BluetoothAddress(_address);
        }
    }
}
