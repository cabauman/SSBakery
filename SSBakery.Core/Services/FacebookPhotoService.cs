using Newtonsoft.Json;
using SSBakery.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SSBakery.Core.Services
{
    public class FacebookPhotoService : IFacebookPhotoService
    {
        public async Task<FacebookAlbumData> GetAlbumsAsync(string pageId, string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/v3.0/{pageId}/albums?fields=id,name,count,cover_photo.fields(images)&access_token={accessToken}");
            var albums = JsonConvert.DeserializeObject<FacebookAlbumData>(json);

            return albums;
        }

        public async Task<List<FacebookPhoto>> GetAlbumPhotos(string albumId, string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/v3.0/{albumId}/photos?fields=images&access_token={accessToken}");
            var photos = JsonConvert.DeserializeObject<FacebookPhotoData>(json);

            return photos.Data;
        }
    }
}
