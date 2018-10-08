using GameCtor.Repository;
using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class RewardsMember : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("totalVisits")]
        public int TotalVisits { get; set; }

        [JsonProperty("unclaimedRewardCount")]
        public int UnclaimedRewardCount { get; set; }
    }
}
