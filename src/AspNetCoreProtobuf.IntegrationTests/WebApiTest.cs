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
        public async Task ReturnErrorCode()
        {
            // Act
            var response = await _client.GetAsync("/api/values/1");
            response.EnsureSuccessStatusCode();

            var responseString = System.Text.Encoding.UTF8.GetString(
                await response.Content.ReadAsByteArrayAsync()
            );

            // Assert
            Assert.Equal("[]", responseString);
        }

        [Fact]
        public void GetProtobufData()
        {
            // HTTP GET with Protobuf Response Body
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:14717/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

            HttpResponseMessage response = client.GetAsync("api/Values/4").Result;

            ////if (response.IsSuccessStatusCode)
            ////{
            ////    // Parse the response body. Blocking!
            ////    var p = response.Content.ReadAsAsync<ProtobufModelDto>(new[] { new ProtoBufFormatter() }).Result;
            ////    Console.WriteLine("{0}\t{1};\t{2}", p.Name, p.StringValue, p.Id);
            ////}
            ////else
            ////{
            ////    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            ////}

            ////// HTTP POST with Protobuf Request Body
            ////var responseForPost = client.PostAsync("api/Values", new ProtobufModelDto { Id = 1, Name = "test", StringValue = "todo" }, new ProtoBufFormatter()).Result;

            ////if (responseForPost.IsSuccessStatusCode)
            ////{
            ////    // Parse the response body. Blocking!
            ////    Console.WriteLine("All ok");
            ////}
            ////else
            ////{
            ////    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            ////}

        }

    }
}
