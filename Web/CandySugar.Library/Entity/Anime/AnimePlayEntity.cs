using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Anime
{
    public class AnimePlayEntity:BaseEntity
    {
        public string PlayURL { get; set; }

        public string AnimeName { get; set; }

        public string CollectName { get; set; }
    }
}
