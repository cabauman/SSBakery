using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class CustomerRewardData : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("totalVisits")]
        public int TotalVisits { get; set; }

        [JsonProperty("unclaimedRewardCount")]
        public int UnclaimedRewardCount { get; set; }
    }
}
