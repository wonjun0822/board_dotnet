using board_dotnet.JWT;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace board_dotnet.Interceptors;

public sealed class AuditingEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IAuthProvider _authProvider;
    
    public AuditingEntitiesInterceptor(IAuthProvider authProvider)
    {
        _authProvider = authProvider;
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
            e.Property("createBy").CurrentValue = _authProvider.GetById();
            e.Property("createAt").CurrentValue = DateTime.Now;

            e.Property("updateBy").CurrentValue = _authProvider.GetById();
            e.Property("updateAt").CurrentValue = DateTime.Now;
        });

        dbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList().ForEach(e => {
            if (!e.Members.Any(e => e.Metadata.Name == "viewCount"))
            {
                e.Property("updateBy").CurrentValue = _authProvider.GetById();
                e.Property("updateAt").CurrentValue = DateTime.Now;
            }

            else if (e.Property("viewCount").IsModified == false)
            {
                e.Property("updateBy").CurrentValue = _authProvider.GetById();
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