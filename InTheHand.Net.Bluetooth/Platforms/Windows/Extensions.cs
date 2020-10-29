// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (.NET Standard)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.UI.Notifications;

namespace System
{
    internal static class StringExtensions
    {
        public static string ToUpper(this string s, CultureInfo cultureInfo)
        {
            return s.ToUpperInvariant();
        }
    }

    /*internal static class TypeExtensions
    {
        public static FieldInfo[] GetFields(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().DeclaredFields.ToArray();
        }
    }*/

    namespace IO
    {
        internal static class StringWriterExtensions
        {
            public static void Close(this StringWriter writer)
            {
                writer.Dispose();
            }
        }
    }

    namespace Reflection
    {
        internal static class TypeExtensions
        {
            public static object[] GetCustomAttributes(this FieldInfo field, Type type, bool inherit = true)
            {
                return field.GetCustomAttributes(type, inherit).ToArray();
            }

            public static object GetRawConstantValue(this FieldInfo field)
            {
                return field.GetValue(null);
            }
        }
    }
}
