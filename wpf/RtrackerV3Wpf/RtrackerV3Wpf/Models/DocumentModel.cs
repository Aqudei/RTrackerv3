using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtrackerV3Wpf.Models
{
    class DocumentModel
    {
        public int Id { get; set; }
        public string ReportTitle { get; set; }
        public string CurrentOffice { get; set; }
        public string CurrentState { get; set; }
        public string LastOffice { get; set; }
        public bool NeedsReview { get; set; }
        public bool WasSent { get; set; }
        public int Owner { get; set; }
    }
}
