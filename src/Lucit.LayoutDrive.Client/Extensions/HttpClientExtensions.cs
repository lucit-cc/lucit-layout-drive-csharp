using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lucit.LayoutDrive.Client.Models;
using Newtonsoft.Json;

namespace Lucit.LayoutDrive.Client.Extensions
{
    internal static class HttpClientExtensions
    {
        public static Task<ResponseWrapper<TReceive>> GetAsync<TReceive>(this HttpClient httpClient, string url)
        {
            return httpClient.SendAsync<TReceive, object>(HttpMethod.Get, url);
        }
        public static Task<ResponseWrapper<TReceive>> PostAsync<TReceive, TSend>(this HttpClient httpClient, string url, TSend values)
        {
            return httpClient.SendAsync<TReceive, TSend>(HttpMethod.Post, url, values);
        }

        public static Task<ResponseWrapper<TReceive>> PutAsync<TSend, TReceive>(this HttpClient httpClient, string url, TSend values)
        {
            return httpClient.SendAsync<TReceive, TSend>(HttpMethod.Put, url, values);
        }

        public static Task<ResponseWrapper<TReceive>> PatchAsync<TSend, TReceive>(this HttpClient httpClient, string url, TSend values)
        {
            return httpClient.SendAsync<TReceive, TSend>(new HttpMethod("PATCH"), url, values);
        }

        public static Task<ResponseWrapper<TReceive>> DeleteAsync<TReceive>(this HttpClient httpClient, string url)
        {
            return httpClient.SendAsync<TReceive, TReceive>(HttpMethod.Delete, url);
        }


        private static async Task<ResponseWrapper<TReceive>> SendAsync<TReceive, TSend>(
           this HttpClient httpClient,
           HttpMethod method,
           string url,
           TSend values = default,
           JsonSerializerSettings serializerSettings = null)
        {
            serializerSettings = serializerSettings ?? new JsonSerializerSettings();

            var result = new ResponseWrapper<TReceive>();

            url = url.Trim('/');

            HttpResponseMessage response;

            if (method == HttpMethod.Post)
            {
                var content = new StringContent(JsonConvert.SerializeObject(values, serializerSettings), Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync(url, content)
                                           .ConfigureAwait(false);
            }
            else if (method.Method.Equals("PATCH"))
            {
                var content = new StringContent(JsonConvert.SerializeObject(values, serializerSettings), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(method, url) { Content = content };

                response = await httpClient.SendAsync(request)
                                           .ConfigureAwait(false);
            }
            else if (method == HttpMethod.Put)
            {
                var content = new StringContent(JsonConvert.SerializeObject(values, serializerSettings), Encoding.UTF8, "application/json");
                response = await httpClient.PutAsync(url, content)
                                           .ConfigureAwait(false);
            }
            else if (method == HttpMethod.Delete)
            {
                response = await httpClient.DeleteAsync(url)
                                            .ConfigureAwait(false);
            }
            else
            {
                response = await httpClient.GetAsync(url)
                                           .ConfigureAwait(false);
            }

            var responseRaw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                result.Exception = new LayoutDriveException($"Something went wong... Http Code: {response.StatusCode}")
                {
                    RawResponse = responseRaw
                };
                return result;
            }

            result.Data = JsonConvert.DeserializeObject<TReceive>(responseRaw);
            return result;
        }
    }
}