using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Anime
{
    public class AnimeDetailEntity:BaseEntity
    {
        public string CollectName { get; set; }

        public string WatchRoute { get; set; }

        public bool IsDownURL { get; set; }

        public string Cover { get; set; }

        public string Name { get; set; }

        public string DetailRoute { get; set; }
    }
}
