using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BorrehSoft.Utilities.Log.Profiling
{
	public class ThreadAwareProfiler
	{
		public ThreadAwareProfiler() {
			this.CorrectForNesting = true;
		}

		long ticksSpent = 0;

		long measurementsMade = 0;

		public long TotalTicksSpent { get { return this.ticksSpent; } }

		public long MeasurementCount { get { return this.measurementsMade; } }

		public long TicksPerMeasurement { get { return this.TotalTicksSpent / Math.Max(1, this.MeasurementCount); } }

		private static ListDictionary threadStopwatches = new ListDictionary ();

		public bool CorrectForNesting { get; set; }

		public void Reset ()
		{
			this.ticksSpent = 0; 
			this.measurementsMade = 0;
		}

		public void Measure(Action runMeasurementSubject) {

			int threadId = Thread.CurrentThread.ManagedThreadId;
			bool unstackProfiler = threadStopwatches.Contains (threadId) && this.CorrectForNesting;
			Stopwatch previousStopwatch = null;
			Stopwatch stopwatch = new Stopwatch ();

			if (unstackProfiler) {
				previousStopwatch = (Stopwatch)threadStopwatches [threadId];
				previousStopwatch.Stop ();
				threadStopwatches.Remove (threadId);
			}

			if (this.CorrectForNesting) {
				threadStopwatches.Add (threadId, stopwatch);
			}

			stopwatch.Start ();
			runMeasurementSubject ();
			stopwatch.Stop ();

			if (this.CorrectForNesting) {
				threadStopwatches.Remove (threadId);
			}

			if (unstackProfiler) {
				threadStopwatches.Add (threadId, previousStopwatch);
				previousStopwatch.Start ();
			}

			Interlocked.Add (ref ticksSpent, stopwatch.ElapsedTicks);
			Interlocked.Increment (ref measurementsMade);
		}
	}
}
