using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue
{
    public class StandardAttribute : IObject
    {
        public string title { get; set; }
        public string description { get; set; }
        public string intention { get; set; }
    }
}
