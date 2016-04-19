using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Parsing
{
	/// <summary>
	/// Name value piper.
	/// </summary>
	public class NameValuePiper<T1>
	{
		/// <summary>
		/// Name value callback.
		/// </summary>
		public delegate void NameValueCallback (string name, string value);

		/// <summary>
		/// Name value reader callback.
		/// </summary>
		public delegate void NameValuePipeCallback(T1 reader, NameValueCallback callback);

		/// <summary>
		/// Gets the incoming callback.
		/// </summary>
		/// <value>The incoming callback.</value>
		public NameValuePipeCallback IncomingCallback { get; private set; }

		/// <summary>
		/// Gets the time out.
		/// </summary>
		/// <value>The time out.</value>
		public TimeSpan TimeOut { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Parsing.NameValuePiper`2"/> class.
		/// </summary>
		/// <param name="incoming">Incoming.</param>
		/// <param name="outgoing">Outgoing.</param>
		public NameValuePiper(NameValuePipeCallback incoming, TimeSpan timeOut)
		{
			this.IncomingCallback = incoming;
			this.TimeOut = timeOut;
		}

		/// <summary>
		/// Run this instance.
		/// </summary>
		public bool TryRun(T1 source, NameValueCallback outgoing) 
		{
			bool success;

			CancellationTokenSource parsingStopper = new CancellationTokenSource ();
			CancellationToken parseStoppingToken = parsingStopper.Token;

			Task incomingTask = Task.Run (delegate() {
				this.IncomingCallback(source, outgoing);
			}, parseStoppingToken);

			if (incomingTask.Wait (this.TimeOut)) {
				success = true;
			} else {
				parsingStopper.Cancel ();				
				success = false;
			}

			return success;
		}
	}
}

