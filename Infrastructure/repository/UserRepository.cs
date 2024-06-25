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

        public void Remove(User entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
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
                    //insere l'UserId connécté dans la table app_context
                    _context.Database.ExecuteSqlRaw("INSERT INTO app_context (user_id) VALUES ({0})", currentUserId);
                    _context.Update(entity);
                    _context.SaveChanges();

                    // Supprimer l'entrée de app_context après mise à jour
                    _context.Database.ExecuteSqlRaw("DELETE FROM app_context WHERE user_id = {0} ORDER BY context_id DESC LIMIT 1", currentUserId);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
