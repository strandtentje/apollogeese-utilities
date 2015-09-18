using System;
using BorrehSoft.Utensils.Parsing;
using BorrehSoft.Utensils.Parsing.Parsers;

namespace BorrehSoft.Utensils
{
	public class DiamondFile : Parser
	{
		CharacterParser opener = new CharacterParser('<');
		CharacterParser closer = new CharacterParser('>');
		FilenameParser file = new FilenameParser();

		object dummy;

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			int filenameLength = -1;

			result = null;

			if (opener.ParseMethod (session, out dummy) > 0) {
				if (file.ParseMethod (session, out result) > 0) {
					if (closer.ParseMethod (session, out dummy) > 0) {
						filenameLength = ((string)result).Length;
					}
				}
			}

			return filenameLength;
		}
	}
}

