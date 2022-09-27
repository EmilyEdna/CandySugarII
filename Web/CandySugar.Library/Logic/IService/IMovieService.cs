using CandySugar.Library.Entity.Movie;
using CandySugar.Library.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface IMovieService
    {
        Task<List<MovieInitEntity>> Init();
        Task<PageOutDto<List<MovieGlobalEntity>>> Category(string input, int page);
        Task<PageOutDto<List<MovieGlobalEntity>>> Search(string input, int searchId, int page);
        Task<List<MovieDetailEntity>> Detail(string input);
        Task<MoviePlayEntity> Play(string input);
    }
}
