using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyComicImpl : ICandyComic
    {
        public async Task Add(CandyComic input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyComic>().FirstAsync(t => t.Route == input.Route);
            if (entity == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<List<CandyComic>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyComic>().ToListAsync();
        }

        public async Task Remove(CandyComic input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyComic>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
