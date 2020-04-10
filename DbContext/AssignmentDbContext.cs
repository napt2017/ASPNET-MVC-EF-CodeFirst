using ASPNET_MVC_EF_CodeFirst.Models;

namespace ASPNET_MVC_EF_CodeFirst.DbContext
{
    public class AssignmentDbContext : System.Data.Entity.DbContext
    {
        public AssignmentDbContext() :base("AssignmentDB")
        {

        }

        public System.Data.Entity.DbSet<Product> Products { get; set; }
    }
}