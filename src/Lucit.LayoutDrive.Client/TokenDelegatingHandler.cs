using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Constants;

namespace Lucit.LayoutDrive.Client
{
    internal class TokenDelegatingHandler : DelegatingHandler
    {
        private readonly string _token;

        public TokenDelegatingHandler(string token, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _token = Uri.EscapeDataString(token);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);

            uriBuilder.Query = string.IsNullOrEmpty(uriBuilder.Query)
                ? $"{LayoutConstants.TokenQueryStringName}={_token}"
                : $"{uriBuilder.Query}&{LayoutConstants.TokenQueryStringName}={_token}";
         
            request.RequestUri = uriBuilder.Uri;
            return base.SendAsync(request, cancellationToken);
        }
    }
}