using System;

namespace InTheHand.Bluetooth
{
    public class GattWriteRequestBusyException : Exception
    {
        public GattWriteRequestBusyException(string uuid) : base($"Unable to write to the characteristic with {uuid}.")
        {
        }
    }
}