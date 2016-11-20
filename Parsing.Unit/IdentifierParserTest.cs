using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class IdentifierParserTest
	{
		IdentifierParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new IdentifierParser ();
		}

		[Test()]
		public void TestNoIdentifier()
		{
			Assert.AreEqual (null, Parse<string>.Using (SUT, ";ignored"));
		}

		[Test()]
		public void TestIdentifier()
		{
			Assert.AreEqual ("KaasTosti_12", Parse<string>.Using (SUT, "KaasTosti_12=ignored")); 
		}
	}
}

