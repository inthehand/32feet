using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleMenuTesting
{

    static class Program
    {
        static void Main(string[] args)
        {
            ConsoleMenu console = new ConsoleMenu();
            BluetoothTesting program = new BluetoothTesting();
            program.console = console;
            console.AddMenus(program);
            console.RunMenu();
        }
    }
}