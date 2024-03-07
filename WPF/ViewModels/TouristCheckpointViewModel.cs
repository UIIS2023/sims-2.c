using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels
{
    public class TouristCheckpointViewModel : INotifyPropertyChanged
    {
        private User tourist;

        public User Tourist
        {
            get { return tourist; }
            set
            {
                tourist = value;
                OnPropertyChanged("User");
            }
        }

        private TourAttendance attendance;

        public TourAttendance Attendance
        {
            get { return attendance; }
            set
            {
                attendance = value;
                OnPropertyChanged("TourAttendace");
            }
        }

        private TourPoint checkpoint;

        public TourPoint Checkpoint
        {
            get { return checkpoint; }
            set
            {
                checkpoint = value;
                OnPropertyChanged("TourPoint");
            }
        }

        private readonly TourAttendanceService attendanceService;

        public TouristCheckpointViewModel(TourPoint selectedTourPoint)
        {
        }

    public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
