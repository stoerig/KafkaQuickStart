// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

var inputSettings = args.Length == 0 ? null : args[0];

var settingsFile = string.IsNullOrWhiteSpace(inputSettings) ? "appsettings.json" : inputSettings;

if (!System.IO.File.Exists(settingsFile))
{
    throw new FileLoadException($"File: {settingsFile} could not be found");
}

var config = new ConfigurationBuilder().AddJsonFile(settingsFile).Build();

var kafkaSettings = config.GetRequiredSection(nameof(KafkaSettings)).Get<KafkaSettings>();

var testData = config.GetRequiredSection("TestData").Get<UserEvent[]>();

using var producer = new ProducerBuilder<string, string>(kafkaSettings.ProducerSettings).Build();

foreach (var userEvent in testData)
{
    var eventData = JsonSerializer.Serialize(userEvent);
    var msg = new Message<string, string>
    {
        Key = userEvent.Id,
        Value = eventData
    };

    producer.Produce(kafkaSettings.Topic, msg, (deliveryReport) => {
        if (deliveryReport.Error.Code != ErrorCode.NoError)
        {
            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
        }
        else
        {
            Console.WriteLine($"Produced event to topic {kafkaSettings.Topic}: {eventData}");
        }
    });
}

Console.WriteLine($"Flushing producer with timeout seconds: {kafkaSettings.FlushTimeoutSeconds}");
producer.Flush(TimeSpan.FromSeconds(kafkaSettings.FlushTimeoutSeconds));