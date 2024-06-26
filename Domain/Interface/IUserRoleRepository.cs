using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.Interface
{
    public interface IUserRoleRepository
    {
        IEnumerable<UserRole> GetAll(Expression<Func<UserRole, bool>>? filter = null, string? includeProperties = null);
        UserRole Get(Expression<Func<UserRole, bool>>? filter, string? includeProperties = null);
        UserRole GetByUserId(int userId);
        void Add(UserRole entity);
        void Update(UserRole entity);
        void Remove(UserRole entity);
        void Save();

        //Task<IEnumerable<UserRole>> GetUserRolesAsync();
        //Task<UserRole> GetUserRoleByIdAsync(int id);
        //Task AddUserRoleAsync(UserRole userRole);
        //Task UpdateUserRoleAsync(UserRole userRole);
        //Task DeleteUserRoleAsync(int id);
        //Task Save();
    }
}
