using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitLib;
using RabbitMQ.Client;

namespace RabbitServerTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Publish();
		}

		private const string _username = "user1";
		public const string _password = "user1";
		private const string _queuename = "test";
		private static void Publish()
		{
			var publisher = new RabbitMqClient(new RabbitConnectionParam()
			{
				HostName = "localhost",
				UserName = _username,
				Password = _password,
				QueueName = _queuename,
				Vhost = "/"
			});

			
			while (true)
			{
				Console.WriteLine("press 'q'+enter to exit");

				var read = Console.ReadLine();
				if (read.ToLower() == "q")
				{
					break;
				}

				publisher.Publish(read);
			}
			
		}
	}
}
