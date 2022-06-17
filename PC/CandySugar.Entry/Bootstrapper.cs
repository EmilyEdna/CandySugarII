﻿using CandySugar.Controls;
using CandySugar.Entry.ViewModels;
using CandySugar.Logic;
using Sdk.Core;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using XExten.Advance.StaticFramework;

namespace CandySugar.Entry
{
    public class Bootstrapper : Bootstrapper<LoginViewModel>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStart()
        {
            //启动删除WebView2的临时文件
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "CandySugar.Entry.exe.WebView2"));
            //启用日志
            SkdOption.EnableLog = true;
            //删除过往日志
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "Logs"));
            //日志
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/Candy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.AddModule(new LogicModule());
            builder.AddModule(new ControlModule());
        }

        /// <summary>
        /// 初始化系统相关参数配置
        /// </summary>
        protected override void Configure()
        {
            DbContext.Candy.InitCandy();
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
            Log.Logger.Error(e.Exception.InnerException ?? e.Exception, "");
            GC.Collect();
            Application.Current.Shutdown();
        }
    }
}
