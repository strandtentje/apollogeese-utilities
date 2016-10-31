using System;

namespace Collections
{
	public class ReverseDuplicateKeyComparer<TKey> : DuplicateKeyComparer<TKey> where TKey : IComparable
	{
		public override int Compare (TKey x, TKey y)
		{
			return -base.Compare (x, y);
		}
	}
}

