using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class CatalogItem : BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("visibleToCustomers")]
        public bool VisibleToCustomers { get; set; }
    }
}
