using System.Data.Entity;
using Instagram.Models;

namespace Instagram.Classes
{
    public class InstagramDBContext : DbContext
    {
        public InstagramDBContext() : base("DbConnection")
        { }
        public DbSet<Account> Users { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}