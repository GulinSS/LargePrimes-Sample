using System;
using GulinSerge.WeSee.HomeTask.Domain;
using GulinSerge.WeSee.HomeTask.Service.Contracts;
using GulinSerge.WeSee.HomeTask.Service.Interfaces;

namespace GulinSerge.WeSee.HomeTask.Service
{
	public class WorkerRemoteImpl : Worker
	{
		private readonly NodeServiceCallback _callback;

		public WorkerRemoteImpl(NodeServiceCallback callback)
		{
			_callback = callback;
		}

		public void CalculationBegin(WorkerTask task)
		{
			_callback.StartExecution(StartExecutionRequest.Build(task));
		}

		public bool IsFree()
		{
			return _callback.IsFree();
		}
	}
}