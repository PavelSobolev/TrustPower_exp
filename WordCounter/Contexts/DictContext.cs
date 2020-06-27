using Microsoft.EntityFrameworkCore;
using WordCounter.Models;

namespace WordCounter.Contexts
{

    /// <summary>
    /// Database context
    /// </summary>
    public class DictContext : DbContext
    {
        public DictContext(DbContextOptions<DictContext> options) : base(options)
        {
        }

        /// <summary>
        /// This is the set of words in the DB
        /// </summary>
        public DbSet<DictEntry> Words { get; set; }
        public DbSet<IgnoredWordModel> IgnoredWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set names of tables in the DB
            modelBuilder.Entity<DictEntry>().ToTable("Words");
            modelBuilder.Entity<IgnoredWordModel>().ToTable("IgnoredWords");
        }
    }
}
