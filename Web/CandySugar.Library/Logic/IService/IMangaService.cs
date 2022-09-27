using CandySugar.Library.Entity.Manga;
using CandySugar.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface IMangaService
    {
        Task<List<MangaInitEntity>> Init();
        Task<PageOutDto<List<MangaGlobalEntity>>> Category(string input, int page);
        Task<PageOutDto<List<MangaGlobalEntity>>> Search(string input, int page);
        Task<List<MangaDetailEntity>> Detail(string input);
    }
}
