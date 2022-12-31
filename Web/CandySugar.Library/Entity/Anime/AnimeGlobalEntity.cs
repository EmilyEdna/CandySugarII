using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Library.Entity.Anime
{
    public class AnimeGlobalEntity : BaseEntity
    {
        public string Route { get; set; }
        public string Cover { get; set; }
        public string Name { get; set; }
        public bool IsSearch { get; set; }
        public override void Create(bool search)
        {
            this.IsSearch = search;
            base.Create(true);
        }
    }
}
