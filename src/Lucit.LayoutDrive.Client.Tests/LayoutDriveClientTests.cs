using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
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
        [TestCase(true, "{\"lucit_layout_drive\":{\"source\":\"Layout by Lucit\",\"source_href\":\"https:\\/\\/lucit.cc\\/lucit-layout\",\"account\":\"DF Motors (Demo)\",\"account_id\":\"lch-4C9E\",\"layout_export_id\":\"lch-4C9D\",\"layout_export_run_id\":\"49548\",\"generated_datetime\":\"2020-08-19T18:09:02+00:00\",\"layout_drive_schema_version\":\"1.0\",\"item_sets\":[{\"location_id\":\"SC_MH_1\",\"location_name\":\"1010 N Draper Blvd\",\"lucit_layout_digital_board_id\":\"19302\",\"item_count\":\"6\",\"item_total_weight\":\"60\",\"item_selected_index\":\"5\",\"items\":[{\"creative_id\":\"C1-4C9D-LP-4PcU\",\"creative_datetime\":\"2020-07-21T19:15:06+00:00\",\"id\":\"51798\",\"object_class\":\"InventoryPhoto\",\"name\":\"16914A 2012 Dodge Durango\",\"slug\":\"16914a_2012_dodge_durango\",\"src\":\"https:\\/\\/lucore-bucket-layout-prod1.s3.us-east-2.amazonaws.com\\/1\\/3599\\/img_5f173eb9b72ee_2822f2efb54aed24a43a.png\",\"width\":\"1856\",\"height\":\"576\",\"weight\":\"10\",\"weight_pct\":\"0.16666667\"}]}]}}")]
        [TestCase(false, "{\"lucit_layout_drive\":{\"source\":\"Layout by Lucit\",\"source_href\":\"https:\\/\\/lucit.cc\\/lucit-layout\",\"account\":\"DF Motors (Demo)\",\"account_id\":\"lch-4C9E\",\"layout_export_id\":\"lch-4C9D\",\"layout_export_run_id\":\"49548\",\"generated_datetime\":\"2020-08-19T18:09:02+00:00\",\"layout_drive_schema_version\":\"1.0\",\"item_sets\":[{\"location_id\":\"SC_MH_1\",\"location_name\":\"1010 N Draper Blvd\",\"lucit_layout_digital_board_id\":\"19302\",\"item_count\":\"6\",\"item_total_weight\":\"60\",\"item_selected_index\":\"5\",\"items\":[]}]}}")]
        [TestCase(false, "{\"lucit_layout_drive\":{\"source\":\"Layout by Lucit\",\"source_href\":\"https:\\/\\/lucit.cc\\/lucit-layout\",\"account\":\"DF Motors (Demo)\",\"account_id\":\"lch-4C9E\",\"layout_export_id\":\"lch-4C9D\",\"layout_export_run_id\":\"49548\",\"generated_datetime\":\"2020-08-19T18:09:02+00:00\",\"layout_drive_schema_version\":\"1.0\",\"item_sets\":[]}}")]
        [TestCase(false, "{\"lucit_layout_drive\":{}}")]
        [TestCase(false, "{}")]
        public async Task ShouldGetCreative(bool pass, string apiResponse)
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(apiResponse)
            };
            var exportId = "lch-4C9D";
            var locationId = "SC_MH_1";
            var client = BuildClient(response);

            //Act
            var result = await client.GetCreativeAsync(exportId, locationId);

            //Assert
            Assert.IsTrue(pass && result != null 
                          || !pass && result == null);
        }

        [Test]
        public void ShouldParseCreative()
        {
            //Arrange
            var json = "{\"creative_id\":\"C1-4C9D-LP-4PcU\",\"creative_datetime\":\"2020-07-21T19:15:06+00:00\",\"id\":\"51798\",\"object_class\":\"InventoryPhoto\",\"name\":\"16914A 2012 Dodge Durango\",\"slug\":\"16914a_2012_dodge_durango\",\"src\":\"https:\\/\\/lucore-bucket-layout-prod1.s3.us-east-2.amazonaws.com\\/1\\/3599\\/img_5f173eb9b72ee_2822f2efb54aed24a43a.png\",\"width\":\"1856\",\"height\":\"576\",\"weight\":\"10\",\"weight_pct\":\"0.16666667\"}";

            //Act
            var result = JsonConvert.DeserializeObject<Creative>(json);

            //Assert
            Assert.IsTrue(result.DateTime != DateTime.MinValue
                        && result.WeightPercentage > 0
                        && result.Height > 0);
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
                LocationId = "2",
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
                LocationId = "2",
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