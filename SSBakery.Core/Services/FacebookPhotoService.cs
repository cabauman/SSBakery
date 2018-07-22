using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSBakery.Models;
using SSBakery.Services.Interfaces;

namespace SSBakery.Services
{
    public class FacebookPhotoService : IFacebookPhotoService
    {
        public async Task<FacebookAlbumContainer> GetAlbumsAsync(string pageId, string accessToken, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();
            string requestUri = $"https://graph.facebook.com/v3.0/{pageId}/albums?fields=id,name,count,cover_photo.fields(images)&access_token={accessToken}";
            var response = await httpClient.GetAsync(requestUri, cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var albums = JsonConvert.DeserializeObject<FacebookAlbumContainer>(json);

            return albums;
        }

        public async Task<IList<FacebookPhoto>> GetAlbumPhotos(string albumId, string accessToken, CancellationToken cancellationToken)
        {
            var requestUri = $"https://graph.facebook.com/v3.0/{albumId}/photos?fields=images&access_token={accessToken}";
            var json = await GetJsonAsync(requestUri, cancellationToken);
            var photos = JsonConvert.DeserializeObject<FacebookPhotoContainer>(json);

            return photos.Data;
        }

        private async Task<string> GetJsonAsync(string requestUri, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(requestUri, cancellationToken);
            using(cancellationToken.Register(() => response.Dispose()))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
