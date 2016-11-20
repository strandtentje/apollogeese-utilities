using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class SequenceParserTest
	{
		SequenceParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new SequenceParser ("cake");
		}

		[Test()]
		public void TestMatch()
		{
			Assert.AreEqual ("cake", Parse<string>.Using (SUT, "cake"));
			Assert.AreEqual ("cake", Parse<string>.Using (SUT, "cakes"));
		}

		[Test()]
		public void TestMismatch()
		{
			Assert.AreNotEqual ("cake", Parse<string>.Using (SUT, "ocake"));
		}

		[Test()]
		public void TestHandover()
		{
			var session = new ParsingSession ("cherry cake", new WhitespaceParser ());
			object result;
			Assert.Less (SUT.Run (session, out result), 0);
			Assert.Greater ((new SequenceParser ("cherry")).Run (session, out result), 0);
			Assert.AreEqual ("cherry", result);
			Assert.Greater (SUT.Run (session, out result), 0);
			Assert.AreEqual ("cake", result);
		}
	}
}

