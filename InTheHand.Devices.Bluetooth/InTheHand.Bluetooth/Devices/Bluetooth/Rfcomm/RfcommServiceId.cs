//-----------------------------------------------------------------------
// <copyright file="RfcommServiceId.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an RFCOMM service ID.
    /// </summary>
    public sealed partial class RfcommServiceId
    {       
        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Serial Port (SPP) service (with short id 0x1101).
        /// </summary>
        public static RfcommServiceId SerialPort
        {
            get
            {
                return FromShortId(0x1101);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Dial-up Networking (DUN) service (with short id 0x1103).
        /// </summary>
        public static RfcommServiceId DialupNetworking
        {
            get
            {
                return FromShortId(0x1103);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Synchronization service (with short id 0x1104).
        /// </summary>
        public static RfcommServiceId IrMCSync
        {
            get
            {
                return FromShortId(0x1104);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized OBEX Object Push service (with short id 0x1105).
        /// </summary>
        public static RfcommServiceId ObexObjectPush
        {
            get
            {
                return FromShortId(0x1105);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized OBEX File Transfer service (with short id 0x1106).
        /// </summary>
        public static RfcommServiceId ObexFileTransfer
        {
            get
            {
                return FromShortId(0x1106);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Headset Profile (HSP) (with short id 0x1108).
        /// </summary>
        public static RfcommServiceId Headset
        {
            get
            {
                return FromShortId(0x1108);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Audio/Video Remote Control Profile (AVRCP) (with short id 0x110E).
        /// </summary>
        public static RfcommServiceId AVRemoteControl
        {
            get
            {
                return FromShortId(0x110E);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Basic Imaging service (with short id 0x111A).
        /// </summary>
        public static RfcommServiceId BasicImaging
        {
            get
            {
                return FromShortId(0x111A);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Hands-free Profile (HFP) (with short id 0x111E).
        /// </summary>
        public static RfcommServiceId Handsfree
        {
            get
            {
                return FromShortId(0x111E);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Basic Printing service (with short id 0x1118).
        /// </summary>
        public static RfcommServiceId DirectPrinting
        {
            get
            {
                return FromShortId(0x1118);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Basic Printing service (with short id 0x1119).
        /// </summary>
        public static RfcommServiceId ReferencePrinting
        {
            get
            {
                return FromShortId(0x1119);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the printing status service for the Basic Printing Profile (BPP) (with short id 0x1123).
        /// </summary>
        public static RfcommServiceId PrintingStatus
        {
            get
            {
                return FromShortId(0x1123);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Hardcopy Cable Replacement Profile (HCRP) for printing (with short id 0x1126).
        /// </summary>
        public static RfcommServiceId HcrPrint
        {
            get
            {
                return FromShortId(0x1126);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Hardcopy Cable Replacement Profile (HCRP) for scanning (with short id 0x1127).
        /// </summary>
        public static RfcommServiceId HcrScan
        {
            get
            {
                return FromShortId(0x1127);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized SIM Access Profile (SAP) (with short id 0x112D).
        /// </summary>
        public static RfcommServiceId SimAccess
        {
            get
            {
                return FromShortId(0x112D);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Phone Book Access (PCE) service (with short id 0x112E).
        /// </summary>
        public static RfcommServiceId PhoneBookAccessPce
        {
            get
            {
                return FromShortId(0x112E);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Phone Book Access (PSE) service (with short id 0x112F).
        /// </summary>
        public static RfcommServiceId PhoneBookAccessPse
        {
            get
            {
                return FromShortId(0x112F);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Generic File Transfer service (with short id 0x1202).
        /// </summary>
        public static RfcommServiceId GenericFileTransfer
        {
            get
            {
                return FromShortId(0x1202);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Health Device Source service for Health Device Profile (HDP) (with short id 0x1401).
        /// </summary>
        public static RfcommServiceId HealthDeviceSource
        {
            get
            {
                return FromShortId(0x1401);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object corresponding to the service id for the standardized Health Device Sink service for Health Device Profile (HDP) (with short id 0x1402).
        /// </summary>
        public static RfcommServiceId HealthDeviceSink
        {
            get
            {
                return FromShortId(0x1402);
            }
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object from a 32-bit service id.
        /// </summary>
        /// <param name="shortId">The 32-bit service id.</param>
        /// <returns>The <see cref="RfcommServiceId"/> object.</returns>
        public static RfcommServiceId FromShortId(uint shortId)
        {
            return FromShortIdImpl(shortId);
        }

        /// <summary>
        /// Creates a <see cref="RfcommServiceId"/> object from a 128-bit service id.
        /// </summary>
        /// <param name="uuid">The 128-bit service id.</param>
        /// <returns>The <see cref="RfcommServiceId"/> object.</returns>
        public static RfcommServiceId FromUuid(Guid uuid)
        {
            return FromUuidImpl(uuid);
        }
        

        /// <summary>
        /// Implicit conversion from RfcommServiceId to Guid.
        /// </summary>
        /// <param name="id"></param>
        public static implicit operator Guid(RfcommServiceId id)
        {
            return id.Uuid;
        }

        /// <summary>
        /// Implicit conversion from Guid to RfcommServiceId
        /// </summary>
        /// <param name="uuid"></param>
        public static implicit operator RfcommServiceId(Guid uuid)
        {
            return RfcommServiceId.FromUuid(uuid);
        }


        /// <summary>
        /// Retrieves the 128-bit service id.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
            }
        }

        /// <summary>
        /// Converts the RfcommServiceId to a 32-bit service id if possible.
        /// </summary>
        /// <returns>Returns the 32-bit service id if the RfcommServiceId represents a standardized service.</returns>
        public uint AsShortId()
        {
            return AsShortIdImpl();
        }

        /// <summary>
        /// Converts the RfcommServiceId to a string.
        /// </summary>
        /// <returns>Returns the string representation of the 128-bit service id.</returns>
        public string AsString()
        {
            return AsStringImpl();
        }

        public override int GetHashCode()
        {
            return Uuid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            RfcommServiceId other = obj as RfcommServiceId;
            if(other != null)
            {
                return this.Uuid == other.Uuid;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return AsString();
        }
    }
}