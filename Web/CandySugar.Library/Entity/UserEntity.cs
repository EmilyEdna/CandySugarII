using CandySugar.Library.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Entity
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }    
        public string Password { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
}
