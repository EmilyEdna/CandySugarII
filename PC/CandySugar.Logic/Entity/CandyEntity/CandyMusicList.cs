﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyMusicList : BaseEntity
    {
        public int Platform { get; set; }
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string SongArtist { get; set; }
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
    }
}
