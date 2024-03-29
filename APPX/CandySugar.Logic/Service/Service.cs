﻿namespace CandySugar.Logic
{
    public class Service : IService
    {
        #region Log
        public async Task AddLog(string Info, Exception Stack)
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
            await Lite.Table<LogEntity>().DeleteAsync(t => t.Id != Guid.Empty);
        }
        public async Task<List<LogEntity>> QueryLog()
        {
            var Lite = DbContext.Lite;
            return await Lite.Table<LogEntity>().OrderByDescending(t => t.Span).ToListAsync();
        }
        #endregion

        #region Opt
        public async Task OptAlter(OptEntity input)
        {
            var Lite = DbContext.Lite;
            var res = await Lite.Table<OptEntity>().FirstAsync();
            if (res != null)
                await Lite.UpdateAsync(input);
            else
            {
                input.InitProperty();
                await Lite.InsertAsync(input);
            }
        }
        public async Task<OptEntity> OptFirst()
        {
            var Lite = DbContext.Lite;
            return await Lite.Table<OptEntity>().FirstOrDefaultAsync();
        }
        #endregion

        #region B
        public async Task<bool> BAdd(BRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<BRootEntity>().FirstOrDefaultAsync(t => t.Name == root.Name);
            if (Parent != null)
            {
                //更新
                await Lite.Table<BElementEntity>().DeleteAsync(t => t.BRootId == Parent.Id);

                root.Children.ForEach(t =>
                {
                    t.InitProperty();
                    t.BRootId = Parent.Id;
                });

                var res = await Lite.InsertAllAsync(root.Children, false) > 0;
                return res;
            };

            root.InitProperty();
            root.Children.ForEach(t =>
            {
                t.InitProperty();
                t.BRootId = root.Id;
            });
            var f1 = await Lite.InsertAsync(root) > 0;
            var f2 = await Lite.InsertAllAsync(root.Children, false) > 0;
            return f1 && f2;
        }
        public async Task BRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<BRootEntity>().DeleteAsync(t => t.Id == root);
            await Lite.Table<BElementEntity>().DeleteAsync(t => t.BRootId == root);
        }
        public async Task<List<BRootEntity>> BQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<BRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
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
            var Parent = await Lite.Table<CRootEntity>().FirstOrDefaultAsync(t => t.Preview == root.Preview);
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
            var roots = await Lite.Table<CRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
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
            var roots = await Lite.Table<DRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
            var elements = await Lite.Table<DElementEntity>().ToListAsync();
            roots.ForEach(item =>
            {
                item.Children = elements.Where(t => t.DRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion

        #region E
        public async Task<bool> EAdd(ERootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<ERootEntity>().FirstOrDefaultAsync(t => t.Name == root.Name && t.Author == root.Author);
            if (Parent != null)
            {
                return true;
            }
            root.InitProperty();
            return await Lite.InsertAsync(root) > 0;
        }
        public async Task ERemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<ERootEntity>().DeleteAsync(t => t.Id == root);
        }
        public async Task<List<ERootEntity>> EQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<ERootEntity>().OrderByDescending(t => t.Span).ToListAsync();
            return roots;
        }
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
            var roots = await Lite.Table<FRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
            var elements = await Lite.Table<FElementEntity>().ToListAsync();
            roots.ForEach(item =>
            {
                item.Children = elements.Where(t => t.FRootId == item.Id).ToList();
            });
            return roots;
        }
        #endregion

        #region G
        public async Task<bool> GAdd(GRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<GRootEntity>().FirstOrDefaultAsync(t => t.Title == root.Title && t.Route == root.Route);
            if (Parent != null)
            {
                return true;
            }
            root.InitProperty();

            return await Lite.InsertAsync(root) > 0;
        }
        public async Task GRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            await Lite.Table<GRootEntity>().DeleteAsync(t => t.Id == root);
        }
        public async Task<List<GRootEntity>> GQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<GRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
            return roots;
        }
        #endregion

        #region H
        public async Task<bool> HAdd(HRootEntity root)
        {
            var Lite = DbContext.Lite;
            var Parent = await Lite.Table<HRootEntity>().FirstOrDefaultAsync(t => t.SongId == root.SongId);
            if (Parent != null)
            {
                return true;
            }
            root.InitProperty();
            return await Lite.InsertAsync(root) > 0;
        }
        public async Task<HRootEntity> HAlter(Guid root, string input)
        {
            var Lite = DbContext.Lite;
            var entity = await Lite.Table<HRootEntity>().FirstOrDefaultAsync(t => t.Id == root);
            entity.Route = input;
            await Lite.UpdateAsync(entity);
            return entity;
        }
        public async Task<bool> HRemove(Guid root)
        {
            var Lite = DbContext.Lite;
            var Entity = await Lite.Table<HRootEntity>().FirstOrDefaultAsync(t => t.Id == root);
            if (!Entity.Platfrom.Equals("本地"))
            {
                if (File.Exists(Entity.Route) == true)
                    File.Delete(Entity.Route);
            }
            return (await Lite.Table<HRootEntity>().DeleteAsync(t => t.Id == root)) > 0;
        }
        public async Task<List<HRootEntity>> HQuery()
        {
            var Lite = DbContext.Lite;
            var roots = await Lite.Table<HRootEntity>().OrderByDescending(t => t.Span).ToListAsync();
            return roots;
        }
        #endregion
    }
}
