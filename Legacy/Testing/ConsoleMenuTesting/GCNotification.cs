// http://www.wintellect.com/CS/blogs/jeffreyr/archive/2009/12/22/receiving-notifications-garbage-collections-occur.aspx

using System;
using System.Collections.Generic;
using System.Threading;
namespace JRichter
{
    public static class GCNotification
    {
        private static Action<Int32> s_gcDone = null; // The event’s field

        public static event Action<Int32> GCDone
        {
            add
            {
                // If there were no registered delegates before, start reporting notifications now 
                if (s_gcDone == null) { new GenObject(0); new GenObject(2); }
                s_gcDone += value;
            }
            remove { s_gcDone -= value; }
        }

        private sealed class GenObject
        {
            private Int32 m_generation;
            public GenObject(Int32 generation) { m_generation = generation; }
            ~GenObject()
            { // This is the Finalize method
                // If this object is in the generation we want (or higher), 
                // notify the delegates that a GC just completed
                if (GC.GetGeneration(this) >= m_generation) {
                    Action<Int32> temp = Interlocked.CompareExchange(ref s_gcDone, null, null);
                    if (temp != null) temp(m_generation);
                }
                // Keep reporting notifications if there is at least one delegate
                // registered, the AppDomain isn't unloading, and the process 
                // isn’t shutting down
                if ((s_gcDone != null) &&
                   !AppDomain.CurrentDomain.IsFinalizingForUnload() &&
                   !Environment.HasShutdownStarted) {
                    // For Gen 0, create a new object; for Gen 2, resurrect the
                    // object & let the GC call Finalize again the next time Gen 2 is GC'd
                    if (m_generation == 0) new GenObject(0);
                    else GC.ReRegisterForFinalize(this);
                } else { /* Let the objects go away */ }
            }
        }


        //--------
        static void BeepAction(int g)
        {
            Console.Beep(g == 0 ? 800 : 8000, 200);
        }

        public static void StartBeeper()
        {
            GCNotification.GCDone += BeepAction;
        }

        public static void StopBeeper()
        {
            GCNotification.GCDone -= BeepAction;
        }
    }

}