using System;
using Caliburn.Micro;

namespace RtrackerV3Wpf.ViewModels.Tracker
{
    class ReportItemViewModel : Screen
    {

        private string _reportTitle;
        private string _lastActionBy;
        private DateTime _lastActionDate;

        public ActionListViewModel ReportActions { get; set; }
            = new ActionListViewModel();

        public string ReportTitle
        {
            get => _reportTitle;
            internal set => Set(ref _reportTitle, value);
        }

        public string LastActionBy
        {
            get => _lastActionBy;
            internal set => Set(ref _lastActionBy, value);
        }

        public DateTime LastActionDate
        {
            get => _lastActionDate;
            internal set
            {
                Set(ref _lastActionDate, value);
                NotifyOfPropertyChange(nameof(LastActionDateFormatted));
            }
        }

        private int _id;
        private string _currentOffice;
        private string _currentState;

        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }


        public string LastActionDateFormatted =>
            $"{LastActionDate:yyyy-MMM-dd}-{LastActionDate:HH:mm tt}";

        public string CurrentOffice
        {
            get => _currentOffice;
            set { Set(ref _currentOffice, value); }
        }

        public string CurrentState
        {
            get => _currentState;
            set => Set(ref _currentState , value);
        }
    }
}
