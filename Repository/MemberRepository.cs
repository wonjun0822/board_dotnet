using board_dotnet.Data;
using board_dotnet.DTO;
using board_dotnet.Interface;
using board_dotnet.Model;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Member?> GetMember(string id, string pwd)
        {
            var member = await _context.Members.Where(x => x.member_id == id && x.pwd == pwd).FirstOrDefaultAsync();

            if (member is null)
                return null;

            return member;
        }
    }
}