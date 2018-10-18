using System.ComponentModel;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class CatalogCategory : BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [DefaultValue(true)]
        [JsonProperty("visibleToUsers")]
        public bool VisibleToUsers { get; set; }
    }
}
