﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.Transfers
{
    public class MenuObject: PropertyChangedBase
    {
        private Type _InstanceType;
        public Type InstanceType
        {
            get => _InstanceType;
            set => SetAndNotify(ref _InstanceType, value);
        }
        private bool _IsEnable;
        public bool IsEnable
        {
            get => _IsEnable;
            set => SetAndNotify(ref _IsEnable, value);
        }
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }
    }
}
