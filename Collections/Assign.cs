using System;
using System.Collections.Generic;

namespace BorrehSoft.Utilities.Collections
{
	public class MapAssignmentException : Exception
	{
		private MapAssignmentException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public static MapAssignmentException NoKeyForValue(Exception innerException)
		{
			return new MapAssignmentException("No key left for this value", innerException);
		}

		public static MapAssignmentException NoKeysForTuple(Exception innerException) 
		{
			return new MapAssignmentException("No keys left for this tuple", innerException);
		}
	}

	/// <summary>
	/// Utility class for assigning crap to Maps, taking into to careful consideration 
	/// </summary>
	public class Assign<T>
	{
		public Map<T> Map { get; private set; }
		public Queue<string> Keys { get; private set; }

		private Assign (Map<T> map, Queue<string> keys)
		{
			this.Map = map;
			this.Keys = keys;
		}

		public static Assign<T> ToMap(Map<T> map, params string[] keys)
		{
			return new Assign<T>(map, new Queue<string>(keys));
		}

		public Assign<T> With(params string[] keys)
		{
			foreach (var key in keys) {
				this.Keys.Enqueue(key);
			}
			return this;
		}

		public Assign<T> IfValueAvailable(T value)
		{
			string key;

			try { 
				key = Keys.Dequeue ();
			} catch(Exception ex) {
				throw MapAssignmentException.NoKeyForValue(ex);
			}

			if (value != null) {
				Map [key] = value;
			}

			return this;
		}

		public Assign<T> IfTupleAvailable(Tuple<T, T> value)
		{
			string key1, key2;

			try {
				key1 = Keys.Dequeue ();
				key2 = Keys.Dequeue ();
			} catch(Exception ex) {
				throw MapAssignmentException.NoKeysForTuple(ex);
			}

			if (value != null) {
				Map [key1] = value.Item1;
				Map [key2] = value.Item2;
			}

			return this;
		}
	}
}

