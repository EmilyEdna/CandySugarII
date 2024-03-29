﻿using Newtonsoft.Json;
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
        public string Name { get; set; }
        public string Route { get; set; }
        public string CollectName { get; set; }
        public List<CandyAnimeElement> Elements { get; set; }
    }

    public class CandyAnimeElement
    {
        public string AnimeName { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
    }
}
