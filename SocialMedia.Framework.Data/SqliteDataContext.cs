using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SocialMedia.Framework.Data
{
    public class SqliteDataContext : SMDBContext
    {
        public SqliteDataContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            //options.UseSqlite(Configuration.GetConnectionString("SqlConnection"));
            options.UseSqlite(Configuration.GetConnectionString("SqlConnection"), b => b.MigrationsAssembly("SocialMedia.Framework.Web"));
        }
    }
}