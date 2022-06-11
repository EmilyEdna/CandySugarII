using CandySugar.Logic.Entity.CandyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.IService
{
    public interface ICandyMusic
    {
        Task Add(CandyMusicList input);
        Task Remove(CandyMusicList input);
        Task Update(CandyMusicList input);
        Task<List<CandyMusicList>> Get();
    }
}
