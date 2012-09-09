using System;
using System.Collections.Generic;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	public interface WorkerResultReceiver
	{
		/// <summary>
		/// ���������� ��������. �������������� ���� ��������.
		/// </summary>
		void CalculationEnd(WorkerResult result);
	}

	/// <summary>
	/// ��� �������������� �����. Bridge-������� ��� ����� �������������� ����� � ���������� �����.
	/// </summary>
	public interface WorkerPool
	{
		/// <summary>
		/// �������� �������������� ����.
		/// </summary>
		/// <param name="worker"></param>
		void RegisterWorker(Worker worker);

		/// <summary>
		/// ������������ ����������
		/// </summary>
		event Action<WorkerResult> PushResult;
		
		/// <summary>
		/// �������� ����� ������. ������ null, ���� ����� �� ��������.
		/// </summary>
		event Func<WorkerTask> TakeTask;

		/// <summary>
		/// ���������� ������. �������������� ���� �������� ��������.
		/// </summary>
		event Action<Task> FreeTask;
	}
}