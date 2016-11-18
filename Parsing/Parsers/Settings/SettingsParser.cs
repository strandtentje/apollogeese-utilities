using System;
using BorrehSoft.Utensils.Parsing;
using BorrehSoft.Utensils.Parsing.Parsers;
using System.Collections.Generic;
using System.Globalization;
using BorrehSoft.Utensils.Log;
using System.IO;
using BorrehSoft.Utensils.Collections.Maps;
using BorrehSoft.Utensils.Parsing.Parsers.Settings;

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

		CharacterParser 
		SuccessorMarker = new CharacterParser ('&'),
		ChainMarker = new CharacterParser (':');
		DiamondFile DiamondParser = new DiamondFile ();
		ReferenceParser ReferenceParser = new ReferenceParser ();
		IdentifierParser NameParser = new IdentifierParser ();
		ConcatenationParser ConfigurationParser;

		/// <summary>
		/// Initializes a new instance of the <see cref="BorrehSoft.Utensils.Settings.SettingsParser"/> class.
		/// </summary>
		public SettingsParser(char entitySe = ';', char arrSe = ',', char couplerChar = '=') : base('{', '}', entitySe)
		{
			ConcatenationParser listParser = new ConcatenationParser ('[', ']', arrSe);
			AnyParser ExpressionParser = new AnyParser (
				new AnyValueParser(), 
				new AnyStringParser(),
				new StringConcatenationParser(),
				listParser, 
				this
			);			
			listParser.InnerParser = ExpressionParser;

			this.InnerParser = new SubstitutionAssignmentParser ("->", "_branch", couplerChar) {
				InnerParser = ExpressionParser
			};

			ConfigurationParser = new ConstructorParser () {
				InnerParser = new SubstitutionAssignmentParser ("<-", "_override", couplerChar) {
					InnerParser = ExpressionParser
				}
			};
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
			object assignments, uncastName, uncastConfiguration = null;
			Settings definition, configuration = new Settings ();

			definition = session.References [session.ContextName] as Settings;
			if (definition == null)
				definition = new Settings ();

			int successCode = -1;
			bool isNamed, isDiamond = false;

			definition ["_configline"] = session.GetLineHint ();

			isNamed = NameParser.ParseMethod (session, out uncastName) > 0;
			if (!isNamed) {
				object firstSetting;
				isDiamond = DiamondParser.ParseMethod (session, out firstSetting) > 0;
				if (isDiamond) {
					configuration ["default"] = firstSetting;
					uncastName = "Module";
				}
			}

			if (ConfigurationParser.ParseMethod (session, out uncastConfiguration) > 0) {
				AssignmentsToSettings (uncastConfiguration, configuration);
			} else if (isNamed) {
				throw new ParsingException (session, ConfigurationParser, session.Ahead, session.Trail);
			}

			if (isNamed || isDiamond) {
				string typeid = uncastName as string;

				definition ["type"] = typeid;
				definition ["modconf"] = configuration;

				successCode = 1;
			}

			object referenceCandidate;

			if (ReferenceParser.ParseMethod (session, out referenceCandidate) > 0) {
				if (referenceCandidate is Settings) {
					Settings referred = (Settings)referenceCandidate;
					foreach (KeyValuePair<string, object> setting in referred.Dictionary) {
						if (!setting.Key.StartsWith ("_")) {
							definition [setting.Key] = setting.Value;
						}
					}
				} else {
					throw new ParsingException (session, ReferenceParser, session.Ahead, session.Trail);
				}
			} 

			object characterCandidate;

			if (ChainMarker.ParseMethod (session, out characterCandidate) > 0) {
				object chainCandidate;
				if (ReferenceParser.ParseMethod (session, out chainCandidate) > 0) {
					if (!(chainCandidate is Settings)) {
						throw new ParsingException (
							session, 
							ReferenceParser, 
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

				definition ["_with_branch"] = chainCandidate;
			}

			if (base.ParseMethod (session, out assignments) > 0) {
				AssignmentsToSettings(assignments, definition);

				successCode = 2;
			}

			if (SuccessorMarker.Run (session, out characterCandidate) > 0) {
				object successorCandidate;
				if (this.Run (session, out successorCandidate) > 0) {
					// good fuck thats ugly
					definition ["_successor_branch"] = successorCandidate;
				}
			}

			result = definition;

			return successCode;
		}	
	}
}