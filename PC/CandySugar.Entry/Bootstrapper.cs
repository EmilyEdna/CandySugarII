using CandySugar.Controls;
using CandySugar.Entry.CandyViewModels;
using CandySugar.Entry.ViewModels;
using CandySugar.Library;
using CandySugar.Logic;
using CandySugar.Logic.IService;
using HandyControl.Controls;
using Sdk.Core;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Diagnostics;
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
            SdkOption.EnableLog = true;
            //删除过往日志
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "Logs"));
            //日志
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/Candy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //校验版本
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var serverVersion = StaticResource.GetVersion();
            if (!currentVersion.Equals(serverVersion))
            {
                //升级
                var result = HandyControl.Controls.MessageBox.Info("检测到新版本，即将升级", "提示");
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "CandySugar.Advance.exe");
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();//启动
                        process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
                        process.Close();//释放与此组件关联的所有资源
                        Environment.Exit(0);
                        Application.Current.Shutdown();
                    }
                    catch (Exception)
                    {
                        Environment.Exit(0);
                        Application.Current.Shutdown();
                    }
                }
            }
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
            Container.Get<ICandyLog>().Remove();
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
            Container.Get<ICandyLog>().Add((e.Exception.InnerException ?? e.Exception).StackTrace);
            Growl.Error("服务异常，请查看日志");
            GC.Collect();
        }
    }
}
