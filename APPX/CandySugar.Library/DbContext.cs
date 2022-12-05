using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class DbContext
    {
        public static SQLiteAsyncConnection Lite => new (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lite.db3"));
        public static async Task InitTabel()
        {
            var Table = typeof(DbContext).Assembly.GetTypes().Where(t => t.IsClass == true).Where(t => t.BaseType == typeof(BasicEntity)).ToArray();
            await Lite.CreateTablesAsync(CreateFlags.None, Table);
        }
    }
}
