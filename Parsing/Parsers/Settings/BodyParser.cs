using System;
using BorrehSoft.Utensils.Collections.Settings;
using System.Collections;
using System.Collections.Generic;

namespace BorrehSoft.Utensils.Parsing.Parsers.SettingsParsers
{
	public class BodyParser : Parser
	{
		ConcatenationParser BlockParser;

		public BodyParser(Parser innerParser, SettingsSyntax syntax) 
		{
			this.BlockParser = new ConcatenationParser (syntax.BodyOpen, syntax.BodyClose, syntax.BodyDelimiter) {
				InnerParser = new SubstitutionAssignmentParser (
					syntax.BodyShorthand, 
					syntax.BodySubstitution, 
					syntax.BodyAssign
				) {
					InnerParser = innerParser
				}
			};
		}

		private Tuple<string, object> CastToTuple (object input)
		{
			return (Tuple<string, object>)input;
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			List<Tuple<string, object>> tuples;
			if (Parse<Tuple<string, object>>.TryListUsing(this.BlockParser, session, out tuples)) {
				result = Settings.FromTuples (tuples);
				return 1;
			} else {
				result = null;
				return -1;
			}

		}
		
	}
}

