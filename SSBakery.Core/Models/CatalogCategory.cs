using System;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class CatalogCategory : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
