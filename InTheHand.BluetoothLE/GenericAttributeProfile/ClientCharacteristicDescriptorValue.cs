using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    [Flags]
    public enum ClientCharacteristicDescriptorValue
    {
        None = 0,
        Notify = 1,
        Indicate = 2,
    }
}
