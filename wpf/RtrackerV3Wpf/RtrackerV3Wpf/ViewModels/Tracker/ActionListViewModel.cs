using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RtrackerV3Wpf.ViewModels.Tracker
{
    class ActionListViewModel : Screen
    {
        public BindableCollection<string> ReportActions { get; set; } = new BindableCollection<string>();

        public ActionListViewModel()
        {
            ReportActions.AddRange(new []{"Test Action 1", "Test Action 2"});
        }
    }
}
