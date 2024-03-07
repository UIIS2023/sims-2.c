using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Document = iTextSharp.text.Document;
using Image = Tourist_Project.Domain.Models.Image;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class StatisticsOfTourViewModel : INotifyPropertyChanged
    {
        private readonly Window window;
        private App app => (App)System.Windows.Application.Current;

        #region Services
        private readonly LocationService locationService = new();
        private readonly TourReservationService tourReservationService = new();
        private readonly ImageService imageService = new();
        #endregion

        #region Statistics
        public int TouristsNumberYounger { get; set; } = 0;
        public int TouristsNumberBetween { get; set; } = 0;
        public int TouristsNumberOlder { get; set; } = 0;
        public double WithVoucher { get; set; }
        public double WithoutVoucher { get; set; }
        #endregion

        #region SeriesCollection

        public SeriesCollection AgeSeriesCollection { get; set; }
        public SeriesCollection VoucherSeriesCollection { get; set; }

        #endregion

        public Tour Tour { get; set; }
        public Location Location { get; set; }
        public User LoggedInUser { get; set; }
        public Image Image { get; set; }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command
        public ICommand BackCommand { get; set; }
        public ICommand PDFReportCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public StatisticsOfTourViewModel(Window window, Tour tour, User user)
        {
            this.window = window;
            Tour = tour;
            LoggedInUser = user;
            Location = locationService.Get(Tour.LocationId);
            Image = imageService.Get(Tour.ImageId);

            BackCommand = new RelayCommand(Back);
            PDFReportCommand = new RelayCommand(PDFReport);
            ToSerbianCommand = new RelayCommand(ToSerbian, CanToSerbian);
            ToEnglishCommand = new RelayCommand(ToEnglish, CanToEnglish);
            ToDarkThemeCommand = new RelayCommand(ToDarkTheme, CanToDarkTheme);
            ToLightThemeCommand = new RelayCommand(ToLightTheme, CanToLightTheme);

            TourAgeStatistics();
            TourVoucherStatistics();
            GenerateSeriesCollection();
        }

        private void ToDarkTheme()
        {
            var app = (App)Application.Current;
            app.CurrentTheme = "Dark";
            app.SwitchTheme(app.CurrentTheme);
        }

        private bool CanToDarkTheme()
        {
            return app.CurrentTheme == "Light";
        }

        private void ToLightTheme()
        {
            var app = (App)Application.Current;
            app.CurrentTheme = "Light";
            app.SwitchTheme(app.CurrentTheme);
        }

        private bool CanToLightTheme()
        {
            return app.CurrentTheme == "Dark";
        }

        private void ToSerbian()
        {
            var app = (App)Application.Current;
            app.CurrentLanguage = "sr-LATN";
            app.ChangeLanguage(app.CurrentLanguage);
        }

        private bool CanToSerbian()
        {
            return app.CurrentLanguage.Equals("en-US");
        }

        private void ToEnglish()
        {
            var app = (App)Application.Current;
            app.CurrentLanguage = "en-US";
            app.ChangeLanguage(app.CurrentLanguage);
        }

        private bool CanToEnglish()
        {
            return app.CurrentLanguage.Equals("sr-LATN");
        }

        private void PDFReport()
        {
            try
            {
                string filePath = OpenFilePicker();
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.SetFullCompression();

                document.Open();

                var information = new Paragraph($"Turistička agencija",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(information);

                var companyName = new Paragraph($"Studentska agencija",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(companyName);

                var address = new Paragraph($"Heroja Pinkija 16",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(address);

                var location = new Paragraph($"21000 Novi Sad, Srbija",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                {
                    SpacingAfter = 25f
                };
                document.Add(location);

                var creationTime = new Paragraph($"{DateTime.Now}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                {
                    SpacingAfter = 25f
                };
                creationTime.Alignment = Element.ALIGN_RIGHT;
                document.Add(creationTime);

                var heading = new Paragraph($"Statistika za turu",
                    new Font(Font.FontFamily.TIMES_ROMAN, 26, Font.BOLD))
                {
                    SpacingAfter = 1f
                };
                heading.Alignment = Element.ALIGN_CENTER;
                document.Add(heading);

                var tourNameParagraph = new Paragraph($"{Tour.Name}", new Font(Font.FontFamily.TIMES_ROMAN, 26, Font.BOLD))
                {
                    SpacingAfter = 45f
                };
                tourNameParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(tourNameParagraph);

                var ageParagraph = new Paragraph(
                    $"  Tura je održana dana {Tour.StartTime.Date}, na kojoj je starosna struktura gostiju bila sledeća: ",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(ageParagraph);

                var youngerParagraph = new Paragraph($"Broj gostiju mlađih od 18 godina: {TouristsNumberYounger}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(youngerParagraph);

                var betweenParagraph =
                    new Paragraph($"Broj gostiju koji imaju između 18 i 50 godina: {TouristsNumberBetween}",
                        new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(betweenParagraph);

                var olderParagraph = new Paragraph($"Broj gostiju starijih od 50 godina: {TouristsNumberOlder}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL))
                {
                    SpacingAfter = 15f
                };
                document.Add(olderParagraph);

                var headingVoucher = new Paragraph($"Procentualna statistika upotrebe vaucera",
                    new Font(Font.FontFamily.TIMES_ROMAN, 20, Font.NORMAL))
                {
                    SpacingAfter = 10f
                };
                document.Add(headingVoucher);

                var voucherParagraph = new Paragraph(
                    $"Procenat gostiju koji su iskoristili vaučer: {WithVoucher}%",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(voucherParagraph);

                var voucherSecondPartParagraph = new Paragraph($"Procenat gostiju bez vaučera: {WithoutVoucher}%",
                        new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL)) 
                {
                    SpacingAfter = 45f
                };
                document.Add(voucherSecondPartParagraph);

                var guideParagraph =
                    new Paragraph($"Izveštaj kreiran na osnovu zahteva vodiča",
                        new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(guideParagraph);

                var guideNameParagraph = new Paragraph($"{LoggedInUser.FullName}",
                    new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.NORMAL));
                document.Add(guideNameParagraph);

                document.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private string OpenFilePicker()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            throw new Exception("Save file dialog returned error!");
        }

        private void GenerateSeriesCollection()
        {
            AgeSeriesCollection = new SeriesCollection();

            var youngerSeries = new PieSeries
            {
                Title = "Younger",
                Values = new ChartValues<int> { TouristsNumberYounger }
            };

            var betweenSeries = new PieSeries
            {
                Title = "Between",
                Values = new ChartValues<int> { TouristsNumberBetween }
            };

            var olderSeries = new PieSeries
            {
                Title = "Older",
                Values = new ChartValues<int> { TouristsNumberOlder }
            };

            AgeSeriesCollection.Add(youngerSeries);
            AgeSeriesCollection.Add(betweenSeries);
            AgeSeriesCollection.Add(olderSeries);


            VoucherSeriesCollection = new SeriesCollection();

            var withVoucherSeries = new PieSeries
            {
                Title = "With voucher",
                Values = new ChartValues<double> { WithVoucher }
            };

            var withoutVoucherSeries = new PieSeries
            {
                Title = "Without voucher",
                Values = new ChartValues<double> { WithoutVoucher }
            };

            VoucherSeriesCollection.Add(withVoucherSeries);
            VoucherSeriesCollection.Add(withoutVoucherSeries);
        }

        private void Back()
        {
            window.Close();
        }

        private void TourAgeStatistics()
        {
            TouristsNumberYounger = tourReservationService.CountingTourists(Tour.Id)[0];
            TouristsNumberBetween = tourReservationService.CountingTourists(Tour.Id)[1];
            TouristsNumberOlder = tourReservationService.CountingTourists(Tour.Id)[2];
        }

        private void TourVoucherStatistics()
        {
            WithVoucher = Math.Round(tourReservationService.WithVoucherPercent(Tour.Id), 2);
            WithoutVoucher = Math.Round(tourReservationService.WithOutVoucherPercent(Tour.Id), 2);
        }
    }
}
