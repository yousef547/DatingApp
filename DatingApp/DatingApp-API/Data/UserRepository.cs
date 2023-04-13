using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Helpers;
using DatingApp_API.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Data
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.AppUsers.Where(u=>u.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.AppUsers
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();
            return await PageList<MemberDto>.CreateAsync(query, userParams.pageNumber, userParams.pageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.AppUsers.Include(x => x.Photos).SingleOrDefaultAsync(x=>x.UserName == username);

        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.AppUsers.Include(x=>x.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser uers)
        {
            _context.Entry(uers).State = EntityState.Modified;
        }
    }
}
