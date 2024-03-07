using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public Comment Save(Comment comment);
        public int NextId();
        public void Delete(int id);
        public Comment Update(Comment comment);
        public Comment GetById(int id);
    }

}