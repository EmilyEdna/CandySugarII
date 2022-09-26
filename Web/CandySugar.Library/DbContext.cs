using CandySugar.Library.Entity.Base;
using Furion;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;

namespace CandySugar.Library
{
    public class DbContext
    {
        public static DbContext Instance => new DbContext();

        string Route = SyncStatic.CreateFile(Path.Combine(SyncStatic.CreateDir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase")), App.Configuration["Sqlite"]));
        public SqlSugarScope Scope()
        {
            return new SqlSugarScope(new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = "DataSource=" + Route
            });
        }
        public void InitTables()
        {
            var models = typeof(DbContext).Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseEntity)).ToArray();
            Scope().CodeFirst.InitTables(models);
        }
        protected virtual T Force<T>(Func<bool, T> input)
        {
            return input.Invoke(StaticDictionary.UserAttachEntity.Force);
        }
    }
}
