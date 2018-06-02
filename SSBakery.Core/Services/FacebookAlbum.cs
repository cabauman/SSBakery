using Newtonsoft.Json;
using System.Collections.Generic;

namespace SSBakery.Core.Services
{
    public class FacebookAlbumData
    {
        public List<FacebookAlbum> Data { get; set; }
    }

    public class FacebookPhotoData
    {
        public List<FacebookPhoto> Data { get; set; }

        public FacebookPaging Paging { get; set; }
    }

    public class FacebookPaging
    {
        public string Next { get; set; }
    }

    public class FacebookAlbum
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("cover_photo")]
        public FacebookPhoto CoverPhoto { get; set; }

        public int Count { get; set; }
    }
    
    public class FacebookPhoto
    {
        public List<PlatformImageSource> Images { get; set; }
    }
    
    public class PlatformImageSource
    {
        public string Source { get; set; }

        public uint Height { get; set; }

        public uint Width { get; set; }
    }
}
