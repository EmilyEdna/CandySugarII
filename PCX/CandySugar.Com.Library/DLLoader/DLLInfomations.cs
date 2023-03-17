using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.DLLoader
{
    public class DLLInfomations
    {
        public Type InstanceType { get; set; }
        public Type InstanceConfigType { get; set; }
        public bool IsEnable { get; set; }
        public string Description { get; set; }
    }
}
