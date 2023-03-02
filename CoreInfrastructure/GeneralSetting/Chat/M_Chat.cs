using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.GeneralSetting.Chat
{
    public class M_Chat
    {
        public string Company_Code { get; set; }
        public string Code { get; set; }
        public string SenderID { get; set; }
        public string RecieverID { get; set; }
        public string MessageText { get; set; }
        public string SendingTime { get; set; }
        public string Reply { get; set; }
        public string SeenStatus { get; set; }
        public string SeenTime { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string Locked { get; set; }
    }
}
