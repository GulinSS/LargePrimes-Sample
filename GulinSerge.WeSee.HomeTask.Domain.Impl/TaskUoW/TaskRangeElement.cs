namespace GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW
{
	public class TaskRangeElement
	{
		public TaskRangeElement(Task task)
		{
			Task = task;
			State = TaskRangeElementState.Free;
		}

		public Task Task { get; private set; }
		public TaskRangeElementState State { get; set; }
	}
}