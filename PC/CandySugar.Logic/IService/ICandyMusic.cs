﻿using CandySugar.Logic.Entity.CandyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
