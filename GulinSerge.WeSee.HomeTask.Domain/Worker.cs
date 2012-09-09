namespace GulinSerge.WeSee.HomeTask.Domain
{
	/// <summary>
	/// Вычислительный узел
	/// </summary>
	public interface Worker
	{
		/// <summary>
		/// Приступить к вычислениям.
		/// </summary>
		/// <param name="task"></param>
		void CalculationBegin(WorkerTask task);

		/// <summary>
		/// Свободен ли узел?
		/// </summary>
		/// <returns></returns>
		bool IsFree();
	}
}