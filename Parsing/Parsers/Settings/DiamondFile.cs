using System;
using BorrehSoft.Utilities.Parsing;
using BorrehSoft.Utilities.Parsing.Parsers;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	/// <summary>
	/// Not some sort of fancy trademark, but actually a pathspec between
	/// diamond operators <f"so/like/this.txt">
	/// </summary>
	public class DiamondFile : Parser
	{
		CharacterParser opener, closer;
		Parser innerParser;

		object dummy;

		public DiamondFile(SettingsSyntax syntax) {
			this.opener  = new CharacterParser(syntax.AnonymousHeadOpener);
			this.closer = new CharacterParser(syntax.AnonymousHeadCloser);
			this.innerParser = new AnyStringParser ();
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			int filenameLength = -1;

			result = null;

			if (opener.ParseMethod (session, out dummy) > 0) {
				if (this.innerParser.ParseMethod (session, out result) > 0) {
					if (closer.ParseMethod (session, out dummy) > 0) {
						filenameLength = ((string)result).Length;
					}
				}
			}

			return filenameLength;
		}
	}
}

