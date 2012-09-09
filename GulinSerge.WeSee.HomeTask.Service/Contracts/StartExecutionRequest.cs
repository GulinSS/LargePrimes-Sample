using System.Collections.Generic;
using System.Runtime.Serialization;
using GulinSerge.WeSee.HomeTask.Domain;

namespace GulinSerge.WeSee.HomeTask.Service.Contracts
{
	[DataContract]
	public class StartExecutionRequest
	{
		[DataMember]
		public ulong From { get; set; }

		[DataMember]
		public ulong To { get; set; }

		[DataMember]
		public IEnumerable<ulong> KnownPrimes { get; set; }

		public static StartExecutionRequest Build(WorkerTask result)
		{
			return
				new StartExecutionRequest
				{
					From = result.MainTask.From,
					To = result.MainTask.To,
					KnownPrimes = result.KnownPrimes
				};
		}

		public WorkerTask Convert()
		{
			return new WorkerTask(new Task(From, To), KnownPrimes);
		}
	}
}