using System;
using System.Collections.Generic;
using BorrehSoft.Utensils.Collections.Settings;

namespace BorrehSoft.Utensils.Parsing.Parsers.SettingsParsers
{
	public class PredefinitionParser : Parser
	{
		ReferenceParser ReferenceParser = new ReferenceParser ();

		public PredefinitionParser ()
		{
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			List<Settings> predefinitions = new List<Settings> ();

			object referenceCandidate;
			while (ReferenceParser.ParseMethod (session, out referenceCandidate) > 0) {
				if (referenceCandidate is Settings) {
					predefinitions.Add ((Settings)referenceCandidate);
				} else {
					throw new ParsingException (session, ReferenceParser, session.Ahead, session.Trail);
				}
			} 

			result = predefinitions;
			return predefinitions.Count + 1;
		}
	}
}

