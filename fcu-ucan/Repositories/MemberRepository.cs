using System.Collections.Generic;
using System.Threading.Tasks;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ILogger<MemberRepository> _logger;
        private readonly ApplicationDbContext _dbContext;

        public MemberRepository(ILogger<MemberRepository> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _dbContext.Members
                .ToListAsync();
        }

        public async Task<Member> GetByIdAsync(string id)
        {
            return await _dbContext.Members
                .SingleOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<Member> GetByNetworkIdAsync(string networkId)
        {
            return await _dbContext.Members
                .SingleOrDefaultAsync(x => x.NetworkId == networkId);
        }
        
        public async Task<Member> GetByStudentIdAsync(string studentId)
        {
            return await _dbContext.Members
                .SingleOrDefaultAsync(x => x.StudentId == studentId);
        }

        public async Task<bool> ExistByIdAsync(string id)
        {
            return await _dbContext.Members
                .AnyAsync(x => x.Id == id);
        }
        
        public async Task<bool> ExistByNetworkIdAsync(string networkId)
        {
            return await _dbContext.Members
                .AnyAsync(x => x.NetworkId == networkId);
        }
        
        public async Task<bool> ExistByStudentIdAsync(string studentId)
        {
            return await _dbContext.Members
                .AnyAsync(x => x.StudentId == studentId);
        }
        
        public void Add(Member entity)
        {
            _dbContext.Members.Add(entity);
        }
        
        public void Update(Member entity)
        {
            _dbContext.Members.Update(entity);
        }

        public void Delete(Member entity)
        {
            _dbContext.Members.Remove(entity);
        }
        
        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}