using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XExten.Advance.StaticFramework;

namespace CandySugar.Com.Library.FileDown
{
    public static class DownUtil
    {
        /// <summary>
        /// 文件写入
        /// </summary>
        /// <param name="result"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="component"></param>
        /// <param name="invoke"></param>
        public static void FileCreate(this byte[] result, string fileName, string fileType, string component = "",Action<string> invoke=null)
        {
            var catalog = SyncStatic.CreateDir(Path.Combine(CommonHelper.DownloadPath, component));
            var files = SyncStatic.CreateFile(Path.Combine(catalog, $"{fileName}.{fileType}"));
            SyncStatic.WriteFile(result, files);
            Application.Current.Dispatcher.Invoke(() =>
            {
                invoke?.Invoke(catalog);
            });
        }
    }
}
