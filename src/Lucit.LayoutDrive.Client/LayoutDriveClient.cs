using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Constants;
using Lucit.LayoutDrive.Client.Extensions;
using Lucit.LayoutDrive.Client.Models;
using Newtonsoft.Json.Linq;

namespace Lucit.LayoutDrive.Client
{
    public class LayoutDriveClient : IDisposable
    {
        public HttpClient HttpClient { get; }

        /// <param name="token">Lucit Api Token</param>
        public LayoutDriveClient(string token)
            : this(LayoutConstants.ApiUrl, token)
        {
        }

        /// <param name="token">Lucit Api Token</param>
        /// <param name="messageHandler"></param>
        public LayoutDriveClient(string token, HttpMessageHandler messageHandler)
            : this(LayoutConstants.ApiUrl, token, messageHandler)
        {
        }

        /// <param name="baseUrl">Lucit Api Base Url</param>
        /// <param name="token">Lucit Api Token</param>
        public LayoutDriveClient(string baseUrl, string token)
            : this(baseUrl, token, new HttpClientHandler())
        {
        }

        /// <param name="baseUrl">Lucit Api Base Url</param>
        /// <param name="token">Lucit Api Token</param>
        /// <param name="messageHandler"></param>
        public LayoutDriveClient(string baseUrl, string token, HttpMessageHandler messageHandler)
        {
            baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(token));
            token = token ?? throw new ArgumentNullException(nameof(token));
            messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));

            HttpClient = new HttpClient(new TokenDelegatingHandler(token, messageHandler))
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        /// <summary>
        /// Pull Creative to be shown
        /// </summary>
        /// <param name="exportId">The export id in string lch-id format</param>
        /// <param name="locationId">The digital display id that you are fetching for</param>
        /// <returns>List of lucit locations or empty list if something went wrong</returns>
        public async Task<List<LucitLocation>> GetCreativeAsync(string exportId, string locationId = null)
        {
            var queryString = new Dictionary<string, string>
            {
                { "location_id", locationId }
            };

            var uriBuilder = new UriBuilder
            {
                Path = Routes.Creatives(exportId),
                Query = queryString.Serialize()
            };


            var response = await HttpClient.GetAsync<JObject>(uriBuilder.Uri.PathAndQuery)
                                           .ConfigureAwait(false);
            var responseObject = response.Data;

            if (!response.IsSuccess)
            {
                throw response.Exception;
            }

            return responseObject?["lucit_layout_drive"]?["item_sets"]?.ToObject<List<LucitLocation>>() ?? new List<LucitLocation>();
        }

        /// <summary>
        /// Post PingBack 
        /// </summary>
        [Obsolete("This method will be removed soon, use SubmitPlayAsync")]
        public async Task PingBackAsync(PingBackRequest request)
        {
            var queryString = new Dictionary<string, string>
            {
                { "item_id", request.ItemId.ToString() },
                { "location_id", request.LocationId },
                { "play_datetime", request.PlayDateTime.ToString("O") },
                { "duration", request.Duration.TotalSeconds.ToString("F") },
            };

            var uriBuilder = new UriBuilder
            {
                Path = Routes.PingBack,
                Query = queryString.Serialize()
            };


            var response = await HttpClient.GetAsync<JToken>(uriBuilder.Uri.PathAndQuery)
                                           .ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return;
            }

            throw response.Exception;
        }

        public async Task SubmitPlayAsync(SubmitPlayRequest request)
        {
            var queryString = new Dictionary<string, string>
            {
                { "creative_id", request.CreativeId },
                { "lucit_layout_digital_board_id", request.DigitalBoardId.ToString() },
                { "play_datetime", request.PlayDateTime.ToString("O") },
                { "duration", request.Duration.TotalSeconds.ToString("F") },
            };

            var uriBuilder = new UriBuilder
            {
                Path = Routes.Play,
                Query = queryString.Serialize()
            };


            var response = await HttpClient.GetAsync<JToken>(uriBuilder.Uri.PathAndQuery)
                .ConfigureAwait(false);

            if (response.IsSuccess)
            {
                return;
            }

            throw response.Exception;
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
