using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public class Receiver
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("BasicTest", false, false, false, null);
                var consumer = new EventingBasicConsumer(channel);
                // Event being fired when a delivery of message to the consumer channel on "BasicTest" queue and data/payload of the event is in ea (event arguments object)
               
                consumer.Received += (model, ea) =>
                {

                    // Converting to array of bytes
                    byte[] body = ea.Body.ToArray();
                    // Encoding the message into UTF8 format string
                    var message = Encoding.UTF8.GetString(body);
                    if (message.Contains("number 1."))
                    {
                        Console.Clear();
                    }
                    var messageParts = message.Split(' ');
                    // Wrting out the received message
                    Console.WriteLine("Received message {0} {1}: {2}", messageParts[messageParts.Length - 2], messageParts[messageParts.Length - 1].Replace(".", null),  message);
                   
                };

                // Consuming the message on the channel of queue "BasicTest", auto acknowledge, and passing in the consumer
                channel.BasicConsume("BasicTest", true, consumer);
                Console.WriteLine("Press [Enter] to exit the Consumer.");
                Console.ReadLine();
               
            }
            
        }
    }
}
