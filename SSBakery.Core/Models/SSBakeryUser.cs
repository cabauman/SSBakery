using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class SSBakeryUser : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}
