using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl
{
	public class WorkerLocalImpl : Worker
	{
		private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);
		private readonly object _sync = new object();
		private readonly WorkerResultReceiver _receiver;
		private bool _free = true;
		private Thread _thread;


		public WorkerLocalImpl(WorkerResultReceiver receiver)
		{
			_receiver = receiver;
		}

		public void CalculationBegin(WorkerTask task)
		{
			_free = false;
			_thread = new Thread(() => ExecuteInThread(task));
			_thread.Start();
		}

		public bool IsFree()
		{
			_pauseEvent.WaitOne(Timeout.Infinite);
			return _free;
		}

		private void ExecuteInThread(WorkerTask task)
		{
			var primes = task.KnownPrimes.ToArray();
			List<ulong> primesResult = new List<ulong>();
			Dictionary<ulong, List<ulong>> ordered = new Dictionary<ulong, List<ulong>>();

			Parallel.ForEach(SplitTask(task.MainTask),
				I =>
				{
					var blockSize = CorrectBlockSize(task, I);
					var sieve = BuildSieve(blockSize);

					Sifting(sieve, blockSize, I, primes);

					var partResult = BuildPartResult(I, sieve);

					if (partResult.Count > 0)
						lock (_sync)
							ordered.Add(partResult[0], partResult);
				});

			foreach (var pair in ordered.OrderBy(x => x.Key))
			{
				primesResult.AddRange(pair.Value);
			}

			_pauseEvent.Reset();
			_free = true;
			_receiver.CalculationEnd(new WorkerResult(task.MainTask, primesResult));
			_pauseEvent.Set();
		}

		private static IEnumerable<ulong> SplitTask(Task task)
		{
			for (ulong I = task.From; I < task.To; I += PerformanceConstants.WorkerBlockSize)
			{
				yield return I;
			}
		}

		private static List<ulong> BuildPartResult(ulong I, bool[] sieve)
		{
			List<ulong> partResult = new List<ulong>();
			for (ulong i = 0; i < (ulong) sieve.LongLength; i++)
			{
				if (sieve[i])
					partResult.Add(i + I);
			}
			return partResult;
		}

		private static void Sifting(bool[] sieve, ulong blockSize, ulong I, ulong[] primes)
		{
			for (ulong i = 0; i < (uint) primes.LongLength; i++)
			{
				bool first = true;
				bool inset = primes.First() <= i + I && primes.Last() >= i + I;
				ulong h = I%primes[i];
				ulong j = h == 0 ? 0 : primes[i] - h;
				for (; j < blockSize; j += primes[i])
				{
					if (first && inset)
					{
						first = false;
						continue;
					}
					sieve[j] = false;
				}
			}
		}

		private static bool[] BuildSieve(ulong blockSize)
		{
			bool[] sieve = new bool[blockSize];
			for (ulong i = 0; i < (ulong) sieve.LongLength; i++)
				sieve[i] = true;
			return sieve;
		}

		private static ulong CorrectBlockSize(WorkerTask task, ulong I)
		{
			ulong maxValue = I + PerformanceConstants.WorkerBlockSize;
			ulong blockSize = maxValue > task.MainTask.To
			                  	? PerformanceConstants.WorkerBlockSize - (maxValue - task.MainTask.To)
			                  	: PerformanceConstants.WorkerBlockSize;
			return blockSize;
		}
	}
}