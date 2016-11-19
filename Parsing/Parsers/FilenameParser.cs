using System;
using System.IO;

namespace BorrehSoft.Utilities.Parsing.Parsers
{
	public class FilenameParser : StringParser
	{
		public override string ToString ()
		{
			return string.Format ("Like a StringParser, but with an f-prefix that includes the current working directory.");
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			if ((session.Data [session.Offset++] == 'f') && (session.Data[session.Offset] == '"')) {
				object fileNameObj;
				string fileName, fullPath;
				base.ParseMethod (session, out fileNameObj);
				fileName = (string)fileNameObj;
                

				fullPath = string.Format("{0}{1}{2}",
                    session.WorkingDirectory,
                    Path.DirectorySeparatorChar,   
                    fileName);

				result = fullPath;
				return fullPath.Length;
			} else {
				session.Offset--;
				result = "";
				return -1;
			}
		}
	}
}

