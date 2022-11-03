using Microsoft.Extensions.Configuration;
using Confluent.Kafka;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var kafkaSettings = config.GetSection(nameof(KafkaSettings)).Get<KafkaSettings>();

var deserializer = new UserEventDeserializer();

using var consumer = new ConsumerBuilder<Ignore, UserEvent?>(kafkaSettings.ConsumerSettings)
.SetValueDeserializer(deserializer)
.Build();

consumer.Subscribe(kafkaSettings.Topic);

while (true)
{
    var consumeResult = consumer.Consume();

    if (consumeResult is null) 
    {
        Console.WriteLine("Consumeresult is null");
    }
    else if (consumeResult.IsPartitionEOF)
    {
        Console.WriteLine($"Reached EOF of topic: {kafkaSettings.Topic}");
    }
    else
    {
        var userEvent = consumeResult.Message.Value;
        Console.WriteLine($"Consumed: {userEvent!.Id}");
    }
}