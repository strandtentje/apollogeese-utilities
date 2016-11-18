using System;
using System.Text;
using System.Collections.Generic;

namespace BorrehSoft.Utensils.Parsing.Parsers.Settings
{
	public class StringConcatenationParser : ConcatenationParser
	{
		public StringConcatenationParser () : base('/', '/', '|')
		{
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

