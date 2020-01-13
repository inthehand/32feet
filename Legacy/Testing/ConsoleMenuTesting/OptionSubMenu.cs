using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMenuTesting
{
    class OptionSubMenu : Option
    {
        public OptionSubMenu(string name, Action<string> changeSubMenuMethod, MenuSystem me)
            : base("->" + name, null, changeSubMenuMethod.Method, me, new object[] { name })
        {
        }
    }
}
