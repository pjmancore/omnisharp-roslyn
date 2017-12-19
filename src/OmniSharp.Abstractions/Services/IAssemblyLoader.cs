﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OmniSharp.Services
{
    public interface IAssemblyLoader
    {
        Assembly Load(AssemblyName name);

        IReadOnlyList<Assembly> LoadAllFrom(string folderPath);

        Assembly LoadFrom(string assemblyPath);
    }

    public static class IAssemblyLoaderExtensions
    {
        public static Lazy<Assembly> LazyLoad(this IAssemblyLoader loader, string assemblyName)
        {
            return new Lazy<Assembly>(() => loader.Load(assemblyName));
        }

        public static Assembly Load(this IAssemblyLoader loader, string name)
        {
            var assemblyName = name;
            if (name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                assemblyName = name.Substring(0, name.Length - 4);
            }

            return loader.Load(new AssemblyName(assemblyName));
        }

        public static IEnumerable<Assembly> Load(this IAssemblyLoader loader, params string[] assemblyNames)
        {
            foreach (var name in assemblyNames)
            {
                yield return Load(loader, name);
            }
        }

        public static Assembly LoadByAssemblyNameOrPath(string assemblyName)
        {
            if (File.Exists(assemblyName))
            {
                return Assembly.LoadFrom(assemblyName);
            }
            else
            {
                return Assembly.Load(assemblyName);
            }
        }

        public static IEnumerable<Assembly> LoadByAssemblyNameOrPath(this IAssemblyLoader loader, IEnumerable<string>  assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                yield return LoadByAssemblyNameOrPath(assemblyName);
            }
        }
    }
}
