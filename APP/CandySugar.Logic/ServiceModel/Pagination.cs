using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.ServiceModel
{
    public class Pagination<T>
    {
        public double Total { get; set; }
        public List<T> Result { get; set; }
    }
}
