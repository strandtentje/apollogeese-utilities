using System;
using BorrehSoft.Utensils.Collections.Settings;
using System.Collections.Generic;

namespace BorrehSoft.Utensils.Parsing.Parsers.SettingsParsers
{
	public class HeadParser : Parser
	{
		SettingsSyntax Syntax;
		DiamondFile DiamondParser;
		IdentifierParser NameParser;
		ConcatenationParser ConfigurationParser;

		public HeadParser(Parser expressionParser, SettingsSyntax syntax)
		{
			this.Syntax = syntax;
			this.DiamondParser = new DiamondFile (syntax);
			this.NameParser = new IdentifierParser ();

			ConfigurationParser = new ConstructorParser (syntax) {
				InnerParser = new SubstitutionAssignmentParser (
					syntax.HeadShorthand, 
					syntax.HeadSubstitution, 
					syntax.HeadAssign
				) {
					InnerParser = expressionParser
				}
			};
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			Settings definition = (session.References [session.ContextName] as Settings) ?? new Settings ();

			definition [Syntax.HeadMarker] = session.GetLineHint ();

			string headName = null; object headValue = null;
			if (Parse<string>.TryUsing (NameParser, session, out headName)) {
				definition [Syntax.HeadIdentifier] = headName;
			} else if (Parse<object>.TryUsing (DiamondParser, session, out headValue)) {
				definition [Syntax.HeadIdentifier] = this.Syntax.AnonymousHeadName;
			}


			Settings configuration = null;
			List<Tuple<string, object>> candidate;
			if (Parse<Tuple<string, object>>.TryListUsing (ConfigurationParser, session, out candidate)) {
				configuration = Settings.FromTuples (candidate);
			}

			if (configuration == null) {
				if (headName != null) {
					throw new ParsingException (session, ConfigurationParser, session.Ahead, session.Trail);
				} else if (headValue != null) {
					configuration = new Settings ();
				}
			}

			if (configuration != null) {
				if (headValue != null) {
					configuration [Syntax.HeadAnonymousValueKey] = headValue;
				}
				definition [Syntax.ConfigurationKey] = configuration;
			}

			result = definition;
			return 1;
		}
	}
}

