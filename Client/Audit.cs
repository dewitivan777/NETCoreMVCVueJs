using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class AuditInfo
    {
        public string Message { get; set; }
        public string Reason { get; set; }
        public bool DontSendNotifications { get; set; }
    }
}
