using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Data/forums.csv";
        private readonly Serializer<Forum> serializer = new();
        private List<Forum> forums;
        public ForumRepository()
        {
            forums = serializer.FromCSV(FilePath);
        }
        public List<Forum> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            forums = serializer.FromCSV(FilePath);
            forums.Add(forum);
            serializer.ToCSV(FilePath, forums);
            return forum;
        }

        public int NextId()
        {
            forums = serializer.FromCSV(FilePath);
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            forums = serializer.FromCSV(FilePath);
            var founded = forums.Find(c => c.Id == id);
            forums.Remove(founded);
            serializer.ToCSV(FilePath, forums);
        }

        public Forum Update(Forum forum)
        {
            forums = serializer.FromCSV(FilePath);
            var current = forums.Find(c => c.Id == forum.Id);
            var index = forums.IndexOf(current);
            forums.Remove(current);
            forums.Insert(index, forum);       
            serializer.ToCSV(FilePath, forums);
            return forum;
        }
        public Forum GetById(int id)
        {
            return forums.Find(c => c.Id == id);
        }

        public Forum GetByLocationId(int locationId)
        {
            return forums.Find(c => c.LocationId == locationId);
        }
    }
}