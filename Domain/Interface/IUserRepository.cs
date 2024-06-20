using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll(Expression<Func<User, bool>>? filter = null, string? includeProperties = null);
        User Get(Expression<Func<User, bool>>? filter, string? includeProperties = null);
        void Add(User entity);
        void Update(User entity);
        void Remove(User entity);
        void Save();

        //Task<IEnumerable<User>> GetUsersAsync();
        //Task<User> GetUserByIdAsync(int id);
        //Task<User> GetUserByUsernameAsync(string username);
        //Task AddUserAsync(User user);
        //Task UpdateUserAsync(User user);
        //Task DeleteUserAsync(int id);
    }
}
