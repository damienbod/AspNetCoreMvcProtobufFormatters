using AspNetCoreProtobuf;
using AspNetCoreProtobuf.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreProtobuf.IntegrationTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class WebApiTest
    {

        private readonly TestServer _server;
        private readonly HttpClient _client;

        public WebApiTest()
        {
            //Arrange
            _server = new TestServer(
                new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetProtobufDataAndCheckProtobufContentTypeMediaType()
        {
            // Act
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var response = await _client.GetAsync("/api/values/1");
            response.EnsureSuccessStatusCode();

            var responseString = System.Text.Encoding.UTF8.GetString(
                await response.Content.ReadAsByteArrayAsync()
            );

            // Assert
            Assert.Equal("application/x-protobuf", response.Content.Headers.ContentType.MediaType );
            Assert.Equal("\b\u0001\u0012\nHelloWorld\u001a\u001fMy first MVC 6 Protobuf service", responseString);
        }

        [Fact]
        public void PostProtobufData()
        {
            // HTTP GET with Protobuf Response Body
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

            HttpResponseMessage response = _client.GetAsync("api/Values/4").Result;

            HttpContent data = new StringContent("\b\u0001\u0012\nHelloWorld\u001a\u001fMy first MVC 6 Protobuf service");

            // HTTP POST with Protobuf Request Body
            var responseForPost = _client.PostAsync("api/Values", data).Result;

            Assert.True(responseForPost.IsSuccessStatusCode);
           

        }
    }
}
