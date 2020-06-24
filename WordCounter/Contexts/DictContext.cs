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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set name of table in the DB
            modelBuilder.Entity<DictEntry>().ToTable("Words");
        }
    }
}
