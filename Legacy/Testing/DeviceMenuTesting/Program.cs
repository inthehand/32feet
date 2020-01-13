using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace DeviceMenuTesting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            using (System.Diagnostics.TraceListener tl = new TextWriterTraceListener("DMT.log")) {
                System.Diagnostics.Debug.Listeners.Add(tl);
                System.Diagnostics.Debug.AutoFlush = true;
                Debug.WriteLine("DeviceMenuTesting starting DEBUG at " + DateTime.Now);
                Application.Run(new Form1());
                Debug.WriteLine("DeviceMenuTesting stopping DEBUG at " + DateTime.Now);
            }
        }

#if NETCF && !FX3_5
        class TextWriterTraceListener : System.Diagnostics.TraceListener
        {
            readonly System.IO.TextWriter wtr;
            volatile bool disposed;

            public TextWriterTraceListener(string filename)
            {
                string pathname = System.IO.Path.Combine(GetCurrentFolder(), filename);
                wtr = System.IO.File.AppendText(pathname);
            }

            static string GetCurrentFolder()
            {
                string fullAppName = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
                string fullAppPath = System.IO.Path.GetDirectoryName(fullAppName);
                return fullAppPath;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposed)
                    return;
                disposed = true;
                try {
                    wtr.Close();
                } finally {
                    base.Dispose(disposing);
                }
            }

            public override void Flush()
            {
                if (disposed)
                    return;
                wtr.Flush();
                base.Flush();
            }

            public override void Write(string message)
            {
                if (disposed)
                    return;
                wtr.Write(message);
                if (System.Diagnostics.Debug.AutoFlush) {
                    Flush();
                }
            }

            public override void WriteLine(string message)
            {
                if (disposed)
                    return;
                wtr.WriteLine(message);
                if (System.Diagnostics.Debug.AutoFlush) {
                    Flush();
                }
            }
        }
#endif

    }
}