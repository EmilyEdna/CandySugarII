using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyAxgleImpl : ICandyAxgle
    {
        public async Task Add(CandyAxgle input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyAxgle>().FirstAsync(t => t.VId == input.VId);
            if (entity == null)
                await db.Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<List<CandyAxgle>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyAxgle>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove(CandyAxgle input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyAxgle>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
