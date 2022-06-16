namespace CandySugar.Logic.Serivce
{
    public class CandyNovelImpl : ICandyNovel
    {
        public async Task AddOrUpdate(CandyNovel input)
        {
            var db = DbContext.Candy.Context;
            var Novel = await db.Queryable<CandyNovel>()
                .Where(t => t.BookName.Equals(input.BookName))
                .Where(t => t.Author.Equals(input.Author)).FirstAsync();
            if (Novel == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            else
                await db.Updateable<CandyNovel>().SetColumns(t=>t.Route==input.Route)
                    .SetColumns(t=>t.Chapter==input.Chapter)
                    .SetColumns(t => t.Span == DateTime.Now.Ticks)
                    .Where(t => t.BookName.Equals(input.BookName))
                    .Where(t => t.Author.Equals(input.Author)).ExecuteCommandAsync();
        }

        public async Task<List<CandyNovel>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyNovel>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove(CandyNovel input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyNovel>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
