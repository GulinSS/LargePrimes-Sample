using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace GulinSerge.WeSee.HomeTask.Service.Behaviors
{
	public class WorkerContextMessageInspector : IDispatchMessageInspector
	{
		#region IDispatchMessageInspector Members

		public object AfterReceiveRequest(ref Message request,
		                                  IClientChannel channel,
		                                  InstanceContext instanceContext)
		{
			OperationContext.Current.Extensions.Add(new WorkerContext());
			return request.Headers.MessageId;
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			OperationContext.Current.Extensions.Remove(WorkerContext.Current);
		}

		#endregion
	}
}