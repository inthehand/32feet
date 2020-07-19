//-----------------------------------------------------------------------
// <copyright file="BluetoothUuid.cs" company="In The Hand Ltd">
//   Copyright (c) 2020 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Bluetooth.GenericAttributeProfile;
using System;

namespace InTheHand.Bluetooth
{
    public struct BluetoothUuid
    {
        private static readonly Guid BluetoothBase = new Guid(0x00000000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        private Guid _uuid;

        private BluetoothUuid(Guid uuid)
        {
            _uuid = uuid;
        }

        public static implicit operator Guid(BluetoothUuid uuid)
        {
            return uuid._uuid;
        }

        public static implicit operator BluetoothUuid(Guid uuid)
        {
            return new BluetoothUuid(uuid);
        }

        public static implicit operator BluetoothUuid(ushort uuid)
        {
            return FromShortId(uuid);
        }

        public static explicit operator ushort(BluetoothUuid uuid)
        {
            ushort? val = TryGetShortId(uuid);
            if (val.HasValue)
                return val.Value;

            return default;
        }

        public Guid Value
        {
            get
            {
                return _uuid;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is BluetoothUuid)
            {
                return Value == ((BluetoothUuid)obj).Value;
            }
            else if(obj is ushort)
            {
                ushort? shortVal = TryGetShortId(Value);

                if(shortVal.HasValue && shortVal.Value == (ushort)obj)
                {
                    return true;
                }
            }
            else if(obj is Guid)
            {
                return Value == (Guid)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static BluetoothUuid FromGuid(Guid uuid)
        {
            return new BluetoothUuid(uuid);
        }

        /// <summary>
        /// Returns the bluetooth device UUID from a short ID.
        /// </summary>
        /// <param name="shortId">The short ID.</param>
        /// <returns>Returns the UUID.</returns>
        public static BluetoothUuid FromShortId(ushort shortId)
        {
            byte[] guidBytes = BluetoothBase.ToByteArray();
            BitConverter.GetBytes(shortId).CopyTo(guidBytes, 0);
            return new Guid(guidBytes);
        }

        /// <summary>
        /// Attempts to get the short Bluetooth ID from a UUID.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <returns>Returns the short ID.</returns>
        public static ushort? TryGetShortId(Guid uuid)
        {
            var bytes = uuid.ToByteArray();
            var baseBytes = BluetoothBase.ToByteArray();
            bool match = true;
            for (int i = 4; i < 16; i++)
            {
                if (bytes[i] != baseBytes[i])
                {
                    match = false;
                    break;
                }
            }

            return match ? BitConverter.ToUInt16(bytes, 0) : default;
        }

        public static BluetoothUuid FromId(string uuidName)
        {
            if(uuidName.StartsWith("org.bluetooth.service"))
            {
                return GattServiceUuids.FromBluetoothUti(uuidName);
            }
            else if(uuidName.StartsWith("org.bluetooth.characteristic"))
            {
                return GattCharacteristicUuids.FromBluetoothUti(uuidName);
            }
            else if(uuidName.StartsWith("org.bluetooth.descriptor"))
            {
                return GattDescriptorUuids.FromBluetoothUti(uuidName);
            }

            return default;
        }

        public static BluetoothUuid GetService(string name)
        {
            if (Guid.TryParse(name, out Guid uuid))
            {
                return uuid;
            }
            else if (ushort.TryParse(name, out ushort alias))
            {
                return FromShortId(alias);
            }
            else
            {
                return GenericAttributeProfile.GattServiceUuids.FromBluetoothUti(name);
            }
        }

        public static BluetoothUuid GetCharacteristic(string name)
        {
            if (Guid.TryParse(name, out Guid uuid))
            {
                return uuid;
            }
            else if (ushort.TryParse(name, out ushort alias))
            {
                return FromShortId(alias);
            }
            else
            {
                return GenericAttributeProfile.GattCharacteristicUuids.FromBluetoothUti(name);
            }
        }

        public static BluetoothUuid GetDescriptor(string name)
        {
            if (Guid.TryParse(name, out Guid uuid))
            {
                return uuid;
            }
            else if (ushort.TryParse(name, out ushort alias))
            {
                return FromShortId(alias);
            }
            else
            {
                return GenericAttributeProfile.GattDescriptorUuids.FromBluetoothUti(name);
            }
        }
    }
}
