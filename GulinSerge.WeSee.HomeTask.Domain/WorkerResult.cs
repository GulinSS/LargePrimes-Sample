using System;
using System.Collections.Generic;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	public class WorkerResult
	{
		public WorkerResult(Task mainTask, IEnumerable<ulong> primes)
		{
			MainTask = mainTask;
			Primes = primes;
		}

		public Task MainTask { get; set; }
		public IEnumerable<ulong> Primes { get; set; }
	}
}