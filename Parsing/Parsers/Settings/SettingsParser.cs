using System;
using BorrehSoft.Utensils.Parsing;
using BorrehSoft.Utensils.Parsing.Parsers;
using System.Collections.Generic;
using System.Globalization;
using BorrehSoft.Utensils.Log;
using System.IO;
using BorrehSoft.Utensils.Collections.Maps;

namespace BorrehSoft.Utensils.Collections.Settings
{
	/// WARNING: THIS IS AN UGLY BASTARD

	/// <summary>
	/// Settings parser.
	/// </summary>
	public class SettingsParser : ConcatenationParser
	{
		public override string ToString ()
		{
			return "Settings, accolade-enclosed block with zero or more assignments";
		}

		AnyParser ValueParser;
		AnyParser DefaultValueParser;
		AnyParser ExpressionParser;
		AnyParser NumericParser;
		AnyParser StringishParser;
		ConcatenationParser ConcatenatedStringParser;
		StructAssignmentParser AssignmentParser, OverrideAssignmentParser;
		IdentifierParser TypeIDParser;
		ConcatenationParser ModconfParser;
		CharacterParser SuccessorMarker = new CharacterParser ('&');
		CharacterParser ChainMarker = new CharacterParser (':');
		DiamondFile ModuleParser;
		ReferenceParser PresetBranchesParser = new ReferenceParser ();
		ReferenceParser ChainedBranchParser = new ReferenceParser ();

		/// <summary>
		/// Nulls the parser. (Monodevelop generated this documentation and
		/// I can't stop laughing so I'm going to leave this here for now)
		/// </summary>
		/// <returns><c>true</c>, if parser was nulled, <c>false</c> otherwise.</returns>
		/// <param name="data">Data.</param>
		/// <param name="value">Value.</param>
		private bool nullParser(string data, out object value) 
		{
			value = null;
			return data == "null";
		}


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

			// Secretary.Report (6, session.ParsingProfiler.FinalizeIntoReport().ToString());

			return config;
		}

		public static Settings FromJson (string data)
		{
			ParsingSession session = new ParsingSession(data, new WhitespaceParser());
			SettingsParser parser = new SettingsParser(entitySe: ',', couplerChar: ':');

			object result;

			if (parser.Run(session, out result) < 0)
				return new Settings();

			return (Settings) result;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BorrehSoft.Utensils.Settings.SettingsParser"/> class.
		/// </summary>
		public SettingsParser(
			char startBlock = '{', char endBlock = '}', char entitySe = ';', 
			char startArr = '[', char endArr = ']', char arrSe = ',',
			char couplerChar = '=') : base(startBlock, endBlock, entitySe)
		{
			ConcatenationParser listParser = new ConcatenationParser (startArr, endArr, arrSe);

			NumericParser = new AnyParser (
				new ValueParser<decimal> (decimalParse),
				new ValueParser<int> (int.TryParse), 
				new ValueParser<long> (long.TryParse),
				new ValueParser<float> (floatParse));

			ValueParser = new AnyParser (
				NumericParser,
				new ValueParser<bool> (bool.TryParse, "(True|False|true|false)"));
			
			StringishParser = new AnyParser (
				new FilenameParser (),
				new ReferenceParser (),
				new StringParser ());

			ConcatenatedStringParser = new ConcatenationParser ('/', '/', '|', true);
			ConcatenatedStringParser.InnerParser = StringishParser;

			DefaultValueParser = new AnyParser (
				NumericParser, 
				new FilenameParser (),
				new StringParser (),
				ConcatenatedStringParser);

			ExpressionParser = new AnyParser (
				ValueParser, 
				StringishParser,
				ConcatenatedStringParser,
				listParser, 
				this
			);			

			TypeIDParser = new IdentifierParser ();
			ModconfParser = new ConstructorParser (DefaultValueParser);

			ModuleParser = new DiamondFile (new AnyParser(
				new FilenameParser(),
				new StringParser(),
				new ReferenceParser()
			));

			listParser.InnerParser = ExpressionParser;

			AssignmentParser = new StructAssignmentParser ("->", "_branch", couplerChar);
			AssignmentParser.InnerParser = ExpressionParser;
			this.InnerParser = AssignmentParser;

			OverrideAssignmentParser = new StructAssignmentParser ("<-", "_override", couplerChar);
			OverrideAssignmentParser.InnerParser = ExpressionParser;
			ModconfParser.InnerParser = OverrideAssignmentParser;
		}

        private bool floatParse(string data, out float value)
        {
            return float.TryParse(
				data, 
				NumberStyles.Float, 
				CultureInfo.InvariantCulture.NumberFormat, 
				out value);
        }

		private bool decimalParse(string data, out decimal value) {
			value = 0;
			if (data.StartsWith ("!")) {
				return decimal.TryParse (
					data.Substring (1), 
					NumberStyles.Currency, 
					CultureInfo.InvariantCulture.NumberFormat,
					out value);
			} else {
				return false;
			}
		}

		/// <summary>
		/// Bah, wat vies.
		/// </summary>
		/// <param name="assignments">Assignments.</param>
		/// <param name="target">Target.</param>
		private void AssignmentsToSettings(object assignments, Settings target)
		{			
			foreach (object assignment in (assignments as IEnumerable<object>)) {
				Tuple<string, object> t = assignment as Tuple<string, object>;
				target [t.Item1] = t.Item2;
			}
		}


		/// <summary>
		/// Parsing Method for the <see cref="BorrehSoft.Utensils.Settings"/> type.
		/// </summary>
		/// <returns>
		/// Succes value; zero or higher when succesful.
		/// </returns>
		/// <param name='session'>
		/// Session in which this parsing action will be conducted.
		/// </param>
		/// <param name='result'>
		/// Result of this parsing action
		/// </param>
		internal override int ParseMethod (ParsingSession session, out object result)
		{
			object assignments, uncastTypeid, uncastModconf = null;
			Settings rootconf, modconf = new Settings ();

			rootconf = session.References [session.ContextName] as Settings;
			if (rootconf == null)
				rootconf = new Settings ();

			int successCode = -1;
			bool Identifier;
			bool Module = false;
			object firstSetting;

			rootconf ["_configline"] = string.Format (
				"{0}:{1}", session.SourceFile.FullName, session.CurrentLine);

			Identifier = TypeIDParser.ParseMethod (session, out uncastTypeid) > 0;
			if (!Identifier) {
				Module = ModuleParser.ParseMethod (session, out firstSetting) > 0;
				if (Module) {
					modconf ["default"] = firstSetting;
					uncastTypeid = "Module";
				}
			}

			if (ModconfParser.ParseMethod (session, out uncastModconf) > 0) {
				AssignmentsToSettings (uncastModconf, modconf);
			} else if (Identifier) {
				throw new ParsingException (session, ModconfParser, session.Ahead, session.Trail);
			}

			if (Identifier || Module) {
				string typeid = uncastTypeid as string;

				rootconf ["type"] = typeid;
				rootconf ["modconf"] = modconf;

				successCode = 1;
			}

			object referenceCandidate;

			if (PresetBranchesParser.ParseMethod (session, out referenceCandidate) > 0) {
				if (referenceCandidate is Settings) {
					Settings referred = (Settings)referenceCandidate;
					foreach (KeyValuePair<string, object> setting in referred.Dictionary) {
						if (!setting.Key.StartsWith ("_")) {
							rootconf [setting.Key] = setting.Value;
						}
					}
				} else {
					throw new ParsingException (session, PresetBranchesParser, session.Ahead, session.Trail);
				}
			} 

			object characterCandidate;

			if (ChainMarker.ParseMethod (session, out characterCandidate) > 0) {
				object chainCandidate;
				if (ChainedBranchParser.ParseMethod (session, out chainCandidate) > 0) {
					if (!(chainCandidate is Settings)) {
						throw new ParsingException (
							session, 
							ChainedBranchParser, 
							session.Ahead, 
							session.Trail
						);
					}
				} else if (this.Run(session, out chainCandidate) < 1) {
					throw new ParsingException (
						session, 
						this, 
						session.Ahead,
						session.Trail
					);
				}

				rootconf ["_with_branch"] = chainCandidate;
			}

			if (base.ParseMethod (session, out assignments) > 0) {
				AssignmentsToSettings(assignments, rootconf);

				successCode = 2;
			}

			if (SuccessorMarker.Run (session, out characterCandidate) > 0) {
				object successorCandidate;
				if (this.Run (session, out successorCandidate) > 0) {
					// good fuck thats ugly
					rootconf ["_successor_branch"] = successorCandidate;
				}
			}

			result = rootconf;

			return successCode;
		}	
	}
}