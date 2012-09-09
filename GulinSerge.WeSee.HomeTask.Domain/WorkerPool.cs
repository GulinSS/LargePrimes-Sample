using System;
using System.Collections.Generic;

namespace GulinSerge.WeSee.HomeTask.Domain
{
	public interface WorkerResultReceiver
	{
		/// <summary>
		/// Вычисления окончены. Вычислительный узел свободен.
		/// </summary>
		void CalculationEnd(WorkerResult result);
	}

	/// <summary>
	/// Пул вычислительных узлов. Bridge-паттерн для связи вычислительных узлов с менеджером задач.
	/// </summary>
	public interface WorkerPool
	{
		/// <summary>
		/// Добавить вычислительный узел.
		/// </summary>
		/// <param name="worker"></param>
		void RegisterWorker(Worker worker);

		/// <summary>
		/// Опубликовать вычисления
		/// </summary>
		event Action<WorkerResult> PushResult;
		
		/// <summary>
		/// Получить новую задачу. Вернет null, если задач не осталось.
		/// </summary>
		event Func<WorkerTask> TakeTask;

		/// <summary>
		/// Освободить задачу. Вычислительный узел перестал отвечать.
		/// </summary>
		event Action<Task> FreeTask;
	}
}