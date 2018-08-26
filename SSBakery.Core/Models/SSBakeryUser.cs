using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class SSBakeryUser : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}
