// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using Gtk;

namespace GtkMenuTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //-Pause("Before GTK start");
            Application.Init();
            var w = new Window1();
            w.ShowAll();
            Application.Run();
        }

        private static void Pause(string name)
        {
            Console.Write("paused: {0}> ", name);
            Console.ReadLine();
        }
    }
}
