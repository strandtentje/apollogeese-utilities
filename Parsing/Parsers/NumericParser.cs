using System;
using System.Globalization;

namespace BorrehSoft.Utensils.Parsing.Parsers
{
	public class NumericParser : AnyParser
	{
		private static bool floatParse(string data, out float value)
		{
			return float.TryParse(
				data, 
				NumberStyles.Float, 
				CultureInfo.InvariantCulture.NumberFormat, 
				out value);
		}

		private static bool decimalParse(string data, out decimal value) {
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

		public NumericParser (): base (
			new ValueParser<decimal> (decimalParse),
			new ValueParser<int> (int.TryParse), 
			new ValueParser<long> (long.TryParse),
			new ValueParser<float> (floatParse)
		) { }
	}
}

