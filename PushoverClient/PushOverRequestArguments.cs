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
        public string sound { get; set; }
        public string html { get; set; }
        public string monospace { get; set; }
    }
}