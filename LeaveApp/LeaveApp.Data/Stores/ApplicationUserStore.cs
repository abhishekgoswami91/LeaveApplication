using LeaveApp.Data.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveApp.Data.Stores
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore() : base(new ApplicationDbContext())
        {
        }

    }
}
