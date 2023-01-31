using Microsoft.EntityFrameworkCore;

using board_dotnet.Model;

using System.Security.Claims;

namespace board_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        private readonly string _memberId = string.Empty;

        public AppDbContext() {}

        public AppDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _memberId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(b => b.articleComments)
                .WithOne();

            modelBuilder.Entity<Article>()
                .Navigation(b => b.articleComments)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            // modelBuilder.Entity<Comment>()
            //     .Property<long>("articleId")
            //     .HasColumnName("article_id");

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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList().ForEach(e => {
                e.Property("createId").CurrentValue = _memberId;
                e.Property("createDate").CurrentValue = DateTime.Now;
            });

            ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList().ForEach(e => {
                e.Property("updateId").CurrentValue = _memberId;
                e.Property("updateDate").CurrentValue = DateTime.Now;
            });

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}