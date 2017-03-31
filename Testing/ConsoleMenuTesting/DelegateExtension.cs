using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace ConsoleMenuTesting
{
    class DelegateExtension
    {
        internal static TargetInvocationException CreateTargetInvocationException(Exception ex)
        {
            // NETCFv2 TargetInvocationException..ctor(Exception)
            // does NOT set the InnerException property!!!  Use this
            // constructor instead.
            if (ex == null)
                throw new ArgumentNullException("ex");
            return new TargetInvocationException(ex.Message, ex);
        }

        //--------
        public static IAsyncResult BeginInvoke<T>(/*this*/ Action<T> dlgt,
            T obj, AsyncCallback callback, object @object)
        {
            AsyncResultNoResult ar = new AsyncResultNoResult(callback, @object);
            WaitCallback dlgt2 = delegate {
                try {
                    dlgt(obj);
                } catch (Exception ex) {
                    ar.SetAsCompleted(CreateTargetInvocationException(ex), false);
                    return;
                }
                ar.SetAsCompleted(null, false);
            };
#if THREAD_NOT_POOL && !NETCF
            var f = dlgt2.Method;
            ParameterizedThreadStart dlgt2TP = (ParameterizedThreadStart)
                Delegate.CreateDelegate(typeof(ParameterizedThreadStart), dlgt2.Target, f, true);
            //ThreadStart dlgt2T = (ThreadStart)
            //    Delegate.CreateDelegate(typeof(ThreadStart), dlgt2.Target, f, true);
            var t = new Thread(dlgt2TP);
            t.IsBackground = true;
            t.Start();
#else
            ThreadPool.QueueUserWorkItem(dlgt2);
#endif
            return ar;
        }

        [DebuggerNonUserCode]
        public static void EndInvoke<T>(/*this*/ Action<T> dlgt, IAsyncResult ar)
        {
            AsyncResultNoResult ar2 = (AsyncResultNoResult)ar;
            ar2.EndInvoke();
        }

    }

}
