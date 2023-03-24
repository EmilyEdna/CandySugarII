using Serilog;
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
        public static ConcurrentQueue<Type> Single = new ConcurrentQueue<Type>();
        public static List<DLLInfomations> Dll = new List<DLLInfomations>();
        public AssemblyLoader(string basePath)
        {
            _basePath = basePath;
        }
        /// <summary>
        /// 动态加载Dll
        /// </summary>
        /// <param name="dllFileName"></param>
        /// <param name="typeName"></param>
        public void Load(string dllFileName, string typeName)
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
                        Type InstanceType = assembly.GetTypes().FirstOrDefault(t => t.Name.ToLower().Equals(typeName.ToLower()));
                        Single.Enqueue(InstanceType);
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
        }
        /// <summary>
        /// 动态加载Dll
        /// </summary>
        /// <param name="dllFileName"></param>
        /// <param name="typeName"></param>
        /// <param name="configTypeName"></param>
        /// <param name="description"></param>
        public void Load(string dllFileName, string typeName, string description = "")
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
                        Type InstanceType = assembly.GetTypes().FirstOrDefault(t => t.Name.ToLower().Equals(typeName.ToLower()));
                        Type ViewModel = assembly.GetTypes().FirstOrDefault(t => t.Name.Contains("ViewModel"));
                        Dll.Add(new DLLInfomations
                        {
                            InstanceViewModel = ViewModel,
                            InstanceType = InstanceType,
                            Description = description,
                            IsEnable = true,
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error($"加载节点{dllFileName}-{typeName}发生异常：{ex.Message},{ex.StackTrace}");
                }

            }
            else
            {
                Log.Logger.Error($"节点动态库{dllFileName}不存在：{path}");
            }
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
                    Log.Logger.Error($"加载节点{expectedPath}发生异常：{ex.Message},{ex.StackTrace}");
                }
            }
            else
            {
                Log.Logger.Error($"依赖文件不存在：{expectedPath}");
            }
            return null;
        }
    }
}
