using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SocialMedia.Framework.Core.Login;
using Microsoft.Extensions.Configuration;
using SocialMedia.Framework.Core;
using SocialMedia.Framework.Core.Logging;

namespace SocialMedia.Framework.Data
{
    public class SMDBContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public SMDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            //options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"), b => b.MigrationsAssembly("SocialMedia.Framework.Web"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Deslike> Deslikes { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
