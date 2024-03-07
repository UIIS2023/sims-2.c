using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Data/users.csv";
        private readonly Serializer<User> serializer;
        private List<User> users;

        public UserRepository()
        {
            serializer = new Serializer<User>();
            users = serializer.FromCSV(FilePath);
        }

        public User? GetByUsername(string username)
        {
            users = serializer.FromCSV(FilePath);
            return users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public User GetOne(int id)
        {
            return users.Find(user => user.Id == id);
        }

        public User Update(User user)
        {
            users = serializer.FromCSV(FilePath);
            var current = users.Find(c => c.Id == user.Id);
            var index = users.IndexOf(current);
            users.Remove(current);
            users.Insert(index, user);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, users);
            return user;
        }

        public string GetFullName(int id)
        {
            return GetOne(id).FullName;
        }
    }
}

