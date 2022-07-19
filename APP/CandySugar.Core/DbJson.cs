using System.Text;
using XExten.Advance.InternalFramework.Securities.Common;

namespace CandySugar.Logic
{
    public class DbJson
    {
        string JRoute = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public DbJson()
        {
            CreateTable();
        }
        private void CreateTable()
        {
            this.GetType().Assembly.GetTypes().Where(t => t.IsClass == true)
               .Where(t => t.BaseType == typeof(BaseEntity))
               .Select(t => t.Name).ForEnumerEach(item =>
               {
                   string JPath = Path.Combine(JRoute, $"{item}.mod");
                   SyncStatic.CreateFile(JPath);
               });
        }
        private string Route<T>()
        {
            return Path.Combine(JRoute, $"{nameof(T)}.mod");
        }
        private List<T> Add<T>(List<T> input) where T : BaseEntity
        {
            input.ForEach(item =>
            {
                item.Create();
            });
            var Data = Encoding.UTF8.GetBytes(SyncStatic.Compress(input.ToJson(), SecurityType.Base64));
            SyncStatic.WriteFile(Data, Route<T>());
            return input;
        }
        public List<T> Read<T>()
        {
            var JPath = Route<T>();
            var result = SyncStatic.ReadFile(JPath);
            if (result.IsNullOrEmpty()) return new List<T>();
            return SyncStatic.Decompress(result, SecurityType.Base64).ToModel<List<T>>();
        }
        public List<T> InsertSingle<T>(T input) where T : BaseEntity
        {
            var Result = Read<T>();
            Result.Add(input);
            return Add(Result);
        }
        public List<T> InsertBatch<T>(List<T> input) where T : BaseEntity
        {
            var Result = Read<T>();
            input.ForEach(item =>
            {
                item.Create();
                Result.Add(item);
            });
            return Add(Result);
        }
        public List<T> Delete<T>(T input) where T : BaseEntity
        {
            var Result = Read<T>();
            Result.RemoveAll(t => t.CandyId == input.CandyId);
            return Add(Result);
        }
    }
}
