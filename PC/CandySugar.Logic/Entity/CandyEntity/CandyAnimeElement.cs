using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyAnimeElement : BaseEntity
    {
        public Guid RootId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
    }
}
