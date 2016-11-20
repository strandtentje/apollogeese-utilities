using System;
using NUnit.Framework;
using BorrehSoft.Utilities.Parsing.Parsers;
using BorrehSoft.Utilities.Collections;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Unit
{
	public class SessionTest
	{
		protected virtual Map<object> GivenReferences ()
		{
			var references = new Map<object>();
			return references;
		}

		protected ParsingSession GivenSessionWithText (string parseable)
		{
			return new ParsingSession (parseable, null, false, GivenReferences());
		}

		protected ParsingSession GivenSessionWithText (string parseable, string deepen)
		{
			var session = GivenSessionWithText (parseable);
			session.DeepenContext (deepen);
			return session;
		}
	}
}
