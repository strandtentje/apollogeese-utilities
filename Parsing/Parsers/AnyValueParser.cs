using System;

namespace BorrehSoft.Utilities.Parsing.Parsers
{
	public class AnyValueParser : AnyParser
	{
		public AnyValueParser () : base(new NumericParser(), new BoolParser())
		{
		}
	}
}

