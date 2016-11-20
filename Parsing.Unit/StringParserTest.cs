using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class StringParserTest
	{
		StringParser SUT;

		[SetUp()]
		public void SetUp() 
		{
			this.SUT = new StringParser ();
		}

		[Test()]
		public void EmptyStringTest()
		{
			Assert.AreEqual ("", Parse<string>.Using (SUT, "\"\" ignored"));
		}

		[Test()]
		public void SingleCharacterTest()
		{
			Assert.AreEqual ("a", Parse<string>.Using (SUT, "\"a\" ignored"));
		}

		[Test()]
		public void WordTest()
		{
			Assert.AreEqual ("hoi", Parse<string>.Using (SUT, "\"hoi\" ignored"));
		}

		[Test()]
		public void SentenceTest()
		{
			Assert.AreEqual ("hoi pipeloi", Parse<string>.Using (SUT, "\"hoi pipeloi\" ignored"));
		}

		[Test()]
		public void EscapeNewlineTest()
		{
			Assert.AreEqual ("hoi\npipeloi", Parse<string>.Using (SUT, "\"hoi\\npipeloi\" ignored"));
			Assert.AreEqual ("hoi\npipeloi", Parse<string>.Using (SUT, "\"hoi\npipeloi\" ignored"));
		}

		[Test()]
		public void EscapeTabTest()
		{
			Assert.AreEqual ("hoi\tpipeloi", Parse<string>.Using (SUT, "\"hoi\\tpipeloi\" ignored"));
			Assert.AreEqual ("hoi\tpipeloi", Parse<string>.Using (SUT, "\"hoi\tpipeloi\" ignored"));
		}

		[Test()]
		public void EscapeQuotationTest()
		{
			Assert.AreEqual ("hoi\"pipeloi", Parse<string>.Using (SUT, "\"hoi\\\"pipeloi\" ignored"));
		}
	}
}

