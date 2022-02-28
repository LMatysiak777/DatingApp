using API.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using API.interfaces;
using AutoMapper;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<MemberDto> GetMemberAsync(string username) 
        {
            return await this.context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        } 
        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await this.context.Users
            .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await this.context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(ex => ex.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this.context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;

        }

        public void Update(AppUser user)
        {
            this.context.Entry(user).State = EntityState.Modified;
        }
    }
}