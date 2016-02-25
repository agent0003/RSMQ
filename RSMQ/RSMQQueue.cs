using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSMQ
{
    internal class RSMQQueue
    {
        public int Vt { get; set; }
        public int Delay { get; set; }
        public int Maxsize { get; set; }
        public long Ts { get; set; }
        public string Uid { get; set; }
    }
}
