// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J.McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SDDebug=System.Diagnostics.Debug;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConsoleMenuTesting;

namespace AndroidMenuTesting
{
    class DroidMenuSystem : NullMenuSystem
    {
        Activity1 _parent;
        List<Option> _options;
        internal List<Option> _manualOptions;

        //--
        public DroidMenuSystem (Activity1 parent)
        {
            _parent = parent;
            //
            labelPause = parent.FindViewById<TextView> (Resource.Id.textViewPause);
            itemUnpause = parent.FindViewById<Button> (Resource.Id.buttonUnpause);
            itemUnpause.Click += Unpause_Click;
            SetupPauseControls ();
        }

        #region Override MenuSystem
        //--
        const string NewLine = "\r\n";

        protected override void SafeSetText (string txt)
        {
            _parent.SafeSetText (txt);
        }

        public override void WriteLine (string msg)
        {
            _parent.SafeAppendText (msg + NewLine);
        }

        public override void Write (string msg)
        {
            _parent.SafeAppendText (msg);
        }

        //public override void WriteLine (object arg0);
        //public override void WriteLine (string fmt, params object[] args);
        //public override void Write (string fmt, params object[] args);

        //--------
        public override bool? ReadYesNoCancel (string prompt, bool? defaultYes)
        {
            return ReadYesNoCancel (prompt, defaultYes, true);
        }

        public override bool ReadYesNo (string prompt, bool defaultYes)
        {
            return ReadYesNoCancel (prompt, defaultYes, false).Value;
        }

        class IDIOCancelListener : Java.Lang.Object,
            IDialogInterfaceOnCancelListener
        {
            public Action Action{ get; set; }

            public void OnCancel (IDialogInterface dialog)
            {
                Action ();
            }

        }//class

        public  bool? ReadYesNoCancel (string prompt, bool? defaultYes,
           bool cancelButton)
        {
            bool? result = defaultYes;
            var w = new ManualResetEvent(false);
            Action exit = () => w.Set ();
            IDialogInterfaceOnCancelListener lsnrCancel = new IDIOCancelListener{
                Action = () => {
                    System.Diagnostics.Debug.Assert(result == defaultYes, "NOT equal '" + result + " and '" + defaultYes + "'");
                        exit();
                },
            };
            EventHandler dlgt = delegate {
                var bldr = new AlertDialog.Builder(_parent);
                bldr.SetMessage (prompt);
                bldr.SetCancelable (true);
                bldr.SetOnCancelListener (lsnrCancel);
                bldr.SetPositiveButton ("Yes", delegate {
                    result = true;
                    exit ();
                });
                bldr.SetNeutralButton ("No", delegate {
                    result = false;
                    exit ();
                });
                if (cancelButton) {
                    bldr.SetNegativeButton ("Cancel", delegate {
                        result = null;
                        exit ();
                    });
                }
                var dlg = bldr.Create ();
                dlg.Show ();
            };
            UiInvokeNoWait (dlgt);
            w.WaitOne ();
            return result;
        }

        //----
        public override string SafeReadLine (string prompt, string title)
        {
            string result = null;
            var w = new ManualResetEvent(false);
            Action exit = () => w.Set ();
            IDialogInterfaceOnCancelListener lsnrCancel = new IDIOCancelListener{
                Action = () => {
                    System.Diagnostics.Debug.Assert(result == null, "NOT null '" + result + "'");
                    exit();
                },
            };
            EventHandler dlgt = delegate {
                var bldr = new AlertDialog.Builder(_parent);
                bldr.SetTitle (title);
                bldr.SetMessage (prompt);
                // Set an EditText view to get user input final
                EditText input = new EditText(_parent);
                bldr.SetView (input);
                bldr.SetCancelable (true);
                bldr.SetOnCancelListener (lsnrCancel);
                bldr.SetPositiveButton ("Yes", delegate {
                    result = input.Text;
                    exit ();
                });
                bldr.SetNeutralButton ("No", delegate {
                    result = null;
                    exit ();
                });
                var dlg = bldr.Create ();
                dlg.Show ();
            };
            UiInvokeNoWait (dlgt);
            w.WaitOne ();
            return result;
        }

        //---------------------
#if false // !IMPLEMENT_PAUSE
        [Obsolete("TODO")]
        public override void Pause (string prompt)
        {
        }
#else
        TextView labelPause; // HACK
        //IMenuItem itemUnpause;//, itemClearConsole, itemDoException;
        Button itemUnpause;//, itemClearConsole, itemDoException;

        System.Threading.ManualResetEvent m_pause = new System.Threading.ManualResetEvent(false);
        
        public override void Pause(string prompt)
        {
            m_pause.Reset();
            ShowPause(true, prompt);
            bool x = m_pause.WaitOne();
        }
        
        private void Unpause_Click(object sender, EventArgs e)
        {
            ShowPause(false, null);
            m_pause.Set();
        }
        
        private void SetupPauseControls()
        {
            //this.labelPause.SendToBack();
            ShowPause(false, null);
        }
        
        private void ShowPause(bool pausing, string prompt)
        {
            EventHandler dlgt = delegate
            {
                if (pausing) {
                    WriteLine(prompt);
                    this.labelPause.Text = "Menu->Un-pause: " + prompt;
                    this.labelPause.Visibility = ViewStates.Visible;
                    SDDebug.Assert(itemUnpause != null, "init'd in RunMenu so should be valid!");
                } else {
                    this.labelPause.Visibility = ViewStates.Invisible;
                }
                if (itemUnpause != null) {
                    //itemUnpause.SetEnabled(pausing);
                    itemUnpause.Enabled = (pausing);
                }
            };
            UiInvokeNoWait(dlgt);
        }
#endif

        //---------------------
        public override void UiInvoke (EventHandler dlgt)
        {
            UiInvokeWait (dlgt);
        }

        private void UiInvokeNoWait (EventHandler dlgt)
        {
            _parent.UiInvokeNoWait (dlgt);
        }
        
        private void UiInvokeWait (EventHandler dlgt)
        {
            _parent.UiInvokeWait (dlgt);
        }

        public override bool? InvokeRequired { get { return _parent.InvokeRequired; } }

        public override object InvokeeControl { get { return _parent.InvokeeControl; } }
        //
        /// <summary>
        /// Run the menu system, may or may not be blocking (e.g. ConsoleMenu blocking, Forms not).
        /// </summary>
        public override void RunMenu ()
        {
            _parent.RunMenu ();
        }

        #endregion

        //----
        Dictionary<string, ISubMenu> m_subMenus = new Dictionary<string, ISubMenu>();
        const char BlackRightPointingTriangle = '\u25B6';

        public bool OnCreateOptionsMenu (IMenu menu)
        {
            _options = new List<Option>();
            // Android menu item Numbering starts at 1 so insert a null Option at 0.
            var dummyMethod = this.GetType ().GetMethod("Clear");
            _options.Insert (0, new Option("DUMMY_ZERO", null, dummyMethod, this));
            _options.AddRange (_manualOptions);
            _options.AddRange (Options);
            // Make  these appear at  the top of the menu.
            CreateSubMenu(menu, "BtClient");
            CreateSubMenu(menu, "Device Discovery");
            CreateSubMenu(menu, "Local");
            CreateSubMenu(menu, "BtLsnr");
            CreateSubMenu(menu, "DeviceInfo");
            //
            var options = _options;
            int numRoot = 0;
            for (int i = 1; i < options.Count; ++i) {
                Option cur = options [i];
                if (cur.SubMenu == null || cur.SubMenu == RootName) {
                    AddMenuItem (i, menu, cur);
                    ++numRoot;
                } else {
                    if (!m_subMenus.ContainsKey (cur.SubMenu)) {
                        CreateSubMenu (menu, cur.SubMenu);
                        ++numRoot;
                    }
                    ISubMenu subMenu = m_subMenus [cur.SubMenu];
                    AddMenuItem (i, subMenu, cur);
                }
            }
            SDDebug.WriteLine ("#menu@root: " + numRoot);
            return true;
        }

        ISubMenu CreateSubMenu (IMenu parent, string subMenuName)
        {
            ISubMenu newSubMenu = parent.AddSubMenu (Menu.None, Menu.None, Menu.None,
                subMenuName + ' ' + BlackRightPointingTriangle);
            m_subMenus.Add (subMenuName, newSubMenu);
            return newSubMenu;
        }

        static IMenuItem AddMenuItem (int i, IMenu parent, Option cur)
        {
            System.Diagnostics.Debug.Assert (i >= Menu.First, "Invalid ItemId: " + i);
            var mi = parent.Add (Menu.None, i, Menu.None, cur.name);
            return mi;
        }

        public bool OnOptionsItemSelected (IMenuItem item)
        {
            if (item.HasSubMenu) {
                return _parent.OnOptionsItemSelected_Base (item);
            }
            var options = _options;
            if (item.ItemId >= options.Count) {
                System.Diagnostics.Debug.Fail ("Unknown mi.ItemId: " + item.ItemId);
                return _parent.OnOptionsItemSelected_Base (item);
            }
            var option = options [item.ItemId];
            option.EventHandlerInvokeOnBackground (item, EventArgs.Empty);
            return true;
        }

    }
}

