using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyMovieImpl : ICandyMovie
    {
        public async Task AddOrUpdate(CandyMovie input)
        {
            var db = DbContext.Candy.Context;
            var Movie = await db.Queryable<CandyMovie>()
                .Where(t => t.Title.Equals(input.Title)).FirstAsync();
            if (Movie == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<List<CandyMovie>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyMovie>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove(CandyMovie input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyMovie>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
