using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.VisualBasic;

namespace NHenBrowser
{
    public partial class Form1 : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        
        public static bool Connectblm()
        {
            try
            {
                int conne;
                return InternetGetConnectedState(out conne, 0);
            }
            catch
            {
                return false;
            }
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        private void InitializeChomium()
        {
            //Cef.Initialize(new CefSettings());
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            toolStripTextBox1.Text = "https://whatismyipaddress.com";
            ChromiumWebBrowser ChromeBrowser = new ChromiumWebBrowser(toolStripTextBox1.Text);
            ChromeBrowser.Parent = tabControl1.SelectedTab;
            //panel1.Controls.Add(ChromeBrowser);
            ChromeBrowser.Dock = DockStyle.Fill;
            ChromeBrowser.AddressChanged += Chrome_AddressChanged;
            ChromeBrowser.TitleChanged += Chrome_TittleChanged;
            timer2.Enabled = true;
        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                toolStripTextBox1.Text = e.Address;
            }
                ));
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(
            IntPtr dwL,
            int dw,
            IntPtr dwB,
            int dwBL
        );
        public struct SIPI
        {
            public int dwAT;
            public IntPtr pro;
            public IntPtr prB;
        }

        private void UseProxy(string Proxy)
        {
            const int PO = 38;
            const int POI = 3;

            SIPI ISI = default(SIPI);
            ISI.dwAT = POI;
            ISI.pro = Marshal.StringToHGlobalAnsi(Proxy);
            ISI.prB = Marshal.StringToHGlobalAnsi("local");

            IntPtr INS = Marshal.AllocCoTaskMem(Marshal.SizeOf(ISI));

            Marshal.StructureToPtr(ISI, INS, true);
            bool iR = InternetSetOption(IntPtr.Zero, PO, INS, Marshal.SizeOf(ISI));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar1.Value == 100)
            {
                timer1.Enabled = false;
                toolStripButton4.Enabled = true;
                InitializeChomium();
            }
            else if (toolStripProgressBar1.Value >= 40)
            {
                if (Connectblm())
                {
                    toolStripProgressBar1.Value = toolStripProgressBar1.Value + 10;
                }
                else
                {
                    timer1.Enabled = false;
                    MessageBox.Show("Check your Connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

            }
            else if (toolStripProgressBar1.Value >= 80)
            {
                //WinInetInterop.SetConnectionProxy("1.1.1.1:80");
                toolStripProgressBar1.Value = toolStripProgressBar1.Value + 5;
            }
            else
            {
                toolStripProgressBar1.Value = toolStripProgressBar1.Value + 5;
            }

        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {

                ChromiumWebBrowser ChromeBrowser = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;

                if (ChromeBrowser != null)
                {
                    if (Information.IsNumeric(toolStripTextBox1.Text))
                    {
                        ChromeBrowser.Load("http://nhentai.net/g/" + toolStripTextBox1.Text);
                    }
                    else //Jika isi txtAngka diisi selain angka
                    {
                        ChromeBrowser.Load(toolStripTextBox1.Text);
                    }
                    TabPage current = tabControl1.SelectedTab;
                    ChromeBrowser.Parent = current;
                    if (ChromeBrowser.CanGoBack)
                        toolStripButton1.Enabled = true;
                }
            }  
                     
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                ChromiumWebBrowser ChromeBrowser = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;

                if (ChromeBrowser != null)
                    ChromeBrowser.Refresh();

                TabPage current = tabControl1.SelectedTab;
                ChromeBrowser.Parent = current;
            }

            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                ChromiumWebBrowser ChromeBrowser = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;

                if (ChromeBrowser != null)
                {
                    if (ChromeBrowser.CanGoBack)
                    {

                        ChromeBrowser.Back();

                    }
                    else
                    {
                        toolStripButton1.Enabled = false;
                    }

                    TabPage current = tabControl1.SelectedTab;
                    ChromeBrowser.Parent = current;

                    if (ChromeBrowser.CanGoForward)
                    {
                        toolStripSplitButton1.Enabled = true;
                    }
                }
            }            
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                ChromiumWebBrowser ChromeBrowser = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;

                if (ChromeBrowser != null)
                {
                    if (ChromeBrowser.CanGoForward)
                        ChromeBrowser.Forward();

                    TabPage current = tabControl1.SelectedTab;
                    ChromeBrowser.Parent = current;
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage();
            tab.Text = "Welcome";
            tabControl1.Controls.Add(tab);
            tabControl1.SelectTab(tabControl1.TabCount - 1);

            TabPage current = tabControl1.SelectedTab;

            ChromiumWebBrowser ChromeBrowser = new ChromiumWebBrowser("http://google.com");
            ChromeBrowser.Dock = DockStyle.Fill;
            ChromeBrowser.Parent = current;
            toolStripTextBox1.Text = "http://google.com";
            ChromeBrowser.AddressChanged += Chrome_AddressChanged;
            ChromeBrowser.TitleChanged += Chrome_TittleChanged;

        }

        private void Chrome_TittleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
                {
                    tabControl1.SelectedTab.Text = e.Title;
                }
           ));
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage current = tabControl1.SelectedTab;
            tabControl1.Controls.Remove(current);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                ChromiumWebBrowser ChromeBrowser = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
                
                if (ChromeBrowser != null)
                {
                    if (ChromeBrowser.CanGoForward)
                    {
                        if (ChromeBrowser.CanGoForward)
                        {
                            toolStripSplitButton1.Enabled = true;
                        }

                        if (ChromeBrowser.CanGoBack)
                        { 
                            toolStripButton1.Enabled = true;
                        }
                        TabPage current = tabControl1.SelectedTab;
                        ChromeBrowser.Parent = current;
                    }
                }
            }
            
        }

    }
}
