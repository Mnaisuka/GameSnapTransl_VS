using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace Main
{
    public partial class UI2_Body : Form
    {
        public UI_Body Form1;
        public UI2_Body(UI_Body Form1)
        {
            this.Form1 = Form1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread;
            thread = new Thread(TranslateAndProofread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(UI2_TextBox_Input.Text);
        }

        private async void TranslateAndProofread(object obj)
        {
            Funcs.UpdateControlProperty(this, "Text", "正在翻译...");

            string translated_text = "";
            string proofread_text = "";
            string format_text = "译文：{0}";

            Funcs.UpdateControlProperty(UI2_Button_TL, "Enabled", false);

            List<ChatMessage> body = [ChatMessage.FromSystem("//You will only receive subtitles and are only required to translate, no need for any replies.\n//Note: Content related to Overwatch\n// Translate the input into 'Japanese'"), ChatMessage.FromUser(obj.ToString())];

            await Form1.openai_sdk.Send((string character) =>
            {
                translated_text += character;

                translated_text = translated_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                string temp_text = string.Format(format_text, translated_text);

                Funcs.UpdateControlProperty(UI2_RichTextBox, "Text", temp_text);
                Funcs.UpdateControlProperty(UI2_RichTextBox, "SelectionStart", temp_text.Length);
            }, body, 500, "gpt-3.5-turbo");

            format_text = string.Format(format_text, translated_text);
            format_text += "\r\n\r\n校对：{0}";

            Funcs.UpdateControlProperty(this, "Text", "正在校对...");

            body = [ChatMessage.FromSystem("//You will only receive subtitles and are only required to translate, no need for any replies.\\n//Note: Content related to Overwatch\\n// Translate the input into '简体中文'\""), ChatMessage.FromUser(translated_text)];

            await Form1.openai_sdk.Send((string character) =>
            {
                proofread_text += character;

                proofread_text = proofread_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                string temp_text = string.Format(format_text, proofread_text);

                Funcs.UpdateControlProperty(UI2_RichTextBox, "Text", temp_text);
                Funcs.UpdateControlProperty(UI2_RichTextBox, "SelectionStart", temp_text.Length);
            }, body, 500, "gpt-3.5-turbo");//3.5智力不够，如果不是翻译而是回答，需要改一下内容或者使用GPT4.0

            Funcs.UpdateControlProperty(UI2_Button_TL, "Enabled", true);

            Funcs.UpdateControlProperty(this, "Text", "已完成");
        }

        private void UI2_Body_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            new ImageForm(Properties.Resources.sponsor);
        }

        private void UI2_TextBox_Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UI2_Button_TL.Enabled==true)
                {
                    UI2_Button_TL.PerformClick();
                }
                else
                {
                    Console.WriteLine( "请等待翻译完成后再触发");
                }
                
            }
        }

        private void UI2_TextBox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void UI_Body_2_Load(object sender, EventArgs e)
        {

        }
    }
    public class ImageForm : Form
    {
        private readonly PictureBox pictureBox;

        public ImageForm(Bitmap image)
        {
            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(pictureBox);

            pictureBox.Image = image;
            Width = (int)(image.Width * 0.6);
            Height = (int)(image.Height * 0.6);

            this.ShowInTaskbar = false;

            this.Text = "如果这个项目对你有帮助，请考虑请我喝杯咖啡。";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ShowIcon = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Show();
        }
    }
}
