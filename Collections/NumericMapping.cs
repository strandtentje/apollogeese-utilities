using System;
using BorrehSoft.Utensils.Collections.Maps;
using System.Collections.Generic;

namespace BorrehSoft.Utensils
{
	public class NumericMapping<T> 
	{
		T[] numberedBranches = new T[0];

		public T Default = default(T);

		public T this[int key] {
			get {
				if (numberedBranches.Length > key) {
					if (key < 0) {
						return this.Default;
					} else {
						if (numberedBranches [key] == default(T)) {
							return this.Default;
						} else {
							return numberedBranches [key];
						}
					}
				} else {
					return this.Default;
				}
			} set {
				if (numberedBranches.Length <= key) {
					Array.Resize (ref numberedBranches, key + 1);
				}

				numberedBranches [key] = value;
			}
		}

		public void Set (string name, T service, string prefix = "")
		{
			if (name.StartsWith (prefix)) {
				int branchNumber;
				if (int.TryParse (name.Substring (prefix.Length), out branchNumber)) {
					this [branchNumber] = service;
				}
			}
		}
	}
}
