using System;
using System.IO;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;

namespace CandySugar.Com.Library.Lnk
{
    public class Shortcut
    {
        public static Shortcut Instance => new();
        /// <summary>
        /// 创建桌面图标
        /// </summary>
        public void CreateLnk(string name)
        {
            WshShell shell = new WshShell();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{name}.lnk");
            //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(path);
            //设置快捷方式的目标所在的位置(源程序完整路径)
            shortcut.TargetPath = Path.Combine(Environment.CurrentDirectory, $"{name}Sugar.exe");
            //应用程序的工作目录
            //当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。
            shortcut.WorkingDirectory = Environment.CurrentDirectory;

            //目标应用程序窗口类型(1.Normal window普通窗口,3.Maximized最大化窗口,7.Minimized最小化)
            shortcut.WindowStyle = 1;

            //快捷方式的描述
            shortcut.Description = "甜糖";

            //可以自定义快捷方式图标.(如果不设置,则将默认源文件图标.)
            //shortcut.IconLocation = System.Environment.SystemDirectory + "\\" + "shell32.dll, 165";

            //保存快捷方式
            shortcut.Save();

        }
    }

}