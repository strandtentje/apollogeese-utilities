using System;

namespace BorrehSoft.Utilities.Log.Profiling
{
	public interface ICallbackProfiler
	{
		long TotalTicksSpent { get; }

		long MeasurementCount { get; }

		long TicksPerMeasurement { get; }

		void Reset ();

		void Measure (Action runMeasurement);
	}
}

