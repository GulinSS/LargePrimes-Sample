using System.Collections.Generic;
using System.Linq;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW
{
	public class TaskRange
	{
		private readonly TaskRangeElement[] _elements;

		private static IEnumerable<TaskRangeElement> GenerateRange(Task task)
		{
			ulong i = task.From + PerformanceConstants.TaskBlockSize;

			for (; i <= task.To; i += PerformanceConstants.TaskBlockSize)
			{
				yield return new TaskRangeElement(new Task(i - PerformanceConstants.TaskBlockSize, i));
			}
			if (i > task.To)
			{
				yield return new TaskRangeElement(new Task(i - PerformanceConstants.TaskBlockSize, task.To));
			}
		}

		public TaskRange(Task task)
		{
			_elements = GenerateRange(task).ToArray();
		}

		public Task Take()
		{
			var result = _elements.First(x => x.State == TaskRangeElementState.Free);
			result.State = TaskRangeElementState.InProgress;
			return result.Task;
		}

		public void Push(Task task)
		{
			var taskElement = _elements.First(x => x.Task.Equals(task));
			taskElement.State = TaskRangeElementState.Complete;
		}

		public void Reject(Task task)
		{
			var taskElement = _elements.First(x => x.Task.Equals(task));
			taskElement.State = TaskRangeElementState.Free;
		}

		public bool IsComplete()
		{
			return _elements.All(x => x.State == TaskRangeElementState.Complete);
		}
	}
}