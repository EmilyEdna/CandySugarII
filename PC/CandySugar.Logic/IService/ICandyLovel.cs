namespace CandySugar.Logic.IService
{
    public interface ICandyLovel
    {
        Task AddOrUpdate(CandyLovel input);
        Task Remove(CandyLovel input);
        Task<List<CandyLovel>> Get();
    }
}
