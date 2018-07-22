using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IFacebookPhotoService
    {
        Task<FacebookAlbumContainer> GetAlbumsAsync(string pageId, string accessToken, CancellationToken cancellationToken);

        Task<IList<FacebookPhoto>> GetAlbumPhotos(string albumId, string accessToken, CancellationToken cancellationToken);
    }
}