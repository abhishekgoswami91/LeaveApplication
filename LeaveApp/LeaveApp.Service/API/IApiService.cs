using System.Net.Http;
using System.Threading.Tasks;

namespace LeaveApp.Service.API
{
    public interface IApiService
    {
        
        Task<T> MakeApiCallAsync<T>(string endPoint, HttpMethod httpMethod, dynamic payload = null); 

         Task<T> GetAccessTokenAsync<T>(string endPoint, HttpMethod httpMethod, dynamic payload = null);
        Task<T> MakePrivateApiCallAsync<T>(string endPoint, HttpMethod httpMethod, string accessToken, dynamic payload = null);

    }
}
