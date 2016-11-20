using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class BoolParserTest : SessionTest
	{
		BoolParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new BoolParser ();
		}

		[Test()]
		public void TestTruths()
		{
			object result;
			SUT.Run (GivenSessionWithText ("true; ignored"), out result);
			Assert.AreEqual (true, result);
			SUT.Run (GivenSessionWithText ("True; ignored"), out result);
			Assert.AreEqual (true, result);
		}

		[Test()]
		public void TestFalsehoods()
		{
			object result;
			SUT.Run (GivenSessionWithText ("false; ignored"), out result);
			Assert.AreEqual (false, result);
			SUT.Run (GivenSessionWithText ("False; ignored"), out result);
			Assert.AreEqual (false, result);
		}

		[Test()]
		public void TestHandover()
		{
			object result;
			var session = GivenSessionWithText ("unsure; ignored");
			Assert.Less (SUT.Run (session, out result), 0);
			Assert.Null (result);
			Assert.AreEqual ("unsure", Parse<string>.Using (new IdentifierParser (), session)); 
		}
	}
}

