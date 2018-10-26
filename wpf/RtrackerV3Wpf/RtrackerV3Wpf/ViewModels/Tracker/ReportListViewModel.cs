using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using RtrackerV3Wpf.Events;
using RtrackerV3Wpf.Services;

namespace RtrackerV3Wpf.ViewModels.Tracker
{
    class ReportListViewModel : Screen, IHandleWithTask<DocumentsPulledEvent>
    {
        private readonly DocumentService _documentService;
        private readonly DataPullerService _dataPullerService;
        private readonly IEventAggregator _eventAggregator;

        public BindableCollection<ReportItemViewModel> Reports { get; set; } = new BindableCollection<ReportItemViewModel>();

        public ReportListViewModel(DocumentService documentService, DataPullerService dataPullerService, IEventAggregator eventAggregator)
        {
            _documentService = documentService;
            _dataPullerService = dataPullerService;
            _eventAggregator = eventAggregator;

            _eventAggregator.Subscribe(this);
        }

        protected override async void OnViewLoaded(object view)
        {
            var documents = await _documentService.FetchDocumentsAsync();
        }

        protected override void OnViewReady(object view)
        {

        }

        protected override void OnViewAttached(object view, object context)
        {
            _dataPullerService.StartService();
        }

        Task IHandleWithTask<DocumentsPulledEvent>.Handle(DocumentsPulledEvent message)
        {
            return Task.Run(async () =>
            {
                var documents = message.Documents;
                if (documents != null)
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input, new System.Action(() =>
                    {
                        Reports.Clear();
                    }));

                    foreach (var document in documents)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input, new System.Action(() =>
                        {
                            Reports.Add(new ReportItemViewModel
                            {
                                Id = document.Id,
                                CurrentOffice = document.CurrentOffice,
                                CurrentState = document.CurrentState,
                                ReportTitle = document.ReportTitle,
                            });
                        }));
                    }

                }
            });
        }
    }
}
