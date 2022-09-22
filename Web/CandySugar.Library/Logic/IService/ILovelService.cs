using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Entity.Novel;
using CandySugar.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface ILovelService
    {
        Task<List<LovelInitEntity>> Init();
        Task<PageOutDto<List<LovelSearchEntity>>> Search(string input, int page);
        Task<PageOutDto<List<LovelCategoryEntity>>> Category(string input, int page);
        Task<LovelDetailEntity> Detail(string input);
        Task<List<LovelViewEntity>> View(string input);
        Task<LovelContentEntity> Content(string input);
        Task<byte[]> Download(string input);
    }
}
