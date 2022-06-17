using CandySugar.Resource;
using System.Linq;
using XExten.Advance.LinqFramework;

namespace CandySugar.Logic.Serivce
{
    public class CandyLabelImpl : ICandyLabel
    {
        public async Task AddOrUpdate(CandyLabel input)
        {
            var db = DbContext.Candy.Context;
            var entity = await db.Queryable<CandyLabel>().FirstAsync(t => t.ZhLabel == input.ZhLabel);
            if (entity == null)
                await db.Insertable(input).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
            else
            {
                entity.EnLabel = input.EnLabel;
                entity.Modify = DateTime.Now;
                entity.Span = DateTime.Now.Ticks;
                await db.Updateable(entity).ExecuteCommandAsync();
            }
        }

        public async Task<List<CandyLabel>> Get()
        {
            var db = DbContext.Candy.Context;
            return await db.Queryable<CandyLabel>().OrderBy(t => t.Span, OrderByType.Desc).ToListAsync();
        }

        public async Task<string> GetKey(string input)
        {
            var db = DbContext.Candy.Context;
            if (input.Contains(","))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM CandyLabel WHERE 1=1 ");

                input.Split(",").ForArrayEach<string>(item =>
                {
                    sql.Append($"OR ZhLabel LIKE '%{item}%' ");
                });
                var entity = await db.SqlQueryable<CandyLabel>(sql.ToString()).ToListAsync();

                if (entity != null && entity.Count > 0) return string.Join(" ", entity.Select(t=>t.EnLabel));
                else
                {
                    List<string> temp = new List<string>();
                    ChineseImageLabel.ImageLabel.ForDicEach((k, v) =>
                    {
                        input.Split(",").ForArrayEach<string>(item =>
                        {
                            if (v.Contains(item))
                                temp.Add(k);
                        });
                    });
                    return temp.Distinct().Any() ? string.Join(" ", temp.Distinct()) : SyncStatic.Translate(string.Join(" ", input.Split(",")), "zh", "en");
                }
            }
            else
            {
                var entity = await db.Queryable<CandyLabel>().FirstAsync(t => t.ZhLabel.Contains(input));
                if (entity != null) return string.Join(" ", entity);
                else
                {
                    List<string> temp = new List<string>();
                    ChineseImageLabel.ImageLabel.ForDicEach((k, v) =>
                    {
                        if (v.Contains(input))
                            temp.Add(k);
                    });
                    return temp.Distinct().Any() ? string.Join(" ", temp.Distinct()) : SyncStatic.Translate(input, "zh", "en");
                }
            }
        }

        public async Task Remove(CandyLabel input)
        {
            var db = DbContext.Candy.Context;
            await db.Deleteable<CandyLabel>(t => t.CandyId == input.CandyId).ExecuteCommandAsync();
        }
    }
}
