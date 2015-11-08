using System;
using BorrehSoft.Utensils.Collections.Settings;
using BorrehSoft.Utensils.Collections.Maps;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace BorrehSoft.Utensils.Collections
{
	class BlockingPool<T> : IDisposable
	{
		public Func<T> Constructor {
			get;
			private set;
		}

		public Action<T> Deconstructor {
			get;
			private set;
		}



		public Func<T, bool> Checker {
			get;
			private set;
		}

		private bool DefaultChecker(T a) {
			return true;
		}

		public Queue<T> UnderlyingPool { get; private set; }

		FifoPhore underlyingSignalling;

		public int PoolSize {
			get {
				return UnderlyingPool.Count;
			}
			private set {
				UnderlyingPool = new Queue<T> (value);
				underlyingSignalling = new FifoPhore (value);

				for (int i = 0; i < value; i++) {
					UnderlyingPool.Enqueue (Constructor ());
				}
			}
		}

		public BlockingPool(int poolsize, Func<T> constructor, Action<T> deconstructor, Func<T, bool> checker = null) {
			this.Constructor = constructor;
			this.Deconstructor = deconstructor;
			this.Checker = DefaultChecker;
			if (checker != null)
				this.Checker = DefaultChecker;
			this.PoolSize = poolsize;
		}

		public void Fetch (Action<T> callback)
		{
			underlyingSignalling.WaitOne ();
			T candidate = UnderlyingPool.Dequeue ();
			if (!Checker (candidate)) {
				candidate = Constructor ();
			}
			callback (candidate);
			UnderlyingPool.Enqueue (candidate);
			underlyingSignalling.Release ();
		}

		public void Dispose() {

		}
	}

}

