using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.DLLoader
{
    public class AssemblyLoader
    {
        private string _basePath;
        private AssemblyLoadContext context;
        public static ConcurrentQueue<Type> Types = new ConcurrentQueue<Type>();
        public AssemblyLoader(string basePath)
        {
            _basePath = basePath;
        }

        public Type Load(string dllFileName, string typeName)
        {
            context = new AssemblyLoadContext(dllFileName);
            context.Resolving += Context_Resolving;
            //需要绝对路径
            string path = Path.Combine(_basePath, dllFileName);
            if (File.Exists(path))
            {
                try
                {
                    using (var stream = File.OpenRead(path))
                    {
                        Assembly assembly = context.LoadFromStream(stream);
                        Type type = assembly.GetTypes().FirstOrDefault(t => t.Name.ToLower().Equals(typeName.ToLower()));
                        Types.Enqueue(type);
                        return type;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载节点{dllFileName}-{typeName}发生异常：{ex.Message},{ex.StackTrace}");
                }

            }
            else
            {
                Console.WriteLine($"节点动态库{dllFileName}不存在：{path}");
            }
            return null;
        }

        /// <summary>
        /// 加载依赖文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private Assembly Context_Resolving(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            string expectedPath = Path.Combine(_basePath, assemblyName.Name + ".dll"); ;
            if (File.Exists(expectedPath))
            {
                try
                {
                    using (var stream = File.OpenRead(expectedPath))
                    {
                        return context.LoadFromStream(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载节点{expectedPath}发生异常：{ex.Message},{ex.StackTrace}");
                }
            }
            else
            {
                Console.WriteLine($"依赖文件不存在：{expectedPath}");
            }
            return null;
        }
    }
}
