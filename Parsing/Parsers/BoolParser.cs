using System;

namespace BorrehSoft.Utensils.Parsing.Parsers
{
	public class BoolParser : ValueParser<bool>
	{
		public BoolParser (): base(bool.TryParse, "(True|False|true|false)")
		{
		}
	}
}

