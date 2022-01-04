// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.ObexHeader
// 
// Copyright (c) 2020-21 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net
{
    /// <summary>
    /// Known Target header values for Bluetooth defined services.
    /// </summary>
    public static class ObexTarget
    {
        /// <summary>
        /// Target header to connect to Messaging Access Service (Part of Messaging Access Profile).
        /// </summary>
        public static readonly Guid MessagingAccess = new Guid("bb582b40-420c-11db-b0de-0800200c9a66");

        /// <summary>
        /// Target header to connect to Messaging Notification Service (Part of Messaging Access Profile).
        /// </summary>
        public static readonly Guid MessagingNotification = new Guid("bb582b41-420c-11db-b0de-0800200c9a66");

        /// <summary>
        /// Target header to connect to Phone Book Access Profile.
        /// </summary>
        public static readonly Guid PhoneBookAccess = new Guid("796135f0-f0c5-11d8-0966-0800200c9a66");
    }
}
