using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class TourVoucherRepository : ITourVoucherRepository
    {
        private const string filePath = "../../../Data/tourVouchers.csv";
        private readonly Serializer<TourVoucher> serializer = new();
        private List<TourVoucher> vouchers;
        public TourVoucherRepository() 
        {
            vouchers = serializer.FromCSV(filePath);
        }

        public int NextId()
        {
            vouchers = serializer.FromCSV(filePath);
            if (vouchers.Count < 1)
            {
                return 1;
            }
            return vouchers.Max(v => v.Id) + 1;
        }

        public List<TourVoucher> GetAllForUser(int userId)
        {
            vouchers = serializer.FromCSV(filePath);
            var filteredList = new List<TourVoucher>();

            foreach (var voucher in vouchers)
            {
                if (voucher.TouristId == userId)
                {
                    filteredList.Add(voucher);
                }
            }
            return filteredList;
        }

        public void DeleteInvalidVouchers(int userId)
        {
            vouchers = serializer.FromCSV(filePath);
            foreach (var voucher in vouchers)
            {
                if (voucher.TouristId == userId && voucher.ExpireDate.Date < DateTime.Today.Date)
                {
                    Delete(voucher.Id);
                }
            }
        }

        public TourVoucher Save(TourVoucher voucher)
        {
            voucher.Id = NextId();
            vouchers = serializer.FromCSV(filePath);
            vouchers.Add(voucher);
            serializer.ToCSV(filePath, vouchers);
            return voucher;
        }

        public void Delete(int voucherId)
        {
            vouchers = serializer.FromCSV(filePath);
            var voucherToDelete = vouchers.Find(v => v.Id == voucherId);
            vouchers.Remove(voucherToDelete);
            serializer.ToCSV(filePath, vouchers);
        }

        public TourVoucher GetEarliestByNameForUser(string selectedVoucherName, int userId)
        {
            var usersVouchers = GetAllForUser(userId);
            var earliestToExpire = usersVouchers.Where(tv => tv.Name == selectedVoucherName).Min(tv => tv.ExpireDate);

            return usersVouchers.Find(tv => tv.Name == selectedVoucherName && tv.ExpireDate == earliestToExpire);
        }
    }
}
