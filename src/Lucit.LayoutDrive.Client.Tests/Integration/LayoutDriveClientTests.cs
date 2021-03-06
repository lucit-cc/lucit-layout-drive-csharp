﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Models;
using NUnit.Framework;

namespace Lucit.LayoutDrive.Client.Tests.Integration
{
    public class LayoutDriveClientTests
    {
        private const string LayoutTokenVariableName = "LayoutToken";
        public string Token { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            Token = Environment.GetEnvironmentVariable(LayoutTokenVariableName);

            Assert.IsTrue(!string.IsNullOrEmpty(Token), $"For running integration tests you need to setup '{LayoutTokenVariableName}' environment variable");
        }

        [Test]
        [Category(Constants.Categories.Integration)]
        [TestCase("lch-4C9D", null)]
        [TestCase("11", null)]
        [TestCase("lch-4C9D", "SC_MH_1")]
        public async Task ShouldFetchCreative(string exportId, string locationId)
        {
            //Arrange
            var client = BuildClient();

            //Act
            var result = await client.GetCreativeAsync(exportId, locationId);

            //Assert
            Assert.IsTrue(result?.Any());
        }

        [Test]
        [Category(Constants.Categories.Integration)]
        public async Task ShouldPostPingBack()
        {
            //Arrange
            var pingBackRequest = new PingBackRequest
            {
                PlayDateTime = DateTime.UtcNow,
                Duration = TimeSpan.FromHours(1),
                LocationId = "SC_MH_1",
                ItemId = 51798
            };
            var client = BuildClient();

            //Act
            await client.PingBackAsync(pingBackRequest);

            //Assert
            Assert.Pass();
        }

        [Test]
        [Category(Constants.Categories.Integration)]
        public async Task ShouldSubmitPlay()
        {
            //Arrange
            var playRequest = new SubmitPlayRequest
            {
                PlayDateTime = DateTime.UtcNow,
                Duration = TimeSpan.FromHours(1),
                DigitalBoardId = 19302,
                CreativeId = "C1-4C9D-LP-4V4Y"
            };
            var client = BuildClient();

            //Act
            await client.SubmitPlayAsync(playRequest);

            //Assert
            Assert.Pass();
        }

        [Test]
        [Category(Constants.Categories.Integration)]
        public async Task ShouldValidateItemHash()
        {
            //Arrange
            var exportId = "lch-4C9D";
            var locationId = "SC_MH_1";
            var client = BuildClient();
            var result = await client.GetCreativeAsync(exportId, locationId);

            var creative = result?.FirstOrDefault()?.Items?.First();

            //Act
            var isValidHash = await client.ValidateItemHashAsync(creative);

            //Assert
            Assert.IsTrue(isValidHash);
        }

        private LayoutDriveClient BuildClient()
        {
            return new LayoutDriveClient(Token);
        }
    }
}