using Domain.Entity;
using Domain.Interface;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserServiceRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserServiceRepository(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public void RegisterUser(string name, string surname, string username, string password, string email, string roleName)
        {
            var existingUser = _userRepository.Get(u => u.Username == username);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var role = _roleRepository.Get(r => r.Name == roleName);
            if (role == null)
            {
                role = new Role
                {
                    Name = roleName,
                };
                _roleRepository.Add(role);
                _roleRepository.Save();
            }

            var user = new User
            {
                Name = name,
                Surname = surname,
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash password
                Email = email
            };

            _userRepository.Add(user);
            _userRepository.Save();

            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = role.RoleId
            };

            _userRoleRepository.Add(userRole);
            _userRoleRepository.Save();
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.Get(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }
    }
}
