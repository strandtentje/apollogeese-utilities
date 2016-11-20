using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class AnyParserTest : SessionTest
	{
		AnyParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new AnyParser (
				new SequenceParser ("kaas"),
				new SequenceParser ("koekjes"),
				new SequenceParser ("eten")
			);
		}

		[Test()]
		public void TestIndividualParseIndices() 
		{
			object result;
			Assert.AreEqual (1, SUT.Run (GivenSessionWithText ("kaas; ignored"), out result));
			Assert.AreEqual ("kaas", result);
			Assert.AreEqual (2, SUT.Run (GivenSessionWithText ("koekjes; ignored"), out result));
			Assert.AreEqual ("koekjes", result);
			Assert.AreEqual (3, SUT.Run (GivenSessionWithText ("eten; ignored"), out result));
			Assert.AreEqual ("eten", result);
		}

		[Test()]
		public void TestMismatch()
		{
			object result;
			Assert.AreEqual (-1, SUT.Run (GivenSessionWithText ("stengels; ignored"), out result));
			Assert.Null (result);
		}

		[Test()]
		public void TestSequentialHandover()
		{
			var session = new ParsingSession ("koekjes eten kaas", new WhitespaceParser ());
			object result;
			Assert.AreEqual (2, SUT.Run (session, out result));
			Assert.AreEqual ("koekjes", result);
			Assert.AreEqual (3, SUT.Run (session, out result));
			Assert.AreEqual ("eten", result);
			Assert.AreEqual (1, SUT.Run (session, out result));
			Assert.AreEqual ("kaas", result);
		}
	}
}

