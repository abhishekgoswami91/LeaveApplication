using LeaveApp.Data.Identity;
using System.Data.Entity;

namespace LeaveApp.Data
{
    public class initializer : MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>
    {
    }
}
