using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace ConsoleMenuTesting
{
    class Option
    {
#if !NETCF
        [Obsolete("Not fully working")]
        public static Option CreateOption(System.Linq.Expressions.Expression<Action> expr)
        {
            var body = (System.Linq.Expressions.MethodCallExpression)expr.Body;
            var mi = body.Method;
            var name = mi.Name;
            // We previously assumed every function was on class BluetoothTesting.
            object instance;
            //instance = program;
            // On this line we're trying to find the correct instance but its
            // ok for BluetoothTesting but wrong for us/this/Activity1.
            var obj = body.Object;
            //var objF = obj as System.Linq.Expressions.FieldExpression;
            var objF = obj as System.Linq.Expressions.MemberExpression;
            if (objF != null) {
                var obj2 = objF.Expression;
                obj = obj2;
            }
            //var instance2 = expr.Compile().Target;
            //instance = instance2;
            instance = ((System.Linq.Expressions.ConstantExpression)obj).Value;
            //
            var item = new Option(name, null, mi, instance);
            return item;
        }
#endif

        //====================

        public string name;
        string subMenu;
        MethodInfo methodInfo;
        object methodTarget;
        object[] args;
        private static MenuSystem console;
        //
        public static MenuSystem Console
        {
            get { return console; }
            set
            {
                if (console != null) {
                    // Android keeps static variable when reloading
                    bool doThrow = false;
                    if (doThrow) {
                        throw new ArgumentException("Do not set 'console' twice.");
                    }
                }
                console = value;
            }
        }

        //----
        public Option(string name, string subMenu, MethodInfo method, object target)
        {
            if (Console == null)
                throw new InvalidOperationException("Must initialise the static/Shared 'console' field.");
            //
            this.name = name;
            this.subMenu = subMenu;
            this.methodInfo = method;
            this.methodTarget = target;
        }

        protected Option(string name, string subMenu, MethodInfo method, object target, object[] args)
            : this(name, subMenu, method, target)
        {
            this.args = args;
        }

        public string SubMenu { get { return subMenu; } }

        [DebuggerNonUserCode]
        internal void Invoke()
        {
            console.WriteLine("---- {0} ----", this.name);
            this.methodInfo.Invoke(this.methodTarget, args);
            console.WriteLine("---- CLEAN EXIT ----", this.name);
        }

        public void EventHandlerInvoke(object sender, EventArgs e)
        {
            Invoke();
        }

        public void EventHandlerInvokeOnBackground(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(InvokeWC);
        }

        [DebuggerNonUserCode]
        private void InvokeWC(object unusedParameter)
        {
#if false && !NETCF
            Invoke();
#else
            try {
                Invoke();
            } catch (Exception ex) {
                ex = GetOriginalException(ex);
                //
                string prefix = null;
                var soex = ex as System.Net.Sockets.SocketException;
                if (soex != null) {
                    prefix = "(" + soex.ErrorCode.ToString(CultureInfo.InvariantCulture)
#if !NETCF
                        + " " + soex.SocketErrorCode.ToString()
#endif
                        + ") ";
                }
                console.WriteLine("Swallowed Exception: " + prefix + ex);
                console.ReadLine("Continuing after: " + prefix + ToStringFirstLine(ex));
            }
#endif
        }

        internal static Exception GetOriginalException(Exception ex)
        {
            while (true) {
                var tiex = ex as TargetInvocationException;
                // Desktop and NETCF-later-versions will have wrapped the
                // original exception thrown within the thread in an TIEx
                // so that the original stack-trace is preserved...
                if (tiex == null) {
                    break;
                }
                ex = tiex.InnerException;
                if (ex == null) {
                    // NETCFv2 has bug in TIEx (fixed in 3.5) -- it does NOT
                    // set the InnerException in its ".ctor(Exception)" method!!
                    console.WriteLine("Null InnerException on TIEx!!!!");
                    ex = tiex;
                    break;
                }
            }
            return ex;
        }

        static string ToStringFirstLine(object @this)
        {
            using (System.IO.StringReader rdr = new System.IO.StringReader(@this.ToString())) {
                return rdr.ReadLine();
            }
        }

    }
}
