using Caliburn.Micro;

namespace RtrackerV3Wpf.ViewModels.Tracker
{
    class TrackerViewModel:Screen
    {
        public override string DisplayName { get; set; } = "Report Tracker";

        public ReportListViewModel ReportListViewModel => IoC.Get<ReportListViewModel>();
    }
}
