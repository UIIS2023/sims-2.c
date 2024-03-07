using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetOne(int id);
        public User? GetByUsername(string username);
        public User Update(User user);
        public string GetFullName(int id);
    }
}
