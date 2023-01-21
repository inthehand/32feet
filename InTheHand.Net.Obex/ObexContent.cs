// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.ObexClient
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Obex
{
    /// <preliminary/>
    public abstract class ObexContent
    {
        public Headers.ObexRequestHeaders Headers { get; }
    }

    /// <preliminary/>
    public class ByteArrayContent : ObexContent
    {
    }

    /// <preliminary/>
    public class StreamContent : ObexContent
    {

    }

    /// <preliminary/>
    public class StringContent : ObexContent
    {

    }
}