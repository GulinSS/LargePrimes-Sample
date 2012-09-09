using System;
using System.ServiceModel;
using GulinSerge.WeSee.HomeTask.Domain;
using GulinSerge.WeSee.HomeTask.Domain.Impl;
using GulinSerge.WeSee.HomeTask.Service.Contracts;
using GulinSerge.WeSee.HomeTask.Service.Interfaces;

namespace GulinSerge.WeSee.HomeTask.Client
{
	public class NodeServiceCallbackImpl : NodeServiceCallback, WorkerResultReceiver
	{
		private readonly Worker _worker;
		private readonly Func<NodeService> _nodeServiceFactory;

		public NodeServiceCallbackImpl(Func<NodeService> nodeServiceFactory)
		{
			_worker = new WorkerLocalImpl(this);
			_nodeServiceFactory = nodeServiceFactory;
		}

		public void StartExecution(StartExecutionRequest request)
		{
			_worker.CalculationBegin(request.Convert());
		}

		public bool IsFree()
		{
			return _worker.IsFree();
		}

		public void CalculationEnd(WorkerResult result)
		{
			var service = _nodeServiceFactory();
			service.PublishResult(PublishResultRequest.Build(result));
		}
	}
}