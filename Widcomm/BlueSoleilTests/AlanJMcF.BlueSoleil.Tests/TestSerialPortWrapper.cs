using System;
using System.Text;
using System.Collections.Generic;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using InTheHand.Net.Bluetooth.BlueSoleil;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace InTheHand.Net.Tests.BlueSoleil
{

    class TestSerialPortWrapper : ISerialPortWrapper
    {
        volatile bool _disposed;
        Stream _strm = new MostlyErrorsStream();


        //--------
        #region ISerialPortWrapper Members

        event SerialDataReceivedEventHandler ISerialPortWrapper.DataReceived
        {
            add
            {
                //TODO DataReceived
            }
            remove { throw new NotImplementedException(); }
        }

        void ISerialPortWrapper.Close()
        {
            _disposed = true;
            _strm.Close();
            //throw new NotImplementedException();
        }

        System.IO.Stream ISerialPortWrapper.BaseStream
        {
            get { return _strm; }
        }

        int ISerialPortWrapper.BytesToRead
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("TestSerialPortWrapper");
                return 0;
            }
        }

        int ISerialPortWrapper.WriteBufferSize
        {
            get
            {
                return 100;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ISerialPortWrapper Members


        void ISerialPortWrapper.Open()
        {
        }

        Handshake ISerialPortWrapper.Handshake
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
            }
        }

        string ISerialPortWrapper.PortName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
            }
        }

        int ISerialPortWrapper.ReadBufferSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
            }
        }

        #endregion
    }
}