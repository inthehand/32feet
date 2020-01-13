using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using System.Threading;

namespace InTheHand.Net.Tests.Widcomm
{
    abstract class EventFirer
    {
        internal delegate void Foo(TestRfcommPort port, PORT_EV eventId);
        //
        protected TestRfcommPort port;

        public EventFirer(TestRfcommPort port)
        {
            this.port = port;
        }

        //public abstract void Run();
        internal abstract void Complete();

        #region Delegate End-/dlgt_BeginInvoke
        protected static IAsyncResult dlgt_BeginInvoke(Foo dlgt, TestRfcommPort port, PORT_EV eventId,
            AsyncCallback callback, object state)
        {
#if true || PocketPC
            FooParams args = new FooParams(dlgt, port, eventId);
            AsyncNoResult<FooParams> ar = new AsyncNoResult<FooParams>(callback, state, args);
            ThreadPool.QueueUserWorkItem(dlgt_Runner, ar);
            return ar;
#else
            arDlgt = dlgt.BeginInvoke(port, eventId, null, null); // NETCF-ok
#endif
        }

        protected static void dlgt_EndInvoke(Foo dlgt, IAsyncResult ar)
        {
#if true || PocketPC
            AsyncNoResult<FooParams> ar2 = (AsyncNoResult<FooParams>)ar;
            ar2.EndInvoke();
#else
            dlgt.EndInvoke(arDlgt); // NETCF-ok
#endif
        }

        sealed class FooParams
        {
            public readonly Foo dlgt;
            public readonly TestRfcommPort port;
            public readonly PORT_EV eventId;

            public FooParams(Foo dlgt, TestRfcommPort port, PORT_EV eventId)
            {
                this.dlgt = dlgt;
                this.port = port;
                this.eventId = eventId;
            }

        }

        static void dlgt_Runner(object state)
        {
            AsyncNoResult<FooParams> ar = (AsyncNoResult<FooParams>)state;
            try {
                ar.BeginParameters.dlgt(ar.BeginParameters.port, ar.BeginParameters.eventId);
            } catch (Exception ex) {
                ar.SetAsCompleted(ex, false);
                /* TODO TargetInvocationException tex
                        = new System.Reflection.TargetInvocationException(ex);
                    ar.SetAsCompleted(tex, false);
                 */
                return;
            }
            ar.SetAsCompleted(null, false);
        }
        #endregion
    }

    class OneEventFirer : EventFirer
    {
        const int NoTimeout = -1; // ?or int.MinValue
        protected int preEventDelayMilliseconds = NoTimeout;
        IAsyncResult arDlgt;

        Foo dlgt;

        public OneEventFirer(TestRfcommPort port)
            : base(port)
        {
        }

        public OneEventFirer(TestRfcommPort port, int preEventDelayMilliseconds)
            : base(port)
        {
            this.preEventDelayMilliseconds = preEventDelayMilliseconds;
        }

        public void Run(PORT_EV eventId)
        {
            dlgt = delegate(TestRfcommPort port2, PORT_EV eventId_) {
                if (preEventDelayMilliseconds != -1)
                    Thread.Sleep(preEventDelayMilliseconds);
                port2.NewEvent(eventId_);
            };
            arDlgt = dlgt_BeginInvoke(dlgt, port, eventId, null, null);
        }

        internal override void Complete()
        {
            dlgt_EndInvoke(dlgt, arDlgt); // any exceptions?
        }
    }

    class OneDataEventFirer : EventFirer
    {
        const int NoTimeout = -1; // ??int.MinValue
        protected int preEventDelayMilliseconds = NoTimeout;
        IAsyncResult arDlgt;

        Foo dlgt;

        public OneDataEventFirer(TestRfcommPort port)
            : base(port)
        {
        }

        public void Run(byte[] data)
        {
            dlgt = delegate(TestRfcommPort port2, PORT_EV eventId_) {
                if (preEventDelayMilliseconds != NoTimeout)
                    Thread.Sleep(preEventDelayMilliseconds);
                port2.NewReceive(data);
            };
            arDlgt = dlgt_BeginInvoke(dlgt, port, (PORT_EV)0, null, null);
        }

        internal override void Complete()
        {
            dlgt_EndInvoke(dlgt, arDlgt); // any exceptions?
        }
    }

    class OneEvent100msFirer : OneEventFirer
    {
        public OneEvent100msFirer(TestRfcommPort port)
            : base(port)
        {
            preEventDelayMilliseconds = 100;
        }
    }

    class FireOpenReceiveCloseEvents : EventFirer
    {
        IAsyncResult arDlgt, arDlgt2, arDlgt3;

        Foo dlgt = delegate(TestRfcommPort port2, PORT_EV eventId) {
            port2.NewEvent(eventId);
        };
        Foo dlgtDelay2 = delegate(TestRfcommPort port2, PORT_EV eventId) {
            Thread.Sleep(50);
            port2.NewEvent(eventId);
        };
        Foo dlgtDelay3 = delegate(TestRfcommPort port2, PORT_EV eventId) {
            Thread.Sleep(100);
            port2.NewReceive(new byte[] { (byte)'c' });
        };

        public FireOpenReceiveCloseEvents(TestRfcommPort port)
            : base(port)
        {
        }

        public void Run()
        {
            arDlgt = dlgt_BeginInvoke(dlgt, port, PORT_EV.CONNECTED, null, null);
            arDlgt2 = dlgt_BeginInvoke(dlgtDelay2, port, PORT_EV.CONNECT_ERR, null, null);
            arDlgt3 = dlgt_BeginInvoke(dlgtDelay3, port, PORT_EV.RING, null, null);
        }

        internal override void Complete()
        {
            dlgt_EndInvoke(dlgt, arDlgt); // any exceptions?
            dlgt_EndInvoke(dlgtDelay2, arDlgt2); // any exceptions?
            dlgt_EndInvoke(dlgtDelay3, arDlgt3); // any exceptions?
        }
    }

}
