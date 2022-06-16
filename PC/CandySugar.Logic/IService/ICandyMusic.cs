namespace CandySugar.Logic.IService
{
    public interface ICandyMusic
    {
        Task Add(CandyMusic input);
        Task Remove(CandyMusic input);
        Task Update(CandyMusic input);
        Task<List<CandyMusic>> Get();
    }
}
