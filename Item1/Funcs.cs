using Baidu.Aip.Ocr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using Microsoft.VisualBasic;
using System.Net.Http;
using OpenAI.Managers;
using OpenAI;
using OpenAI.ObjectModels.RequestModels;
using static System.Net.Mime.MediaTypeNames;

namespace Main
{
    public partial class UI_Body : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("screenshot.dll", EntryPoint = "CameraWindow")]
        public static extern int PrScrn();

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 134217728;
        private const int WS_EX_LAYERED = 524288;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_HOTKEY:
                    ProcessHotKeyMessage(m.WParam.ToInt32());
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public Funcs.Baidu baidu_sdk;
        public Funcs.OpenAI openai_sdk;

        public string openai_prompt;

        public bool ChooseConfig()
        {
            var parser = new FileIniDataParser();

            string IniPath = "config.ini";

            IniData data_ini;

            if (!File.Exists(IniPath))
            {
                File.Create(IniPath).Close();
                data_ini = parser.ReadFile(IniPath);

                data_ini.Sections.AddSection("baidu");
                data_ini["baidu"].AddKey("api_key", "");
                data_ini["baidu"].AddKey("secret_key", "");

                data_ini.Sections.AddSection("openai");
                data_ini["openai"].AddKey("base", "");
                data_ini["openai"].AddKey("token", "");

                data_ini.Sections.AddSection("openai");
                data_ini["openai"].AddKey("base", "");
                data_ini["openai"].AddKey("token", "");

                parser.WriteFile(IniPath, data_ini);

                data_ini = parser.ReadFile(IniPath);
            }
            else
            {
                data_ini = parser.ReadFile(IniPath);
            }


            var list = new List<List<string>>()
            {
                new List<string>() { "baidu", "api_key" ,"请输入百度OCR的Api Key：",""},
                new List<string>() { "baidu", "secret_key" ,"请输入百度OCR的Secret Key：",""},
                new List<string>() { "openai", "base" ,"请输入OpenAI的接口：","https://api.openai.com/"},
                new List<string>() { "openai", "token", "请输入OpenAI的秘钥：", "" },
                new List<string>() { "openai", "prompt", "请为OpenAI提供任务提示词：", "示例：这是守望先锋的聊天内容，请帮我翻译为简体中文。"}
            };

            foreach (var inner in list)
            {
                string text = data_ini[inner[0]][inner[1]];
                if (string.IsNullOrEmpty(text))//值为空
                {
                    string input = Interaction.InputBox(inner[2], "缺少值:", inner[3]);

                    if (string.IsNullOrEmpty(input))
                    {
                        MessageBox.Show($"程序运行必须依赖该值：{inner[0]} & {inner[1]}","运行失败:");
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        Console.WriteLine("存储值 > {0} > {1} > {2}", inner[0],inner[1], input);

                        data_ini[inner[0]][inner[1]] = input;

                        parser.WriteFile(IniPath, data_ini);
                    }
                }
            }
            
            baidu_sdk = new Funcs.Baidu(data_ini["baidu"]["api_key"], data_ini["baidu"]["secret_key"]);

            if (baidu_sdk.stat==false)
            {
                MessageBox.Show($"错误信息：{baidu_sdk.msg}", "百度OCR出错：");
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                Console.WriteLine("已加载百度OCR");
            }

            openai_prompt = data_ini["openai"]["prompt"];

            openai_sdk = new Funcs.OpenAI(data_ini["openai"]["base"], data_ini["openai"]["token"]);

            return true;
        }

        public string OcrAndTransl(byte[] bit_data)
        {
            string resp_text = JsonConvert.SerializeObject(baidu_sdk.Ocr(bit_data));
            JObject json_object = JObject.Parse(resp_text);
            JArray words_result = (JArray)json_object["words_result"];
            string words_join = "";
            foreach (var item in words_result)
            {
                string words = (string)item["words"];
                if (words_join == "")
                {
                    words_join += words;
                }
                else
                {
                    words_join += "\r\n" + words;
                }
            }

            Funcs.UpdateControlProperty(UI_TextBox, "Text", words_join);

            Console.WriteLine("开始翻译");

            var transl_text = "";
            _ = openai_sdk.Send((string character) =>
            {
                transl_text += character;

                string replacedText = transl_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                Funcs.UpdateControlProperty(UI_TextBox, "Text", replacedText);
            },
            [
                ChatMessage.FromSystem(openai_prompt),
                ChatMessage.FromUser(words_join)
            ], 500, "gpt-3.5-turbo");

            Console.WriteLine("翻译结束");

            return transl_text;
        }

        private void on_hot_ocr_regs()
        {
            Funcs.UpdateControlProperty(this, "Visible", false);
            Rectangle captureRect;
            captureRect = new Rectangle(45, 420, 450, 235);
            Bitmap bitmap = new Bitmap(captureRect.Width, captureRect.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(captureRect.Location, Point.Empty, captureRect.Size);
            }
            Funcs.UpdateControlProperty(this, "Visible", true);
            OcrAndTransl(Funcs.BitmapToByteArray(bitmap));
        }

        private void on_hot_ocr()
        {
            bool sveble = this.Visible;
            Funcs.UpdateControlProperty(this, "Visible", false);
            int PrSata = PrScrn();
            if (PrSata == 1)
            {
                Console.WriteLine("截图成功");
                if (Clipboard.ContainsImage())
                {
                    Funcs.UpdateControlProperty(this, "Visible", true);
                    OcrAndTransl(Funcs.BitmapToByteArray((Bitmap)Clipboard.GetImage()));

                }
                else
                {
                    Funcs.UpdateControlProperty(this, "Visible", sveble);
                    Console.WriteLine("从剪辑板获取图片失败");
                }
            }
            else
            {
                Funcs.UpdateControlProperty(this, "Visible", sveble);
                Console.WriteLine("取消截图");
            }
        }
    }
    public class Funcs
    {
        public static void UpdateControlProperty<TControl, TProperty>(TControl control, string propertyName, TProperty value) where TControl : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<TControl, string, TProperty>(UpdateControlProperty), new object[] { control, propertyName, value });//Invoke 将该方法抛到控件线程中执行
            }
            else
            {
                var propertyInfo = control.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.CanWrite && propertyInfo.PropertyType == typeof(TProperty))
                {
                    propertyInfo.SetValue(control, value);
                }
            }
        }
        public class Baidu
        {
            private readonly Ocr client;
            private readonly string api_key;
            private readonly string secret_key;
            public string msg;
            public bool stat;

            public Baidu(string id, string key)
            {
                api_key = id;
                secret_key = key;
                (stat,msg) = this.Check();
                client = new Ocr(api_key, secret_key)
                {
                    Timeout = 60000  // 修改超时时间
                };
            }

            public object Ocr(byte[] file_data)
            {
                var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                {"probability", "true"}};
                var result = client.GeneralBasic(file_data, options);
                Console.WriteLine(result);
                return result;
            }

            public (bool,string) Check()
            {
                using (var httpClient = new HttpClient())
                {
                    var url = "https://aip.baidubce.com/oauth/2.0/token";
                    var content = new StringContent($"grant_type=client_credentials&client_id={api_key}&client_secret={secret_key}");
                    var response = httpClient.PostAsync(url, content).Result;
                    string resp = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return (true, "success");
                    }
                    else
                    {
                        try
                        {
                            var jresp = JsonConvert.DeserializeObject<dynamic>(resp);
                            return (false, jresp["error_description"]);
                        }
                        catch (JsonReaderException)
                        {
                            return (false, "unknown error");
                        }
                    }
                }
            }
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        public class OpenAI
        {
            private string host;
            private string token;
            private OpenAIService openai_service;
            public OpenAI(string host, string token)
            {
                this.host = host;
                this.token = token;

                openai_service = new OpenAIService(new OpenAiOptions()
                {
                    ApiKey = this.token,
                    BaseDomain = this.host
                });
            }
            public delegate void TestCallbackDelegate(string param);
            public async Task<bool> Send(TestCallbackDelegate callback, List<ChatMessage> Messages, int MaxTokens, string Model)
            {
                var completionResult = openai_service.ChatCompletion.CreateCompletionAsStream(
                    new ChatCompletionCreateRequest
                    {
                        Messages = Messages,
                        Stream = true,
                        MaxTokens = MaxTokens,
                        Model = Model
                    });
                try
                {
                    await foreach (var completion in completionResult)
                    {
                        if (completion.Successful)
                        {
                            callback(completion.Choices.FirstOrDefault()?.Message.Content);
                        }
                        else
                        {
                            Console.WriteLine($"未知错误:{completion.Error.Code}: {completion.Error.Message}");
                            return false;
                        }
                    }
                }
                catch (Exception e) // 捕获子线程异常
                {
                    Console.WriteLine($"发生异常: {e.Data}");
                    throw;
                }
                return true;
            }
        }
    }
}
