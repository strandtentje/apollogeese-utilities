using System;
using BorrehSoft.Utensils.Log;
using System.IO;
using BorrehSoft.Utensils.Parsing;
using BorrehSoft.Utensils.Parsing.Parsers;

namespace BorrehSoft.Utensils.Collections.Settings
{
	public static class SettingsLoader
	{
		/// <summary>
		/// Acquires settings from the file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="file">File.</param>
		public static Settings FromFile(string file, string workingDirectory = null)
		{
			Secretary.Report (5, "Loading settings file ", file);

			if (!File.Exists (file)) {
				File.WriteAllText(file, "{}");
				Secretary.Report (5, file, " didn't exist. Has been created.");
			}

			ParsingSession session = ParsingSession.FromFile(file, new IncludeParser());

			if (workingDirectory == null) {
				Directory.SetCurrentDirectory (session.SourceFile.Directory.FullName);
			} else if (Directory.Exists(workingDirectory)) {
				Directory.SetCurrentDirectory(workingDirectory);
				session.WorkingDirectory = workingDirectory;
			}

			SettingsParser parser = new SettingsParser();
			object result;

			Settings config;

			if (parser.Run (session, out result) < 0)
				config = new Settings ();
			else 
				config = (Settings)result;

			config.SourceFile = session.SourceFile;

			Secretary.Report (5, "Settings finished loading from: ", file);

			return config;
		}
	}
}

