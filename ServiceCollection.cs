using board_dotnet.JWT;
using board_dotnet.Repository;
using board_dotnet.Service;
using board_dotnet.Data;
using board_dotnet.Interceptors;

using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollection
    {
        private static IServiceCollection? _services;
        private static IConfiguration? _config;

        private static readonly string connection = @"Server=rds-mysql.cvewupsq1piq.ap-northeast-2.rds.amazonaws.com,3066;User Id=wonjun;Password=ekdud0822;Database=MYSQL_DB_NET";

        public static IServiceCollection AddServiceCollection(this IServiceCollection services, IConfiguration config)
        {
            _services = services;
            _config = config;

            AddDbContextPool();
                        
            AddServiceCollection();

            AddConfigGroup();
            AddCompression();

            return _services;
        }

        private static void AddDbContextPool()
        {
            //_services?.AddDbContextPool<AppDbContext>(o => o.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            _services?.AddDbContextPool<AppDbContext>((serviceProvider, contextBuilder) => {
                var auditingEntitiesInterceptor = serviceProvider.GetService<AuditingEntitiesInterceptor>()!;

                contextBuilder.UseMySql(connection, ServerVersion.AutoDetect(_config["ConnectionStrings:MySQL"]));
                contextBuilder.AddInterceptors(auditingEntitiesInterceptor);
            });
        }

        private static void AddConfigGroup()
        {
            _services?.ConfigureOptions<JwtOptionSetup>();
        }

        private static void AddCompression()
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
            // Singleton - 서비스 시작 시 생성되어 종료될때까지 오직 1개만 존재
            // Scoped - Request 요청부터 Response 응답까지 유지 / 연결 된 클라이언트의 수만큼 존재하게 될 수 있음
            // Transient - 서비스 요청시마다 생성
            _services?.AddSingleton<AuditingEntitiesInterceptor>();
            
            _services?.AddSingleton<IAuthProvider, AuthProvider>();
            _services?.AddSingleton<IJwtProvider, JwtProvider>();

            _services?.AddScoped<IAuthService, AuthService>();
            _services?.AddScoped<IArticleService, ArticleService>();
            _services?.AddScoped<ICommentService, CommentService>();
            _services?.AddScoped<IMemberService, MemberService>();
            _services?.AddScoped<IAttachFileService, AttachFileService>();
            _services?.AddScoped<IAzureStorageService, AzureStorageService>();
            _services?.AddScoped<IRedisService, RedisService>();

            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_config["ConnectionStrings:Redis"]);

            _services?.AddScoped(s => redis.GetDatabase());
        }
    }
}