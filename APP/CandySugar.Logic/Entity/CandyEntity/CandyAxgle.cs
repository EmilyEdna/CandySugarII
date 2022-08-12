using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyAxgle : BaseEntity
    {
        public int VId { get; set; }
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public string Channel { get; set; }
        public string Duration { get; set; }
        public string AddTime { get; set; }
        public int Views { get; set; }
        public string Preview { get; set; }
        public string Play { get; set; }
    }
}
