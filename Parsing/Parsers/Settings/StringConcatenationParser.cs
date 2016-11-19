using System;
using System.Text;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class StringConcatenationParser : ConcatenationParser
	{
		public StringConcatenationParser (SettingsSyntax syntax) : base(
			syntax.StringConcatenationOpen, 
			syntax.StringConcatenationClose,
			syntax.StringConcatenationDelimiter
		) {
			InnerParser = new AnyStringParser ();
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			if (base.ParseMethod (session, out result) > 0) {
				var target = (IEnumerable<object>)result;
				StringBuilder mergedString = new StringBuilder ();
				foreach (object component in target) {
					mergedString.Append (component);
				}
				result = mergedString.ToString ();
				return mergedString.Length + 1;
			} else {
				return -1;
			}
		}
	}
}

