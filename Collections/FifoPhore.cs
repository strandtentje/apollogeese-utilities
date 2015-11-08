using System;
using System.Collections.Generic;
using System.Threading;

namespace Borrehsoft.Utensils.Collections
{
	/// <summary>
	/// Evil lovebaby between semaphore and queue.
	/// </summary>
	public class FifoPhore
	{
		object overLock = new object();

		public List<Thread> WaitingThreads { get; private set; }

		public int CurrentToken { get; private set; }

		public FifoPhore(int StartToken)
		{
			this.CurrentToken = StartToken;
			this.WaitingThreads = new List<Thread>();
		}

		/// <summary>
		/// Wait for turn
		/// </summary>
		public void WaitOne()
		{
			bool waitMode = false; 
			Thread waitingThread = Thread.CurrentThread;
			
			lock (overLock)
			{
				if (CurrentToken > 0)    
					// We are free to go because there are tokens
					this.CurrentToken--;
				else
				{
					// We ran out of tokens and thus need to enqueue.
					WaitingThreads.Add(waitingThread);

					waitMode = true;
					// In waitmode, we acquire a lock on ourselves (rawr)...
					Monitor.Enter(waitingThread);
				}
			}

			if (waitMode)
			{
				// And then seemingly create a deadlock, but check void Exit!
				Monitor.Wait(waitingThread);
				// We were pulsed and thusly permanently release our own lock
				Monitor.Exit(waitingThread);
			}
		}

		/// <summary>
		/// Conclude turn
		/// </summary>
		public void Release()
		{
			lock (overLock)
			{
				if (WaitingThreads.Count > 0) {
					// If threads were waiting, we won't increase the token
					// but instead allow the next thread in by releasing itself
					// from its own lock.
					Thread nextThread = WaitingThreads [0];
					WaitingThreads.RemoveAt (0);                
						
					Monitor.Pulse (nextThread);                    
				} else {
					// If no threads were waiting, we will increase the token
					this.CurrentToken++;
				}
			}
		}
	}
}