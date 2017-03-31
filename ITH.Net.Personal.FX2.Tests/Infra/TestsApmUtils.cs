using System;
using System.IO;

namespace InTheHand.Net.Tests
{
    class TestsApmUtils
    {
        public static int SafeNoHangRead(Stream strm, byte[] buf, int offset, int length)
        {
            IAsyncResult ar = strm.BeginRead(buf, offset, length, null, null);
            SafeNoHangWait(ar, "Read");
            return strm.EndRead(ar);
        }

        public static void SafeNoHangEndWrite(Stream strm, IAsyncResult ar)
        {
            SafeNoHangWait(ar, "Write");
            strm.EndWrite(ar);
        }

        public static void SafeNoHangWaitShort(IAsyncResult ar, string opName)
        {
            SafeNoHangWait(ar, opName, TimeSpan.FromMilliseconds(100));
        }

        public static void SafeNoHangWait(IAsyncResult ar, string opName)
        {
            SafeNoHangWait(ar, opName, new TimeSpan(0, 0, 10));
        }

        public static void SafeNoHangWait(IAsyncResult ar, string opName, TimeSpan timeout)
        {
            if (ar.IsCompleted)
                return;
            bool signalled = ar.AsyncWaitHandle.WaitOne(TestsUtils.TimespanToMilliseconds(timeout), false);
            if (!signalled)
                throw new InvalidOperationException("Test timeout at " + opName);
        }


        internal static void SafeNotCompletesShort(IAsyncResult ar, string opName)
        {
            var timeout = TimeSpan.FromMilliseconds(100);
            SafeNotCompletes(ar, opName, timeout);
        }

        internal static void SafeNotCompletesMiddling(IAsyncResult ar, string opName)
        {
            var timeout = TimeSpan.FromMilliseconds(500);
            SafeNotCompletes(ar, opName, timeout);
        }

        internal static void SafeNotCompletes(IAsyncResult ar, string opName, TimeSpan timeout )
        {
            //
            bool signalled = ar.AsyncWaitHandle.WaitOne(TestsUtils.TimespanToMilliseconds(timeout), false);
            if (signalled)
                throw new InvalidOperationException("Unxpected completion by " + opName + "!");
        }
    }
}
