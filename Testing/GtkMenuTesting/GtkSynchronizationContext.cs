// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Diagnostics;
using System.Threading;
using Gtk;

namespace GtkMenuTesting
{
    class GtkSynchronizationContext : SynchronizationContext
    {
        readonly Widget _c;
        readonly Thread _ctorThread;
        Thread _checkedThread;

        public GtkSynchronizationContext(Widget w)
        {
            _c = w;
            _ctorThread = Thread.CurrentThread;
            Application.Invoke(delegate {
                _checkedThread = Thread.CurrentThread;
                Debug.Assert(_checkedThread.Equals(_ctorThread), "GtkSynchronizationContext not created on UI thread!");
                if (!_checkedThread.Equals(_ctorThread)) {
                    Debug.WriteLine("GtkSynchronizationContext not created on UI thread!");
                    Console.WriteLine("GtkSynchronizationContext not created on UI thread!");
                }
            });
        }

        private void CheckCorrectInit()
        {
            if (!_checkedThread.Equals(_ctorThread)) {
                throw new InvalidOperationException("GtkSynchronizationContext not created on UI thread!");
            }
        }

        private bool InvokeRequired
        {
            get
            {
                bool same = _ctorThread == Thread.CurrentThread;
#if DEBUG
                if (!same) { // Invoke Required
                } else { // Not
                }
#endif
                return !same;
            }
        }

        /// <summary>
        /// When overridden in a derived class, dispatches an asynchronous message to
        /// a synchronization context.
        /// </summary>
        public override void Post(SendOrPostCallback d, object state)
        {
            CheckCorrectInit();
            Application.Invoke(delegate { d(state); });
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            CheckCorrectInit();
            if (InvokeRequired) {
                using (var ev = new ManualResetEvent(false)) {
                    SendOrPostCallback d2 = delegate {
                        try {
                            d(state);
                        } finally {
                            if (ev != null) ev.Set();
                        }
                    };
                    Post(d2, null);
                    ev.WaitOne();
                }//using
            } else {
                d(state);
            }
        }

    }
}
