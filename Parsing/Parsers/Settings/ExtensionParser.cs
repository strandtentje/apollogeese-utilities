using System;
using BorrehSoft.Utilities.Collections.Settings;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class ExtensionParser : Parser
	{
		CharacterParser ExtensionMarker;
		ReferenceParser ReferenceParser;
		Parser InnerParser;
		string Substitution;

		public ExtensionParser(char marker, string substitution, Parser innerParser)
		{
			ExtensionMarker = new CharacterParser (marker);
			ReferenceParser = new ReferenceParser ();
			InnerParser = innerParser;
			Substitution = substitution;
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			if (Parse<char>.Try (ExtensionMarker, session)) {
				var chainDefinition = Parse<Settings>.Using (ReferenceParser, session);

				if (chainDefinition == null) {
					session.DeepenContext (Substitution);
					chainDefinition = Parse<Settings>.Using (InnerParser, session);
					session.SurfaceContext (Substitution);
				}

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

