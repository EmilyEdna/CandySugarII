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
    public class Service : IService
    {
        #region B
        public async Task<BRootEntity> BAdd(BRootEntity root)
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
        public async Task BRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<BRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<BElementEntity>().DeleteAsync(t => t.BRootId == root);
        }
        public async Task BAlter(BElementEntity root)
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
        public async Task<List<BRootEntity>> BQuery(string key)
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
        #endregion

        #region C
        public async Task<bool> CAdd(CRootEntity root)
        {
            root.InitProperty();
            root.Tage.ForEach(t =>
            {
                t.InitProperty();
                t.CRootId = root.Id;
            });
            var Lite = DbContext.Lite;
            var res = await Lite.InsertAsync(root) > 0 && await Lite.InsertAllAsync(root.Tage, false) > 0;
            return res;
        }
        public async Task CRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<CRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<CElementEntity>().DeleteAsync(t => t.CRootId == root);
        }
        public async Task<List<CRootEntity>> CQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<CRootEntity>().ToListAsync();
            var elements = await Lite.Table<CElementEntity>().ToListAsync();
            roots.ForEach(item =>
            {
                item.Tage = elements.Where(t => t.CRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion
    }
}
