#region Copyright (c) 2007, Andy Hume <andyhume32@yahoo.co.uk>
/************************************************************************************
'
' Copyright © 2007, Andy Hume
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Copyright © 2007, Andy Hume
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace NetcfTestRunner
{
    public partial class Form1 : Form
    {
        private NetcfTestRunnerWorkings m_workings;
        internal readonly string saveFileDialog1_FileName = "NETCF NUnit log.txt";

        //----------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
            m_workings = new NetcfTestRunnerWorkings(this);
        }

        //----------------------------------------------------------------------

        private void menuQuit_Click(object sender, EventArgs e)
        {
            m_workings.menuQuit_Click(sender, e);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            m_workings.buttonStop_Click(sender, e);
        }

        private void comboBox1_GotFocus(object sender, EventArgs e)
        {
            m_workings.comboBox1_GotFocus(sender, e);
        }//fn

        private void buttonAllClasses_Click(object sender, EventArgs e)
        {
            m_workings.buttonAllClasses_Click(sender, e);
        }

        private void buttonSelectedClass_Click(object sender, EventArgs e)
        {
            m_workings.buttonSelectedClass_Click(sender, e);
        }

        private void buttonShowSelectedMessage_Click(object sender, EventArgs e)
        {
            m_workings.buttonShowSelectedMessage_Click(sender, e);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_workings.buttonSave_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_workings.Form1_Load(sender, e);
        }

        private void menuItemLoadAssembly_Click(object sender, EventArgs e)
        {
            m_workings.menuItemLoadAssembly_Click(sender, e);
        }

        private void menuItemListAssemblies_Click(object sender, EventArgs e)
        {
            AppDomain ad = AppDomain.CurrentDomain;
            Assembly exa = Assembly.GetExecutingAssembly();
            Assembly ca = Assembly.GetCallingAssembly();
        }

    }//class
}