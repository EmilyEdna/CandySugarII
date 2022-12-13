using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class LogEntity : BasicEntity
    {
        public string Info { get; set; }
        public string Stack { get; set; }
    }
}
