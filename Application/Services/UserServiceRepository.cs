using Domain.DTO;
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

        //definir une exception personnamisé
        public class AfficheException : Exception
        {
            public AfficheException(string message) : base(message) { }
        }

        public void RegisterUser(string name, string surname, string username, string password, string email, string roleName)
        {
            //verifie si username exist
            var existingUser = _userRepository.Get(u => u.Username == username);
            if (existingUser != null)
            {
                throw new AfficheException("Username already exists");
            }
            else
            {
                //verifie si le le role existe deja et insert le role dans la base sinon
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

                //definir l-user à inserer
                var user = new User
                {
                    Name = name,
                    Surname = surname,
                    Username = username,
                    Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash password
                    Email = email
                };

                _userRepository.Add(user);

                //insertion dans UserRole
                var userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId
                };

                _userRoleRepository.Add(userRole);
                _userRoleRepository.Save();
            }

        }

        //-- update user -----
        public void UpdateUser(int UserId, string name, string surname, string username, string password, string email, string roleName)
        {
            // Récupérer l'utilisateur existant
            var existingUser = _userRepository.Get(u => u.UserId == UserId);
            if (existingUser == null)
            {
                throw new AfficheException("User not found");
            }

            // Vérifier si un autre utilisateur avec le même nom d'utilisateur existe
            var userWithSameUsername = _userRepository.Get(u => u.Username == username && u.UserId != UserId);
            if (userWithSameUsername != null)
            {
                throw new AfficheException("Username already exists");
            }
            else
            {
                //definir l-user à inserer
                var user = new User
                {
                    UserId = UserId,
                    Name = name,
                    Surname = surname,
                    Username = username,
                    Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash password
                    Email = email
                };
                var role = _roleRepository.Get(r => r.Name == roleName); //pour recuperer l'RoleId
                var userRoleToUpdate = _userRoleRepository.Get(ur => ur.UserId == user.UserId); //pour recuperer IdUserRole

                if (role != null && userRoleToUpdate != null)
                {
                    userRoleToUpdate.UserId = user.UserId;
                    userRoleToUpdate.RoleId = role.RoleId;                    
                    _userRoleRepository.Update(userRoleToUpdate);
                    
                    //excecute l'update
                    _userRepository.Update(user);               
                }
            }
        }
        // -------------------------------------------

        public UserListWithRole Authenticate(string username, string password)
        {
            var user = _userRepository.Get(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public Role GetRoleByUserId(int userId)
        {
            var userRole = _userRoleRepository.GetByUserId(userId);
            if (userRole != null)
            {
                return _roleRepository.Get(r => r.RoleId == userRole.RoleId);
            }
            return null;
        }

        public IEnumerable<UserListWithRole> GetUser()
        {
            return _userRepository.GetAll();
        }

        public UserListWithRole GetUserById(int userId)
        {
            return _userRepository.Get(u => u.UserId == userId);
        }

        public User GetUserWithoutRole(int userId)
        {
            return _userRepository.GetUserWithoutRole(u => u.UserId == userId);
        }

        //-- Delete User -----------
        public void DeleteUserService(User model)
        {
            _userRepository.Remove(model);
        }
        //-----------------------------------------
    }
}
