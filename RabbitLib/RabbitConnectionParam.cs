using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitLib
{
	public class RabbitConnectionParam
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string QueueName { get; set; }
		public string HostName { get; set; }
		public string Vhost { get; set; }
	}
}
