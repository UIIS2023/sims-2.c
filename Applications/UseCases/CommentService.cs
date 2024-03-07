using System.Collections.Generic;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class CommentService
    {
        private static readonly Injector injector = new();

        private readonly ICommentRepository commentRepository = injector.CreateInstance<ICommentRepository>();
        public CommentService() { }

        public Comment Create(Comment comment)
        {
            return commentRepository.Save(comment);
        }

        public List<Comment> GetAll()
        {
            return commentRepository.GetAll();
        }

        public Comment Get(int id)
        {
            return commentRepository.GetById(id);
        }

        public Comment Update(Comment comment)
        {
            return commentRepository.Update(comment);
        }

        public void Delete(int id)
        {
            commentRepository.Delete(id);
        }
    }

}