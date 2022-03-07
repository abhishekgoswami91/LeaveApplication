using LeaveApp.Core.ViewModel;
using System.Threading.Tasks;

namespace LeaveApp.Service.Mail
{
    public interface IMailService
    {
         bool SendApplyLeaveMail(MailModel model, bool ToAdmin);
    }
}
