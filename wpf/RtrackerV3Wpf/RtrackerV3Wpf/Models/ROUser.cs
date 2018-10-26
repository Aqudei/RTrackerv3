using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtrackerV3Wpf.Models
{
    public class ROUserModel
    {

        public int Id { get; set; }
        public string Office { get; set; }
        public UserModel _User { get; set; }
        public FlowModel Flow { get; set; }
    }
}
