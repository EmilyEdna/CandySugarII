﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Entity.CandyEntity
{
    public class CandyNovel:BaseEntity
    {
        public string Status { get; set; }
        public string BookType { get; set; }
        public string Cover { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Route { get; set; }
        public string Chapter { get; set; }
    }
}
