﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library
{
    public class CommonHelper
    {
        /// <summary>
        /// 程序目录
        /// </summary>
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 默认方法
        /// </summary>
        public static string DefaultMethod => "SearchHandler";
        /// <summary>
        /// 配置目录
        /// </summary>
        public static string OptionPath => "Component";
        /// <summary>
        /// 组件错误消息
        /// </summary>
        public static string ComponentErrorInformation => "组件内部异常，请查看日志!";
        /// <summary>
        /// 网络异常消息
        /// </summary>
        public static string InternetErrorInformation => "网络异常请检查当前网络是否连接成功!";
        /// <summary>
        /// 配置文件
        /// </summary>
        public static List<string> OptionFile = new List<string> 
        {
            "Component.json",
            "SystemOption.json"
        };
    }
}
