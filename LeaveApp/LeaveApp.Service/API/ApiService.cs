using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace LeaveApp.Service.API
{
    public class ApiService : IApiService
    {
        public async Task<T> MakeApiCallAsync<T>(string endPoint, HttpMethod httpMethod, dynamic payload = null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            var baseApiUrl = WebConfigurationManager.AppSettings["BaseApiUrl"];
            HttpContent content = null;
            if (payload != null)
            {
                content = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );
            }
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(baseApiUrl + endPoint, UriKind.Absolute),
                Method = httpMethod,
                Content = content
            };
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(240);
            var httpResponseMessage = await httpClient.SendAsync(httpRequest);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                return response;
            }
            else
            {
                var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }

        public async Task<T> GetAccessTokenAsync<T>(string endPoint, HttpMethod httpMethod, dynamic payload = null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            var baseApiUrl = WebConfigurationManager.AppSettings["BaseApiUrl"];
            HttpContent content = null;
            if (payload != null)
            {
                content = new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                    payload.Email, payload.Password), 
                    System.Text.Encoding.UTF8, 
                    "application/x-www-form-urlencoded");
            }
            
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(240);
            var httpResponseMessage = await httpClient.PostAsync(new Uri(baseApiUrl + endPoint, UriKind.Absolute), content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                return response;
            }
            else
            {
                var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }

        public async Task<T> MakePrivateApiCallAsync<T>(string endPoint, HttpMethod httpMethod, string accessToken, dynamic payload = null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            var baseApiUrl = WebConfigurationManager.AppSettings["BaseApiUrl"];
            HttpContent content = null;
            if (payload != null)
            {
                content = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );
            }
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(baseApiUrl + endPoint, UriKind.Absolute),
                Method = httpMethod,
                Content = content,
            };
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(240);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var httpResponseMessage = await httpClient.SendAsync(httpRequest);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                    return response;
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
        }
    }
}
