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
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Role entity)
        {
            _context.Add(entity);
        }

        public Role Get(Expression<Func<Role, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<Role> query = _context.Set<Role>();
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

        public IEnumerable<Role> GetAll(Expression<Func<Role, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Role> query = _context.Set<Role>();
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

            return query.ToList();
        }

        public void Remove(Role entity)
        {
            _context.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Role entity)
        {
            _context.SaveChanges();
        }

        //public async Task<IEnumerable<Role>> Get()
        //{
        //    return await _context.Roles.ToListAsync();
        //}

        //public async Task<Role> GetRoleByIdAsync(int id)
        //{
        //    return await _context.Roles.FindAsync(id);
        //}

        //public async Task<Role> GetRoleByNameAsync(string name)
        //{
        //    return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        //}

        //public async Task AddRoleAsync(Role role)
        //{
        //    await _context.Roles.AddAsync(role);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateRoleAsync(Role role)
        //{
        //    _context.Roles.Update(role);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteRoleAsync(int id)
        //{
        //    var role = await _context.Roles.FindAsync(id);
        //    if (role != null)
        //    {
        //        _context.Roles.Remove(role);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
