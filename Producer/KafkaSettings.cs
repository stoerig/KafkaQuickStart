public class KafkaSettings
{
    public int FlushTimeoutSeconds {get; init;}
    public string Topic {get;init;} = string.Empty;
    public Dictionary<string, string> ProducerSettings {get; init;} = new ();
}