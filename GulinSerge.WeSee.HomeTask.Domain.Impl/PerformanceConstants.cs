namespace GulinSerge.WeSee.HomeTask.Domain.Impl
{
	public static class PerformanceConstants
	{
		private const int MULTIPLIER = 100;
		public const int WorkerBlockSize = 30000;
		public const int TaskBlockSize = WorkerBlockSize * MULTIPLIER;
	}
}