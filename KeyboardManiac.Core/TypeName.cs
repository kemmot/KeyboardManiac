using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KeyboardManiac.Core
{
    public class TypeName
    {
        public static TypeName ParseFromAssemblyQualifiedName(string assemblyQualifiedName)
        {
            TypeName typeName = new TypeName();
            typeName.AssemblyQualifiedName = assemblyQualifiedName;
            string[] parts = assemblyQualifiedName.Split(',');
            if (parts.Length > 1)
            {
                typeName.AssemblyName = parts[1];
            }
            typeName.FullName = parts[0];

            string[] fullNameParts = typeName.FullName.Split('.');
            typeName.ClassName = fullNameParts[fullNameParts.Length - 1];
            return typeName;
        }

        public string AssemblyQualifiedName { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string FullName { get; set; }
    }
}
