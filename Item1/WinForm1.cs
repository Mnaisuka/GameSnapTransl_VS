using Baidu.Aip.Speech;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.InteropServices;

namespace Main
{
    public partial class UI_Body : Form
    {
        public UI_Body()
        {
            InitializeComponent();
        }

        private void UI_PanelBox_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void UI_Label_Tip_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        public const int key_show_or_hide = 32;
        public const int key_close = 64;
        public const int key_ocr = 4;
        public const int key_ocr_regs = 3;
        public const int key_win_transl= 2;
        private void ProcessHotKeyMessage(int hotKeyId)
        {
            Thread thread;
            switch (hotKeyId)
            {
                case key_win_transl:
                    if (WinForm2.IsDisposed)
                    {
                        WinForm2 = new UI2_Body(this)
                        {
                            StartPosition = FormStartPosition.Manual
                        };
                        if (this.Visible)
                        {
                            WinForm2.Left = this.Left + this.Width - 8;
                            WinForm2.Top = this.Top;
                        }
                        WinForm2.Show();
                    }
                    else
                    {
                        WinForm2.Visible = !WinForm2.Visible;
                    }
                    Console.WriteLine($"输入窗口 : ${hotKeyId}");//F2
                    break;
                case key_ocr_regs:
                    Console.WriteLine($"固定区域识别 : ${hotKeyId}");//F3
                    thread = new Thread(on_hot_ocr_regs);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    break;
                case key_ocr:
                    Console.WriteLine($"自定义区域识别 : ${hotKeyId}");//F4
                    thread = new Thread(on_hot_ocr);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    break;
                case key_show_or_hide: 
                    this.Visible = !this.Visible;
                    Console.WriteLine($"显示或隐藏 : ${hotKeyId}");//Home
                    break;
                case key_close:
                    Console.WriteLine($"关闭程序 : ${hotKeyId}");//End
                    this.Close();
                    break;
            }
        }

        private void UI_Body_Load(object sender, EventArgs e)
        {
            this.UI_Label_Tip.BackColor = Color.Transparent;
            this.Opacity = 0.85;
            this.TopMost = true;
            this.ShowInTaskbar = false; // 需要在热键注册前调用,否则事件会被注销

            SetWindowLong(this.Handle,GWL_EXSTYLE, WS_EX_NOACTIVATE|WS_EX_LAYERED);

            List<bool> HotStat = [
                NativeMethods.RegisterHotKey(this.Handle, key_win_transl, KeyModifiers.None, Keys.F2),
                NativeMethods.RegisterHotKey(this.Handle, key_ocr_regs, KeyModifiers.None, Keys.F3),
                NativeMethods.RegisterHotKey(this.Handle, key_ocr, KeyModifiers.None, Keys.F4),
                NativeMethods.RegisterHotKey(this.Handle, key_show_or_hide, KeyModifiers.None, Keys.Home),
                NativeMethods.RegisterHotKey(this.Handle, key_close, KeyModifiers.None, Keys.End)
            ];

            this.Left = 165;
            this.Top = 702;

            WinForm2 = new UI2_Body(this);

            WinForm2.StartPosition = FormStartPosition.Manual;

            WinForm2.Left = this.Left + this.Width - 8;
            WinForm2.Top = this.Top;

            this.ChooseConfig();
        }
        public UI2_Body WinForm2;

        private void UI_TextBox_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
