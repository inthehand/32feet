using System;
using NMock2;
using NMock2.Monitoring;
using System.Diagnostics;
using System.Globalization;

namespace InTheHand.Net.Tests.Infra
{
    internal static class FillArrayIndexedParameterAction
    {
        public static IAction Fill<T>(int parameterIndex, T[] values, bool mustBeSameLength)
        {
            return new FAIPAction<T>(parameterIndex, values, mustBeSameLength);
        }

        //----
        private class FAIPAction<T> : IAction
        {
            readonly int _parameterIndex;
            readonly T[] _values;
            readonly bool _mustBeSameLength;


            internal FAIPAction(int parameterIndex, T[] values, bool mustBeSameLength)
            {
                _parameterIndex = parameterIndex;
                _values = (T[])values.Clone();
                _mustBeSameLength = mustBeSameLength;
            }

            #region IInvokable Members

            void IInvokable.Invoke(Invocation invocation)
            {
                object p = invocation.Parameters[_parameterIndex];
                var arrP = p as T[];
                if (arrP == null)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                        "Excpected parameter {0} to be an array of element type {1}.",
                        _parameterIndex, typeof(T).FullName));
                if (_mustBeSameLength && arrP.Length != _values.Length)
                    throw new ArgumentException("Different array lengths: "
                        + "expected: " + _values.Length + ", but parameter array was length: " + arrP.Length);
                if (arrP.Length < _values.Length)
                    throw new ArgumentException("The parameter array is shorter than the source array: "
                        + "source length: " + _values.Length + ", and parameter array is length: " + arrP.Length);
                _values.CopyTo(arrP, 0);
            }

            #endregion

            #region ISelfDescribing Members

            void ISelfDescribing.DescribeTo(System.IO.TextWriter writer)
            {
                writer.WriteLine("Copying to parameter {0} the content of array of element type {1} of length {2}.",
                    _parameterIndex, typeof(T).FullName, _values.Length);
            }

            #endregion
        }
    }

    internal class ArrayMatcher
    {
        public static Matcher MatchContentExactly<T>(T[] expectedArray)
            where T : struct
        {
            return new ArrayMatcher_<T>(expectedArray);
        }

        //----
        internal class ArrayMatcher_<T> : Matcher
            where T : struct
        {
            readonly T[] _expectedArray;
            string _lastError;

            public ArrayMatcher_(T[] expectedArray)
            {
                _expectedArray = expectedArray;
            }

            //----
            public override void DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("ArrayMatcher");
                if (_lastError != null) {
                    writer.Write(", last error: ");
                    writer.Write(_lastError);
                }
                writer.WriteLine('.');
            }

            public override bool Matches(object o)
            {
                var arr = o as Array;
                if (arr == null) {
                    _lastError = "Not an Array";
                    return false;
                }
                var arrT = o as T[];
                if (arrT == null) {
                    _lastError = "Not an Array of " + typeof(T).Name;
                    return false;
                }
                if (_expectedArray.Length != arrT.Length) {
                    _lastError = "Different length array, expected "
                        + _expectedArray.Length
                        + " but was: " + arrT.Length;
                    return false;
                }
                for (int i = 0; i < _expectedArray.Length; ++i) {
                    if (!_expectedArray[i].Equals(arrT[i])) {
                        _lastError = "Different array content at index: " + i
                            + ", expected " + _expectedArray.Length
                            + " but was: " + arrT.Length;
                        return false;
                    }
                }//for
                _lastError = null;
                return true;
            }

        }
    }

    class MyDelegateAction : IAction
    {
        internal delegate void MyAction<T1>(T1 p1);
        //
        MyAction<NMock2.Monitoring.Invocation> _dlgt;

        public MyDelegateAction(MyAction<NMock2.Monitoring.Invocation> dlgt)
        {
            if (dlgt == null) throw new ArgumentNullException();
            _dlgt = dlgt;
        }

        #region IInvokable Members
        void NMock2.Monitoring.IInvokable.Invoke(NMock2.Monitoring.Invocation invocation)
        {
            _dlgt(invocation);
        }
        #endregion

        #region ISelfDescribing Members
        void ISelfDescribing.DescribeTo(System.IO.TextWriter writer)
        {
            writer.Write("[[MyDelegateAction]]");
        }
        #endregion
    }

}
