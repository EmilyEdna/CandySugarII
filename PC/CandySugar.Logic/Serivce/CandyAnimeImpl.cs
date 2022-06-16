namespace CandySugar.Logic.Serivce
{
    public class CandyAnimeImpl : ICandyAnime
    {
        public async Task AddElement(List<CandyAnimeElement> input)
        {
            var db = DbContext.Candy.Context;
            await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<CandyAnimeRoot> AddOrUpdateRoot(CandyAnimeRoot input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyAnimeRoot>().FirstAsync(t => t.AnimeName == input.AnimeName);
            if (entity == null)
                return await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteReturnEntityAsync();
            else
            {
                await db.Updateable<CandyAnimeRoot>()
                    .SetColumns(t => t.CollectName == input.CollectName)
                    .SetColumns(t => t.Route == input.Route)
                    .SetColumns(t => t.Span == DateTime.Now.Ticks)
                    .Where(t => t.AnimeName == input.AnimeName).ExecuteCommandAsync();
                return input;
            }
        }
        public async Task<List<CandyAnimeRoot>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyAnimeRoot>()
                .Includes(t => t.Elements)
                .OrderBy(t => t.Span, OrderByType.Desc)
                .ToListAsync();
        }

        public async Task Remove(CandyAnimeRoot input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyAnimeRoot>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
            await db.Deleteable<CandyAnimeElement>(t => t.RootId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
