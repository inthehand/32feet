using System;
using System.IO;
using InTheHand.Net.Sockets;
using System.Net;
using InTheHand.Net.Tests.ObexRequest;

namespace InTheHand.Net.Tests.Infra
{
    //sealed class SocketStreamAdapterAvailableTwoWayStream : SocketStreamAdapterNoAvailable
    //{
    //    TwoWayStream m_strm_TwoWayStream;
    //
    //    public SocketStreamAdapterAvailableTwoWayStream(TwoWayStream strm)
    //        : base(strm)
    //    {
    //        m_strm_TwoWayStream = strm;
    //    }
    //
    //    public override int Available
    //    {
    //        get { return m_strm_TwoWayStream.HackAvailableForSocketAdapter; }
    //    }
    //}



    sealed class SocketStreamAdapterAvailablePropertyThrows : SocketStreamAdapterNoAvailable
    {
        public SocketStreamAdapterAvailablePropertyThrows(Stream strm)
            : base(strm)
        {
        }

        public override int Available
        {
            get { throw new NotSupportedException("Shouldn't be using Available!"); }
        }
    }


    sealed class SocketStreamAdapterAvailableOneByte : SocketStreamAdapterNoAvailable
    {
        public SocketStreamAdapterAvailableOneByte(Stream strm)
            : base(strm)
        {
        }

        public override int Available
        {
            get { return 1; }
        }
    }


    abstract class SocketStreamAdapterNoAvailable : SocketStreamIOAdapter
    {
        Stream m_strm;

        public SocketStreamAdapterNoAvailable(Stream strm)
            : base(strm)
        {
            m_strm = strm;
        }

        public override System.Net.EndPoint LocalEndPoint
        {
            get { return new FakeEndPoint(); }
        }

        public override System.Net.EndPoint RemoteEndPoint
        {
            get { return new FakeEndPoint(); }
        }

    }


    class FakeEndPoint : EndPoint
    {
    }
}
