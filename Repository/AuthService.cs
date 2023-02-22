using System.Security.Cryptography;
using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;
using board_dotnet.Service;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class AuthService : IAuthService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IAuthProvider _authProvider;
        private readonly IMemberService _memberService;
        private readonly IRedisService _redisService;

        public AuthService(IJwtProvider jwtProvider, IAuthProvider authProvider, IMemberService memberService, IRedisService redisService)
        {
            _jwtProvider = jwtProvider;
            _authProvider = authProvider;
            _memberService = memberService;
            _redisService = redisService;
        }

        public async Task<TokenDTO?> Login(string email, string pwd)
        {
            var member = await _memberService.GetMemberByPassword(email, pwd);

            if (member is null)
                return null;

            string accessToken = _jwtProvider.GenerateToken(member);
            string refreshToken = GenerateRefreshToken();

            await _redisService.StringSet(member.id.ToString(), refreshToken, TimeSpan.FromDays(7));

            return new TokenDTO() {
                accessToken = accessToken,
                refreshToken = refreshToken
            };
        }

        public async Task<TokenDTO?> RefreshToken()
        {
            long memberId = _authProvider.GetById();

            string serverRefreshToken = await _redisService.StringGet(memberId.ToString());
            string clientRefreshToken = _authProvider.GetCookie("refreshToken");

            if (serverRefreshToken == clientRefreshToken) {
                var member = await _memberService.GetMemberById(Convert.ToInt64(memberId));

                string accessToken = _jwtProvider.GenerateToken(member);
                string refreshToken = GenerateRefreshToken();

                await _redisService.StringSet(memberId.ToString(), refreshToken, TimeSpan.FromDays(7));

                return new TokenDTO() {
                    accessToken = accessToken,
                    refreshToken = refreshToken
                };
            }

            return null;
        }
        
        public async Task Logout()
        {
            long id = _authProvider.GetById();
            
            await _redisService.DeleteKey(id.ToString());
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}