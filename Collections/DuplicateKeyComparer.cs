using System;
using System.Collections.Generic;

namespace Collections
{
	/// <summary>
	/// Comparer for comparing two keys, handling equality as beeing greater
	/// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	{
		// you saw it here first: http://stackoverflow.com/questions/5716423
		public virtual int Compare (TKey x, TKey y)
		{
			int result = x.CompareTo (y);

			if (result == 0)
				return 1;   // Handle equality as beeing greater
		else
				return result;
		}
	}
}