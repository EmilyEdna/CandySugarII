using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.ViewModel.HniDto
{
    public class SearchDto
    {
        public int Page { get; set; }
        public string Keyword { get; set; }
        public string HnimeType { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Tags { get; set; }
    }
}
