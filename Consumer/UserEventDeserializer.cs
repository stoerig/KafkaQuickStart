using Confluent.Kafka;
using System.Text.Json;

public class UserEventDeserializer : IDeserializer<UserEvent?>
{
    public UserEvent? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return default;

        return JsonSerializer.Deserialize<UserEvent>(data);
    }
}