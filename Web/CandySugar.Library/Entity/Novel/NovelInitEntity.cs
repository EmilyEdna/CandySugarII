using CandySugar.Library.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Novel
{
    public class NovelInitEntity : BaseEntity
    {
        public string CategoryName { get; set; }

        public string CollectRoute { get; set; }
    }
}
