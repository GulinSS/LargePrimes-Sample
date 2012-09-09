using System.ServiceModel;
using GulinSerge.WeSee.HomeTask.Domain;
using GulinSerge.WeSee.HomeTask.Service;
using GulinSerge.WeSee.HomeTask.Service.Behaviors;
using GulinSerge.WeSee.HomeTask.Service.Contracts;
using GulinSerge.WeSee.HomeTask.Service.Interfaces;

namespace GulinSerge.WeSee.HomeTask.Server
{
	[ServiceBehavior(
		MaxItemsInObjectGraph = 1000 * 1000,
		ConcurrencyMode = ConcurrencyMode.Reentrant, 
		InstanceContextMode = InstanceContextMode.PerSession)]
	[WorkerContextBehavior]
	public class NodeServiceImpl : NodeService
	{
		private readonly WorkerPool _workerPool;

		public NodeServiceImpl(WorkerPool workerPool)
		{
			_workerPool = workerPool;
		}

		#region NodeService Members

		public void Subscribe()
		{
			var callback = OperationContext.Current.GetCallbackChannel<NodeServiceCallback>();
			WorkerContext.Current.WorkerRemoteImpl = new WorkerRemoteImpl(callback);
			_workerPool.RegisterWorker(WorkerContext.Current.WorkerRemoteImpl);
		}

		public void PublishResult(PublishResultRequest request)
		{
			((WorkerResultReceiver) _workerPool).CalculationEnd(request.Convert());
		}

		#endregion
	}
}