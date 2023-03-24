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

        private static OptionObjectModel _OptionObjectModels;
        /// <summary>
        /// 系统配置
        /// </summary>
        public static OptionObjectModel OptionObjectModels
        {
            get
            {
                if (_OptionObjectModels != null) return _OptionObjectModels;
                else
                {
                    OptionObjectModel Model = new();
                    JsonReader.Configuration.Bind("Option", Model);
                    _OptionObjectModels = Model;
                    return _OptionObjectModels;
                }
            }
        }
    }
}
