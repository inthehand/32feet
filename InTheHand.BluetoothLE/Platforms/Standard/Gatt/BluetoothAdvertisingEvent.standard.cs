using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        ushort GetAppearance()
        {
            return 0;
        }

        Guid[] GetUuids()
        {
            return new Guid[] { };
        }

        string GetName()
        {
            return string.Empty;
        }

        short GetRssi()
        {
            return 0;
        }

        IReadOnlyDictionary<ushort, byte[]> GetManufacturerData()
        {
            return new ReadOnlyDictionary<ushort, byte[]>(new Dictionary<ushort,byte[]>());
        }

        IReadOnlyDictionary<Guid, byte[]> GetServiceData()
        {
            return new ReadOnlyDictionary<Guid, byte[]>(new Dictionary<Guid, byte[]>());
        }
    }
}