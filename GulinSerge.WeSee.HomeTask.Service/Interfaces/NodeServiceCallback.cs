using System.ServiceModel;
using GulinSerge.WeSee.HomeTask.Service.Contracts;

namespace GulinSerge.WeSee.HomeTask.Service.Interfaces
{
	public interface NodeServiceCallback
	{
		[OperationContract(IsOneWay = true)]
		void StartExecution(StartExecutionRequest request);

		[OperationContract(IsOneWay = false)]
		bool IsFree();
	}
}