using System.Net.Http;
using WebEF.Repository.IRequest;

namespace WebEF.Repository.Request
{
    public class Petition : IPetition
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient httpClient = new();
        public Petition(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ClientAsync(string tipoPeticion, string peticion, HttpContent? body = null)
        {
            var url = _configuration["UrlAPI"];

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            switch (tipoPeticion)
            {
                case "Get":
                    httpResponse = await httpClient.GetAsync(url + peticion);

                    break;
                case "Post":
                    httpResponse = await httpClient.PostAsync(url + peticion, body);
                    break;
                case "Delete":
                    httpResponse = await httpClient.DeleteAsync(url + peticion);
                    break;
                case "Put":
                    httpResponse = await httpClient.PutAsync(url + peticion, body);
                    break;
                default:
                    break;

            }
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            else
            {
                return "";
            }
        }
    }
}
