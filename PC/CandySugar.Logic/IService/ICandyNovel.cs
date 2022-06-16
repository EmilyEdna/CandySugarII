namespace CandySugar.Logic.IService
{
    public interface ICandyNovel
    {
        Task AddOrUpdate(CandyNovel input);
        Task Remove(CandyNovel input);
        Task<List<CandyNovel>> Get();
    }
}
