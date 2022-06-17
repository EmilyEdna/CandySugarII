namespace CandySugar.Logic.IService
{
    public interface ICandyLabel
    {
        Task AddOrUpdate(CandyLabel input);
        Task Remove(CandyLabel input);
        Task<List<CandyLabel>> Get();
        Task<string> GetKey(string input);
    }
}
