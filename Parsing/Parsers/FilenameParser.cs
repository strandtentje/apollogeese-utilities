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
            result = "";

            if (session.Data[session.Offset + 1] == '"')
            {
                string prefix;
                char c = session.Data[session.Offset];

                if (c == 'f')
                    prefix = session.WorkingDirectory;
                else if (c == 'q')
                    prefix = string.Format("{0}{1}{2}",
                        session.WorkingDirectory, 
                        Path.DirectorySeparatorChar, 
                        "queries");
                else if (c == 't')
                    prefix = string.Format("{0}{1}{2}",
                        session.WorkingDirectory,
                        Path.DirectorySeparatorChar,
                        "templates");
                else
                    return -1;

                session.Offset++;
                base.ParseMethod(session, out object fileNameObj);
                string fileName = (string)fileNameObj;

                string fullPath = string.Format("{0}{1}{2}",
                    prefix,
                    Path.DirectorySeparatorChar,
                    fileName);

                result = fullPath;
                return fullPath.Length;
            }
            else
            {
                return -1;
            }
		}
	}
}

