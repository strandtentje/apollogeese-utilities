using System;
using System.IO;
using System.Collections.Generic;

namespace BorrehSoft.Utilities
{
	public class Header : Row
	{
		public Header (Table table, IEnumerable<string> header) : base(table, header)
		{
		}
	}
}

