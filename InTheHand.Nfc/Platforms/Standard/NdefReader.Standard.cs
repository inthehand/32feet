// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Nfc;

partial class NdefReader
{
    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }

    public void Dispose()
    {
    }
}