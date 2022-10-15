using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.ViewModel.MicDto
{
    public class ComicViewDto
    {
        public Dictionary<string, string> Author { get; set; }

        public Dictionary<string, string> Group { get; set; }

        public Dictionary<string, string> Parodies { get; set; }

        public Dictionary<string, string> Language { get; set; }

        public Dictionary<string, string> Category { get; set; }

        public Dictionary<string, string> Tag { get; set; }

        public List<string> Previews { get; set; }

        public List<string> Realviews { get; set; }

        public string UpdateDate { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }
        public string Route { get; set; }
    }
}
