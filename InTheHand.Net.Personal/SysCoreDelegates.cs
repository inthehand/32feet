// 32feet.NET - Personal Area Networking for .NET
//
// Copy of NET 3.5's System.Core general delegates for use when compiling for FX2.
// 
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if FX3_5
// TODO #warning Can remove the InTheHand.SystemCore delegate copies when compiling the library for FX3.5.
#endif

// Can remove all of these when we move the library to be a FX3.5 assembly.
namespace InTheHand.SystemCore
{
#if !FX3_5
    internal delegate void Action();
    internal delegate void Action<T1, T2>(T1 arg1, T2 arg2);
    internal delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    internal delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    //
    internal delegate TResult Func<TResult>();
    internal delegate TResult Func<T, TResult>(T arg);
    internal delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
    internal delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    internal delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
#endif

    class DummmyRemoveABCDEF
    {
    }
}
