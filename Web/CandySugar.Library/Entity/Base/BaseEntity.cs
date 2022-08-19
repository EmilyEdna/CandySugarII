using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public void Create()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
