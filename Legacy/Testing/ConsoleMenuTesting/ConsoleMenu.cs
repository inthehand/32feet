using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using InTheHand.Net;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace ConsoleMenuTesting
{
    class ConsoleMenu : MenuSystem
    {
        protected volatile bool quitMenu;
        TextReader m_rdr;
        TextWriter m_wtr;
        string m_subMenu;
        bool m_backChosen;
        //
        const string promptArrow = ">";
        readonly int sizeOfBaseMenus; // Quit, Back etc

        public ConsoleMenu()
            : this(Console.In, Console.Out)
        {
        }


        public ConsoleMenu(TextReader rdr, TextWriter wtr)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Options.Add(new Option("Quit", null, new ThreadStart(Quit).Method, this));
            Options.Add(new Option("<-Back", null, new ThreadStart(Back).Method, this));
            sizeOfBaseMenus = Options.Count;
            //
            m_rdr = rdr;
            m_wtr = wtr;
        }

        [Conditional("DEBUG")]
        private void TestGetOriginalException()
        {
            Exception input, result;
            //
            Console.WriteLine("Test GetOriginalException 0");
            input = null;
            result = Option.GetOriginalException(input);
            //
            Console.WriteLine("Test GetOriginalException 1");
            input = new RankException("Outer.");
            result = Option.GetOriginalException(input);
            //
            Console.WriteLine("Test GetOriginalException 2");
            input = new TargetInvocationException(null);
            result = Option.GetOriginalException(input);
            //
            Console.WriteLine("Test GetOriginalException 3");
            input = new TargetInvocationException("Outer", new RankException("Inner."));
            result = Option.GetOriginalException(input);
            //
            Console.WriteLine("Test GetOriginalException 4");
            input = new TargetInvocationException(new RankException("Inner."));
            result = Option.GetOriginalException(input);
            //
            Console.WriteLine("Test GetOriginalException DONE");
        }

        private void AddSubMenuMenus()
        {
            int idx = sizeOfBaseMenus;
            foreach (string curSubMenu in SubMenus) {
#if false // ADD_SUB_MENUS_AT_BOTTOM
                Options.Add(new OptionSubMenu(curSubMenu, MenuSubMenu, this));
#else
                Options.Insert(idx++, new OptionSubMenu(curSubMenu, MenuSubMenu, this));
#endif
            }
        }

        //--------

        public override void RunMenu()
        {
            AddSubMenuMenus();
            //
            TestGetOriginalException();
            //
            try {
                m_backChosen = true; //reset for init
                while (!quitMenu) {
                    if (m_backChosen) {
                        m_subMenu = "root";
                    }
                    m_backChosen = false;
                    //
                    IList<Option> shownOptions = DisplayMenu(Options);
                    int item = ReadInteger("option");
                    if (item < 1 || item > shownOptions.Count) {
                        WriteLine("Not a menu item number");
                        continue;
                    }

                    Option selected = shownOptions[item - 1];
                    try {
                        try {
                            selected.EventHandlerInvoke(null, null);
                        } catch (TargetInvocationException tiex) {
                            Console.WriteLine("Original exception :"
                                + Environment.NewLine + tiex.InnerException);
                            throw tiex.InnerException;
                        }
                    } catch (EndOfStreamException) {
                        throw;
                    } catch (Exception ex) {
                        WriteLine("Exception: {0}", ex);
                        bool cont = ReadYesNo("Continue after that exception", true);
                        if (!cont) {
                            throw;
                        }
                    }
                }//while
            } catch (EndOfStreamException eosex) {
                Thread.Sleep(2000);
                if (quitMenu) {
                    Console.WriteLine("[suppressed: " + FirstLine(eosex) + "]");
                } else {
                    throw;
                }
            }
        }

        private string FirstLine(Exception eosex)
        {
            string t = eosex.ToString();
            using (TextReader rdr = new StringReader(t)) {
                return rdr.ReadLine();
            }
        }

        private IList<Option> DisplayMenu(IList<Option> options)
        {
            int menuNum = 0;
            IList<Option> shown = new List<Option>();
            for (int i = 0; i < options.Count; ++i) {
                Option cur = options[i];
                if (cur.SubMenu == null
                        || cur.SubMenu == m_subMenu) {
                    WriteLine("{0,2} -- {1}", menuNum + 1, cur.name);
                    shown.Add(cur);
                    ++menuNum;
                } else {
                    //DEBUG
                }
            }//for
            return shown;
        }

        void Quit()
        {
            bool yes = this.ReadYesNo("Quit", false);
            if (!yes) return;
            quitMenu = true;
        }

        void Back()
        {
            m_backChosen = true;
        }

        void MenuSubMenu(string curSubMenu)
        {
            m_subMenu = curSubMenu;
        }

        void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Does this allow finalizers to run?
            quitMenu = true;
            if (quit != null)
                quit.Set();
            MemoryBarrier();
            Console.WriteLine("Letting Finalizers run...");
            BluetoothTesting.RunFinalizersAfterGc_();
            //
            //// TODO Console.WriteLine("Nulling peer stream.");
            //peer = null;
            //Console.WriteLine("Letting Finalizers run...");
            //BluetoothTesting.RunFinalizersAfterGc_();
            if (e.SpecialKey != ConsoleSpecialKey.ControlBreak) // May not cancel it!
                e.Cancel = true;
        }

        private static void MemoryBarrier()
        {
            Thread.MemoryBarrier();
        }

        //--------
        bool _newlineBeforeNextOutput;

        public override void WriteLine(string msg)
        {
            if (_newlineBeforeNextOutput) m_wtr.WriteLine();
            _newlineBeforeNextOutput = false;
            m_wtr.WriteLine(msg);
        }
        public override void Write(string msg)
        {
            if (_newlineBeforeNextOutput) m_wtr.WriteLine();
            _newlineBeforeNextOutput = false;
            m_wtr.Write(msg);
        }

        public override void WriteLine(object arg0)
        {
            if (_newlineBeforeNextOutput) m_wtr.WriteLine();
            _newlineBeforeNextOutput = false;
            m_wtr.WriteLine(arg0);
        }

        public override void WriteLine(string fmt, params object[] args)
        {
            if (_newlineBeforeNextOutput) m_wtr.WriteLine();
            _newlineBeforeNextOutput = false;
            m_wtr.WriteLine(fmt, args);
        }
        public override void Write(string fmt, params object[] args)
        {
            if (_newlineBeforeNextOutput) m_wtr.WriteLine();
            _newlineBeforeNextOutput = false;
            m_wtr.Write(fmt, args);
        }

        //--------
        public override string ReadLine(string prompt)
        {
            Write(prompt + promptArrow);
            string line = m_rdr.ReadLine();
            if (line == null)
                throw new EndOfStreamException();
            return line;
        }

        public override int ReadInteger(string prompt)
        {
            Write(prompt);
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                int result;
                if (int.TryParse(line, out result))
                    return result;
                Write("Invalid number");
            }
        }

        public override int? ReadOptionalInteger(string prompt)
        {
            Write(prompt);
            Write(" (optional)");
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                int result;
                if (int.TryParse(line, out result))
                    return result;
                else
                    return null;
            }
        }

        public override int? ReadOptionalIntegerHexadecimal(string prompt)
        {
            Write(prompt);
            Write(" (optional)");
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                int result;
                if (int.TryParse(line, NumberStyles.HexNumber, null, out result))
                    return result;
                else
                    return null;
            }
        }


        public override InTheHand.Net.BluetoothAddress ReadBluetoothAddress(string prompt)
        {
            return ReadBluetoothAddress(prompt, false);
        }

        public override InTheHand.Net.BluetoothAddress ReadOptionalBluetoothAddress(string prompt)
        {
            return ReadBluetoothAddress(prompt, true);
        }

        InTheHand.Net.BluetoothAddress ReadBluetoothAddress(string prompt, bool optional)
        {
            Write(prompt);
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                if (optional && line.Length == 0) {
                    return null;
                }
                BluetoothAddress result;
                if (BluetoothAddress.TryParse(line, out result))
                    return result;
                Write("Invalid address");
            }
        }

        public override void Pause(string prompt)
        {
            Write(prompt);
            Write(promptArrow);
            _newlineBeforeNextOutput = true;
            string line = m_rdr.ReadLine();
            if (line == null)
                throw new EndOfStreamException();
        }

        public override bool ReadYesNo(string prompt, bool defaultYes)
        {
            bool? result = ReadYesNoCancel_(false, prompt, defaultYes);
            return result.Value;
        }

        public override bool? ReadYesNoCancel(string prompt, bool? defaultYes)
        {
            return ReadYesNoCancel_(false, prompt, defaultYes);
        }

        bool? ReadYesNoCancel_(bool includeCancel, string prompt, bool? defaultYes)
        {
            Write(prompt);
            if (!includeCancel) {
                if (defaultYes.Value)
                    Write(" [Y/n]");
                else
                    Write(" [y/N]");
            } else {
                switch (defaultYes) {
                    case true:
                        Write(" [Y/n/c]");
                        break;
                    case false:
                        Write(" [y/N/c]");
                        break;
                    case null:
                        Write(" [y/n/C]");
                        break;
                }
            }
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                if (line.Length == 0)
                    return defaultYes;
                char val = line.Trim().ToUpper()[0];
                if (val == 'Y')
                    return true;
                else if (val == 'N')
                    return false;
                else if (val == 'C')
                    return null;
                Write("Need Y or N");
                if (includeCancel)
                    Write(" or C");
            }
        }

        public override Guid? ReadOptionalBluetoothUuid(string prompt, Guid? promptDefault)
        {
            Write(prompt + " Int32 or GUID");
            Write(" (optional)");
            if (promptDefault != null) {
                Write(" (default: " + promptDefault.ToString() + ")");
            }
            while (true) {
                Write(promptArrow);
                string line = m_rdr.ReadLine();
                if (line == null)
                    throw new EndOfStreamException();
                if (line.Length == 0)
                    return null;
                Guid result;
                if (BluetoothService_TryParseIncludingShortForm(line, out result))
                    return result;
                Write("Invalid UUID, re-enter");
            }
        }

        public override string GetFilename()
        {
            return GetFilenameWinForms(this);
        }

        public override void UiInvoke(EventHandler dlgt)
        {
            dlgt(null, EventArgs.Empty);
        }

        public override bool? InvokeRequired { get { return null; } }
        public override object InvokeeControl { get { return null; } }

    }
}
