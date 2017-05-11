using System;

namespace BorrehSoft.Utilities.Log.Profiling
{
	public class DummyProfiler : ICallbackProfiler
	{
		public long TotalTicksSpent { get { return 0; } }

		public long MeasurementCount { get { return 0; } }

		public long TicksPerMeasurement { get { return 0; } }

		public void Reset ()
		{
			
		}

		public void Measure (Action runMeasurement)
		{
			runMeasurement ();
		}
	}
}

