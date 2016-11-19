using System;
using BorrehSoft.Utilities.Collections.Maps;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Collections
{
	public class IntMap<T> 
	{
		T[] numberedBranches = new T[0];

		public T Default = default(T);

		public T this[int key] {
			get {
				if (numberedBranches.Length > key) {
					if (key < 0) {
						return this.Default;
					} else {
                        try {
                            if (numberedBranches[key].Equals(default(T)))
                                return this.Default;                            
                            else
                                return numberedBranches[key];
                            
                        } catch(NullReferenceException ex) {
                            // go eat a bag of dicks, c#
                            return this.Default;
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
