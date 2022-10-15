using CandySugar.Library.ViewModel.MicDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface IComicService
    {
        Task<ComicViewDto> View(ComicDto input);
    }
}
