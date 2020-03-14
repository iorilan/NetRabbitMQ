using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitLib
{
	public class RabbitMqClient
	{
        public RabbitConnectionParam ConnectionParam { get; }
        private ILog _log = LogManager.GetLogger(typeof(RabbitMqClient));

        public RabbitMqClient(RabbitConnectionParam connectionParam)
        {
            ConnectionParam = connectionParam;
        }

        public void Publish(string json)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = ConnectionParam.HostName, UserName = ConnectionParam.UserName, Password = ConnectionParam.Password };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        string message = json;
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                            routingKey: ConnectionParam.QueueName,
                            basicProperties: null,
                            body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
        }

        private IConnection _conn = null;
        private IModel _channel = null;
        private EventingBasicConsumer _consumer = null;
        public void BlockConsume(Action<string> onMessage)
        {
            _log.Debug("BlockConsume --> enter");

            try
            {
                if (_conn == null)
                {
                    _log.Debug("BlockConsume --> create connection");
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        UserName = ConnectionParam.UserName,
                        Password = ConnectionParam.Password,
                        //VirtualHost = ConnectionParam.Vhost,
                        HostName = ConnectionParam.HostName,
                    };
                    _conn = factory.CreateConnection();
                    _channel = _conn.CreateModel();
                    _consumer = new EventingBasicConsumer(_channel);
                    _channel.BasicConsume(ConnectionParam.QueueName, true, _consumer);
                    _log.Debug("BlockConsume --> listening");
                }
                _log.Debug("BlockConsume --> adding event handler");
                _consumer.Received += (ch, ea) => {
                    _log.Debug("BlockConsume --> received");
                    try
                    {
                        var body = ea.Body;
                        var str = Encoding.UTF8.GetString(body);
                        onMessage(str);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                    }
                };
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
                Task.Delay(10 * 1000);
            }
            _log.Debug("BlockConsume --> exit");
        }
    }
}
