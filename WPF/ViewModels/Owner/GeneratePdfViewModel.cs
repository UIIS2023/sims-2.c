using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class GeneratePdfViewModel
    {
        public AccommodationViewModel AccommodationViewModel { get; set; }
        public DateSpan DateSpan { get; set; }
        private readonly ReservationService reservationService = new();
        private readonly UserService userService = new();
        public ICommand GenerateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public List<Reservation> Reservations { get; set; }

        private readonly IBindableBase bindableBase;
        public GeneratePdfViewModel(AccommodationViewModel accommodationViewModel, IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            Reservations = reservationService.GetAllByAccommodation(accommodationViewModel.Accommodation.Id).OrderBy(o => o.CheckIn).ToList();
            DateSpan = Reservations.Count != 0 ? new DateSpan(Reservations.First().CheckIn, Reservations.Last().CheckOut) : new DateSpan(DateTime.Now, DateTime.Now);
            AccommodationViewModel = accommodationViewModel;
            GenerateCommand = new RelayCommand(Generate, CanGenerate);
            CloseCommand = new RelayCommand(Close);
        }

        public void Generate()
        {
            PDFReport();
        }

        public bool CanGenerate()
        {
            return true;
        }

        public void Close()
        {
            bindableBase.CloseWindow();
        }

        private void PDFReport()
        {
            try
            {
                var filePath = OpenFilePicker();
                Document document = new();
                var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                var information = new Paragraph($"Tourist agency",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(information);

                var companyName = new Paragraph($"Student agency",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(companyName);

                var address = new Paragraph($"Heroja Pinkija 16",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(address);

                var location = new Paragraph($"21000 Novi Sad, Serbia",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                {
                    SpacingAfter = 25f
                };
                document.Add(location);

                var creationTime = new Paragraph($"{DateTime.Now:D}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                {
                    SpacingBefore = 0f,
                    SpacingAfter = 25f,
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(creationTime);

                var heading = new Paragraph(
                        $"Reservation report for:   {AccommodationViewModel.Accommodation.Name}, {AccommodationViewModel.Location}\n" +
                        $"for date span from {DateSpan.StartingDate:dd.MM.yyyy}  to  {DateSpan.EndingDate:dd.MM.yyyy}", 
                        new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                    {
                        SpacingAfter = 10f,
                        Alignment = Element.ALIGN_CENTER
                    };
                document.Add(heading);

                var table = new PdfPTable(5);
                table.AddCell("Guest name");
                table.AddCell("Checked in");
                table.AddCell("Checked out");
                table.AddCell("No of guests");
                table.AddCell("Staying days");

                foreach (var reservation in reservationService.GetAllOrderedInDateSpan(DateSpan, AccommodationViewModel.Accommodation.Id))
                {
                    table.AddCell(userService.GetOne(reservation.GuestId).FullName);
                    table.AddCell(reservation.CheckIn.ToString("dd.MM.yyyy"));
                    table.AddCell(reservation.CheckOut.ToString("dd.MM.yyyy"));
                    table.AddCell(reservation.GuestsNum.ToString());
                    table.AddCell(reservation.StayingDays.ToString());
                }

                document.Add(table);

                document.Close();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private static string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                DefaultExt = "pdf"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            throw new Exception("Save file dialog returned error!");
        }
    }

}