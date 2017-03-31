using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMenuTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class SubMenuAttribute : Attribute
    {
        private string m_menuName;

        public SubMenuAttribute()
        {
        }

        public SubMenuAttribute(string menuName)
        {
            m_menuName = menuName;
        }

        public string MenuName
        {
            get { return m_menuName; }
        }

    }
}
