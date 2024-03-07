using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class RenovationRepository : IRenovationRepository
    {
        private const string FilePath = "../../../Data/renovations.csv";
        private readonly Serializer<Renovation> serializer = new();
        private List<Renovation> renovations;

        public RenovationRepository()
        {
            renovations = serializer.FromCSV(FilePath);
        }
        public List<Renovation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Renovation Save(Renovation renovation)
        {
            renovation.Id = NextId();
            renovations = serializer.FromCSV(FilePath);
            renovations.Add(renovation);
            serializer.ToCSV(FilePath, renovations);
            return renovation;
        }

        public int NextId()
        {
            renovations = serializer.FromCSV(FilePath);
            if (renovations.Count < 1)
            {
                return 1;
            }
            return renovations.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            renovations = serializer.FromCSV(FilePath);
            var founded = renovations.Find(c => c.Id == id);
            renovations.Remove(founded);
            serializer.ToCSV(FilePath, renovations);
        }

        public Renovation Update(Renovation renovation)
        {
            renovations = serializer.FromCSV(FilePath);
            var current = renovations.Find(c => c.Id == renovation.Id);
            var index = renovations.IndexOf(current);
            renovations.Remove(current);
            renovations.Insert(index, renovation);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, renovations);
            return renovation;
        }

        public Renovation GetById(int id)
        {
            return renovations.Find(c => c.Id == id);
        }
    }

}