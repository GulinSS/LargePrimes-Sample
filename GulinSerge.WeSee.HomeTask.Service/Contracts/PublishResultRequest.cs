using System.Collections.Generic;
using System.Runtime.Serialization;
using GulinSerge.WeSee.HomeTask.Domain;

namespace GulinSerge.WeSee.HomeTask.Service.Contracts
{
	[DataContract]
	public class PublishResultRequest
	{
		[DataMember]
		public ulong From { get; set; }

		[DataMember]
		public ulong To { get; set; }

		[DataMember]
		public IEnumerable<ulong> Primes { get; set; }

		public static PublishResultRequest Build(WorkerResult result)
		{
			return
				new PublishResultRequest
					{
						From = result.MainTask.From,
						To = result.MainTask.To,
						Primes = result.Primes
					};
		}

		public WorkerResult Convert()
		{
			return new WorkerResult(new Task(From, To), Primes);
		}
	}
}