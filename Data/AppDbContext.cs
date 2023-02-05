using Microsoft.EntityFrameworkCore;

using board_dotnet.Model;

namespace board_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<AttachFile> AttachFiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(b => b.articleComments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>()
                .Navigation(b => b.articleComments)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Article>()
                .HasMany(b => b.articleFiles)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>()
                .Navigation(b => b.articleFiles)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Article>()
                .HasOne<Member>(b => b.member)
                .WithMany()
                .HasForeignKey(b => b.createBy)
                .HasPrincipalKey(b => b.member_id);

            modelBuilder.Entity<Article>()
                .Navigation(b => b.member)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            modelBuilder.Entity<Comment>()
                .HasOne<Member>(b => b.member)
                .WithMany()
                .HasForeignKey(b => b.createBy)
                .HasPrincipalKey(b => b.member_id);

            modelBuilder.Entity<Comment>()
                .Navigation(b => b.member)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }

        // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
        //     ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList().ForEach(e => {
        //         e.Property("createBy").CurrentValue = string.Empty;
        //         e.Property("createAt").CurrentValue = DateTime.Now;
        //     });

        //     ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList().ForEach(e => {
        //         e.Property("updateBy").CurrentValue = string.Empty;
        //         e.Property("updateAt").CurrentValue = DateTime.Now;
        //     });

        //     return base.SaveChangesAsync(cancellationToken);
        // }
    }
}