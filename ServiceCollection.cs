using board_dotnet.JWT;
using board_dotnet.Repository;
using board_dotnet.Data;

using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using board_dotnet.Interceptors;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollection
    {
        private static IServiceCollection? _services;

        private static readonly string connection = @"Server=rds-mysql.cvewupsq1piq.ap-northeast-2.rds.amazonaws.com,3066;User Id=wonjun;Password=ekdud0822;Database=MYSQL_DB_NET";

        public static IServiceCollection AddServiceCollection(this IServiceCollection services, IConfiguration config)
        {
            _services = services;

            AddDbContextPool();
                        
            AddServiceCollection();

            AddConfigGroup(config);
            AddCompression(config);

            return _services;
        }

        private static void AddDbContextPool()
        {
            //_services?.AddDbContextPool<AppDbContext>(o => o.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            _services?.AddDbContextPool<AppDbContext>((serviceProvider, contextBuilder) => {
                var auditingEntitiesInterceptor = serviceProvider.GetService<AuditingEntitiesInterceptor>()!;

                contextBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
                contextBuilder.AddInterceptors(auditingEntitiesInterceptor);
            });
        }

        private static void AddConfigGroup(IConfiguration config)
        {
            _services?.ConfigureOptions<JwtOptionSetup>();
        }

        private static void AddCompression(IConfiguration config)
        {
            _services?.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = new[] { "text/plain", "text/css", "application/javascript", "text/html", "application/json", "application/vnd.ms-excel" };
            });

            _services?.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            _services?.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }

        private static void AddServiceCollection()
        {
            _services?.AddSingleton<AuditingEntitiesInterceptor>();
            _services?.AddSingleton<IUserResolverProvider, UserResolverProvider>();

            _services?.AddScoped<IArticleRepository, ArticleRepository>();
            _services?.AddScoped<ICommentRepository, CommentRepository>();
            _services?.AddScoped<IMemberRepository, MemberRepository>();

            _services?.AddScoped<IJwtProvider, JwtProvider>();
        }
    }
}