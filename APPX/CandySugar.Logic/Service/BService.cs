using CandySugar.Library;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Logic
{
    public class BService : IBService
    {
        public async Task<BRootEntity> Add(BRootEntity root)
        {
            root.InitProperty();
            root.Collection.ForEach(t =>
            {
                t.InitProperty();
                t.BRootId = root.Id;
                t.IsWatching = false;
            });
            var Lite = DbContext.Lite;
            await Lite.InsertAsync(root);
            await Lite.InsertAllAsync(root.Collection, false);
            return root;
        }
        public async Task Remove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<BRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<BElementEntity>().DeleteAsync(t => t.BRootId == root);
        }
        public async Task Alter(BElementEntity root)
        {
            var Lite = DbContext.Lite;

            var Elements = await Lite.Table<BElementEntity>().Where(t => t.BRootId == root.BRootId).ToListAsync();
            Elements.ForEach(t =>
            {
                t.IsWatching = false;
            });
            await Lite.UpdateAllAsync(Elements, false);
            await Lite.UpdateAsync(root);
        }
        public async Task<List<BRootEntity>> Query(string key)
        {
            var Lite = DbContext.Lite;

            var query = Lite.Table<BRootEntity>();
            if (!key.IsNullOrEmpty()) query = query.Where(t => t.Name.Contains(key));

            var roots = await query.ToListAsync();

            var elements = await Lite.Table<BElementEntity>().ToListAsync();

            roots.ForEach(item =>
            {
                item.Collection = elements.Where(t => t.BRootId == item.Id).ToList();
            });

            return roots;

        }
    }
}
