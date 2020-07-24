// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.ObexWebResponse
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Net;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace InTheHand.Net
{
    /// <summary>
    /// Provides an OBEX implementation of the <see cref="WebResponse"/> class.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Are we!?!")]
    public class ObexWebResponse : WebResponse
    {
        private MemoryStream responseStream;

        internal ObexWebResponse(MemoryStream stream, WebHeaderCollection headers, ObexStatusCode code)
        {
            responseStream = stream;
            Headers = headers;
            StatusCode = code;
        }

        /// <summary>
        /// Gets the headers associated with this response from the server.
        /// </summary>
        public override WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets the length of the content returned by the request.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                string len = Headers["LENGTH"];
                if (len != null && len != string.Empty) {
                    return long.Parse(len);
                }
                return 0;
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the content type of the response.
        /// </summary>
        public override string ContentType
        {
            get
            {
                return Headers["TYPE"];
            }
            set
            {
            }
        }

        /// <summary>
        /// Returns a status code to indicate the outcome of the request.
        /// </summary>
        /// -
        /// <remarks><para>Note, if a error occurs locally then the status code
        /// <see cref="F:InTheHand.Net.ObexStatusCode.InternalServerError"/> is returned.
        /// Therefore that error code could signal local or remote errors.
        /// </para>
        /// </remarks>
        public ObexStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the stream used to read the body of the response from the server.
        /// </summary>
        /// -
        /// <returns>A <see cref="Stream"/> containing the body of the response.</returns>
        public override Stream GetResponseStream()
        {
            return responseStream;
        }

        /// <summary>
        /// Frees the resources held by the response.
        /// </summary>
        public override void Close()
        {
            if (responseStream != null) {
                responseStream.Close();
                responseStream = null;
            }
        }
    }
}
