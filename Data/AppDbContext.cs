using Microsoft.EntityFrameworkCore;

using board_dotnet.Model;

namespace board_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        public AppDbContext() {}

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"Server=rds-mysql.cvewupsq1piq.ap-northeast-2.rds.amazonaws.com,3066;User Id=wonjun;Password=ekdud0822;Database=MYSQL_DB";

            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(b => b.articleComments)
                .WithOne();

            modelBuilder.Entity<Article>()
                .Navigation(b => b.articleComments)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList().ForEach(e => {
                e.Property("createId").CurrentValue = "wonjun";
                e.Property("createDate").CurrentValue = DateTime.Now;
            });

            ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList().ForEach(e => {
                e.Property("updateId").CurrentValue = "wonjun";
                e.Property("updateDate").CurrentValue = DateTime.Now;
            });

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}