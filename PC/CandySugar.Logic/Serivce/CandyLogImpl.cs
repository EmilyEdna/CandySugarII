using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyLogImpl : ICandyLog
    {
        public async Task Add(string input)
        {
            var db = DbContext.Candy.Context;
            await db.Insertable(new CandyLog { Stack = input }).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<List<CandyLog>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyLog>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove()
        {
            var db = DbContext.Candy.Context;
            var day = DateTime.Now.AddDays(-2).Ticks;
           await db.Deleteable<CandyLog>().Where(t => t.Span <= day).ExecuteCommandAsync();
        }
    }
}
