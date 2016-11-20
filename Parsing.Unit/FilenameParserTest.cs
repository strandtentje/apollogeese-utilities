using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;
using System.IO;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class FilenameParserTest : SessionTest
	{
		FilenameParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new FilenameParser();
		}

		[Test()]
		public void TestPicksUpRelativeFile()
		{
			var session = GivenSessionWithText ("f\"tosti.txt\"");
			session.WorkingDirectory = "kaas";
			Assert.AreEqual ("kaas" + Path.DirectorySeparatorChar + "tosti.txt", Parse<string>.Using (SUT, session));
		}

		[Test()]
		public void TestIgnoresString()
		{
			var session = GivenSessionWithText ("\"tosti.txt\"");
			Assert.AreEqual (null, Parse<string>.Using (SUT, session));
		}

		[Test()]
		public void TestIgnoresFIdentifier()
		{
			var session = GivenSessionWithText ("fKaasTosti");
			Assert.AreEqual (null, Parse<string>.Using (SUT, session));
		}
	}
}

