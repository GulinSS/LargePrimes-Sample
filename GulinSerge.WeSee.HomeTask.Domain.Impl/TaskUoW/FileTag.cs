namespace GulinSerge.WeSee.HomeTask.Domain.Impl.TaskUoW
{
	public class FileTag
	{
		public FileTag(WorkerResult result, string fileName)
		{
			Result = result;
			FileName = fileName;
		}

		public WorkerResult Result { get; private set; }
		public string FileName { get; private set; }
	}
}