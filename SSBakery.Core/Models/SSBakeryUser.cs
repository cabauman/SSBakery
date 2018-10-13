using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class SSBakeryUser : BaseModel
    {
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
