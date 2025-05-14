// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefErrorEventArgs
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Nfc;

public class NdefErrorEventArgs(Exception exception) : EventArgs
{
    public Exception Error => exception;
}