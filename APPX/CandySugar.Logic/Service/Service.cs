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
        #region Log
        public async Task AddLog(string Info,Exception Stack)
        {
            LogEntity input = new LogEntity
            {
                Info = Info,
                Stack = Stack.StackTrace
            };
            var Lite = DbContext.Lite;
            input.InitProperty();
            await Lite.InsertAsync(input);
        }
        public async Task ClearLog()
        {
            var Lite = DbContext.Lite;
            await Lite.Table<LogEntity>().DeleteAsync();
        }
        public async Task<List<LogEntity>> QueryLog()
        {
            var Lite = DbContext.Lite;
            return await Lite.Table<LogEntity>().ToListAsync();
        }
        #endregion

        #region B
        public async Task<BRootEntity> BAdd(BRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<BRootEntity>().FirstOrDefaultAsync(t => t.Name == root.Name);
            if (Parent != null)
            {
                Parent.Children = await Lite.Table<BElementEntity>().Where(t => t.BRootId == Parent.Id).ToListAsync();
                return Parent;
            }
            root.InitProperty();
            root.Children.ForEach(t =>
            {
                t.InitProperty();
                t.BRootId = root.Id;
                t.IsWatching = false;
            });

            await Lite.InsertAsync(root);
            await Lite.InsertAllAsync(root.Children, false);
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
                item.Children = elements.Where(t => t.BRootId == item.Id).ToList();
            });

            return roots;

        }
        #endregion

        #region C
        public async Task<bool> CAdd(CRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<CRootEntity>().FirstOrDefaultAsync(t => t.Priview == root.Priview);
            if (Parent != null) return true;
            root.InitProperty();
            root.Children.ForEach(t =>
            {
                t.InitProperty();
                t.CRootId = root.Id;
            });

            var res = await Lite.InsertAsync(root) > 0 && await Lite.InsertAllAsync(root.Children, false) > 0;
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
                item.Children = elements.Where(t => t.CRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion

        #region D
        public async Task<bool> DAdd(DRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<DRootEntity>().FirstOrDefaultAsync(t => t.Name == root.Name);
            if (Parent != null)
            {
                return true;
            }
            root.InitProperty();
            root.Children.ForEach(t =>
            {
                t.InitProperty();
                t.DRootId = root.Id;
            });
            return await Lite.InsertAsync(root) > 0 && await Lite.InsertAllAsync(root.Children, false) > 0;
        }
        public async Task DRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<DRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<DElementEntity>().DeleteAsync(t => t.DRootId == root);
        }
        public async Task<List<DRootEntity>> DQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<DRootEntity>().ToListAsync();
            var elements = await Lite.Table<DElementEntity>().ToListAsync();
            roots.ForEach(item =>
            {
                item.Children = elements.Where(t => t.DRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion

        #region E
        #endregion

        #region F
        public async Task<bool> FAdd(FRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<FRootEntity>().FirstOrDefaultAsync(t => t.Name == root.Name);
            if (Parent != null)
            {
                return true;
            }
            root.InitProperty();
            root.Children.ForEach(t =>
            {
                t.InitProperty();
                t.FRootId = root.Id;
            });
            return await Lite.InsertAsync(root) > 0 && await Lite.InsertAllAsync(root.Children, false) > 0;
        }
        public async Task FRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<FRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<FElementEntity>().DeleteAsync(t => t.FRootId == root);
        }
        public async Task<List<FRootEntity>> FQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<FRootEntity>().ToListAsync();
            var elements = await Lite.Table<FElementEntity>().ToListAsync();
            roots.ForEach(item =>
            {
                item.Children = elements.Where(t => t.FRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion
    }
}
