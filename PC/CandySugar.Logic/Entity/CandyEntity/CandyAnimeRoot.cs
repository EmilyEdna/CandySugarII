using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyAnimeRoot : BaseEntity
    {
        public string Cover { get; set; }
        public string AnimeName { get; set; }
        public string Route { get; set; }
        public string CollectName { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(CandyAnimeElement.RootId))]
        public List<CandyAnimeElement> Elements { get; set; }
    }
}
