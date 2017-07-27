using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascsc
{
    public class gencode
    {
        public static string getCompilerStr()
        {
            CompilerInfo info = CodeDomProvider.GetCompilerInfo("CSharp");
            var dotnetver = info.CodeDomProviderType.Assembly.ImageRuntimeVersion;
            return dotnetver;
        }
        public static CompilerResults GenCode(List<string> codes, bool autoAddFrameworkDll = false, List<string> dllfilenames = null)
        {
            CompilerParameters cplist = new CompilerParameters();
            //cplist.CompilerOptions.
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = false;
            cplist.ReferencedAssemblies.Add("System.dll");
            if (dllfilenames != null)
            {
                foreach (var dn in dllfilenames)
                {
                    cplist.ReferencedAssemblies.Add(dn);
                }
            }
            if (autoAddFrameworkDll)
            {
                cplist.ReferencedAssemblies.Add("AntShares.SmartContract.Framework.dll");
            }
            // 编译代理类，C# CSharp都可以
            CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");



            CompilerResults cr = provider1.CompileAssemblyFromSource(cplist, codes.ToArray());
            return cr;
        }

    }
}
