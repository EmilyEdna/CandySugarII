namespace CandySugar.Logic.Serivce
{
    public class CandyHnimeImpl : ICandyHnime
    {
        public async Task Add(CandyHnime input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyHnime>().FirstAsync(t => t.Route == input.Route && t.Name == input.Name);
            if (entity == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public async Task<List<CandyHnime>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyHnime>().ToListAsync();
        }

        public async Task Remove(CandyHnime input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyHnime>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
