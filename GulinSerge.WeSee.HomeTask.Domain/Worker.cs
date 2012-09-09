namespace GulinSerge.WeSee.HomeTask.Domain
{
	/// <summary>
	/// �������������� ����
	/// </summary>
	public interface Worker
	{
		/// <summary>
		/// ���������� � �����������.
		/// </summary>
		/// <param name="task"></param>
		void CalculationBegin(WorkerTask task);

		/// <summary>
		/// �������� �� ����?
		/// </summary>
		/// <returns></returns>
		bool IsFree();
	}
}