using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private const string FilePath = "../../../Data/comments.csv";
        private readonly Serializer<Comment> serializer = new();
        private List<Comment> comments;
        public CommentRepository()
        {
            comments = serializer.FromCSV(FilePath);
        }

        public List<Comment> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Comment Save(Comment comment)
        {
            comment.Id = NextId();
            comments = serializer.FromCSV(FilePath);
            comments.Add(comment);
            serializer.ToCSV(FilePath, comments);
            return comment;
        }

        public int NextId()
        {
            comments = serializer.FromCSV(FilePath);
            if (comments.Count < 1)
            {
                return 1;
            }
            return comments.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            comments = serializer.FromCSV(FilePath);
            var founded = comments.Find(c => c.Id == id);
            comments.Remove(founded);
            serializer.ToCSV(FilePath, comments);
        }

        public Comment Update(Comment comment)
        {
            comments = serializer.FromCSV(FilePath);
            var current = comments.Find(c => c.Id == comment.Id);
            var index = comments.IndexOf(current);
            comments.Remove(current);
            comments.Insert(index, comment);
            serializer.ToCSV(FilePath, comments);
            return comment;
        }

        public Comment GetById(int id)
        {
            return comments.Find(c => c.Id == id);
        }
    }

}