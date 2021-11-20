using RabbitMQ.Client; // Need for RabbitMQ messaging
using System;
using System.Text; // Need for text message

namespace Producer
{
    public class Sender
    {
        public static void Main(string[] args)
        {
            // Creating a connection factory
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // Creating a connection
            using (var connection = factory.CreateConnection())
            // Creating a channel and sending a message to the queue on RabbitMQ
            using (var channel = connection.CreateModel())
            {
                var input = string.Empty;
                bool resultOfParse;
                int parsedNumber;
                string message = string.Empty;
                byte[] body = null;


                while (input.Trim().ToUpper() != "X".ToString())
                {
                    Console.Clear();
                    Console.WriteLine("Enter number of messages to send or press letter X to exit the Sender");
                    input = Console.ReadLine();
                    resultOfParse = int.TryParse(input.ToString(), out parsedNumber);
                    while (!resultOfParse && input.Trim().ToUpper() != "X".ToString())
                    {
                        Console.WriteLine("Enter VALID number of messages to send or press letter X to exit the Sender");
                        input = Console.ReadLine();
                        resultOfParse = int.TryParse(input.ToString(), out parsedNumber);
                    }

                    if (input.Trim().ToUpper() != "X".ToString())
                    {
                        for (int i = 1; i <= parsedNumber; ++i)
                        {
                            // Creating a message queue named BasicTest
                            channel.QueueDeclare("BasicTest", false, false, false, null);
                            message = $"My message to RabbitMQ using .Net Core RabbitMQ message number {i}.";
                            // Encoding the message into bytes array
                            body = Encoding.UTF8.GetBytes(message);
                            // Sending message containing in the variable body to the queue name "BasicTest" as routingKey using default exchange name indicated by ""
                            // 
                            channel.BasicPublish("", "BasicTest", null, body);
                            Console.WriteLine("Sent message number {0}: {1}", i, message);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press X to exit sender. Otherwise, press any other key to continue sending message(s).");
                        var c = Console.ReadKey();
                        input = c.Key.ToString();
                    }
                    
                    
                }
            }
            
            
        }
    }
}
