using Sdk.Component.Anime.sdk.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.ViewModel.AniDto
{
    public class CateDto
    {
        public string Area { get; set; }
        public string Year { get; set; }
        public string Letter { get; set; }
        public int Page { get; set; } = 1;
        public string Route { get; set; }
        public string Type { get; set; }
    }
}
