namespace CandySugar.Logic.Serivce
{
    public class CandyMangaImpl : ICandyManga
    {
        public async Task AddOrUpdate(CandyManga input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyManga>()
                .Where(t => t.Name == input.Name)
                .Where(t => t.Key == input.Key).FirstAsync();
            if (entity == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            else
                await db.Updateable<CandyManga>().SetColumns(t => t.CollectName == input.CollectName)
                      .SetColumns(t => t.Route == input.Route)
                      .Where(t => t.CandyId == entity.CandyId).ExecuteCommandAsync();
        }

        public async Task<List<CandyManga>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyManga>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task Remove(CandyManga input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyManga>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
