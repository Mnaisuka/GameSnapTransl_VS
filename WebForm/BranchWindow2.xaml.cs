using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WebForm
{
    /// <summary>
    /// TranslWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class TranslWindow2 : Window
    {
        public MainWindow Form1;
        public TranslWindow2(MainWindow Form1)
        {
            this.Form1 = Form1;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (UI2_ComBox_Lang.SelectedItem!=null)
            {
                var item = (ComboBoxItem)UI2_ComBox_Lang.SelectedItem;
                Thread thread;
                thread = new Thread(TranslateAndProofread);
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start(new Tuple<string, string> (UI2_TextBox_Input.Text, item.Tag.ToString()));

            }
        }

        private async void TranslateAndProofread(object obj)
        {
            Tuple<string, string> args = (Tuple<string, string>)obj;

            Funcs.UpdateProperty(this, "Text", "正在翻译...");

            string translated_text = "";
            string proofread_text = "";
            string format_text = "译文：{0}";

            Funcs.UpdateProperty(UI2_Button_Transl, "IsEnabled", false);

            List<ChatMessage> body = [ChatMessage.FromSystem($"//You will only receive subtitles and are only required to translate, no need for any replies.\n//Note: Content related to Overwatch\n// Translate the input into '{args.Item2}'"), ChatMessage.FromUser(args.Item1)];

            await Form1.openai_sdk.Send((string character) =>
            {
                translated_text += character;

                translated_text = translated_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                string temp_text = string.Format(format_text, translated_text);

                Funcs.UpdateProperty(UI2_RichTextBox, "Text", temp_text);
                Funcs.UpdateProperty(UI2_RichTextBox, "SelectionStart", temp_text.Length);
            }, body, 500, "gpt-3.5-turbo");

            format_text = string.Format(format_text, translated_text);
            format_text += "\r\n\r\n校对：{0}";

            Funcs.UpdateProperty(this, "Text", "正在校对...");

            body = [ChatMessage.FromSystem("//You will only receive subtitles and are only required to translate, no need for any replies.\\n//Note: Content related to Overwatch\\n// Translate the input into 'Chinese'\""), ChatMessage.FromUser(translated_text)];

            await Form1.openai_sdk.Send((string character) =>
            {
                proofread_text += character;

                proofread_text = proofread_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                string temp_text = string.Format(format_text, proofread_text);

                Funcs.UpdateProperty(UI2_RichTextBox, "Text", temp_text);
                Funcs.UpdateProperty(UI2_RichTextBox, "SelectionStart", temp_text.Length);
            }, body, 500, "gpt-3.5-turbo");//3.5智力不够，如果不是翻译而是回答，需要改一下内容或者使用GPT4.0

            Funcs.UpdateProperty(UI2_Button_Transl, "IsEnabled", true);

            Funcs.UpdateProperty(this, "Text", "已完成");
        }

        private void UI2_TextBox_Input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        private void UI2_TextBox_Input_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (UI2_Button_Transl.IsEnabled == true)
                {
                    UI2_Button_Transl.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                }
                else
                {
                    Console.WriteLine("请等待翻译完成后再触发");
                }

            }
        }
    }
}
