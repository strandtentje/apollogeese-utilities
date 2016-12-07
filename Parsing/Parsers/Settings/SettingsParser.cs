using System;
using BorrehSoft.Utilities.Parsing;
using BorrehSoft.Utilities.Parsing.Parsers;
using System.Collections.Generic;
using System.Globalization;
using BorrehSoft.Utilities.Log;
using System.IO;
using BorrehSoft.Utilities.Collections.Maps;
using BorrehSoft.Utilities.Collections.Settings;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	/// <summary>
	/// Settings parser.
	/// </summary>
	public class SettingsParser : Parser
	{
		SettingsSyntax Syntax;
		HeadParser HeadParser;
		PredefinitionParser PredefParser;
		ExtensionParser ChainParser;
		BodyParser BodyParser;
		ExtensionParser SuccessorParser;

		public SettingsParser(SettingsSyntax proposedSyntax = null)
		{
			this.Syntax = proposedSyntax ?? new SettingsSyntax();

			ConcatenationParser listParser = new ConcatenationParser (
				this.Syntax.ArrayOpen, this.Syntax.ArrayClose, this.Syntax.ArrayDelimiter
			);
			AnyParser ExpressionParser = new AnyParser (
				new AnyValueParser(), 
				new AnyStringParser(),
				new StringConcatenationParser(this.Syntax),
				listParser, 
				this
			);			
			listParser.InnerParser = ExpressionParser;

			this.HeadParser = new HeadParser (ExpressionParser, this.Syntax);
			this.PredefParser = new PredefinitionParser();
			this.ChainParser = new ExtensionParser (this.Syntax.Chainer, this.Syntax.ChainSubstitution, this);
			this.BodyParser = new BodyParser (ExpressionParser, this.Syntax);
			this.SuccessorParser = new ExtensionParser (this.Syntax.Successor, this.Syntax.SuccessorSubstitution, this);
		}

		internal override int ParseMethod (ParsingSession session, out object result)
		{
			var headDefinition = Parse<Settings>.Using (HeadParser, session);
			var baseDefinitions = Parse<List<Settings>>.Using (PredefParser, session);
			var chainedDefinition = Parse<Settings>.Using (ChainParser, session);
			var bodyDefinition = Parse<Settings>.Using (BodyParser, session);
			var successorDefinition = Parse<Settings>.Using (SuccessorParser, session);

			if (chainedDefinition != null) {
				headDefinition [Syntax.ChainSubstitution] = chainedDefinition;
			}
			if (successorDefinition != null) {
				headDefinition [Syntax.SuccessorSubstitution] = successorDefinition;
			}

			baseDefinitions.Add (headDefinition);
			baseDefinitions.Add (bodyDefinition);

			var cleanDefinitions = baseDefinitions.FindAll (delegate(Settings obj) {
				return obj != null;
			});
			cleanDefinitions.Reverse ();

			result = Settings.FromMerge (cleanDefinitions.ToArray());

			return 1;
		}	
	}
}