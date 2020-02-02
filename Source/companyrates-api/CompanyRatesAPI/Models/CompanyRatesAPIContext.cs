using System.Data.Entity;

namespace CompanyRatesAPI.Models
{
    public class CompanyRatesAPIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        //
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public CompanyRatesAPIContext() : base("name=CompanyRatesAPIContext")
        {
        }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.VoteCompany> VoteCompanies { get; set; }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.VoteReview> VoteReviews { get; set; }

        public System.Data.Entity.DbSet<CompanyRatesAPI.Models.LoginSession> LoginSessions { get; set; }
    }
}