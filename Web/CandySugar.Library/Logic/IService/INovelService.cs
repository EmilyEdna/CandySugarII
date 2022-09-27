using CandySugar.Library.Entity.Novel;
using CandySugar.Library.ViewModel;

namespace CandySugar.Library.Logic.IService
{
    public interface INovelService
    {
        Task<List<NovelInitEntity>> Init();
        Task<List<NovelSearchEntity>> Search(string input);
        Task<PageOutDto<List<NovelCategoryEntity>>> Category(string input, string type, int page);
        Task<NovelDetailEntity> Detail(string input, int page);
        Task<NovelContentEntity> Content(string input);
    }
}
