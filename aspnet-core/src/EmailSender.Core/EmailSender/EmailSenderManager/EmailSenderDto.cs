using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailSenderManager
{
    public class EmailSenderDto
    {
        public string username { get; set; }
        public string useremail { get; set; }
        public string OrderStatus { get; set; }

        public string verificationurl { get; set; }

        public string userId { get; set; }

        public string OrderId { get; set; }

        public string TotolPrice { get; set; }
    }
}
