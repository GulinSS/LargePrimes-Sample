using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl
{
	public class WorkerPoolImpl : WorkerPool, WorkerResultReceiver
	{
		private readonly List<WorkerTaskRecord> _workerStates = new List<WorkerTaskRecord>(); 
		private readonly Timer _progressTimer = new Timer(1000*30);

		public WorkerPoolImpl()
		{
			_progressTimer.AutoReset = false;
			_progressTimer.Elapsed += ProgressTimerOnElapsed;
			_progressTimer.Start();
		}

		public void CalculationEnd(WorkerResult result)
		{
			PushResult(result);
			var record = _workerStates.First(x => x.Task != null && x.Task.Equals(result.MainTask));
			record.Task = null;
			StartTask(record);
		}

		private void StartTask(WorkerTaskRecord record)
		{
			if (TakeTask == null)
				return;

			var task = TakeTask();
			if (task == null) return;

			record.Task = task.MainTask;

			try
			{
				record.Worker.CalculationBegin(task);
			}
			catch (Exception) //TODO: определить исключения
			{
				FreeTask(record.Task);
				_workerStates.Remove(record);
			}
		}

		private void ProgressTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			List<WorkerTaskRecord> freeWorkers = new List<WorkerTaskRecord>();

			foreach (var record in _workerStates.ToArray())
			{
				try
				{
					if (record.Worker.IsFree())
						freeWorkers.Add(record);
				}
				catch (Exception) //TODO: определить исключения
				{
					FreeTask(record.Task);
					_workerStates.Remove(record);
				}
			}
			freeWorkers.ForEach(StartTask);
			_progressTimer.Start();
		}

		public void RegisterWorker(Worker worker)
		{
			var record = new WorkerTaskRecord {Task = null, Worker = worker};
			
			_workerStates.Add(record);
			
			StartTask(record);
		}

		public event Action<WorkerResult> PushResult;
		public event Func<WorkerTask> TakeTask;
		public event Action<Task> FreeTask;

		private class WorkerTaskRecord
		{
			public Worker Worker { get; set; }
			public Task Task { get; set; }
		}
	}
}