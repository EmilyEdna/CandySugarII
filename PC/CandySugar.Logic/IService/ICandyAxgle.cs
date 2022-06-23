namespace CandySugar.Logic.IService
{
    public interface ICandyAxgle
    {
        Task Add(CandyAxgle input);
        Task Remove(CandyAxgle input);
        Task<List<CandyAxgle>> Get();
    }
}
