using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Process p;

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                Arguments = "c:\\temp\\test1.ps1",
                WindowStyle = ProcessWindowStyle.Minimized
            };

            p = new Process();
            p.StartInfo = psi;
            p.OutputDataReceived += P_OutputDataReceived;
            p.ErrorDataReceived += P_OutputDataReceived;
            p.EnableRaisingEvents = true;

            Task t = new Task(() =>
            {
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();

                WT("process exited");
                p.Close();
            });

            t.Start();
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            WT(e.Data, true);
        }

        private void WT(string text, bool WriteConsole = false)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => WT(text, WriteConsole)));
                return;
            }

            textBox1.Text += text + Environment.NewLine;
            Console.WriteLine(text);
        }
    }
}
