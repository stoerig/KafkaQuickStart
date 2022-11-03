public class KafkaSettings
{
    public int ConsumeTimeoutSeconds {get; init;} = 5;
    public string Topic {get;init;} = string.Empty;
    public Dictionary<string, string> ConsumerSettings {get; init; } = new ();
}