using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WebForm
{
    /// <summary>
    /// OptionWindow3.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow3 : Window
    {
        IniData OPDATA;
        public OptionWindow3(ref IniData Data)
        {
            OPDATA = Data;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TV1.Text = OPDATA["baidu"]["api_key"];
            TV2.Text = OPDATA["baidu"]["secret_key"];
            TV3.Text = OPDATA["openai"]["base"];
            TV4.Text = OPDATA["openai"]["token"];
            TV5.Text = OPDATA["openai"]["prompt"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Grid_Box.RenderSize.Width);
            Console.WriteLine(Grid_Box.RenderSize.Height);
        }

        private void TextBox_TextChanged_bd_id(object sender, TextChangedEventArgs e)
        {
            OPDATA["baidu"]["api_key"] = ((TextBox)sender).Text;
            Title = "已保存更改。";
        }

        private void TextBox_TextChanged_bd_key(object sender, TextChangedEventArgs e)
        {
            OPDATA["baidu"]["secret_key"] = ((TextBox)sender).Text;
            Title = "已保存更改。";
        }

        private void TextBox_TextChanged_ai_host(object sender, TextChangedEventArgs e)
        {
            OPDATA["openai"]["base"] = ((TextBox)sender).Text;
            Title = "已保存更改。";
        }

        private void TextBox_TextChanged_ai_key(object sender, TextChangedEventArgs e)
        {
            OPDATA["openai"]["token"] = ((TextBox)sender).Text;
            Title = "已保存更改。";
        }

        private void TextBox_TextChanged_ai_pr(object sender, TextChangedEventArgs e)
        {
            OPDATA["openai"]["prompt"] = ((TextBox)sender).Text;
            Title = "已保存更改。";
        }
    }
}
