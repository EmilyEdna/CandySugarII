using CandySugar.Library.Entity;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface ISysService
    {
        Task<bool> UserRegist(UserRegistDto input);
        Task<bool> UserLogin(UserLoginDto input);
        Task<PageOutDto<List<UserEntity>>> GetUser(GetUserDto input);
        Task<bool> RemoveUser(List<Guid> input);
    }
}
