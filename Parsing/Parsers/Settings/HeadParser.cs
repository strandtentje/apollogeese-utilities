using System;
using BorrehSoft.Utilities.Collections.Settings;
using System.Collections.Generic;
using System.Linq;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class HeadParser : Parser
	{
		SettingsSyntax Syntax;
		DiamondFile DiamondParser;
        SelectorString SelectorParser;
		IdentifierParser NameParser;
		ConcatenationParser ConfigurationParser;

		public HeadParser(Parser expressionParser, SettingsSyntax syntax)
		{
			this.Syntax = syntax;
			this.DiamondParser = new DiamondFile (syntax);
			this.NameParser = new IdentifierParser ();
            this.SelectorParser = new SelectorString(syntax);

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
                var fn = headValue.ToString();
                if (fn.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
                    definition[Syntax.HeadIdentifier] = this.Syntax.ExtAnonHeadName[".sql"];
                else if (fn.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase))
                    definition[Syntax.HeadIdentifier] = this.Syntax.ExtAnonHeadName[".html"];
                else
                    definition[Syntax.HeadIdentifier] = this.Syntax.DefaultAnonHeadName;

			}

            Settings configuration = null;
			List<Tuple<string, object>> candidate;
			if (Parse<Tuple<string, object>>.TryListUsing (ConfigurationParser, session, out candidate)) {
				configuration = Settings.FromTuples (candidate);
			}

            if (Parse<string>.TryUsing(SelectorParser, session, out string selector))
            {
                configuration[Syntax.SelectSetting] = selector;
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

