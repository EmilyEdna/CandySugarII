using CandySugar.Library.Logic.IService;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Logic.Service
{
    public class HnimeService:DbContext, IHnimeService,IScoped
    {
    }
}
