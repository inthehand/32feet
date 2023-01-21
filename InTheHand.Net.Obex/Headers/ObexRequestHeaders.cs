// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.Headers.ObexRequestHeaders
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Obex.Headers
{
    public class ObexRequestHeaders : ObexHeaders
    {
        public uint ConnectionID
        {
            get => (uint)GetValue(ObexHeader.ConnectionID);
            set => Add(ObexHeader.ConnectionID, value);
        }

        public uint Count
        {
            get => (uint)GetValue(ObexHeader.Count);
            set => Add(ObexHeader.Count, value);
        }

        public string Description
        {
            get => GetValue(ObexHeader.Description).ToString();
            set => Add(ObexHeader.Description, value);
        }

        public string Http
        {
            get => GetValue(ObexHeader.Http).ToString();
            set => Add(ObexHeader.Http, value);
        }

        public string Name
        {
            get => GetValue(ObexHeader.Name).ToString();
            set => Add(ObexHeader.Name, value);
        }

        public byte[] Target
        {
            get => (byte[])GetValue(ObexHeader.Target);
            set => Add(ObexHeader.Target, value);
        }

        public string Type
        {
            get => GetValue(ObexHeader.Type).ToString();
            set => Add(ObexHeader.Type, value);
        }

        public uint Length
        {
            get => (uint)GetValue(ObexHeader.Length);
            set => Add(ObexHeader.Length, value);
        }

        public DateTimeOffset? Time
        {
            get
            {
                string isoDate = GetValue(ObexHeader.Time).ToString();
                if (DateTimeOffset.TryParseExact(isoDate, "yyyyMMdd'T'HHmmss'Z'", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var dt))
                    return dt;

                return null;
            }
            set
            {
                if(value.HasValue)
                {
                    var str = value.Value.UtcDateTime.ToString("yyyyMMdd'T'HHmmss'Z'");
                    Add(ObexHeader.Time, str);
                }
            }
        }
    }
}
