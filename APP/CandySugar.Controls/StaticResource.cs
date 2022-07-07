using XExten.Advance.LinqFramework;

namespace CandySugar.Controls
{
    public class StaticResource
    {
        public static void RegistRoute()
        {
            typeof(StaticResource).Assembly.ExportedTypes.Where(t => t.BaseType == typeof(ContentPage)).ForEnumerEach(item =>
            {
                Routing.RegisterRoute(item.Name, item);
            });
        }
    }
}
