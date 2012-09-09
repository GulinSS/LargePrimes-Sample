using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW;
using Timer = System.Timers.Timer;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl
{
	public class PrimeCalculatorImpl : PrimeCalculator
	{
		private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);
		private readonly Seeder _seeder;
		private readonly WorkerPool _pool;
		private TaskManagerUofW _taskManager;

		public PrimeCalculatorImpl(Seeder seeder, WorkerPool pool)
		{
			_seeder = seeder;
			_pool = pool;
			_pool.PushResult += PoolOnPushResult;
			_pool.TakeTask += PoolOnTakeTask;
			_pool.FreeTask += PoolOnFreeTask;
		}

		private void PoolOnFreeTask(Task task)
		{
			_taskManager.UnlockTask(task);
		}

		private WorkerTask PoolOnTakeTask()
		{
			return _taskManager.BuildTask();
		}

		private void PoolOnPushResult(WorkerResult workerResult)
		{
			_taskManager.WriteResult(workerResult);
			_pauseEvent.Set();
		}

		public IEnumerable<ulong> Calc(Task task)
		{
			task = CheckTask(task);

			IEnumerable<ulong> seed = _seeder.PrepareSeed(task.To);
			_taskManager = new TaskManagerUofW(task, seed);
			_pool.RegisterWorker(new WorkerLocalImpl((WorkerResultReceiver) _pool));
			while (true)
			{
				_pauseEvent.WaitOne(Timeout.Infinite);
				if (_taskManager.IsComplete)
					break;

				_pauseEvent.Reset();
			}

			return _taskManager.GetResult();
		}

		private static Task CheckTask(Task task)
		{
			if (task.From < 2)
				task = new Task(2, task.To);
			return task;
		}
	}
}
