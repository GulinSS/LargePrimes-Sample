using System.ServiceModel;

namespace GulinSerge.WeSee.HomeTask.Service.Behaviors
{
	public class WorkerContext : IExtension<OperationContext>
	{
		//The "current" custom context
		public static WorkerContext Current
		{
			get { return OperationContext.Current.Extensions.Find<WorkerContext>(); }
		}

		public WorkerRemoteImpl WorkerRemoteImpl { get; set; }

		#region IExtension<OperationContext> Members

		public void Attach(OperationContext owner)
		{
		}

		public void Detach(OperationContext owner)
		{
		}

		#endregion
	}
}