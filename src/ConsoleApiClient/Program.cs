using AspNetCoreProtobuf.Model;
using IdentityModel.Client;
using Serilog;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiClient
{
    public class Program
    {
        private static ApiTokenInMemoryClient _tokenService;

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Verbose()
              .Enrich.WithProperty("App", "ApiProtoClient")
              .Enrich.FromLogContext()
              //.WriteTo.Seq("http://localhost:5341")
              .WriteTo.ColoredConsole()
              .CreateLogger();

            
            _tokenService = new ApiTokenInMemoryClient("https://localhost:44318", new HttpClient());
            var client = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:44336")
            };

            var access_token = await _tokenService.GetApiToken(
                   "ClientProtectedApi",
                   "apiproto",
                   "apiprotoSecret"
               );

            client.SetBearerToken(access_token);

            Log.Logger.Information("GOT TOKENS FROM IDENTITYSERVER4: {AccessToken}", access_token);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var response = await client.GetAsync("/api/values/1");
            response.EnsureSuccessStatusCode();

            var responseString = System.Text.Encoding.UTF8.GetString(
                await response.Content.ReadAsByteArrayAsync()
            );

            Log.Logger.Information("GOT DATA FROM THE RESOURCE SERVER");

            // Write data to the server
            
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<ProtobufModelDto>(stream, new ProtobufModelDto
            {
                Id = 2,
                Name = "lovely data",
                StringValue = "amazing this ah"

            });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/Values")
            {
                Content = new StringContent(
                    StreamToString(stream),
                    Encoding.UTF8,
                    "application/x-protobuf")//CONTENT-TYPE header
            };

            var responseForPost = await client.SendAsync(request);
          
            Log.Logger.Information($"GOT DATA FROM THE RESOURCE SERVER code: {responseForPost.StatusCode}");
        }

        private static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}