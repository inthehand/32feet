using System;
using NUnit.Framework;

namespace InTheHand.Net.Tests
{
    public 
#if ! FX1_1
        static/*for now?*/ 
#endif
    class Tests_Values
    {
        public const String NewLine = "\r\n";


        internal static ArgumentOutOfRangeException new_ArgumentOutOfRangeException(string paramName, string message)
        {
            return new ArgumentOutOfRangeException(
#if ! FX1_1
paramName,
#endif
 message);
        }

    }//class

}


