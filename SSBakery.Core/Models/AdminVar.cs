using System;
using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class AdminVar : IModel
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("catalogSyncTimestamp")]
        public string CatalogSyncTimestamp { get; set; }

        [JsonProperty("customerSyncTimestamp")]
        public string CustomerSyncTimestamp { get; set; }
    }
}
