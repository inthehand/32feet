// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using ConsoleMenuTesting;

namespace DeviceMenuTesting
{
    class RemoteConsole : IAuxConsole
    {
        public const int Port = 0x616c; //=24940
        const string ActiveSync_PcHostname = "PPP_PEER";
        //
        readonly MenuSystem _menus;
        //
        readonly Stream _peer;
        readonly List<IDisposable> _thingsToKeepAlive = new List<IDisposable>();
        readonly TextWriter _wtr;

        internal RemoteConsole(MenuSystem menuSystem)
            : this(ActiveSync_PcHostname, menuSystem)
        {
        }

        internal RemoteConsole(string hostname, MenuSystem menuSystem)
        {
            _menus = menuSystem;
            TcpClient cli;
            try {
                cli = new TcpClient(hostname, Port);
            } catch (SocketException ex) {
                menuSystem.WriteLine("Connect failed: " + ex.ErrorCode);
                return;
            }
            _peer = cli.GetStream();
            // Arrrgggh NETCF's TcpClient Disposes the socket in it Finalizer!!
            _thingsToKeepAlive.Add(cli);
            _wtr = new StreamWriter(_peer);
            ThreadPool.QueueUserWorkItem(ThreadRunner);
        }


        void ThreadRunner(object state)
        {
            var rdr = new StreamReader(_peer);
            while (true) {
                string line;
                try {
                    line = rdr.ReadLine();
                } catch (EndOfStreamException) {
                    break;
                }
                var splits = SplitAtFirst(line, Commands.Separator);
                switch (splits[0]) {
                    case Commands.MENUOPTION:
                        var option = SelectOrDefault(_menus.Options, splits[1]);
                        if (option == null) {
                            Send(Commands.Error, "Unknown menu option: " + splits[1]);
                        } else {
                            ThreadPool.QueueUserWorkItem(MenuOption_Runner, option);
                            Send(Commands.RUNNING, splits[1]);
                        }
                        break;
                    default:
                        Send(Commands.Error, "Unsupported command: " + splits[0]);
                        Trace.Assert(false, "Unsupported command: " + splits[0]);
                        break;
                }
            }//while
        }

        private object SelectOrDefault(IList<Option> list, string optionName)
        {
            foreach (var cur in list) {
                if (cur.name == optionName) {
                    return cur;
                }
            }//for
            return null;
        }

        static string[] SplitAtFirst(string this_, char sepa)
        {
            int idx = this_.IndexOf(sepa);
            string a = this_.Substring(0, idx);
            string b = (idx < this_.Length)
                ? this_.Substring(idx + 1)
                : string.Empty;
            return new string[] { a, b };
        }

        void MenuOption_Runner(object state)
        {
            var option = (Option)state;
            try {
                option.Invoke();
            } catch (Exception ex) {
                var txt = ex.ToString();
                using (var rdr = new StringReader(txt)) {
                    Send(Commands.EXCEPTIONSTRING, rdr, Commands.EXCEPTIONSTRINGEND);
                }
            }
        }

        //------------------------
        static class Commands
        {
            public const char Separator = ':';
            public const string EXCEPTIONSTRING = "EXCEPTIONSTRING";
            public const string EXCEPTIONSTRINGEND = "EXCEPTIONSTRINGEND";
            public const string TEXT = "TEXT";
            public const string Error = "Error";
            public const string MENUOPTION = "MENUOPTION";
            public const string RUNNING = "RUNNING";
        }


        //----------------------
        void Send(string command, string text)
        {
            lock (_wtr) {
                _wtr.WriteLine(command + Commands.Separator + text);
                _wtr.Flush();
            }
        }

        void Send(string command, StringReader text, string commandEnd)
        {
            lock (_wtr) {
                string line;
                while ((line = text.ReadLine()) != null) {
                    _wtr.WriteLine(command + Commands.Separator + line);
                }//whle
                _wtr.WriteLine(commandEnd + Commands.Separator);
                _wtr.Flush();
            }
        }

        void IAuxConsole.AppendText(string txt)
        {
            lock (_wtr) {
                _wtr.WriteLine(Commands.TEXT + Commands.Separator + txt);
                _wtr.Flush();
            }
        }

    }
}
