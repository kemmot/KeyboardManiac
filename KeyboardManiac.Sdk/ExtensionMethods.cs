using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace KeyboardManiac.Sdk
{
    public static class ExtensionMethods
    {
        public static void AppendLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendLine(string.Format(format, args));
        }

        public static string GetLoadedAssemblyText(this AppDomain domain)
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetSortedAssemblies();

            StringBuilder text = new StringBuilder();
            text.AppendLine("{0} loaded assemblies", assemblies.Count);
            for (int index = 0; index < assemblies.Count; index++)
            {
                Assembly assembly = assemblies[index];
                text.AppendLine(
                    "Assembly {0:D2}/{1:D2}: {2}",
                    index + 1,
                    assemblies.Count,
                    assembly.ToString());
            }

            return text.ToString();
        }

        public static List<Assembly> GetSortedAssemblies(this AppDomain domain)
        {
            List<Assembly> assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            assemblies.Sort((left, right) => { return left.FullName.CompareTo(right.FullName); });
            return assemblies;
        }
    }
}
