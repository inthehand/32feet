using System;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestContext = System.Version; //dummy
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

class AssertAdapter
{
    public static void IsInstanceOfType(object value, Type expectedType)
    {
#if NUNIT
        Assert.IsInstanceOfType(expectedType, value);
#else
        Assert.IsInstanceOfType(value, expectedType);
#endif
    }

}
