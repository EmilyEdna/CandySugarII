namespace CandySugar.Logic.IService
{
    public interface ICandyManga
    {
        Task AddOrUpdate(CandyManga input);
        Task Remove(CandyManga input);
        Task<List<CandyManga>> Get();
    }
}
