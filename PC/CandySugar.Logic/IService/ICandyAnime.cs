namespace CandySugar.Logic.IService
{
    public interface ICandyAnime
    {
        Task<CandyAnimeRoot> AddOrUpdateRoot(CandyAnimeRoot input);
        Task AddElement(List<CandyAnimeElement> input);
        Task Remove(CandyAnimeRoot input);
        Task<List<CandyAnimeRoot>> Get();
    }
}
