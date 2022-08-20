using Sdk.Component.Novel.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.IService
{
    public interface INovelService
    {
        Task<List<NovelInitCategoryResult>> Init();
    }
}
