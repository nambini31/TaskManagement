using Domain.DTO;
using Domain.Entity;
using Domain.Interface;
using Domain.Helper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserServiceRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly DataEncryptor _dataEncryptor;

        public UserServiceRepository(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, DataEncryptor dataEncryptor)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _dataEncryptor = dataEncryptor;
        }

        //definir une exception personnamisé
        public class AfficheException : Exception
        {
            public AfficheException(string message) : base(message) { }
        }

        //-- Initialiser une uesr par default --
        public void InitialiseUser()
        {
            // Insère un Role admin par défaut
            var adminRole = _roleRepository.Get(r => r.Name == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role { Name = "Admin" };
                _roleRepository.Add(adminRole);
                _roleRepository.Save();
            }
        }
        //---------------------------------------------------

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
                    Password = _dataEncryptor.Encrypt(password), // Hash password
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
        public void UpdateUser(int UserId, string name, string surname, string username, string password, string email, string roleName, int currentUserId)
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
                    Password = _dataEncryptor.Encrypt(password), // Hash password
                    Email = email
                };
                var role = _roleRepository.Get(r => r.Name == roleName); //pour recuperer l'RoleId
                var userRoleToUpdate = _userRoleRepository.Get(ur => ur.UserId == user.UserId); //pour recuperer IdUserRole


                //On met a jour les UserRole
                if (role != null && userRoleToUpdate != null)
                {
                    userRoleToUpdate.UserId = user.UserId;
                    userRoleToUpdate.RoleId = role.RoleId;                    
                    _userRoleRepository.Update(userRoleToUpdate);
                    
                    //excecute l'update
                    _userRepository.Update(user, currentUserId);               
                }
            }
        }
        // -------------------------------------------

        public UserListWithRole Authenticate(string username, string password)
        {    
            var cryptPass = "";
            if (password != null || password != "") 
            {
                cryptPass = _dataEncryptor.Encrypt(password);
            }
            return _userRepository.Authenticate(username, cryptPass);
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
            var u = _userRepository.Get(u => u.UserId == userId);
            UserListWithRole user = new UserListWithRole
            {
                UserId = u.UserId,
                Name = u.Name,
                Surname = u.Surname,
                Username = u.Username,
                Email = u.Email,
                Password = _dataEncryptor.Decrypt(u.Password),
                RoleName = u.RoleName,
            };
            return user;
            //1F994D1D91ACCA2CFB57E69858C04BC0661B2651C7C86701C4F2E8FB06226712AD641ABE6F1B8ABEBAE1B3A331B57807B02A9CD2FC57C28DF1FA88F356E56F18
        }

        public User GetUserWithoutRole(int userId)
        {
            return _userRepository.GetUserWithoutRole(u => u.UserId == userId);
        }

        //-- Delete User -----------
        public void DeleteUserService(User model, int currentUserId)
        {
            _userRepository.Remove(model, currentUserId);
        }
        //-----------------------------------------
    }
}
