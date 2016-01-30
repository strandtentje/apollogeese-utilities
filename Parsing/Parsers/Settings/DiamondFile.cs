using System;
using BorrehSoft.Utensils.Parsing;
using BorrehSoft.Utensils.Parsing.Parsers;

namespace BorrehSoft.Utensils
{
	/// <summary>
	/// Not some sort of fancy trademark, but actually a pathspec between
	/// diamond operators <f"so/like/this.txt">
	/// </summary>
	public class DiamondFile : Parser
	{
		CharacterParser opener = new CharacterParser('<');
		CharacterParser closer = new CharacterParser('>');
		Parser innerParser;

		object dummy;

		public DiamondFile() {
			innerParser = new FilenameParser ();
		}

		public DiamondFile(Parser innerParser) {
			this.innerParser = innerParser;
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

