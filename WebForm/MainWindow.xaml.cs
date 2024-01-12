using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebForm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public IntPtr hwnd ;
        public const int key_show_or_hide = 32;
        public const int key_close = 64;
        public const int key_ocr = 4;
        public const int key_ocr_regs = 3;
        public const int key_win_transl = 2;

        private void ProcessHotKeyMessage(int hotKeyId)
        {
            Thread thread;
            switch (hotKeyId)
            {
                case key_win_transl:
                    if (!Form2.IsLoaded)
                    {
                        Form2 = new TranslWindow2(this);
                        if (this.IsVisible)
                        {
                            Form2.Left = this.Left + this.Width - 8;
                            Form2.Top = this.Top;
                        }
                        Form2.Topmost = true;
                        Form2.Show();
                    }
                    else
                    {
                        Form2.Visibility = Form2.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                        Form2.Activate();
                        Form2.UI2_TextBox_Input.Focus();
                        Form2.Topmost = true;
                    }
                    Console.WriteLine($"输入窗口 : ${hotKeyId}");//F2
                    break;
                case key_ocr_regs:
                    thread = new Thread(CB_hot_ocr_regs);
                    thread.IsBackground = true;
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    Console.WriteLine($"固定区域识别 : ${hotKeyId}");//F3
                    break;
                case key_ocr:
                    thread = new Thread(CB_hot_ocr);
                    thread.IsBackground = true;
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    Console.WriteLine($"自定义区域识别 : ${hotKeyId}");//F4
                    break;
                case key_show_or_hide:
                    this.Visibility = this.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    Console.WriteLine($"显示或隐藏 : ${hotKeyId}");//Home
                    break;
                case key_close:
                    this.IClose();
                    Console.WriteLine($"关闭程序 : ${hotKeyId}");//End
                    break;
            }
        }

        public TranslWindow2 Form2;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false; // 取消任务栏按钮
            
            hwnd = new WindowInteropHelper(this).Handle; // 获取句柄

            List<bool> HotStat = [
                NativeMethods.RegisterHotKey(hwnd, key_win_transl, KeyModifiers.None, Keys.F2),
                NativeMethods.RegisterHotKey(hwnd, key_ocr_regs, KeyModifiers.None, Keys.F3),
                NativeMethods.RegisterHotKey(hwnd, key_ocr, KeyModifiers.None, Keys.F4),
                NativeMethods.RegisterHotKey(hwnd, key_show_or_hide, KeyModifiers.None, Keys.Home),
                NativeMethods.RegisterHotKey(hwnd, key_close, KeyModifiers.None, Keys.End)
            ]; // 注册热键


            Left = 165;
            Top = 702;

            Form2 = new TranslWindow2(this);

            Form2.Left = this.Left + this.Width - 8;
            Form2.Top = this.Top;

            this.OptionInfo();
        }
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            
            Topmost = true; // 顶置 // 在loadre里时候偶尔会被覆盖掉（这种鬼畜BUG完全没法修）
        }

        public void IClose()
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();//彻底关闭
        }

    }   
}
