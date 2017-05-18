using System;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Parsing.Parsers
{
	public static class Parse<T>
	{
		public static T Using(Parser parser, string data) {
			object candidate;
			return Using (parser, new ParsingSession (data));
		}

		public static Tuple<T, T> CoupleUsing(Parser first, Parser then, ParsingSession session) {
			object firstResult, nextResult;
			if ((first.Run(session, out firstResult) > 0) && (then.Run(session, out nextResult) > 0)) {
				return new Tuple<T, T>((T)firstResult, (T)nextResult);
			} else {
				return default(Tuple<T, T>);
			}
		}

		public static T Using(Parser parser, ParsingSession session) {
			object candidate;
			if (parser.Run (session, out candidate) > 0) {
				return (T)candidate;
			} else {
				return default(T);
			}
		}

		public static bool Try(Parser parser, ParsingSession session) {
			object candidate;
			return parser.Run (session, out candidate) > 0;
		}

		public static bool TryUsing(Parser parser, ParsingSession session, out T value) {
			object candidate;
			if (parser.Run (session, out candidate) > 0) {
				value = (T)candidate;
				return true;
			} else {
				value = default(T);
				return false;
			}
		}

		public static bool TryListUsing(Parser parser, ParsingSession session, out List<T> value) {
			List<object> candidate;
			if (Parse<List<object>>.TryUsing (parser, session, out candidate)) {
				value = candidate.ConvertAll<T> (delegate(object input) {
					return (T)input;
				});
				return true;
			} else {
				value = default(List<T>);
				return false;
			}
		}
	}
}

