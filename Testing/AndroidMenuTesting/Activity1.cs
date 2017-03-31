// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J.McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt
using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ConsoleMenuTesting;
using DeviceMenuTesting;
using System.Threading;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace AndroidMenuTesting
{
    [Activity (Label = "AndroidMenuTesting", MainLauncher = true)]
    public class Activity1 : Activity
    {
        DroidMenuSystem _console;
        BluetoothTesting program = new BluetoothTesting ();
        TextView _tb;
        bool _disposed;
        //
        Handler _handler;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            //
            _handler = new Handler ();

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Go
            TextView txtView = FindViewById<TextView> (Resource.Id.textView1);
            _tb = txtView;
            _console = new DroidMenuSystem (this);
            _secondConsole = null;
            program.console = _console;
            SetupManualButtons ();
            SetupManualOptions (_console);
            _console.AddMenus (program);
            _console.RunMenu ();
        }

        public override bool OnCreateOptionsMenu (IMenu menu)
        {
#if MENU_SHORT
            var options = _optionsShort;
            for (int i = 0; i < options.Count; ++i) {
                Option cur = options [i];
                var mi = menu.Add (0, i, 0, cur.name);
            }
            return true;
#else
            return _console.OnCreateOptionsMenu (menu);
#endif
        }

        public bool OnOptionsItemSelected_Base (IMenuItem item)
        {
            return base.OnOptionsItemSelected (item);
        }

        public override bool OnOptionsItemSelected (IMenuItem item)
        {
#if !MENU_SHORT
            return _console.OnOptionsItemSelected (item);
#else
            var options = _optionsShort;
            if (item.ItemId >= options.Count) {
                System.Diagnostics. Debug.Fail("Unknown mi.ItemId: " + item.ItemId);
            return base.OnOptionsItemSelected (item);
            }
            var option = options[item.ItemId];
            option.EventHandlerInvokeOnBackground(item, EventArgs.Empty);
            return true;
#endif
        }

        void SetupManualButtons ()
        {
            Button b;
            Option item;
            //
            b = FindViewById<Button> (Resource.Id.buttonClear);
            item = new Option ("Clear console", null, this.GetType ().GetMethod ("Clear"), this);
            //NO WORKEE item = CreateOption (() => Clear());
            AddOption (b, item);
            //
            b = FindViewById<Button> (Resource.Id.buttonGar);
            item = CreateOption ("GetAllRadios");
            AddOption (b, item);
            //
            b = FindViewById<Button> (Resource.Id.buttonNewBdi);
#if TEST_YES_NO_DIALOG
            item = CreateOption (() => program.TestReadYesNo ());
            AddOption (b, item);
            item = CreateOption (() => program.TestReadYesNoCancel ());
            AddOption (b, item);
#else
            item = CreateOption (() => program.GetDeviceInfo ());
            AddOption (b, item);
#endif
            //
            b = FindViewById<Button> (Resource.Id.buttonDisco);
            item = CreateOption (() => program._FlagsDiscover ());
            AddOption (b, item);
            //
            b = FindViewById<Button> (Resource.Id.buttonConnR);
            item = CreateOption (() => program.Connect_SimpleClosingFirst ());
            AddOption (b, item);
            //
            b = FindViewById<Button> (Resource.Id.buttonLsnr);
            item = CreateOption (() => program.Listen ());
            AddOption (b, item);
        }

        List<Option> _optionsShort = new List<Option> ();

        void AddOption (Button b, Option item)
        {
            b.Click += item.EventHandlerInvokeOnBackground;
            _optionsShort.Add (item);
        }

        //--------
        public void RunMenu ()
        {
            // BTW The menus are created in OnCreateOptionsMenu.
            Clear ();
        }

        void SetupManualOptions (DroidMenuSystem console)
        {
            Option item;
            var list = console._manualOptions = new List<Option> ();
            //
            item = new Option ("Clear console", null, this.GetType ().GetMethod ("Clear"), this);
            list.Add (item);
            //
            item = new Option ("Save Text", null, this.GetType ().GetMethod ("SaveText"), this);
            list.Add (item);
        }
                                 

        //--------

        Option CreateOption (string name)
        {
            var target = program;
            return CreateOption (name, target);
        }

        internal static Option CreateOption (string name, object target)
        {
            var tTarget = target.GetType ();
            var mi = tTarget.GetMethod (name);
            if (mi == null)
                throw new ArgumentException ("Method '" + name + "'not found.");
            var item = new Option (name, null, mi, target);
            return item;
        }

        Option CreateOption (Expression<Action> expr)
        {
            var body = (MethodCallExpression)expr.Body;
            var mi = body.Method;
            var name = mi.Name;
            // We previously assumed every function was on class BluetoothTesting.
            object instance = program;
            // On this line we're trying to find the correct instance but its
            // ok for BluetoothTesting but wrong for us/this/Activity1.
            //instance = body.Object;
            //
            var item = new Option (name, null, mi, instance);
            return item;
        }

        Option CreateOption (Expression<Action> expr, object target_NeedDueToBug)
        {
            var body = (MethodCallExpression)expr.Body;
            var mi = body.Method;
            var name = mi.Name;
            // We previously assumed every function was on class BluetoothTesting.
            object instance = program;
            // On this line we're trying to find the correct instance but its
            // ok for BluetoothTesting but wrong for us/this/Activity1.
            instance = body.Object;
            //
            var item = new Option (name, null, mi, target_NeedDueToBug);
            return item;
        }

        //--
        void SaveText_Click (object sender, EventArgs e)
        {
            SaveText ();
        }

        public void SaveText ()
        {
            var dest = this.GetFileStreamPath ("mt_output.txt");
            _console.WriteLine ("Saving to: " + dest);
            using (var f = this.OpenFileOutput("mt_output.txt", FileCreationMode.WorldReadable))
            using (var wtr = new System.IO.StreamWriter(f)) {
                wtr.WriteLine (this._tb.Text);
            }
        }

        //--------
        const string NewLine = "\r\n";
        IAuxConsole _secondConsole;
        
        public void SafeAppendText (string txt)
        {
            if (_secondConsole != null) {
                _secondConsole.AppendText (txt);
            }
            //
            EventHandler dlgt = delegate {
                _tb.Text += txt;
            };
            SafeInvoke (_tb, dlgt);
        }

        public void SafeSetText (string txt)
        {
            EventHandler dlgt = delegate {
                _tb.Text = txt;
            };
            SafeInvoke (_tb, dlgt);
        }
        
        private void SafeInvoke (View c, EventHandler dlgt)
        {
            if (_disposed) {
                return;
            }
            if (InvokeRequired != false) {
                UiInvokeWait (dlgt);
            } else {
                dlgt.Invoke (c, EventArgs.Empty);
            }
        }
        
        public void Clear ()
        {
            SafeSetText (string.Empty);
        }
        
        //--------
        public void UiInvokeWait (EventHandler dlgt)
        {
            var w = new ManualResetEvent (false);
            _handler.Post (() => {
                try {
                    dlgt (null, EventArgs.Empty);
                } finally {
                    w.Set ();
                }
            });
            w.WaitOne ();
        }
        
        public void UiInvokeNoWait (EventHandler dlgt)
        {
            _handler.Post (() => dlgt (null, EventArgs.Empty));
        }
        
        public bool? InvokeRequired {
            get {
                var onUi = Looper.MyLooper () == Looper.MainLooper;
                return !onUi;
            }
        }
        
        public object InvokeeControl {
            get { 
                Activity act = this;
                return act;
            }
        }

    }
}


