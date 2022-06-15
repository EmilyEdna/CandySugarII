using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyMusic : BaseEntity
    {
        public int Platform { get; set; }
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string SongArtist { get; set; }
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
        public bool IsComplete { get; set; }
        public string LocalRoute { get; set; }
        public string NetRoute { get; set; }
    }
}
