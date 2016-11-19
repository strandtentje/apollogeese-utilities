using System;
using BorrehSoft.Utilities.Parsing.Parsers;
using BorrehSoft.Utilities.Parsing;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Parsers.SettingsParsers
{
	public class SubstitutionAssignmentParser : AssignmentParser
	{
		private string CouplerSuffix { get; set; }
		List<CharacterParser> couplerParsers = new List<CharacterParser>();

		public SubstitutionAssignmentParser(string couplerString, string couplerSuffix, char regularCoupler = '=') : base(regularCoupler) {
			foreach (char couplerPart in couplerString) {
				couplerParsers.Add (new CharacterParser (couplerPart));
			}

			this.CouplerSuffix = couplerSuffix;
		}

		protected override bool Couple (ParsingSession session, ref object identifier)
		{
			if (!base.Couple (session, ref identifier)) {
				foreach (CharacterParser parser in couplerParsers) {
					if (parser.Run (session) < 0) {
						return false;
					}
				}

				identifier = string.Concat (identifier, this.CouplerSuffix);
			}

			return true;
		}
	}
}

