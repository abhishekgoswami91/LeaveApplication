using LeaveApp.Data.DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeaveApp.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("leaveConnection")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<EmployeeLeave>()
        //                .HasRequired(r => r.ApplicationUser)
        //                .WithMany(p => p.EmployeeLeaves)
        //                .HasForeignKey(p => p.EmployeeId);
        //}
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<EmployeeLeaveDetail> EmployeeLeaveDetails { get; set; }
        public DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        
        public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<EmployeeBonusLeave> EmployeeBonusLeaves { get; set; }
        public DbSet<IssueDetail> IssueDetails { get; set; }

    }
}
