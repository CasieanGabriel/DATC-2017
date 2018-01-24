using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DATC
{
    public interface IFacebookClient
    {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
        Task<T> GetAsync<T>(string id);
        Task PostAsync(object data);
        Task PutAsync(string id, object data);
        Task DeleteAsync(string id);
    }

    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _httpClient;

        public FacebookClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetAsync<T>(string id)
        {
            var response = await _httpClient.GetAsync("http://localhost:38501/api/account/" + id);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task PostAsync(object data)
        {
            var payload = GetPayload(data);
            await _httpClient.PostAsync("http://localhost:38501/api/accounts", payload);
        }

        public async Task PutAsync(string id, object data)
        {
            var payload = GetPayload(data);
            await _httpClient.PutAsync("http://localhost:38501/api/account/" + id, payload);
        }

        public async Task DeleteAsync(string id)
            => await _httpClient.DeleteAsync("http://localhost:38501/api/account/" + id);
    }
}
