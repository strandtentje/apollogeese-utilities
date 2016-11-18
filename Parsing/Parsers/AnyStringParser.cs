using System;

namespace BorrehSoft.Utensils.Parsing.Parsers
{
	public class AnyStringParser : AnyParser
	{
		public AnyStringParser () : base (
			new FilenameParser (),
			new ReferenceParser (),
			new StringParser ()
		) { }
	}
}

