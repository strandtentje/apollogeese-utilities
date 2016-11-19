using System;
using BorrehSoft.Utilities.Parsing;
using BorrehSoft.Utilities.Parsing.Parsers;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class ConstructorParser : ConcatenationParser
	{
		private Parser valueParser;

		public ConstructorParser (SettingsSyntax syntax) : base(
			syntax.HeadOpen, syntax.HeadClose, syntax.HeadDelimiter
		) {
			this.valueParser = new AnyParser (
				new NumericParser(), 
				new FilenameParser(),
				new StringParser(),
				new StringConcatenationParser(syntax)
			);
		}		

		protected override int ParseListBody (ParsingSession session, ref List<object> target)
		{			
			object defaultValue;
			bool thereIsMore = true;

			if (valueParser.ParseMethod (session, out defaultValue) > -1) {
				target.Add (new Tuple<string, object> ("default", defaultValue));
				thereIsMore = (coupler.Run(session) > 0);
			}

			if (thereIsMore) {
				return base.ParseListBody (session, ref target);
			} else {
				return closer.Run(session);
			}
		}
	}
}

