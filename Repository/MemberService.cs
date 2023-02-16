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

        public async Task<MemberDTO?> GetMemberById(string id)
        {
            var member = await _context.Members
                    .AsNoTracking()
                    .Where(x => x.member_id == id)
                    .Select(
                        s => new MemberDTO() { 
                            id = s.member_id,
                            email = s.email,
                            nickname = s.nickname
                        }
                    ).FirstOrDefaultAsync();
            
            return member;
        }

        public async Task<MemberDTO?> GetMemberByPassword(string id, string pwd)
        {
            var member = await _context.Members
                    .AsNoTracking()
                    .Where(x => x.member_id == id && x.pwd == pwd)
                    .Select(
                        s => new MemberDTO() { 
                            id = s.member_id,
                            email = s.email,
                            nickname = s.nickname
                        }
                    ).FirstOrDefaultAsync();
            
            return member;
        }
    }
}