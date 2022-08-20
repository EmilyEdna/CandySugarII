using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.ViewModel
{
    public class PageOutDto<T>
    {
        public int Total { get; set; }
        public T Data { get; set; }
    }
}
