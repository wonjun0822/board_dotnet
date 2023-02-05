using System.Linq;
using board_dotnet.JWT;
using board_dotnet.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace board_dotnet.Interceptors;

public sealed class AuditingEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IUserResolverProvider _userResolverProvider;
    
    public AuditingEntitiesInterceptor(IUserResolverProvider userResolverProvider)
    {
        _userResolverProvider = userResolverProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    ) {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(
                eventData,
                result, 
                cancellationToken
            );
        }

        dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).ToList().ForEach(e => {
            e.Property("createBy").CurrentValue = _userResolverProvider.GetById();
            e.Property("createAt").CurrentValue = DateTime.Now;

            e.Property("updateBy").CurrentValue = _userResolverProvider.GetById();
            e.Property("updateAt").CurrentValue = DateTime.Now;
        });

        dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList().ForEach(e => {
            if (!e.Members.Any(e => e.Metadata.Name == "viewCount"))
            {
                e.Property("updateBy").CurrentValue = _userResolverProvider.GetById();
                e.Property("updateAt").CurrentValue = DateTime.Now;
            }

            else if (e.Property("viewCount").IsModified == false)
            {
                e.Property("updateBy").CurrentValue = _userResolverProvider.GetById();
                e.Property("updateAt").CurrentValue = DateTime.Now;
            }
        });

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken
        );
    }
}