using CandySugar.Library.Entity.Anime;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.AniDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface IAnimeService
    {
        Task<InitDto> Init();
        Task<PageOutDto<List<AnimeGlobalEntity>>> Category(CateDto input);
        Task<PageOutDto<List<AnimeGlobalEntity>>> Search(string key, int page);
        Task<List<AnimeDetailEntity>> Detail(string input);
        Task<AnimePlayEntity> Watch(string input);
    }
}
