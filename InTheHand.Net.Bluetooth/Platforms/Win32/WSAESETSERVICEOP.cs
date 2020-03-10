// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.WSAESETSERVICEOP
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth.Win32
{
    internal enum WSAESETSERVICEOP : int
    {
        /// <summary>
        /// Register the service. For SAP, this means sending out a periodic broadcast.
        /// This is an NOP for the DNS namespace.
        /// For persistent data stores, this means updating the address information. 
        /// </summary>
        RNRSERVICE_REGISTER = 0,

        /// <summary>
        ///  Remove the service from the registry.
        ///  For SAP, this means stop sending out the periodic broadcast.
        ///  This is an NOP for the DNS namespace.
        ///  For persistent data stores this means deleting address information. 
        /// </summary>
        RNRSERVICE_DEREGISTER,

        /// <summary>
        /// Delete the service from dynamic name and persistent spaces.
        /// For services represented by multiple CSADDR_INFO structures (using the SERVICE_MULTIPLE flag), only the specified address will be deleted, and this must match exactly the corresponding CSADDR_INFO structure that was specified when the service was registered 
        /// </summary>
        RNRSERVICE_DELETE,
    }
}
