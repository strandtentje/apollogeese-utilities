using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class NumericParserTest : SessionTest
	{
		NumericParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new NumericParser ();
		}

		[Test()]
		public void DecimalTest()
		{
			object result;
			this.SUT.Run (GivenSessionWithText ("!10.3"), out result);
			Assert.IsInstanceOf<decimal> (result);
			Assert.AreEqual (10.3M, result);

			this.SUT.Run (GivenSessionWithText ("!-10.3"), out result);
			Assert.IsInstanceOf<decimal> (result);
			Assert.AreEqual (-10.3M, result);
		}

		[Test()]
		public void IntegerTest()
		{
			object result;
			this.SUT.Run (GivenSessionWithText ("-10"), out result);
			Assert.IsInstanceOf<int> (result);
			Assert.AreEqual (-10, result);

			this.SUT.Run (GivenSessionWithText ("10"), out result);
			Assert.IsInstanceOf<int> (result);
			Assert.AreEqual (10, result);
		}

		[Test()]
		public void LongTest() 
		{
			object result;
			this.SUT.Run (GivenSessionWithText ("-100000000000"), out result);
			Assert.IsInstanceOf<long>(result);
			Assert.AreEqual (-100000000000, result);

			this.SUT.Run (GivenSessionWithText ("100000000000"), out result);
			Assert.IsInstanceOf<long>(result);
			Assert.AreEqual (100000000000, result);
		}

		[Test()]
		public void FloatTest()
		{
			object result;
			this.SUT.Run (GivenSessionWithText ("-10.3"), out result);
			Assert.IsInstanceOf<float> (result);
			Assert.AreEqual (-10.3f, result);

			this.SUT.Run (GivenSessionWithText ("10.3"), out result);
			Assert.IsInstanceOf<float> (result);
			Assert.AreEqual (10.3f, result);
		}
	}
}

