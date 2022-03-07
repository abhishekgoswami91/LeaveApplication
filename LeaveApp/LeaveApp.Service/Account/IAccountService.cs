using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Account
{
    public interface IAccountService
    {
        void CreateUser();
        string LogIn(string UserName, string Password);

    }
}
