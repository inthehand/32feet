// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.Headers.ObexResponseHeaders
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Obex.Headers
{
    public class ObexResponseHeaders : ObexHeaders
    {
        public uint ConnectionID
        {
            get => (uint)base.GetValue(ObexHeader.ConnectionID);
            set => base.Add(ObexHeader.ConnectionID, value);
        }

        public uint Count
        {
            get => (uint)base.GetValue(ObexHeader.Count);
            set => base.Add(ObexHeader.Count, value);
        }

        public string Description
        {
            get => base.GetValue(ObexHeader.Description).ToString();
            set => base.Add(ObexHeader.Description, value);
        }

        public string Http
        {
            get => base.GetValue(ObexHeader.Http).ToString();
            set => base.Add(ObexHeader.Http, value);
        }

        public string Name
        {
            get => base.GetValue(ObexHeader.Name).ToString();
            set => base.Add(ObexHeader.Name, value);
        }

        public byte[] Target
        {
            get => (byte[])base.GetValue(ObexHeader.Target);
            set => base.Add(ObexHeader.Target, value);
        }

        public string Type
        {
            get => base.GetValue(ObexHeader.Type).ToString();
            set => base.Add(ObexHeader.Type, value);
        }

        public uint Length
        {
            get => (uint)base.GetValue(ObexHeader.Length);
            set => base.Add(ObexHeader.Length, value);
        }

        public DateTimeOffset? Time
        {
            get
            {
                string isoDate = base.GetValue(ObexHeader.TimeIso8601).ToString();
                if (DateTimeOffset.TryParseExact(isoDate, "yyyyMMdd'T'HHmmss'Z'", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var dt))
                    return dt;

                return null;
            }
            set
            {
                if(value.HasValue)
                {
                    var str = value.Value.UtcDateTime.ToString("yyyyMMdd'T'HHmmss'Z'");
                    base.Add(ObexHeader.TimeIso8601, str);
                }
            }
        }
    }
}
