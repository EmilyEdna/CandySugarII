using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic.Serivce
{
    public class CandyImageImpl : ICandyImage
    {
        public async Task<bool> Add(CandyImage input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyImage>().FirstAsync(t => t.Preview == input.Preview);
            if (entity == null)
                return await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync() > 0;
            return false;
        }

        public async Task<Tuple<List<CandyImage>, int>> Get(int input)
        {
            var db = DbContext.Candy.Context;
            var total = new RefAsync<int>();
          
            var data = await db.Queryable<CandyImage>().OrderBy(t => t.Span,OrderByType.Desc).ToPageListAsync(input, 12, total);
            return new Tuple<List<CandyImage>, int>(data, (int)Math.Ceiling(total / 12d));
        }

        public async Task Remove(CandyImage input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyImage>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
