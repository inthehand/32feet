// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Exceptions
// 
// Copyright (c) 2018-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net
{
    internal static class Exceptions
    {
        public static NotImplementedException GetNotImplementedException()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
