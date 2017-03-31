using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Globalization;
using InTheHand.Net.Bluetooth;
using System.Diagnostics;

namespace ConsoleMenuTesting
{
    abstract class MenuSystem
    {
        protected ManualResetEvent quit;
        //==
        IList<Option> options = new List<Option>();
        IList<string> subMenuNames = new List<string>();


        protected MenuSystem()
        {
            Option.Console = this;
        }

        //---------------------
        public abstract void WriteLine(string msg);
        public abstract void Write(string msg);
        public abstract void WriteLine(object arg0);
        public abstract void WriteLine(string fmt, params object[] args);
        public abstract void Write(string fmt, params object[] args);

        //---------------------
        public abstract string ReadLine(string prompt);
        public abstract void Pause(string prompt);
        public abstract InTheHand.Net.BluetoothAddress ReadBluetoothAddress(string prompt);
        public abstract InTheHand.Net.BluetoothAddress ReadOptionalBluetoothAddress(string prompt);
        public abstract int ReadInteger(string prompt);
        public abstract int? ReadOptionalInteger(string prompt);
        public abstract int? ReadOptionalIntegerHexadecimal(string prompt);
        public abstract bool ReadYesNo(string prompt, bool defaultYes);
        public abstract bool? ReadYesNoCancel(string prompt, bool? defaultYes);
        public abstract Guid? ReadOptionalBluetoothUuid(string prompt, Guid? promptDefault);
        public abstract string GetFilename();
        //
        public abstract void UiInvoke(EventHandler dlgt);
        public abstract bool? InvokeRequired { get; }
        public abstract object InvokeeControl{ get; }
        //
        /// <summary>
        /// Run the menu system, may or may not be blocking (e.g. ConsoleMenu blocking, Forms not).
        /// </summary>
        public abstract void RunMenu();

        //---------------------
        protected internal IList<Option> Options { get { return options; } }
        protected IList<string> SubMenus { get { return subMenuNames; } }

        public void AddMenus(object menusHost)
        {
            ReflectGetMenuItems_(menusHost, options, subMenuNames);
        }

        public const string RootName = "root";

        static void ReflectGetMenuItems_(object menuItems, IList<Option> options, IList<string> subMenuNames)
        {
            if (menuItems == null)
                throw new ArgumentNullException("menuItems");
            Type type = menuItems.GetType();
            MethodInfo[] miArr = type.GetMethods(BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo cur in miArr) {
                if (cur.GetCustomAttributes(typeof(MenuItemAttribute), false).Length != 0) {
                    ParameterInfo[] pi = cur.GetParameters();
                    if (pi.Length != 0) {
                        throw new ArgumentException("Menu methods must take no parameters.");
                    }
                    //
                    object[] smaList = cur.GetCustomAttributes(typeof(SubMenuAttribute), false);
                    Debug.Assert(smaList.Length == 0 || smaList.Length == 1, "allowMultiple=false, but count=" + smaList.Length);
                    string subMenu = RootName;
                    if (smaList.Length > 0) {
                        SubMenuAttribute subMenuAttr = (SubMenuAttribute)smaList[0];
                        subMenu = subMenuAttr.MenuName;
                        if (!subMenuNames.Contains(subMenu)) {
                            subMenuNames.Add(subMenu);
                        }
                    }
                    //
                    options.Add(new Option(cur.Name, subMenu, cur, menuItems));
                }
            }//for
        }


        internal ManualResetEvent NewManualResetEvent(bool initialState)
        {
            ManualResetEvent old = quit;
            try {
                quit = new ManualResetEvent(initialState);
                return quit;
            } finally {
                if (old != null)
                    old.Close();
            }
        }
        //----------------
        /// <summary>
        /// Attempt to create a Bluetooth UUID from user input either in 128-bit 
        /// UUID form or integer short-form.
        /// </summary>
        /// <param name="line">The user input.</param>
        /// <param name="result"></param>
        /// <returns><see langword="true"/> if a valid UUID was input,
        /// <see langword="false"/> otherwise.
        /// </returns>
        protected bool BluetoothService_TryParseIncludingShortForm(string line, out Guid result)
        {
            if (Guid_TryParse(line, out result))
                return true;
            UInt32 u32;
#if !NETCF
            if (UInt32.TryParse(line, NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                    out u32)) {
                result = BluetoothService.CreateBluetoothUuid(u32);
                return true;
            }
            return false;
#else
            try {
                u32 = UInt32.Parse(line, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            } catch (FormatException) {
                return false;
            }
            result = BluetoothService.CreateBluetoothUuid(u32);
            return true;
#endif
        }

        protected bool Guid_TryParse(string line, out Guid result)
        {
            const int DigitsInUuid = 32;
            if (string.IsNullOrEmpty(line) || line.Length < DigitsInUuid) {
                // Check for length to not throw in the common case where we're
                // prompting for integer or UUID, and we don't want slow exception
                // throw in the (common) case where an integer is entered.
                goto error;
            }
            try {
                Guid val = new Guid(line);
                result = val;
                return true;
            } catch (FormatException) { // Just drop through to error case.
            }
        error:
            result = Guid.Empty;
            return false;
        }

        //----
        public TimeSpan? ReadTimeSecondsOptional(string prompt)
        {
            int? t = ReadOptionalInteger(prompt + " TimeSpan seconds");
            if (t == null)
                return null;
            else {
                TimeSpan ts = TimeSpan.FromSeconds(t.Value);
                return ts;
            }
        }

        //----
#if NO_APP_WINFORMS
        [Obsolete("NO_APP_WINFORMS")]
#endif
        internal static string GetFilenameWinForms(MenuSystem console)
        {
#if NO_APP_WINFORMS
            throw new NotSupportedException("NO_APP_WINFORMS");
#else
            string fileSource;
            try {
                EventHandler action = delegate {
                    var d = new System.Windows.Forms.OpenFileDialog();
                    if (d.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
                        return;
                    }
                    fileSource = d.FileName;
                };
                fileSource = null;
                console.UiInvoke(action);
                if (fileSource == null) {
                    return null;
                }
            } catch (NotSupportedException) {
#if !NETCF
                Trace.Fail("Why NotSupportedException on OpenFileDialog?!?");
#endif
                // NETCF on Standard
                fileSource = console.ReadLine("Which file");
            }
            return fileSource;
#endif
        }

    }
}
