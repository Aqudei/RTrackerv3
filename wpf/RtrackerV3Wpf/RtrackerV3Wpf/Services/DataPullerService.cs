using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RtrackerV3Wpf.Services
{
    class DataPullerService
    {
        private readonly DocumentService _documentService;
        private readonly IEventAggregator _eventAggregator;

        public DataPullerService(DocumentService documentService, IEventAggregator eventAggregator)
        {
            _documentService = documentService;
            _eventAggregator = eventAggregator;

        }


        public void StartService()
        {
            Task.Run(async () => await PullData());
        }

        private async Task PullData()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(4);
                    var documents = await _documentService.FetchDocumentsAsync();
                    _eventAggregator.PublishOnBackgroundThread(documents);

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error in DataPullerService");
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
