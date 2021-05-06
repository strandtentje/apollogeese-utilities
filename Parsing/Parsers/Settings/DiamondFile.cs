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
		Parser opener, closer, innerParser;

		object dummy;

		public DiamondFile(SettingsSyntax syntax, Parser o = null, Parser c = null, Parser i = null) {
			this.opener = o ?? new CharacterParser(syntax.AnonymousHeadOpener);
			this.closer = c ?? new CharacterParser(syntax.AnonymousHeadCloser);
			this.innerParser = i ?? new AnyStringParser ();
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

    /// <summary>
    /// Used for concisely specifying resource selection on services using a [name] block
    /// after the (settings="settings") bit.
    /// </summary>
    public class SelectorString : DiamondFile
    {
        public SelectorString(SettingsSyntax s) : base(s, 
            new CharacterParser(s.ArrayOpen), 
            new CharacterParser(s.ArrayClose),
            new IdentifierParser())
        {

        }
    }
}

