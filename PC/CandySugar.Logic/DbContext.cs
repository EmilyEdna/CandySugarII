namespace CandySugar.Logic
{
    public class DbContext
    {
        public SqlSugarScope Context => new(new ConnectionConfig
        {
            DbType = DbType.Sqlite,
            InitKeyType = InitKeyType.Attribute,
            ConnectionString = $"DataSource={SyncStatic.CreateFile(Path.Combine(SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "Candy")), "Candy.db"))}",
            IsAutoCloseConnection = true,
        });

        public static DbContext Candy => new();

        public void InitCandy()
        {
            var Table = this.GetType().Assembly.GetTypes().Where(t => t.IsClass == true).Where(t => t.BaseType == typeof(BaseEntity)).ToArray();
            Candy.Context.CodeFirst.InitTables(Table);
        }
    }
}
