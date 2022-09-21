using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Base
{
    public class BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        public DateTime Span { get; set; }

        public void Create(bool AutoId = true)
        {
            if (AutoId)
                this.Id = Guid.NewGuid();
            this.Span = DateTime.Now;
        }
    }
}
