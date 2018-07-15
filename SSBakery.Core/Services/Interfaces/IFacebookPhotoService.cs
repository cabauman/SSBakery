using System.Collections.Generic;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IFacebookPhotoService
    {
        Task<List<FacebookPhoto>> GetAlbumPhotos(string albumId, string accessToken);

        Task<FacebookAlbumContainer> GetAlbumsAsync(string pageId, string accessToken);
    }
}