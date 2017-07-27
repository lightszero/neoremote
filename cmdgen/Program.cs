using AntShares.Compiler.MSIL;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ascsc
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error:param error.");
                Console.WriteLine("Use like this:");
                Console.WriteLine("    ascsc.exe xxx.cs yyy.avm.");
                return;
            }
            string code = null;
            CompilerResults r = null;

            try
            {
                code = System.IO.File.ReadAllText(args[0]);
            }
            catch(Exception err)
            {
                Console.WriteLine("Error:open .cs file error.");
                Console.WriteLine(err.ToString());
                return;
            }


            try
            {
                 r = gencode.GenCode(new List<string> { code}, true);
                if (r.Errors.Count > 0)
                {
                    Console.WriteLine("Error:csharp compile.");
                  
                    for (var i = 0; i < r.Errors.Count; i++)
                    {
                        string outline = r.Errors[i].ErrorText;
                        if (r.Errors[i].FileName != null)
                        {
                            outline += r.Errors[i].FileName + "(" + r.Errors[i].Line + ")";
                        }
                        Console.WriteLine(outline);
                    }
                    return;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error:csharp compile.");
                Console.WriteLine(err.ToString());
                return;
            }

            //conv
            try
            {
                var st = System.IO.File.OpenRead(r.PathToAssembly);
                using (st)
                {
                    var bs = Converter.Convert(st);
                    if (bs != null)
                    {
                        string outfile = args[1];
                        System.IO.File.WriteAllBytes(outfile, bs);
                        Console.WriteLine("Out File:"+outfile);
                    }
                    else
                    {

                        Console.WriteLine("Error:conv cs 2 avm.");
                        Console.WriteLine("unknown error.");

                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error:conv cs 2 avm.");
                Console.WriteLine(err.ToString());
                return;
            }
        }
    }
}
