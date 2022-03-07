
namespace LeaveApp.Web.Models
{
    public class AccessTokenResultModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}