﻿namespace CandySugar.Logic.Serivce
{
    public class CandyMusicImpl : ICandyMusic
    {
        public async Task Add(CandyMusic input)
        {
            var db = DbContext.Candy.Context;
            var check = await db.Queryable<CandyMusic>().Where(t => t.Platform == input.Platform).Where(t => t.SongId == input.SongId).FirstAsync();
            if (check == null)
            {
                input.IsComplete = false;
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            }
            await Task.CompletedTask;
        }

        public async Task Remove(CandyMusic input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyMusic>().Where(t => t.Platform == input.Platform).Where(t => t.SongId == input.SongId).ExecuteCommandAsync();
        }

        public async Task Update(CandyMusic input)
        {
            var db = DbContext.Candy.Context;
            await db.Updateable<CandyMusic>().SetColumns(t => t.IsComplete == true)
                .SetColumns(t => t.NetRoute == input.NetRoute)
                .SetColumns(t => t.LocalRoute == input.LocalRoute)
                .Where(t => t.SongId == input.SongId)
                .Where(t => t.Platform == input.Platform)
                .ExecuteCommandAsync();
        }

        public async Task<List<CandyMusic>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyMusic>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }
    }
}
