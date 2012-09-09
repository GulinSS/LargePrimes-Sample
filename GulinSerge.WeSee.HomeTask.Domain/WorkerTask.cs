using System.Collections.Generic;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	public class WorkerTask
	{
		public WorkerTask(Task mainTask, IEnumerable<ulong> knownPrimes)
		{
			MainTask = mainTask;
			KnownPrimes = knownPrimes;
		}

		public Task MainTask { get; set; }
		public IEnumerable<ulong> KnownPrimes { get; set; }
	}
}