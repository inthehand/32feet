using System;

namespace NUnit.Framework
{
	/// <summary>
	/// Asserter that verifies two objects are the same.
	/// </summary>
	public class SameAsserter : ComparisonAsserter
	{
		/// <summary>
		/// Construct a SameAsserter object
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="message">A user-defined message for use in reporting errors</param>
		/// <param name="args">Arguments to be used in formatting the user-defined message</param>
		public SameAsserter( object expected, object actual, string message, params object[] args )
			: base( expected, actual, message, args ) { }

		/// <summary>
		/// Test that actual and expected reference the same object
		/// </summary>
		public override bool Test()
		{
#if NETCF && FX1_1 //andyh
            return object_ReferenceEquals(expected, actual);
            //throw new NotSupportedException("Assert.Same (ReferenceEquals) not supported on CFv1.");
#else
			return object.ReferenceEquals( expected, actual );
#endif
		}

#if NETCF && FX1_1 //andyh
        internal static bool object_ReferenceEquals(object expected, object actual)
        {
            return ((object)expected == (object)actual);
        }
#endif

		/// <summary>
		/// Provide error message when the objects are different.
		/// </summary>
		public override string Message
		{
			get
			{
				FailureMessage.AddLine( "Object instances should be the same, but are not" );
				FailureMessage.DisplayExpectedAndActual( this.expected, this.actual );
				return FailureMessage.ToString();
			}
		}
	}
}
