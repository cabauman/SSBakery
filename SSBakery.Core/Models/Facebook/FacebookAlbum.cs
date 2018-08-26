using Newtonsoft.Json;

namespace SSBakery.Models
{
    public class FacebookAlbum
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("cover_photo")]
        public FacebookPhoto CoverPhoto { get; set; }

        public int Count { get; set; }
    }
}
