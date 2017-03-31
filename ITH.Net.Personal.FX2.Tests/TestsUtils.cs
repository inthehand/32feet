using System;

namespace InTheHand.Net.Tests
{
    class TestsUtils
    {
        public static int TimespanToMilliseconds(TimeSpan timeSpan)
        {
            double dd = timeSpan.TotalMilliseconds;
            long ll = (long)dd;
            return checked((int)ll);
        }

        public static void SetIsInNetcfTestRunner()
        {
            System.Reflection.Assembly assm = typeof(InTheHand.Net.BluetoothAddress).Assembly;
            Type ttu = assm.GetType("InTheHand.Net.TestUtilities");
            System.Diagnostics.Debug.Assert(ttu != null, "NO TestUtilities?!!");
            if (ttu != null) {
                System.Reflection.MethodInfo mi = ttu.GetMethod("SetIsInNetcfTestRunner",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                System.Diagnostics.Debug.Assert(mi != null, "NO SetIsInNetcfTestRunner?!!");
                if (mi != null) {
                    mi.Invoke(null, null);
                }
            }
        }
    }

}
