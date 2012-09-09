using System;
using System.Collections.Generic;
using System.Linq;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW
{
	public class TaskManagerUofW
	{
		private readonly ResultWriter _writer = new ResultWriter();
		private readonly ulong[] _seed;
		private readonly TaskRange _taskRange;

		public TaskManagerUofW(Task task, IEnumerable<ulong> seed)
		{
			_taskRange = new TaskRange(task);
			_seed = seed.ToArray();
		}

		public void WriteResult(WorkerResult result)
		{
			_taskRange.Push(result.MainTask);
			_writer.WriteResult(result);
		}

		public bool IsComplete { get { return _taskRange.IsComplete(); } }

		public IEnumerable<ulong> GetResult()
		{
			return _writer.GetResult();
		}

		public WorkerTask BuildTask()
		{
			try
			{
				return new WorkerTask(_taskRange.Take(), _seed);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}

		public void UnlockTask(Task task)
		{
			_taskRange.Reject(task);
		}
	}
}