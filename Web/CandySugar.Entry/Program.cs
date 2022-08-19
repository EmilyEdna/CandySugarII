public class Program
{
    public static void Main(string[] args)
    {
        WebApplication.CreateBuilder(args).Inject().Build().Run();
    }
}