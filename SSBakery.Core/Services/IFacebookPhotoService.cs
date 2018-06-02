using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSBakery.Core.Services
{
    public interface IFacebookPhotoService
    {
        Task<List<FacebookPhoto>> GetAlbumPhotos(string albumId, string accessToken);

        Task<FacebookAlbumData> GetAlbumsAsync(string pageId, string accessToken);
    }
}