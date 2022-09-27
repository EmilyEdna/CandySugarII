using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Movie
{
    public class MovieDetailEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public string DetailRoute { get; set; }
    }
}
