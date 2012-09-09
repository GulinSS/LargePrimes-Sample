using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace GulinSerge.WeSee.HomeTask.Service.Behaviors
{
	public class WorkerContextBehaviorAttribute : Attribute, IServiceBehavior
	{
		#region IServiceBehavior Members

		public void AddBindingParameters(ServiceDescription serviceDescription,
		                                 ServiceHostBase serviceHostBase,
		                                 Collection<ServiceEndpoint> endpoints,
		                                 BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
			{
				foreach (EndpointDispatcher ed in cd.Endpoints)
				{
					ed.DispatchRuntime.MessageInspectors.Add(new WorkerContextMessageInspector());
				}
			}
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		#endregion
	}
}