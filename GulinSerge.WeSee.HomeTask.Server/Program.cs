using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using Autofac;
using Autofac.Integration.Wcf;
using GulinSerge.WeSee.HomeTask.Domain;
using GulinSerge.WeSee.HomeTask.Domain.Impl;
using GulinSerge.WeSee.HomeTask.Service.Interfaces;

namespace GulinSerge.WeSee.HomeTask.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.Register(c => new WorkerPoolImpl()).AsImplementedInterfaces().SingleInstance();
			builder.Register(c => new NodeServiceImpl(c.Resolve<WorkerPool>())).As<NodeService>();
			builder.RegisterType<PrimeCalculatorImpl>().AsImplementedInterfaces();
			builder.RegisterType<SeederImpl>().AsImplementedInterfaces();

			using (IContainer container = builder.Build())
			{
				Uri address = new Uri(ConfigurationManager.AppSettings["host"]);
				ServiceHost host = new ServiceHost(typeof(NodeServiceImpl), address);

				host.AddServiceEndpoint(typeof(NodeService), 
					new WSDualHttpBinding(WSDualHttpSecurityMode.None)
						{
							SendTimeout = new TimeSpan(0, 0, 10),
							ReceiveTimeout = new TimeSpan(0, 0, 10),
							MaxBufferPoolSize = 1024 * 1024 * 1,
							MaxReceivedMessageSize = 1024 * 1024 * 20,
							MessageEncoding = WSMessageEncoding.Mtom,
							ReaderQuotas =
								new XmlDictionaryReaderQuotas
								{
									MaxArrayLength = 1000000,
								}
						}, string.Empty);
				host.AddDependencyInjectionBehavior<NodeService>(container);
				host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true, HttpGetUrl = address });
				host.Open();

				var calc = container.Resolve<PrimeCalculator>();
				WriteFile(calc.Calc(GetTaskFromInFile()));

				host.Close();
				Environment.Exit(0);
			}
		}

		private static Task GetTaskFromInFile()
		{
			try
			{
				using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["in"]))
				{
					ulong from = Convert.ToUInt64(sr.ReadLine());
					ulong to = Convert.ToUInt64(sr.ReadLine());
					return new Task(from, to);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Файл не может быть прочитан:");
				Console.WriteLine(e.Message);
			}
			return null;
		}

		private static void WriteFile(IEnumerable<ulong> result)
		{
			try
			{
				using (StreamWriter sr = new StreamWriter(ConfigurationManager.AppSettings["out"], false))
				{
					foreach (ulong value in result)
					{
						sr.WriteLine(value);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Файл не может быть записан:");
				Console.WriteLine(e.Message);
			}
		}
	}
}
