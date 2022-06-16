namespace CandySugar.Logic.Serivce
{
    public class CandyLovelImpl : ICandyLovel
    {
        public async Task AddOrUpdate(CandyLovel input)
        {
            var db = DbContext.Candy.Context;
            var Novel = await db.Queryable<CandyLovel>()
                .Where(t => t.BookName.Equals(input.BookName))
                .Where(t => t.Author.Equals(input.Author)).FirstAsync();
            if (Novel == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            else
                await db.Updateable<CandyLovel>().SetColumns(t => t.Route == input.Route)
                    .SetColumns(t => t.Chapter == input.Chapter)
                    .SetColumns(t => t.Span == DateTime.Now.Ticks)
                    .Where(t => t.BookName.Equals(input.BookName))
                    .Where(t => t.Author.Equals(input.Author)).ExecuteCommandAsync();
        }

        public async Task<List<CandyLovel>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyLovel>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove(CandyLovel input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyLovel>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
