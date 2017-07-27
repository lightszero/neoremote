using Neo.Compiler.MSIL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remotegen
{
    public partial class Form1 : Form, Neo.Compiler.ILogger
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void Log(string str)
        {
            Action call = () =>
             {
                 this.listBox1.Items.Add(str);
             };
            this.Invoke(call);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //log 本地IP
            {
                System.Net.IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
                System.Net.IPAddress ipv4 = System.Net.IPAddress.Any;
                System.Net.IPAddress ipv4self = System.Net.IPAddress.Any;
                foreach (var ip in addressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.IsIPv6LinkLocal == false)
                    {
                        ipv4self = ip;
                    }
                }

                Log("本地ip:" + ipv4self.ToString());
            }
            //启动服务器
            {
                hhgate.CustomServer.BeginServer();
                Log("服务器已经启动:http://*:8080/_api/ver");
            }

            {//测试编译器
                Log("dotnet 编译器版本:" + gencode.getCompilerStr());

                List<string> codes = new List<string>();
                codes.Add(System.IO.File.ReadAllText("tcode.cs"));
                var r = gencode.GenCode(codes, true);

                Log(r.Errors.Count == 0 ? "dotnet 编译器 正常" : "dotnet 编译器 异常");
                Log("测试小蚁编译器");
                if (r.Errors.Count == 0)
                {
                    try
                    {
                        var st = System.IO.File.OpenRead(r.PathToAssembly);
                        using (st)
                        {
                            var bs = Converter.Convert(st, this);
                            if (bs != null)
                            {
                                Log("测试小蚁编译器正常");
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Log(err.ToString());
                        Log("测试小蚁编译器失败");
                    }
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
