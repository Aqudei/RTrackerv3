using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using RtrackerV3Wpf.Services;
using RtrackerV3Wpf.ViewModels.Tracker;

namespace RtrackerV3Wpf.ViewModels
{
    sealed class MainViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly IWindowManager _windowManager;
        private readonly DocumentService _documentService;

        public MainViewModel(IWindowManager windowManager, DocumentService documentService)
        {
            _windowManager = windowManager;
            _documentService = documentService;
        }

        protected override async void OnViewReady(object view)
        {
            var unAuthenticated = true;
            do
            {
                try
                {
                    var rouser=  await _documentService.EnsureAuthenticated();
                    unAuthenticated = false;
                }
                catch (Exception e)
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
                    {
                        _windowManager.ShowDialog(IoC.Get<SettingsViewModel>());
                    }));

                    Debug.WriteLine(e);
                }

            } while (unAuthenticated);
            ActivateItem(IoC.Get<TrackerViewModel>());
        }
    }
}
