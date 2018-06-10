using System.Collections.Generic;

namespace SSBakery.Models
{
    public class FacebookPhotoContainer
    {
        public List<FacebookPhoto> Data { get; set; }

        public FacebookPaging Paging { get; set; }
    }
}
