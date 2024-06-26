using Domain.DTO;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.Interface
{
    public interface IUserRepository
    {
        IEnumerable<UserListWithRole> GetAll(Expression<Func<User, bool>>? filter = null, string? includeProperties = null);
        UserListWithRole Get(Expression<Func<User, bool>>? filter, string? includeProperties = null);
        User GetUserWithoutRole(Expression<Func<User, bool>>? filter, string? includeProperties = null);
        
        void Add(User entity);
        void Update(User entity, int currentUserId);
        void Remove(User entity, int currentUserId);
        void Save();

        //Task<IEnumerable<User>> GetUsersAsync();
        //Task<User> GetUserByIdAsync(int id);
        //Task<User> GetUserByUsernameAsync(string username);
        //Task AddUserAsync(User user);
        //Task UpdateUserAsync(User user);
        //Task DeleteUserAsync(int id);
    }
}
