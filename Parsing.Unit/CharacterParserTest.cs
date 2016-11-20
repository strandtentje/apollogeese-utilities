using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class CharacterParserTest : SessionTest
	{
		CharacterParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new CharacterParser ('a');
		}

		[Test()]
		public void TestCharacterMatch()
		{
			Assert.AreEqual ('a', Parse<char>.Using (SUT, "a; ignored"));
		}

		[Test()]
		public void TestCharacterMismatch()
		{
			Assert.AreEqual (default(char), Parse<char>.Using (SUT, "b; ignored"));
		}

		[Test()]
		public void TestHandover()
		{
			var session = GivenSessionWithText ("ba; ignored");
			Assert.AreEqual (default(char), Parse<char>.Using (SUT, session));
			Assert.AreEqual ('b', Parse<char>.Using (new CharacterParser('b'), session));
		}
	}
}

