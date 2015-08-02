using System;

namespace BorrehSoft.Utensils.Parsing.Parsers
{
	public class CharacterParser : Parser
	{
		public char TargetCharacter { get; private set; }

		public CharacterParser (char TargetCharacter)
		{
			this.TargetCharacter = TargetCharacter;
		}

		public override string ToString ()
		{
			return string.Format ("Single Character: {0}", TargetCharacter);
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			if (session.Data.Length > session.Offset) {
				if (session.Data [session.Offset] == TargetCharacter) {
					session.Offset++;
					result = TargetCharacter;
					return 1;
				}
			}
			result = null;
			return -1;
		}
	}
}

