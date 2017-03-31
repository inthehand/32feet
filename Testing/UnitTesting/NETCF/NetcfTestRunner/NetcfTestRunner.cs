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
#if ! FX1_1
using System.Collections.Generic;
#endif
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace NetcfTestRunner
{
    public class NetcfTestRunnerWorkings
    {
        private Form1 m_form1;
        private Assembly m_testeeAssembly;
        //
        System.Windows.Forms.OpenFileDialog openFileDialogTesteeAssembly;
        System.Windows.Forms.SaveFileDialog saveFileDialog1;

        //----------------------------------------------------------------------
        internal NetcfTestRunnerWorkings(Form1 form1)
        {
            m_form1 = form1;
            SetTesteeAssembly(typeof(InTheHand.Net.Tests.Sdp2.SdpHelperTests).Assembly);
            SetUiStateIdle();
            CreateFileDialogs();
        }

        //----------------------------------------------------------------------
        void SetUiStateIdle()
        {
            m_form1.menuItemRunAll.Enabled = true;
            m_form1.menuItemRunAll2.Enabled = true;
            m_form1.menuItemRunSelected.Enabled = true;
            m_form1.menuItemStop.Enabled = false;
        }

        void SetUiStateRunning()
        {
            m_form1.label1.Text = "Started...";
            m_form1.menuItemRunAll.Enabled = false;
            m_form1.menuItemRunAll2.Enabled = false;
            m_form1.menuItemRunSelected.Enabled = false;
            m_form1.menuItemStop.Enabled = true;
            m_userStopRequested = false;
        }

        //----------------------------------------------------------------------

        internal void menuQuit_Click(object sender, EventArgs e)
        {
            m_form1.Close();
        }

        internal void Form1_Load(object sender, EventArgs e)
        {
            Version clrVersion = Environment.Version;
            String clr = "CLR: " + clrVersion.ToString();
            OperatingSystem osVersion = Environment.OSVersion;
            String os = "OS: " + osVersion.ToString();
            m_form1.listView1.Items.Add(new ListViewItem(clr));
            m_form1.listView1.Items.Add(new ListViewItem(os));
        }

        private void SetTesteeAssembly(Assembly assm)
        {
            Type ttu = assm.GetType("InTheHand.Net.Tests.TestsUtils");
            System.Diagnostics.Debug.Assert(ttu != null, "NO TestUtils?!!");
            if (ttu != null) {
                MethodInfo mi = ttu.GetMethod("SetIsInNetcfTestRunner",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                System.Diagnostics.Debug.Assert(mi != null, "NO SetIsInNetcfTestRunner?!!");
                if (mi != null) {
                    mi.Invoke(null, null);
                }
            }
            //----
            m_form1.comboBox1.DataSource = null;
            m_testeeAssembly = assm;
        }

        Assembly GetTesteeAssembly()
        {
            return m_testeeAssembly;
        }

        internal void buttonStop_Click(object sender, EventArgs e)
        {
            m_userStopRequested = true;
        }

        internal void comboBox1_GotFocus(object sender, EventArgs e)
        {
            if (m_form1.comboBox1.DataSource == null) {
                try {
                    Cursor.Current = Cursors.WaitCursor;

                    Type[] allTestTypes = GetAllTestClasses(GetTesteeAssembly());
                    m_form1.comboBox1.DisplayMember = "Name";
                    m_form1.comboBox1.DataSource = allTestTypes;
                } finally {
                    Cursor.Current = Cursors.Default;
                }
            }
        }//fn

        internal void buttonAllClasses_Click(object sender, EventArgs e)
        {
            DoTestsOnAllClasses(GetTesteeAssembly());
        }

        internal void buttonSelectedClass_Click(object sender, EventArgs e)
        {
            // Check the value we get out of the ComboBox...
            Object selectedValue = m_form1.comboBox1.SelectedValue;
            Type selectedType = selectedValue as Type;
            // Go.
            if (selectedType == null) {
                MessageBox.Show("No class selected.");
            } else {
                DoTestsOnClass(selectedType);
            }
        }

        internal void buttonShowSelectedMessage_Click(object sender, EventArgs e)
        {
            String message;
            if (m_form1.listView1.SelectedIndices.Count > 0) {
                message = ((ListViewItem)m_form1.listView1.Items[m_form1.listView1.SelectedIndices[0]]).Text;
            } else {
                message = ((ListViewItem)m_form1.listView1.Items[0]).Text;
            }
            if (message == null) {
            } else {
                MessageBox.Show(message);
            }
        }

        internal void buttonSave_Click(object sender, EventArgs e)
        {
            string fileName;
            if (!GetLogFileName(out fileName)) {
                return;
            }
            //
            using (System.IO.TextWriter wtr
                    = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.UTF8)) {
                Version clrVersion = Environment.Version;
                OperatingSystem osVersion = Environment.OSVersion;
                wtr.WriteLine("OS Version: " + osVersion.ToString()
                    + "    .NET Version: " + clrVersion.ToString());
                wtr.WriteLine();
                foreach (ListViewItem row in m_form1.listView1.Items) {
                    String text = row.Text;
                    wtr.WriteLine(text);
                }//for
            }//using
        }

        internal void menuItemLoadAssembly_Click(object sender, EventArgs e)
        {
            string fileName;
            if (!GetAssemblyFilename(out fileName)) {
                return;
            }
            Assembly assm = Assembly.LoadFrom(fileName);
            SetTesteeAssembly(assm);
        }

        //----------------------------------------------------------------------

        void CreateFileDialogs()
        {
            try {
                saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.FileName = m_form1.saveFileDialog1_FileName;
            } catch (NotSupportedException) {
                // SmartPhone?!
            }
            try{
                openFileDialogTesteeAssembly = new System.Windows.Forms.OpenFileDialog();
                openFileDialogTesteeAssembly.Filter = "DLLs (*.dll)|*.dll|Apps (*.exe)|*.exe|All files (*.*)|*.*";
            } catch (NotSupportedException) {
                // SmartPhone?!
            }
        }


        internal bool GetLogFileName(out string fileName)
        {
            if (saveFileDialog1 == null) {
                fileName = m_form1.saveFileDialog1_FileName;
#if !FX1_1
                string mydocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
                string mydocs = @"\My Documents";
#endif
                fileName = System.IO.Path.Combine(mydocs, fileName);
                return true;
            }
            DialogResult rslt = saveFileDialog1.ShowDialog();
            if (rslt == DialogResult.OK) {
                fileName = saveFileDialog1.FileName;
                return true;
            } else {
                fileName = null;
                return false;
            }
        }

        internal bool GetAssemblyFilename(out string fileName)
        {
            if (openFileDialogTesteeAssembly == null) {
                fileName = null;
                return false;
            }
            DialogResult rslt = openFileDialogTesteeAssembly.ShowDialog();
            if (rslt == DialogResult.OK) {
                fileName = openFileDialogTesteeAssembly.FileName;
                return true;
            } else {
                fileName = null;
                return false;
            }
        }

        //----------------------------------------------------------------------

        bool m_userStopRequested;

        long m_progressCount;
        long m_progressCountOfTestClasses;
        long m_countOfRunningTestClasses;
        String m_progressText;

        long m_countPassed;
        long m_countNotRun;
        long m_countFailedByAssert;
        long m_countFailedByException;

        private void PrepareForTests()
        {
            if (m_form1.InvokeRequired) {
                m_form1.Invoke(new EventHandler(PrepareForTests__));
            } else {
                PrepareForTests__(null, null);
            }
        }
        private void PrepareForTests__(object sender, EventArgs e)
        {
            SetUiStateRunning();
            //
            m_countOfRunningTestClasses = m_progressCountOfTestClasses = 0;
            m_progressCount = 0;
            m_countPassed = m_countNotRun = m_countFailedByAssert = m_countFailedByException = 0;
            m_progressText = String.Empty;
            //
            m_form1.listView1.Items.Clear();
            // For dots.
            m_form1.listView1.Items.Add(new ListViewItem(""));
            // For counts
            m_form1.listView1.Items.Add(new ListViewItem(""));
#if ! FX1_1
            m_FailedByException = new List<string>();
#else
            m_FailedByException = new System.Collections.ArrayList();
#endif
        }

        private void TestsFinished()
        {
            if (m_form1.InvokeRequired) {
                m_form1.Invoke(new EventHandler(TestsFinished__));
            } else {
                TestsFinished__(null, null);
            }
        }
        private void TestsFinished__(object sender, EventArgs e)
        {
            UpdateProgress();
            const String LogHeaderLine = "------------------------------------------------------------";
            WriteToLog(LogHeaderLine);
            //
            ((ListViewItem)m_form1.listView1.Items[0]).Text += "]]]";
            ((ListViewItem)m_form1.listView1.Items[1]).Text
                += "Pass " + m_countPassed
                + ", NtRn " + m_countNotRun
                + ", Ex " + m_countFailedByException
                + ", Asrt " + m_countFailedByAssert
                + ".";
            WriteToLog(((ListViewItem)m_form1.listView1.Items[0]).Text);
            WriteToLog(((ListViewItem)m_form1.listView1.Items[1]).Text);
            //
            int i = 1;
            foreach (String failedMessage in m_FailedByException) {
                m_form1.listView1.Items.Add(new ListViewItem(failedMessage));
                WriteToLog((String)failedMessage);
                ++i;
            }
            //
            WriteToLog(LogHeaderLine);
            SetUiStateIdle();
        }

        private void WriteToLog(String line)
        {
            System.Diagnostics.Debug.WriteLine(line);
            Console.WriteLine(line);
        }

        private void TheTestPassed(MethodInfo curMethod)
        {
            m_countPassed++;
            AddProgressDot(".");
        }

        private void AddProgressDot(String dotOrFOrEtc)
        {
            m_progressText += dotOrFOrEtc;
            //----
            if (m_form1.InvokeRequired) {
                m_form1.Invoke(new EventHandler(AddProgressDot__));
                return;
            } else {
                AddProgressDot__(null, null);
            }
        }

        void AddProgressDot__(object sender, EventArgs e)
        {
            m_progressCount++;
            UpdateProgress();
            ((ListViewItem)m_form1.listView1.Items[0]).Text = m_progressText;
        }

        private void UpdateProgress()
        {
            m_form1.label1.Text = m_progressCount.ToString()
                + " (" + m_progressCountOfTestClasses.ToString() + "/" + m_countOfRunningTestClasses.ToString() + ")";
        }


#if ! FX1_1
        List<String> m_FailedByException;
#else
        System.Collections.ArrayList m_FailedByException;
#endif
        Type m_testFailedByExceptionLastType;

        private void TheTestFailedByException(MethodInfo testMethod, Exception failureException)
        {
            m_countFailedByException++;
            // Now the message
            String message = String.Empty;
            NUnit.Framework.AssertionException exceptionAsAssertionEx
                = failureException as NUnit.Framework.AssertionException;
            if (exceptionAsAssertionEx != null) {
                message += exceptionAsAssertionEx.Message;
                AddProgressDot("F");
            } else {
                // Not 'FullName' as it's too long.
                message += failureException.GetType().Name + ": " + failureException.Message;
                AddProgressDot("E");
            }
            TheTestFailed__(testMethod, message);
        }

        private void TheTestNotRun(MethodInfo testMethod, string ignoreMessage)
        {
            m_countNotRun++;
            AddProgressDot("N");
            TheTestFailed__(testMethod, ignoreMessage, true);
        }

        private void TheTestFailedByNotExpectedException(MethodInfo testMethod, NUnit.Framework.ExpectedExceptionAttribute[] expExAttrs)
        {
            m_countFailedByAssert++;
            //
            String message = String.Empty;
            //message += "{" + expExAttrs.Length.ToString() + "} ";
            NUnit.Framework.ExpectedExceptionAttribute attribute = expExAttrs[0];
            if (attribute.ExceptionName != null) {
                message += attribute.ExceptionName;
            } else {
                message += attribute.ExceptionType.Name;
            }
            message += " was expected.";
            //
            AddProgressDot("F");
            TheTestFailed__(testMethod, message);
        }

        private void TheTestFailed__(MethodInfo testMethod, String message)
        {
            TheTestFailed__(testMethod, message, string.Empty);
        }
        private void TheTestFailed__(MethodInfo testMethod, String message, bool ignored)
        {
            TheTestFailed__(testMethod, message, ignored ? "ignored" : string.Empty);
        }
        private void TheTestFailed__(MethodInfo testMethod, String message, string classification)
        {
            // Log the failing test's containing class, but only the first time for each class.
            Type testType = testMethod.ReflectedType; // .DeclaringType is wrong.
            if (!testType.Equals(m_testFailedByExceptionLastType)) {
                m_testFailedByExceptionLastType = testType;
                String noteNewClass = "* " + testType.Name;
                m_FailedByException.Add(noteNewClass);
            }
            m_FailedByException.Add("**" + classification + " " + testMethod.Name + ": " + message);
        }

        //----------------------------------------------------------------------
        bool DoTestsOnBackgroundThread = true;

        delegate void DoTestWithTypeDelegate(Type type);

        void DoTestsOnClass(Type testFixture)
        {
            if (DoTestsOnBackgroundThread) {
                System.Threading.ThreadPool.QueueUserWorkItem(
                    new System.Threading.WaitCallback(DoTestsOnClass__), testFixture);
            } else {
                DoTestsOnClass__(testFixture);
            }
        }

        void DoTestsOnClass__(Object testFixture)
        {
            PrepareForTests();
            //
            m_countOfRunningTestClasses = 1;
            StringBuilder timings = new StringBuilder();
            InternalDoTestsOnClass((Type)testFixture, timings);
            //
            SaveTimings(timings);
            TestsFinished();
        }


        void DoTestsOnAllClasses(Assembly assemblyToBeTested)
        {
            if (DoTestsOnBackgroundThread) {
                System.Threading.ThreadPool.QueueUserWorkItem(
                    new System.Threading.WaitCallback(DoTestsOnAllClasses__), assemblyToBeTested);
            } else {
                DoTestsOnAllClasses__(assemblyToBeTested);
            }
        }

        void DoTestsOnAllClasses__(Object assemblyToBeTestedAsObject)
        {
            try {
                PrepareForTests();
                StringBuilder timings = new StringBuilder();
                //
                Type[] allTypes = GetAllTestClasses(assemblyToBeTestedAsObject);
                m_countOfRunningTestClasses = allTypes.Length;
                foreach (Type type in allTypes) {
                    if (m_userStopRequested) {
                        break;
                    }
                    //Object instance = Activator.CreateInstance(type);
                    InternalDoTestsOnClass(type, timings);
                }
                //
                SaveTimings(timings);
                TestsFinished();
            } catch (Exception ex) {
                WriteToLog(ex.ToString());
                m_FailedByException.Add("Runner failed with: " + ex);
                TestsFinished();
                throw;
            }
        }

        private static void SaveTimings(StringBuilder timings)
        {
            using (System.IO.StreamWriter dst = System.IO.File.CreateText("timings.csv")) {
                dst.Write(timings.ToString());
            }
        }

        private static Type[] GetAllTestClasses(Object assemblyToBeTestedAsObject)
        {
            Assembly assemblyToBeTested = (Assembly)assemblyToBeTestedAsObject;
            Type[] allTypes = assemblyToBeTested.GetTypes();
#if ! FX1_1
            List<Type> testTypes = new List<Type>();
#else
            System.Collections.ArrayList testTypes = new System.Collections.ArrayList();
#endif
            foreach (Type type in allTypes) {
                bool defined = type.IsDefined(typeof(NUnit.Framework.TestFixtureAttribute), false);
                Object[] attributes = type.GetCustomAttributes(
                    typeof(NUnit.Framework.TestFixtureAttribute), false);
                if (attributes == null || attributes.Length == 0) {
                    System.Diagnostics.Debug.Assert(!defined, "testing IsDefined equivalent [not]");
                    // Not a [TestFixture] class.
                    continue;
                }
                System.Diagnostics.Debug.Assert(defined, "testing IsDefined equivalent [is]");
                if (type.IsNotPublic) {
                    //NUnit itself doesn't run on non-public classes!
                    continue;
                }
                if (type.IsAbstract) {
                    continue;
                }
                testTypes.Add(type);
            }//for
#if ! FX1_1
            return testTypes.ToArray();
#else
            return (Type[])testTypes.ToArray(typeof(Type));
#endif
        }


        //----------------------------------------------------------------------
        void InternalDoTestsOnClass(Type testFixtureType, StringBuilder timings)
        {
            Object instance = Activator.CreateInstance(testFixtureType);
            // foreach method with attribute [Test], invoke, 
            //   handling exception by checking if they're in a [ExcpectedException] attribute
            //
            MethodInfo[] methods = testFixtureType.GetMethods();
            //----------
            // [TestFixtureSetUpAttribute]
            foreach (MethodInfo curMethod in methods) {
                bool defined = curMethod.IsDefined(typeof(NUnit.Framework.TestFixtureSetUpAttribute), false);
                object[] attributes = curMethod.GetCustomAttributes(typeof(NUnit.Framework.TestFixtureSetUpAttribute), false);
                if (attributes == null || attributes.Length == 0) {
                    System.Diagnostics.Debug.Assert(!defined, "testing IsDefined equivalent [not]");
                    // Not [TestFixtureSetUp] method.
                    continue;
                }
                System.Diagnostics.Debug.Assert(defined, "testing IsDefined equivalent [not]");
                try {
                    curMethod.Invoke(instance, null);
                } catch (Exception ex) {
                    TheTestFailedByException(curMethod, ex);
                    goto exit;
                }
            }
            //----------
            foreach (MethodInfo curMethod in methods) {
                if (m_userStopRequested) {
                    break;
                }
                bool defined = curMethod.IsDefined(typeof(NUnit.Framework.TestAttribute), false);
                object[] attributes = curMethod.GetCustomAttributes(typeof(NUnit.Framework.TestAttribute), false);
                if (attributes == null || attributes.Length == 0) {
                    System.Diagnostics.Debug.Assert(!defined, "testing IsDefined equivalent [not]");
                    // Not [Test] method.
                    continue;
                }
                System.Diagnostics.Debug.Assert(defined, "testing IsDefined equivalent [not]");
                NUnit.Framework.ExpectedExceptionAttribute[] expctdExAttributes
                    = (NUnit.Framework.ExpectedExceptionAttribute[])
                        curMethod.GetCustomAttributes(
                            typeof(NUnit.Framework.ExpectedExceptionAttribute), false);
                System.Diagnostics.Debug.Assert(expctdExAttributes.Length == 0
                    || expctdExAttributes.Length == 1);
                try {
                    object[] ignoreAttributes = curMethod.GetCustomAttributes(typeof(NUnit.Framework.IgnoreAttribute), false);
                    if (ignoreAttributes != null && ignoreAttributes.Length > 0) {
                        // [Ignore] test method.
                        System.Diagnostics.Debug.Assert(ignoreAttributes.Length == 1, "FAIL: attributes.Length == 1");
                        NUnit.Framework.IgnoreAttribute attr = (NUnit.Framework.IgnoreAttribute)ignoreAttributes[0];
                        throw new NUnit.Framework.IgnoreException(attr.Reason);
                    }
                    object[] explicitAttributes = curMethod.GetCustomAttributes(typeof(NUnit.Framework.ExplicitAttribute), false);
                    if (explicitAttributes != null && explicitAttributes.Length > 0) {
                        // [Explicit] test method.
                        System.Diagnostics.Debug.Assert(explicitAttributes.Length == 1, "FAIL: attributes.Length == 1");
                        continue;
                    }
                    //--------------------------------------------------
                    // Do it!!!!!!!
                    int startTickCount = Environment.TickCount;
                    curMethod.Invoke(instance, null);
                    int elapsed = Environment.TickCount - startTickCount;
                    const String Sep = ",";
                    timings.Append(testFixtureType.Name).Append(".");
                    timings.Append(curMethod.Name).Append(Sep);
                    timings.Append(elapsed).Append("\r\n");
                    if (expctdExAttributes != null && expctdExAttributes.Length > 0) {
                        /*Failed due to missing expected exception.*/
                        TheTestFailedByNotExpectedException(curMethod, expctdExAttributes);
                        continue;
                    }
                    TheTestPassed(curMethod);
                    // Done or thrown...
                    //--------------------------------------------------
                } catch (NUnit.Framework.IgnoreException iex) {
                    TheTestNotRun(curMethod, iex.Message);
                } catch (Exception testResultEx) {
                    if (expctdExAttributes != null && expctdExAttributes.Length > 0) {
                        NUnit.Framework.ExpectedExceptionAttribute expExAttr
                            = expctdExAttributes[0];
                        Type expExType = expExAttr.ExceptionType;
                        String expExName = expExAttr.ExceptionName;
                        if (testResultEx.GetType().Equals(expExType)) {
                            /* NOP Expected */
                            TheTestPassed(curMethod);
                            continue;
                        } else if (expExName != null
                            && testResultEx.GetType().FullName.Equals(expExName)) {
                            // separate just now for initial testing; fold into the above...
                            /* NOP Expected */
                            TheTestPassed(curMethod);
                            continue;
                        }
                    }
                    // Otherwise unexpected!
                    TheTestFailedByException(curMethod, testResultEx);
                }
            }//for
            //----------
            // [TestFixtureTearDownAttribute]
            foreach (MethodInfo curMethod in methods) {
                bool defined = curMethod.IsDefined(typeof(NUnit.Framework.TestFixtureTearDownAttribute), false);
                object[] attributes = curMethod.GetCustomAttributes(typeof(NUnit.Framework.TestFixtureTearDownAttribute), false);
                if (attributes == null || attributes.Length == 0) {
                    System.Diagnostics.Debug.Assert(!defined, "testing IsDefined equivalent [not]");
                    // Not [TestFixtureSetUp] method.
                    continue;
                }
                System.Diagnostics.Debug.Assert(defined, "testing IsDefined equivalent [not]");
                try{
                    curMethod.Invoke(instance, null);
                } catch (Exception ex) {
                    TheTestFailedByException(curMethod, ex);
                    goto exit;
                }
            }
            //----------
        exit:
            m_progressCountOfTestClasses++;
        }

    }//class
}
