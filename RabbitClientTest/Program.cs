using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitLib;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitClientTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var consumer = new RabbitMqClient(new RabbitConnectionParam()
			{
				HostName = "localhost",
				UserName = _username,
				Password = _password,
				QueueName = _queuename,
				Vhost = "/"
			});

			consumer.BlockConsume((s) =>
			{
				Console.WriteLine(s);
			});

			Console.ReadLine();
		}


		private const string _username = "user1";
		private const string vhost = "/";
		public const string _password = "user1";
		private const string _queuename = "footage";
		private const string hostname = "localhost";
		
	}
}
