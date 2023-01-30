using board_dotnet.Authentication;
using board_dotnet.Interface;
using board_dotnet.Repository;

using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollection
    {
        private static IServiceCollection? _services;

        public static IServiceCollection AddServiceCollection(this IServiceCollection services, IConfiguration config)
        {
            _services = services;
            
            AddConfigGroup(config);
            AddCompression(config);
            
            AddScopeGroup();

            return _services;
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

        private static void AddScopeGroup()
        {
            _services?.AddScoped<IArticleRepository, ArticleRepository>();
            _services?.AddScoped<ICommentRepository, CommentRepository>();
            _services?.AddScoped<IMemberRepository, MemberRepository>();
            _services?.AddScoped<IJwtProvider, JwtProvider>();
        }
    }
}