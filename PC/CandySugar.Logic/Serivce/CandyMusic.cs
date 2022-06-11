using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyMusic : ICandyMusic
    {
        public async Task Add(CandyMusicList input)
        {
            var db = DbContext.Candy.Context;
            var check = await db.Queryable<CandyMusicList>().Where(t => t.Platform == input.Platform).Where(t => t.SongId == input.SongId).FirstAsync();
            if (check == null)
            {
                input.IsComplete = false;
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            }
            await Task.CompletedTask;
        }

        public async Task Remove(CandyMusicList input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyMusicList>().Where(t => t.Platform == input.Platform).Where(t => t.SongId == input.SongId).ExecuteCommandAsync();
        }

        public async Task Update(CandyMusicList input)
        {
            var db = DbContext.Candy.Context;
            await db.Updateable<CandyMusicList>().SetColumns(t => t.IsComplete == true)
                .SetColumns(t => t.NetRoute == input.NetRoute)
                .SetColumns(t => t.LocalRoute == input.LocalRoute)
                .Where(t => t.SongId == input.SongId)
                .Where(t => t.Platform == input.Platform)
                .ExecuteCommandAsync();
        }

        public async Task<List<CandyMusicList>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyMusicList>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }
    }
}
