using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public UserListWithRole Authenticate(string? username, string? pass)
        {
            try
            {
                var result = new UserListWithRole();
                var user = _context.User.FirstOrDefault(u => u.Username == username && u.Password == pass);
                if (user != null)
                {
                    result = new UserListWithRole
                    {
                        UserId = user.UserId,
                        Surname = user.Surname,
                        Name = user.Name,
                        Username = user.Username,  
                        Email = user.Email,
                    };     
                    return result;   
                }
                else
                {
                    return result=null;
                }  

            }
            catch (Exception ex) {
                throw;
            }
        }

        public UserListWithRole Get(Expression<Func<User, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<User> query = _context.Set<User>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            //recuperation des Role where l'USER et l'injecte dans User
            var result = query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserListWithRole
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Surname = u.Surname,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    RoleName = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                })
                .FirstOrDefault();

            return result;

            //return query.FirstOrDefault();
        }

        public User GetUserWithoutRole(Expression<Func<User, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<User> query = _context.Set<User>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }


        public IEnumerable<UserListWithRole> GetAll(Expression<Func<User, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<User> query = _context.Set<User>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Liste des noms d'utilisateur à exclure
            var excludedUsernames = new List<string> { "defaultUser", "georges", "nico", "jeanpierre" };
            // Exclure les utilisateurs avec les noms d'utilisateur spécifiés
            query = query.Where(u => !excludedUsernames.Contains(u.Username));

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            //recuperation des Role where l'USER
            var result = query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserListWithRole
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Surname = u.Surname,
                    Username = u.Username,
                    Email = u.Email,
                    RoleName = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                })
                .ToList();

            return result;
        }

        public void Remove(User entity, int currentUserId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //denifinition du session pour utiliser dans trigger
                    _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", currentUserId);

                    _context.Remove(entity);
                    _context.SaveChanges();

                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Save() 
        {
            _context.SaveChanges();
        }

        public void Update(User entity, int currentUserId)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //denifinition du session pour utiliser dans trigger
                    _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", currentUserId);

                    _context.Update(entity);
                    _context.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        //public void Remove(User entity, int currentUserId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
