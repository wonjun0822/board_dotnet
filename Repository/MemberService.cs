using board_dotnet.Data;
using board_dotnet.Model;
using board_dotnet.Service;

using Microsoft.EntityFrameworkCore;

namespace board_dotnet.Repository
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
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