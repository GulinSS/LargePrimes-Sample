using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using Autofac;
using Autofac.Integration.Wcf;
using GulinSerge.WeSee.HomeTask.Domain;
using GulinSerge.WeSee.HomeTask.Domain.Impl;
using GulinSerge.WeSee.HomeTask.Service.Contracts;
using GulinSerge.WeSee.HomeTask.Service.Interfaces;

namespace GulinSerge.WeSee.HomeTask.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<NodeServiceCallbackImpl>()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.Register(c => new DuplexChannelFactory<NodeService>(
				c.Resolve<NodeServiceCallback>(),
				new WSDualHttpBinding(WSDualHttpSecurityMode.None)
					{
						SendTimeout = new TimeSpan(0, 0, 10),
						ReceiveTimeout = new TimeSpan(0, 0, 10),
						ClientBaseAddress = new Uri(ConfigurationManager.AppSettings["client"]),
						MaxBufferPoolSize = 1024 * 1024 * 1,
						MaxReceivedMessageSize = 1024 * 1024 * 20,
						MessageEncoding = WSMessageEncoding.Mtom,
						ReaderQuotas = 
							new XmlDictionaryReaderQuotas
								{
									MaxArrayLength = 1000000,
								}
					},
				new EndpointAddress(ConfigurationManager.AppSettings["host"])))
				.SingleInstance();

			builder.Register(c => PrepareFactory(c.Resolve<DuplexChannelFactory<NodeService>>()))
				.As<NodeService>()
				.SingleInstance();

			var container = builder.Build();

			var service = container.Resolve<NodeService>();
			service.Subscribe();
			
			Console.ReadLine();
		}

		private static NodeService PrepareFactory(DuplexChannelFactory<NodeService> factory)
		{
			foreach (OperationDescription op in factory.Endpoint.Contract.Operations)
			{
				var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (dataContractBehavior != null)
				{
					dataContractBehavior.MaxItemsInObjectGraph = 1000 * 1000;
				}
			}
			return factory.CreateChannel();
		}
	}
}
