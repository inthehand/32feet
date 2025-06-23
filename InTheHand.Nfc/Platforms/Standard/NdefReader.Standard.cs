// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace InTheHand.Nfc;

partial class NdefReader
{
    private static Task<bool> PlatformGetAvailability() => Task.FromResult(false);

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }

    private Task PlatformWriteAsync(NdefMessage message, CancellationToken cancellationToken)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }
    
    private Task PlatformMakeReadOnlyAsync(CancellationToken cancellationToken)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }

    private void Dispose(bool disposing)
    {
        _disposed = true;
    }
}