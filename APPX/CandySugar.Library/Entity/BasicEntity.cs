using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Library
{
    public class BasicEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Span { get; set; }
        public void InitProperty()
        {
            this.Id = Guid.NewGuid();
            this.Span = DateTime.Now;
        }
    }
}
