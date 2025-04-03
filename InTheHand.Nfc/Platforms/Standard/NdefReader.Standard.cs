// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;

namespace InTheHand.Nfc;

partial class NdefReader
{
    private Task PlatformScanAsync(NdefScanOptions options = null)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }

    private void Dispose(bool disposing)
    {
        _disposed = true;
    }
}