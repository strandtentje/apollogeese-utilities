using System;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Parsers
{
	public class SequenceParser : Parser
	{
		string Sequence { get ; set; }
		List<Parser> parserSequence = new List<Parser>();

		public SequenceParser (string sequence)
		{
			this.Sequence = sequence;

			foreach (char character in sequence) {
				parserSequence.Add (new CharacterParser (character));
			}
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			result = "";

			var bookmark = session.Offset;

			foreach (Parser parserInSequence in parserSequence) {
				if (parserInSequence.Run (session) < 0) {
					session.Offset = bookmark;
					return -1;
				}
			}

			result = this.Sequence;

			return Sequence.Length;
		}
	}
}

