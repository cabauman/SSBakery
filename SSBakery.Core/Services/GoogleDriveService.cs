using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSBakery.Core.Services
{
    public class GoogleDriveService
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Sweet Sensations Bakery";

        public GoogleDriveService()
        {
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, ".credentials/ssbakery.json");

            UserCredential credential = AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = Config.Credentials.OAUTH_CLIENT_ID,
                    ClientSecret = Config.Credentials.OAUTH_CLIENT_SECRET
                },
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;

            Console.WriteLine("Credential file saved to: " + credPath);

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Q = "'FolderID or root' in parents"; // "mimetype='image/jpeg'"
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if(files != null && files.Count > 0)
            {
                foreach(var file in files)
                {
                    //file.MimeType != 'application/vnd.google-apps.folder'
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
        }

        // It's unforunate this is a public field. But it cannot be changed due to backward compatibility.
        /// <summary>The folder which is used by the <see cref="Google.Apis.Util.Store.FileDataStore"/>.</summary>
        /// <remarks>
        /// The reason that this is not 'private const' is that a user can change it and store the credentials in a
        /// different location.
        /// </remarks>
        public static string Folder = "Google.Apis.Auth";

        /// <summary>
        /// Asynchronously authorizes the specified user.
        /// Requires user interaction; see <see cref="GoogleWebAuthorizationBroker"/> remarks for more details.
        /// </summary>
        /// <remarks>
        /// In case no data store is specified, <see cref="Google.Apis.Util.Store.FileDataStore"/> will be used by 
        /// default.
        /// </remarks>
        /// <param name="clientSecrets">The client secrets.</param>
        /// <param name="scopes">
        /// The scopes which indicate the Google API access your application is requesting.
        /// </param>
        /// <param name="user">The user to authorize.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel an operation.</param>
        /// <param name="dataStore">The data store, if not specified a file data store will be used.</param>
        /// <returns>User credential.</returns>
        public static async Task<UserCredential> AuthorizeAsync(ClientSecrets clientSecrets,
            IEnumerable<string> scopes, string user, CancellationToken taskCancellationToken,
            IDataStore dataStore = null)
        {
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
            };

            initializer.Scopes = scopes;
            initializer.DataStore = dataStore ?? new FileDataStore(Folder);

            var flow = new GoogleAuthorizationCodeFlow(initializer);

            // Try to load a token from the data store.
            var token = await flow.LoadTokenAsync(user, taskCancellationToken).ConfigureAwait(false);
            if(token == null)
            {
                token = new TokenResponse()
                {
                    AccessToken = "",
                    RefreshToken = "",
                    TokenType = "",
                    Scope = "",
                    IdToken = "",
                    IssuedUtc = default(DateTime),
                    ExpiresInSeconds = 3600
                };

                await flow.DataStore.StoreAsync<TokenResponse>(user, token).ConfigureAwait(false);
            }

            return new UserCredential(flow, user, token);
        }
    }
}
