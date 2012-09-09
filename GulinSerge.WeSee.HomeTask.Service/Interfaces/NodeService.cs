using System.ServiceModel;
using GulinSerge.WeSee.HomeTask.Service.Contracts;

namespace GulinSerge.WeSee.HomeTask.Service.Interfaces
{
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(NodeServiceCallback))]
	public interface NodeService
	{
		[OperationContract(IsOneWay = true, IsInitiating = true, IsTerminating = false)]
		void Subscribe();

		[OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
		void PublishResult(PublishResultRequest request);
	}
}