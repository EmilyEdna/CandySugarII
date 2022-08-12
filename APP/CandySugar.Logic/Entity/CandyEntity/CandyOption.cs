using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyOption : BaseEntity
    {
        public int Cache { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        public int QueryModule { get; set; }
        public int Wait { get; set; }
        public string LightAccount { get; set; }
        public string LightPwd { get; set; }
        public int Module { get; set; }
    }
}
