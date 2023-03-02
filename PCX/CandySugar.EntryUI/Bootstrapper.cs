﻿using HandyControl.Controls;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using XExten.Advance.StaticFramework;
using XExten.Advance;
using Stylet;
using CandySugar.EntryUI.ViewModels;
using StyletIoC;
using CandySugar.Com.Library.DLLoader;
using CandySugar.Com.Library;

namespace CandySugar.EntryUI
{
    public class Bootstrapper : Bootstrapper<IndexViewModel>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStart()
        {
            AssemblyLoader Loader = new AssemblyLoader(CommonHelper.AppPath);
            Loader.Load("CandySugar.LightNovel.dll", "IndexView", "LightNovelModule");
            HttpEvent.HttpActionEvent = new Action<HttpClient, Exception>((client, ex) =>
            {
               
            });
            HttpEvent.RestActionEvent = new Action<RestClient, Exception>((client, ex) =>
            {
                
            });
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            AssemblyLoader.ConfTypes.TryDequeue(out Type ConfigType);
            builder.AddModule((StyletIoCModule)Activator.CreateInstance(ConfigType));
        }

        /// <summary>
        /// 初始化系统相关参数配置
        /// </summary>
        protected override void Configure()
        {
            


            base.Configure();
        }

        /// <summary>
        /// 初始化VM
        /// </summary>
        protected override void Launch()
        {
            base.Launch();
        }

        /// <summary>
        /// 加载首页VM
        /// </summary>
        /// <param name="rootViewModel"></param>
        protected override void DisplayRootView(object rootViewModel)
        {
            base.DisplayRootView(rootViewModel);
        }

        /// <summary>
        ///VM加载完毕
        /// </summary>
        protected override void OnLaunch()
        {
            base.OnLaunch();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
         
        }
    }
}
