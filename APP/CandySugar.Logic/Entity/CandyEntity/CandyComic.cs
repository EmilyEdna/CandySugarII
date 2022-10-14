using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyComic : BaseEntity
    {
        public string Cover { get; set; }
        public string Name { get; set; }
        public List<string> Route { get; set; }
    }
}
