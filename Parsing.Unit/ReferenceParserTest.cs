using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;
using BorrehSoft.Utilities.Collections;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	[TestFixture()]
	public class ReferenceParserTest : SessionTest
	{
		ReferenceParser SUT;

		[SetUp()]
		public void SetUp()
		{
			this.SUT = new ReferenceParser ();
		}

		protected override Map<object> GivenReferences ()
		{
			var references = base.GivenReferences ();
			references ["kaas"] = "geel";
			references ["kaas.tosti"] = "lekker";
			references ["banaan"] = "geel";
			references ["banaan.milkshake"] = "bruut";
			references ["fietsen"] = "lekker";
			return references;
		}

		[Test()]
		public void TestAbsoluteValidReference()
		{			
			Assert.AreEqual ("geel", Parse<string>.Using (SUT, GivenSessionWithText("kaas; ignored")));
			Assert.AreEqual ("lekker", Parse<string>.Using (SUT, GivenSessionWithText("kaas.tosti; ignored")));
		}

		public void TestIgnoreInvalidWordReference()
		{
			var session = GivenSessionWithText ("ham; ignored");
			Parse<string>.Using (SUT, session);
			Assert.AreEqual ("ham", Parse<string>.Using (new IdentifierParser (), session));
		}

		[Test()]
		[ExpectedException(typeof(ReferenceParser.InvalidReferenceException))]
		public void TestRelativeInvalidReference()
		{
			Parse<string>.Using (SUT, GivenSessionWithText (".ham; ignored", "kaas"));
		}
	}
}
