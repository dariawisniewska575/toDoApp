using Newtonsoft.Json;

namespace ToDoApp.Models
{
    public class ToDo
    {
        [JsonProperty("eventId")]
        public Guid EventId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
