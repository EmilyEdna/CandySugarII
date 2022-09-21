using CandySugar.Library.Entity.Base;
using SqlSugar;

namespace CandySugar.Library.Entity
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }    
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(Id))]
        public UserAttachEntity Option { get; set; }
    }
    public class UserAttachEntity : BaseEntity
    {
        /// <summary>
        /// 请求类型 1:Multi 2:Rest 3:RPC
        /// </summary>
        public int RequestType { get; set; }
    }
}
