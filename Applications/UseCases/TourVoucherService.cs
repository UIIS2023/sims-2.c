using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Microsoft.Win32;

namespace Tourist_Project.Applications.UseCases
{
    public class TourVoucherService
    {
        private static readonly Injector injector = new();

        private readonly ITourVoucherRepository voucherRepository = injector.CreateInstance<ITourVoucherRepository>();

        private readonly TourReservationService reservationService = new();
        private readonly TourAttendanceService attendanceService = new();
        private readonly UserService userService = new();

        public TourVoucherService() 
        {
        }
        public TourVoucher Create(TourVoucher voucher)
        {
            return voucherRepository.Save(voucher);
        }

        public void Delete(int voucherId)
        {
            voucherRepository.Delete(voucherId);
        }

        public void VouchersDistribution(int id)
        {
            foreach(var reservation in reservationService.GetAllByTourId(id))
            {
                var tourVoucher = new TourVoucher(reservation.UserId, reservation.TourId, "Guide cancellation"); //GuideId dodati
                Create(tourVoucher);
            }
        }

        public void VoucherDistributionForAnyTour(Tour tour)
        {
            foreach (var reservation in reservationService.GetAllByTourId(tour.Id))
            {
                var tourVoucher = new TourVoucher(reservation.UserId, reservation.TourId, "Guide cancellation");
                Create(tourVoucher);
            }
        }

        public List<TourVoucher> GetAllForUser(int userId)
        {
            return voucherRepository.GetAllForUser(userId);
        }

        public void DeleteInvalidVouchers(int userId)
        {
            voucherRepository.DeleteInvalidVouchers(userId);
        }

        public void ClaimFiveToursInAYearVoucher(int userId)
        {
            var user = userService.GetOne(userId);

            if (user.VoucherAcquiredDate.AddYears(1) < DateTime.Now)
            {
                user.AcquiredYearlyVoucher = false;
                userService.Update(user);
            }

            if (user.AcquiredYearlyVoucher) return;
            
            var pastYearAttendancesCount = attendanceService.GetUsersPastYearAttendancesCount(userId);
            if (pastYearAttendancesCount >= 5)
            {
                var voucher = new TourVoucher(userId, "5 tours in 1 year", DateTime.Now.AddMonths(6));
                Create(voucher);

                user.VoucherAcquiredDate = DateTime.Now;
                user.AcquiredYearlyVoucher = true;
                userService.Update(user);
            }

        }

        public TourVoucher GetEarliestByNameForUser(string selectedVoucherName, int userId)
        {
            return voucherRepository.GetEarliestByNameForUser(selectedVoucherName, userId);
        }

        public ObservableCollection<string> LoadVouchers(int userId)
        {
            var retVal = new ObservableCollection<string> { "Without voucher" };
            foreach (var voucher in GetAllForUser(userId).Select(tv => tv.Name).Distinct())
            {
                retVal.Add(voucher);
            }
            return retVal;
        }

        private string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            throw new Exception("Save file dialog returned error!");
        }

        public bool GeneratePDFReport(int userId)
        {
            try
            {
                string filePath = OpenFilePicker();

                Document document = new();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                Paragraph heading = new(
                    "Intergalactic Travel Agency\n5711 Darth Vader Road\n10016 New York, USA\n",
                    new Font(Font.FontFamily.TIMES_ROMAN, 15))
                {
                    SpacingAfter = 15f
                };
                document.Add(heading);

                Paragraph date = new(
                    DateTime.Now + "\n\n",
                    new Font(Font.FontFamily.TIMES_ROMAN, 15))
                {
                    SpacingAfter = 15f,
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(date);

                Paragraph title = new(
                    "A report of all your currently valid vouchers",
                    new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD))
                {
                    SpacingAfter = 15f,
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(title);

                PdfPTable table = new(3);
                table.DefaultCell.Phrase = new Phrase { Font = new Font(Font.FontFamily.TIMES_ROMAN, 14) };
                table.AddCell("Name");
                table.AddCell("Acquired");
                table.AddCell("Valid until");

                foreach (var voucher in GetAllForUser(userId))
                {
                    table.AddCell(voucher.Name);
                    table.AddCell(voucher.WayAcquired);
                    table.AddCell(voucher.ExpireDate.ToShortDateString());
                }

                document.Add(table);

                Paragraph conclusion = new(
                    $"\n\nThe report was created by the request of\n{userService.GetOne(userId).FullName}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 15))
                {
                    SpacingAfter = 15f,
                    Alignment = Element.ALIGN_LEFT
                };
                document.Add(conclusion);

                document.Close();

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
