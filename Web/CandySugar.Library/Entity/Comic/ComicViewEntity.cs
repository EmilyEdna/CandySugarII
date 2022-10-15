using CandySugar.Library.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Comic
{
    public class ComicViewEntity : BaseEntity
    {
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Cover { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string ViewRoute { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Author { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Group { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Parodies { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Language { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Category { get; set; }
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Tag { get; set; }
        [SugarColumn(IsNullable = true)]
        public string UpdateDate { get; set; }
    }
}
