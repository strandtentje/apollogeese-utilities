using NUnit.Framework;
using System;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture ()]
	public class WhitespaceParserTest
	{
		WhitespaceParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new WhitespaceParser ();
		}

		[Test ()]
		public void TestSingleSpace ()
		{
			Assert.AreEqual (" ", Parse<string>.Using (this.SUT, " A"));
		}

		[Test()]
		public void TestDoubleSpace() 
		{
			Assert.AreEqual ("  ", Parse<string>.Using (this.SUT, "  A"));
		}

		[Test()]
		public void TestComplex()
		{
			Assert.AreEqual ("  \r\n\t  ", Parse<string>.Using (this.SUT, "  \r\n\t  A"));
		}
	}
}

