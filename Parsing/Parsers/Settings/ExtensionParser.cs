using System;
using BorrehSoft.Utensils.Collections.Settings;

namespace BorrehSoft.Utensils.Parsing.Parsers.SettingsParsers
{
	public class ExtensionParser : Parser
	{
		CharacterParser ExtensionMarker;
		ReferenceParser ReferenceParser;
		Parser InnerParser;

		public ExtensionParser(char marker, Parser innerParser)
		{
			ExtensionMarker = new CharacterParser (marker);
			ReferenceParser = new ReferenceParser ();
			InnerParser = innerParser;
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			if (Parse<char>.Try (ExtensionMarker, session)) {
				var chainDefinition = 
					Parse<Settings>.Using (ReferenceParser, session) ??
					Parse<Settings>.Using (InnerParser, session);

				if (chainDefinition == null) {
					throw new ParsingException (session, InnerParser, session.Ahead, session.Trail);
				}

				result = chainDefinition;
				return 1;
			} else {
				result = null;
				return -1;
			}
		}
	}
}

