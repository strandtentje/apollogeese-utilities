using System;

namespace BorrehSoft.Utensils.Parsing.Parsers
{
	public class AnyValueParser : AnyParser
	{
		public AnyValueParser () : base(new NumericParser(), new BoolParser())
		{
		}
	}
}

