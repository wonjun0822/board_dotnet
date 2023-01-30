using board_dotnet.Data;

using Microsoft.EntityFrameworkCore;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                try
                {
                    appContext.Database.Migrate();
                }
                
                catch (Exception)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        }

        return webApp;
    }
}