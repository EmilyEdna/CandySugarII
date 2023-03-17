using CandySugar.Com.Library.ReadFile;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Com.Options.ComponentObject
{
    public class ComponentBinding
    {
        private static List<ComponentObjectModel> _ComponentObjectModels;
        /// <summary>
        /// 组件信息
        /// </summary>
        public static List<ComponentObjectModel> ComponentObjectModels
        {
            get
            {
                if (_ComponentObjectModels != null) return _ComponentObjectModels;
                else
                {
                    List<ComponentObjectModel> Model = new();
                    JsonReader.Configuration.Bind("ComponentInfos", Model);
                    _ComponentObjectModels = Model;
                    return _ComponentObjectModels;
                }
            }
        }
    }
}
