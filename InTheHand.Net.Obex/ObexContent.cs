// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.ObexClient
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Obex
{
    public abstract class ObexContent
    {
        public Headers.ObexRequestHeaders Headers { get; }
    }

    public class ByteArrayContent : ObexContent
    {
    }

    public class StreamContent : ObexContent
    {

    }

    public class StringContent : ObexContent
    {

    }
}