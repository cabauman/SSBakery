using Newtonsoft.Json;
using System;

namespace SSBakery.Models
{
    public class SSBakeryUser
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("totalVisits")]
        public int TotalVisits { get; set; }

        [JsonProperty("unclaimedRewardCount")]
        public int UnclaimedRewardCount { get; set; }
    }
}
