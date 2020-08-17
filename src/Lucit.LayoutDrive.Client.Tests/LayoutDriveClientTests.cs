using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Models;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Lucit.LayoutDrive.Client.Tests
{
    public class Tests
    {
        [Test]
        public async Task ShouldPassToken()
        {
            //Arrange
            var token = "test_token";
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var client = BuildClient(response, token);

            //Act
            var result = await client.HttpClient.GetAsync("/test");

            //Assert
            Assert.IsTrue(result.RequestMessage.RequestUri.Query.Contains(token));
        }

        [Test]
        public async Task ShouldGetCreatives()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            };
            var filter = new CreativesFilter(5);
            var client = BuildClient(response);

            //Act
            var result = await client.GetCreativesAsync(filter);

            //Assert
            Assert.IsTrue(result.Count == 0);
        }


        [Test]
        public async Task ShouldPostPingBack()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            };
            var pingBackRequest = new PingBackRequest
            {
                ItemId = 1523,
                LocationId = 2,
                PlayDateTime = DateTime.UtcNow,
                Duration = TimeSpan.FromHours(1)
            };
            var client = BuildClient(response);

            //Act
            await client.PingBackAsync(pingBackRequest);

            //Assert
            Assert.Pass();
        }

        [Test]
        public void ShouldThrowLayoutDriveException()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(string.Empty)
            };
            var pingBackRequest = new PingBackRequest
            {
                ItemId = 1523,
                LocationId = 2,
                PlayDateTime = DateTime.UtcNow,
                Duration = TimeSpan.FromHours(1)
            };
            var client = BuildClient(response);

            //Act
            //Assert
            Assert.CatchAsync<LayoutDriveException>(() => client.PingBackAsync(pingBackRequest));
        }

        private LayoutDriveClient BuildClient(HttpResponseMessage response, string token = "test_token")
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    response.RequestMessage = request;
                    return response;
                });

            return new LayoutDriveClient(token, httpMessageHandlerMock.Object);
        }
    }
}