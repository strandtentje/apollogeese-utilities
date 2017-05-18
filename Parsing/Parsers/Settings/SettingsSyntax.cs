using System;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class SettingsSyntax
	{
		public virtual char BodyOpen { get { return '{'; } }
		public virtual char BodyClose { get { return '}'; } }
		public virtual char BodyDelimiter { get { return ';'; } }

		public virtual string BodyShorthand { get { return "->"; } }
		public virtual string BodySubstitution { get { return "_branch"; } }
		public virtual char BodyAssign { get { return '='; } }

		public virtual string ConfigurationKey { get { return "modconf"; } }

		public virtual string HeadMarker { get { return "_configline"; } }
		public virtual string HeadIdentifier { get { return "type"; } }
		public virtual string HeadAnonymousValueKey { get { return "default"; } }

		public virtual char HeadOpen { get { return '('; } }
		public virtual char HeadClose { get { return ')'; } }
		public virtual char HeadDelimiter { get { return ','; } }

		public virtual char AnonymousHeadOpener { get { return '<'; } }
		public virtual char AnonymousHeadCloser { get { return '>'; } }
		public virtual string AnonymousHeadName { get { return "Module"; } }

		public virtual string HeadShorthand { get { return "<-"; } }
		public virtual string HeadSubstitution { get { return "_override"; } }
		public virtual char HeadAssign { get { return '='; } }

		public virtual char StringConcatenationOpen { get { return '/'; } }
		public virtual char StringConcatenationClose { get { return '/'; } }
		public virtual char StringConcatenationDelimiter { get { return '|'; } }

		public virtual char ArrayOpen { get { return '['; } }
		public virtual char ArrayClose { get { return ']'; } }
		public virtual char ArrayDelimiter { get { return ','; } }

		public virtual char Chainer { get { return ':'; } }
		public virtual char Successor { get { return '&'; } }
		public virtual string ChainSubstitution { get { return "_with_branch"; } }
		public virtual string SuccessorSubstitution { get { return "_successor_branch"; } }

		public virtual char TernaryPositiveToken { get { return '?'; } }
		public virtual char TernaryNegativeToken { get { return ':'; } }
		public virtual string TernaryPositiveSubstitution { get { return "successful_branch"; } }
		public virtual string TernaryNegativeSubstitution { get { return "failure_branch"; } }
	}
}

