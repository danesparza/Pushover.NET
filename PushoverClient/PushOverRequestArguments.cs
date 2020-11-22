// ReSharper disable InconsistentNaming
namespace PushoverClient
{
    public class PushoverRequestArguments
    {
        public string token { get; set; }
        public string user { get; set; }
        public string device { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public int priority { get; set; }
        public int timestamp { get; set; }
        public string sound { get; set; }
        public int retry { get; set; }
        public int expire { get; set; }
    }
}