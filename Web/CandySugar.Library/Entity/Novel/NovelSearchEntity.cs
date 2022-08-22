﻿using CandySugar.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Novel
{
    public class NovelSearchEntity:BaseEntity
    {
        public Guid KeyId { get; set; }
        public string RecommendType { get; set; }
        public string DetailRoute { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string UpdateDate { get; set; }
        public void SetKeyCreate(Guid input) 
        {
            base.Create();
            this.KeyId = input;
        }
    }
    public class NovelSeachKeyEntity : BaseEntity
    { 
        public string Key { get; set; }
    }
}
