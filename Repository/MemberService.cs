using System.Security.Cryptography;
using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.JWT;
using board_dotnet.Service;

using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace board_dotnet.Repository
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberDTO?> GetMemberById(long id)
        {
            var member = await _context.Members
                    .AsNoTracking()
                    .Where(x => x.id == id)
                    .Select(
                        s => new MemberDTO() { 
                            id = s.id,
                            email = s.email,
                            nickname = s.nickname
                        }
                    ).FirstOrDefaultAsync();
            
            return member;
        }

        public async Task<MemberDTO?> GetMemberByPassword(string email, string pwd)
        {
            var member = await _context.Members
                    .AsNoTracking()
                    .Where(x => x.email == email && x.pwd == pwd)
                    .Select(
                        s => new MemberDTO() { 
                            id = s.id,
                            email = s.email,
                            nickname = s.nickname
                        }
                    ).FirstOrDefaultAsync();
            
            return member;
        }
    }
}