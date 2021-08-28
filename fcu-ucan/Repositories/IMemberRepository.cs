using System.Collections.Generic;
using System.Threading.Tasks;
using fcu_ucan.Entities;

namespace fcu_ucan.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();

        Task<Member> GetByIdAsync(string id);

        Task<Member> GetByNetworkIdAsync(string networkId);

        Task<Member> GetByStudentIdAsync(string studentId);

        Task<bool> ExistByIdAsync(string id);

        Task<bool> ExistByNetworkIdAsync(string networkId);

        Task<bool> ExistByStudentIdAsync(string studentId);

        void Add(Member entity);

        void Update(Member entity);

        void Delete(Member entity);

        Task<bool> SaveAsync();
    }
}