using System;
using System.Threading;
using System.Reflection;

namespace InTheHand.Net.Tests
{
    class Delegate2
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
        public static IAsyncResult BeginInvoke<TResult>(Func<TResult> dlgt, AsyncCallback asyncCallback, object state)
        {
            AsyncResult<TResult, Func<TResult>> ar = new AsyncResult<TResult, Func<TResult>>(asyncCallback, state, dlgt);
            WaitCallback runnerDlgt = _RunnerResult<TResult>;
            ThreadPool.QueueUserWorkItem(runnerDlgt, ar);
            return ar;
        }

        public static TResult EndInvoke<TResult>(Func<TResult> dlgt, IAsyncResult ar)
        {
            AsyncResult<TResult, Func<TResult>> ar2 = (AsyncResult<TResult, Func<TResult>>)ar;
            return ar2.EndInvoke();
        }

        public static IAsyncResult BeginInvoke(System.Threading.ThreadStart dlgt, AsyncCallback asyncCallback, object state)
        {
            AsyncNoResult<ThreadStart> ar = new AsyncNoResult<ThreadStart>(
                asyncCallback, state, dlgt);
            ThreadPool.QueueUserWorkItem(_RunnerNoResult, ar);
            return ar;
        }

        public static void EndInvoke(System.Threading.ThreadStart dlgt, IAsyncResult ar)
        {
            AsyncNoResult<ThreadStart> ar2 = (AsyncNoResult<ThreadStart>)ar;
            ar2.EndInvoke();
        }

        static void _RunnerNoResult(object state)
        {
            AsyncNoResult<ThreadStart> ar = (AsyncNoResult<ThreadStart>)state;
            try {
                ThreadStart dlgt = ar.BeginParameters;
                dlgt();
            } catch (Exception ex) {
                TargetInvocationException tex
                    = CreateTargetInvocationException(ex);
                ar.SetAsCompleted(tex, false);
                return;
            }
            ar.SetAsCompleted(null, false);
        }

        static void _RunnerResult<TResult>(object state)
        {
            AsyncResult<TResult, Func<TResult>> ar = (AsyncResult<TResult, Func<TResult>>)state;
            TResult result;
            try {
                Func<TResult> dlgt = ar.BeginParameters;
                result = dlgt();
            } catch (Exception ex) {
                System.Reflection.TargetInvocationException tex
                    = CreateTargetInvocationException(ex);
                ar.SetAsCompleted(tex, false);
                return;
            }
            ar.SetAsCompleted(result, false);
        }

    }

    //delegate TResult Func<TResult>();

}