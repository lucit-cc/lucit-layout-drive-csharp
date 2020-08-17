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

        public LayoutDriveClient(string token)
            : this(LayoutConstants.ApiUrl, token)
        {
        }

        public LayoutDriveClient(string token, HttpMessageHandler messageHandler)
            : this(LayoutConstants.ApiUrl, token, messageHandler)
        {
        }

        public LayoutDriveClient(string baseUrl, string token)
            : this(baseUrl, token, new HttpClientHandler())
        {
        }

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


        public async Task<List<Creative>> GetCreativesAsync(CreativesFilter filter)
        {
            var queryString = new Dictionary<string, string>
            {
                { "location_id", filter.LocationId.ToString() },
            };

            var uriBuilder = new UriBuilder
            {
                Path = Routes.Creatives(filter.ExportId),
                Query = queryString.Serialize()
            };


            var response = await HttpClient.GetAsync<List<Creative>>(uriBuilder.Uri.ToString());

            if (response.IsSuccess)
            {
                return response.Data;
            }

            throw response.Exception;
        }


        public async Task PingBackAsync(PingBackRequest request)
        {
            var queryString = new Dictionary<string, string>
            {
                { "item_id", request.ItemId.ToString() },
                { "location_id", request.LocationId.ToString() },
                { "play_datetime", request.PlayDateTime.ToString("O") },
                { "duration", request.Duration.TotalSeconds.ToString("F") },
            };

            var uriBuilder = new UriBuilder
            {
                Path = Routes.PingBack, 
                Query = queryString.Serialize()
            };


            var response = await HttpClient.GetAsync<JToken>(uriBuilder.Uri.PathAndQuery);

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
