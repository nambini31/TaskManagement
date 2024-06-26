using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Domain.Interface
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll(Expression<Func<Role, bool>>? filter = null, string? includeProperties = null);
        Role Get(Expression<Func<Role, bool>>? filter, string? includeProperties = null);
        void Add(Role entity);
        void Update(Role entity);
        void Remove(Role entity);
        void Save();

        //Task<IEnumerable<Role>> GetRolesAsync();
        //Task<Role> GetRoleByIdAsync(int id);
        //Task<Role> GetRoleByNameAsync(string name);
        //Task AddRoleAsync(Role role);
        //Task UpdateRoleAsync(Role role);
        //Task DeleteRoleAsync(int id);
    }
}
