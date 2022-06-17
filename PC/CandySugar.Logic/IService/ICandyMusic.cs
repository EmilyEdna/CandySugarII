namespace CandySugar.Logic.IService
{
    public interface ICandyMusic
    {
        Task AddOrUpdate(CandyMusic input);
        Task Remove(CandyMusic input);
        Task<List<CandyMusic>> Get();
    }
}
