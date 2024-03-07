using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourVoucherRepository
    {
        public TourVoucher Save(TourVoucher tourVoucher);
        public int NextId();
        List<TourVoucher> GetAllForUser(int userId);
        void DeleteInvalidVouchers(int userId);
        void Delete(int voucherId);
        TourVoucher GetEarliestByNameForUser(string selectedVoucherName, int userId);
    }
}
