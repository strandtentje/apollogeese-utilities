using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class ConcatenationParserTest
	{
		ConcatenationParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new ConcatenationParser ('(', ')', ',');
			this.SUT.InnerParser = new IdentifierParser ();
		}

		[Test()]
		public void TestMissingConcatenation()
		{
			Assert.Null (Parse<object>.Using (this.SUT, ""));
		}

		[Test()]
		public void TestEmptyConcatenation()
		{
			Assert.IsEmpty (Parse<IEnumerable<object>>.Using (SUT, "()"));
		}

		[Test()]
		public void TestContainsSingleString()
		{
			Assert.AreEqual ("kaas", Parse<List<object>>.Using (SUT, "(kaas)")[0]);
		}

		[Test()]
		public void TestContainsMultipleStrings()
		{
			var result = Parse<List<object>>.Using (SUT, "(kaas,tosti)");
			Assert.AreEqual ("kaas", result[0]);
			Assert.AreEqual ("tosti", result[1]);
		}

		[Test()]
		public void TestSuffixDelimiter()
		{
			var result = Parse<List<object>>.Using (SUT, "(kaas,tosti,)");
			Assert.AreEqual ("kaas", result[0]);
			Assert.AreEqual ("tosti", result[1]);
		}


		[Test()]
		[ExpectedException(typeof(ParsingException))]
		public void TestPrefixDelimiter()
		{
			var result = Parse<List<object>>.Using (SUT, "(,kaas,tosti)");
			Assert.AreEqual ("kaas", result[0]);
			Assert.AreEqual ("tosti", result[1]);
		}

		[Test()]
		public void TestExcessiveDelimiter()
		{
			var result = Parse<List<object>>.Using (SUT, "(kaas,,,tosti)");
			Assert.AreEqual ("kaas", result[0]);
			Assert.AreEqual ("tosti", result[1]);
		}
	}
}

