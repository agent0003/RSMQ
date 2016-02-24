using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSMQ
{
    public class MessageOptions
    {
        public string Message { get; set; }
        public string QueueName { get; set; }
        public int Delay { get; set; }
    }
}
