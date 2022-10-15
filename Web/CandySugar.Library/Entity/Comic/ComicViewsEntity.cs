using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Comic
{
    public class ComicViewsEntity : BaseEntity
    {
        public string Route { get; set; }
        public bool IsReal { get; set; }
        public Guid ViewId { get; set; }
    }
}
