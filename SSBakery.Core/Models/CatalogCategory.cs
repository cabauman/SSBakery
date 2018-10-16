using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class CatalogCategory : BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("visibleToCustomers")]
        public bool VisibleToCustomers { get; set; }
    }
}
