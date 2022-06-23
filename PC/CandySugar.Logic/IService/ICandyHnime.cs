namespace CandySugar.Logic.IService
{
    public interface ICandyHnime
    {
        Task Add(CandyHnime input);
        Task Remove(CandyHnime input);
        Task<List<CandyHnime>> Get();
    }
}
