// 32feet.NET - Personal Area Networking for .NET
//
// Net.Sockets.SocketError
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;

using System.Collections.Generic;
using System.Text;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Defines error codes for the <see cref="System.Net.Sockets.Socket"/> class.
    /// </summary>
    /// <remarks>Equivalent to System.Net.Sockets.SocketError in the full .NET Framework.</remarks>
    public enum SocketError
    {
#pragma warning disable 1591 // warning CS1591: Missing XML comment for publicly visible type or member '...'
        AccessDenied = 0x271d,
        AddressAlreadyInUse = 0x2740,
        AddressFamilyNotSupported = 0x273f,
        AddressNotAvailable = 0x2741,
        AlreadyInProgress = 0x2735,
        ConnectionAborted = 0x2745,
        ConnectionRefused = 0x274d,
        ConnectionReset = 0x2746,
        DestinationAddressRequired = 0x2737,
        Disconnecting = 0x2775,
        Fault = 0x271e,
        HostDown = 0x2750,
        HostNotFound = 0x2af9,
        HostUnreachable = 0x2751,
        InProgress = 0x2734,
        Interrupted = 0x2714,
        InvalidArgument = 0x2726,
        IOPending = 0x3e5,
        IsConnected = 0x2748,
        MessageSize = 0x2738,
        NetworkDown = 0x2742,
        NetworkReset = 0x2744,
        NetworkUnreachable = 0x2743,
        NoBufferSpaceAvailable = 0x2747,
        NoData = 0x2afc,
        NoRecovery = 0x2afb,
        NotConnected = 0x2749,
        NotInitialized = 0x276d,
        NotSocket = 0x2736,
        OperationAborted = 0x3e3,
        OperationNotSupported = 0x273d,
        ProcessLimit = 0x2753,
        ProtocolFamilyNotSupported = 0x273e,
        ProtocolNotSupported = 0x273b,
        ProtocolOption = 0x273a,
        ProtocolType = 0x2739,
        Shutdown = 0x274a,
        SocketError = -1,
        SocketNotSupported = 0x273c,
        Success = 0,
        SystemNotReady = 0x276b,
        TimedOut = 0x274c,
        TooManyOpenSockets = 0x2728,
        TryAgain = 0x2afa,
        TypeNotFound = 0x277d,
        VersionNotSupported = 0x276c,
        WouldBlock = 0x2733
    }
}
