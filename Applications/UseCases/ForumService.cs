using System.Collections.Generic;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class ForumService
    {
        private static readonly Injector injector = new();

        private readonly IForumRepository forumRepository = injector.CreateInstance<IForumRepository>();
        public ForumService() { }

        public Forum Create(Forum forum)
        {
            return forumRepository.Save(forum);
        }

        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }

        public Forum Get(int id)
        {
            return forumRepository.GetById(id);
        }

        public Forum Update(Forum forum)
        {
            return forumRepository.Update(forum);
        }

        public void Delete(int id)
        {
            forumRepository.Delete(id);
        }

        public Forum GetByLocationId(int locationId)
        {
            return forumRepository.GetByLocationId(locationId);
        }
    } 
}
