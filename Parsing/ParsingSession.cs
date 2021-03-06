using System;
using System.IO;
using BorrehSoft.Utilities.Parsing.Parsers;
using System.Collections.Generic;
using BorrehSoft.Utilities.Collections;
using BorrehSoft.Utilities.Log.Profiling;

namespace BorrehSoft.Utilities.Parsing
{
	/// <summary>
	/// Parsing Session State
	/// </summary>
	public class ParsingSession
	{
		public string WorkingDirectory { get; set; }

		private Stack<string> context = new Stack<string>();

		public Parser whitespaceParser;
		/// <summary>
		/// Gets the data to be parsed
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		public string Data { get; set; }

		public bool ProfilingEnabled { get; private set; }
		public Profiler ParsingProfiler;

		/// <summary>
		/// Context Stack Name, i.e., if we're currently processing the item Alittle in Had of Mary, this will say
		/// Mary.Had.Alittle
		/// </summary>
		/// <value>
		/// The name of the context.
		/// </value>
		public string ContextName {
			get {
				string[] contextname = context.ToArray();
				Array.Reverse(contextname);
				return string.Join(".", contextname);
			}
		}

		public string GetAhead (int nchar = 24)
		{
			int len = Math.Min(nchar, Data.Length - this.Offset);

			return Data.Substring(this.Offset, len);
		}

		public string GetTrail(int nchar = 24)
		{
			int startpos = this.Offset - Math.Min(nchar, this.Offset);
			int len = Math.Min(nchar, this.Offset);

			return Data.Substring(startpos, len);
		}

		public object GetLineHint ()
		{
			return string.Format (
				"{0}:{1}", 
				SourceFile.FullName, 
				CurrentLine + 1
			);
		}

		public string Ahead { get { return GetAhead (); } }

		public string Trail { get { return GetTrail (); } }

		/// <summary>
		/// Gets a list representing context names from parentmost to childmost.
		/// </summary>
		/// <value>
		/// The context.
		/// </value>
		public Stack<string> Context {
			get { return context; }
			private set { 
				this.context = value;
			}
		}

		public Map<object> References { get; private set; }

		/// <summary>
		/// Gets or sets the current line index, readout intended
		/// for reporting parsing errors. Write intended for Parser-
		/// components
		/// </summary>
		/// <value>
		/// The current line.
		/// </value>
		public int CurrentLine { get; set; }
		/// <summary>
		/// Gets or sets the current cursor position within the
		/// serial data.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public int Offset { get; set; }
		/// <summary>
		/// Gets or sets the current column index.
		/// </summary>
		/// <value>The current column.</value>
		public int CurrentColumn { get; set; }

		public ParsingSession(string data, Parser whitespaceParser = null, bool profilingEnabled = false, Map<object> references = null)
		{
			this.whitespaceParser = whitespaceParser;
			this.References = references ?? new Map<object>();
			this.Data = data;
			this.Offset = 0;
			this.CurrentLine = 0;
			this.ProfilingEnabled = profilingEnabled;
			if (profilingEnabled)
				this.ParsingProfiler = new Profiler();
		}

		/// <summary>
		/// Reads all contents from a file for usage in a ParsingSession
		/// </summary>
		/// <returns>
		/// The immediately usable <see cref="BorrehSoft.Utilities.Parsing.ParsingSession"/>
		/// </returns>
		/// <param name='file'>
		/// Filename to read serial data from.
		/// </param>
		public static ParsingSession FromFile(string file, bool profilingEnabled = false)
		{
			return FromFile (file, new WhitespaceParser (), profilingEnabled);
		}

		/// <summary>
		/// Reads all contents from a file for usage in a ParsingSession, and uses
		/// and alternative parser for whitespace regions (i.e. #operations)
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="file">File.</param>
		/// <param name="whitespaceParser">Whitespace parser.</param>
		public static ParsingSession FromFile(string file, Parser whitespaceParser, bool profilingEnabled = false)
		{
			ParsingSession result = new ParsingSession (File.ReadAllText (file), whitespaceParser, profilingEnabled);
			result.SourceFile = new FileInfo (file);
			result.WorkingDirectory = result.SourceFile.Directory.FullName;
			return result;
		}

		/// <summary>
		/// Gets the source file.
		/// </summary>
		/// <value>The source file.</value>
		public FileInfo SourceFile {
			get;
			private set;
		}

		/// <summary>
		/// Deepens the context with the supplied identifier.
		/// </summary>
		/// <param name='identifier'>
		/// Identifier.
		/// </param>
		public void DeepenContext (string identifier)
		{
			Context.Push(identifier);
		}

		public void ContextRegister(object reference)
		{
			References[ContextName] = reference;
		}

		/// <summary>
		/// Brings the context back to the surface by one.
		/// </summary>
		/// <param name='identifier'>
		/// Identifier.
		/// </param>
		public void SurfaceContext (string identifier)
		{
			if (Context.Peek() == identifier) {
				Context.Pop();
			} else {
				throw new Exception(string.Format(
					"A little accident occured where a deepened context wasn't surfaced properly. Line/Offset/Col/Context/Attempt: {0}/{1}/{2}/{3}/{4}",
					this.CurrentLine, this.Offset, this.CurrentColumn, this.ContextName, identifier));
			}
		}
	}
}

