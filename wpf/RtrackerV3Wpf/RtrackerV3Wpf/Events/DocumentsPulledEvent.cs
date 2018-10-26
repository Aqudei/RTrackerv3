using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtrackerV3Wpf.Models;

namespace RtrackerV3Wpf.Events
{
    class DocumentsPulledEvent
    {
        public IEnumerable<DocumentModel> Documents { get; }

        public DocumentsPulledEvent(IEnumerable<DocumentModel> documents)
        {
            Documents = documents;
        }
    }
}
