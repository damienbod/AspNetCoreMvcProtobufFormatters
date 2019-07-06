using Serilog;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApiClient
{
    public class Program
    {
        private readonly HttpClient _client;
        private static ApiTokenInMemoryClient _tokenService;

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Verbose()
              .Enrich.WithProperty("App", "ConsoleResourceOwnerFlowRefreshToken")
              .Enrich.FromLogContext()
              .WriteTo.Seq("http://localhost:5341")
              .WriteTo.ColoredConsole()
              //.WriteTo.RollingFile("../Log/ConsoleResourceOwnerFlowRefreshToken")
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
        }
    }
}