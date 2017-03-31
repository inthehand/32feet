using System;

using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chat2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            using (TraceListener tl = StartOptionalLogging()) {
                Debug.WriteLine("Starting DEBUG at " + DateTime.Now);
                Application.Run(new Form1());
                Debug.WriteLine("Stopping DEBUG at " + DateTime.Now);
            }
        }

        static TraceListener StartOptionalLogging()
        {
            bool logging = false;
            if (logging) {
                var tl = new InTheHand.TextWriterTraceListener32f("32feet.log");
                Debug.Listeners.Add(tl);
                Debug.AutoFlush = true;
                return tl;
            } else {
                return null;
            }
        }

    }
}