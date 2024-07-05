using System;
using System.Collections.Generic;

namespace ClientManagement.Models

{
    public class ActionHistory
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}