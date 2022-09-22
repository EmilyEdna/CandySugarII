using CandySugar.Library.Entity.Base;
using Furion.LinqBuilder;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Lovel
{
    public class LovelContentEntity : BaseEntity
    {
        public string ChapterRoute { get; set; }
        public string Content { get; set; }

        public string Picture { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<string> Pictures => !Picture.IsNullOrEmpty() ? Picture.Split(",").ToList() : null;
    }
}
