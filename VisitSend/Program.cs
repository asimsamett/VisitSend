using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class Visit
{
    public long Id { get; set; }
    public long DoctorId { get; set; }
    public string? Description { get; set; }
    public long PatientId { get; set; }
}
class Program
{
    private static readonly string _url = "amqps://mswaottz:SVECk7VkYXbpms7jfEEmth8BE5OrUQcf@cow.rmq2.cloudamqp.com/mswaottz";
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri(_url);
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

    }

    public static void consumer(IModel channel)
    {
        channel.QueueDeclare(queue: "visitQueue", true, false, false);
        var consumer = new EventingBasicConsumer(channel);

        channel.BasicConsume(queue: "visitQueue", true, consumer);

        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var getVisit = System.Text.Json.JsonSerializer.Deserialize<Visit>(message);
            Console.WriteLine($"bulunan obje = id : {getVisit.Id}- patientid: {getVisit.PatientId}-doctorid : {getVisit.DoctorId}- description : {getVisit.Description}");

        };

        channel.QueueDeclare(queue: "ismail", );
    }

    public static void Publisher(IModel channel)
    {
        var visit = new Visit
        {
            Id = 4,
            DoctorId = 7,
            Description = "Description",
            PatientId = 1
        };

        var message = System.Text.Json.JsonSerializer.Serialize(visit);

        channel.QueueDeclare(
            queue: "visitQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "",
            routingKey: "visitQueue",
            basicProperties: null,
            body: body
        );

        Console.WriteLine("Sent message: {0}", message);

        Console.WriteLine(" Press [enter] to exit.");

        Console.ReadLine();

    }
}