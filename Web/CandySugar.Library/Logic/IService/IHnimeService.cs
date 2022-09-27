using CandySugar.Library.Entity.Hnime;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.HniDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface IHnimeService
    {
        Task<List<HnimeInitEntity>> Init();
        Task<PageOutDto<List<HnimeGlobalEntity>>> Category(string input, int page);
        Task<PageOutDto<List<HnimeGlobalEntity>>> Search(SearchDto input);
        Task<List<HnimePlayEntity>> Play(string input, string name);
    }
}
