// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.ObexResponseMessage
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Obex
{
    /// <preliminary/>
    public class ObexResponseMessage
    {
        public Headers.ObexResponseHeaders Headers { get; }

        public ObexStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode 
        { 
            get
            {
                // return true if success code and final flag
                if (((byte)StatusCode & 0xA0) == 0xA0)
                    return true;

                return false;
            }
        }
    }
}