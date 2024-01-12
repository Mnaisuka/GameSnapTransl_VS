using Baidu.Aip.Ocr;
using IniParser;
using IniParser.Model;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WebForm
{
    public partial class MainWindow : Window
    {
        [DllImport("screenshot.dll", EntryPoint = "CameraWindow")]
        public static extern int PrScrn();

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 134217728;
        private const int WS_EX_LAYERED = 524288;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_HOTKEY:
                    ProcessHotKeyMessage(wParam.ToInt32());
                    break;
            }
            return IntPtr.Zero;
        }

        public Funcs.Baidu baidu_sdk;
        public Funcs.OpenAI openai_sdk;
        public string openai_prompt;

        public bool OptionInfo()
        {
            var parser = new FileIniDataParser();

            string ini_path = "config.ini";

            IniData data_ini;

            if (!File.Exists(ini_path))
            {
                File.Create(ini_path).Close();
                data_ini = parser.ReadFile(ini_path);

                data_ini.Sections.AddSection("baidu");
                data_ini["baidu"].AddKey("api_key", "");
                data_ini["baidu"].AddKey("secret_key", "");

                data_ini.Sections.AddSection("openai");
                data_ini["openai"].AddKey("base", "");
                data_ini["openai"].AddKey("token", "");
                data_ini["openai"].AddKey("prompt", "这是一款游戏的聊天内容，请帮我翻译为简体中文。");

                parser.WriteFile(ini_path, data_ini);

                data_ini = parser.ReadFile(ini_path);
            }
            else
            {
                data_ini = parser.ReadFile(ini_path);
            }

            var retrieve = new List<List<string>>()
            {
                new() { "baidu", "api_key" ,"请输入百度OCR的Api Key："},
                new() { "baidu", "secret_key" ,"请输入百度OCR的Secret Key："},
                new() { "openai", "base" ,"请输入OpenAI的接口："},
                new() { "openai", "token", "请输入OpenAI的秘钥：" },
                new() { "openai", "prompt", "请为OpenAI提供任务提示词："}
            };

            string tip = "";
            foreach (var inner in retrieve)
            {
                string text = data_ini[inner[0]][inner[1]];
                if(text.Length == 0)
                {
                    tip += $"空值：{inner[0]} > {inner[1]}\n";
                }
            }
            if (tip.Length!=0)
            {
                Visibility = Visibility.Collapsed;
                MessageBox.Show(tip, "运行需要以下值：");
                var op = new OptionWindow3(ref data_ini);
                op.ShowDialog();
                Visibility = Visibility.Visible;

                parser.WriteFile(ini_path, data_ini);
            }

            baidu_sdk = new Funcs.Baidu(data_ini["baidu"]["api_key"], data_ini["baidu"]["secret_key"]);

            if (baidu_sdk.stat == false)
            {
                MessageBox.Show($"错误信息：{baidu_sdk.msg}", "百度OCR出错：");
                Close();
            }
            else
            {
                Console.WriteLine("已加载百度OCR");
            }

            openai_prompt = data_ini["openai"]["prompt"];

            openai_sdk = new Funcs.OpenAI(data_ini["openai"]["base"], data_ini["openai"]["token"]);

            return true;
        }

        private void CB_hot_ocr()
        {
            Visibility sveble = this.Visibility;
            Funcs.UpdateProperty(this, "Visibility", Visibility.Collapsed);
            int PrSata = PrScrn();
            if (PrSata == 1)
            {
                Console.WriteLine("截图成功");
                if (Clipboard.ContainsImage())
                {
                    Funcs.UpdateProperty(this, "Visibility", Visibility.Visible);
                    BitmapSource image = Clipboard.GetImage();
                    using MemoryStream stream = new();
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(stream);
                    byte[] image_bytes = stream.ToArray();
                    OcrAndTransl(image_bytes);
                }
                else
                {
                    Funcs.UpdateProperty(this, "Visibility", sveble);
                    Console.WriteLine("从剪辑板获取图片失败");
                }
            }
            else
            {
                Funcs.UpdateProperty(this, "Visibility", sveble);
                Console.WriteLine("取消截图");
            }
        }

        private void CB_hot_ocr_regs()
        {
            Funcs.UpdateProperty(this, "Visible", false);
            Rectangle captureRect;
            captureRect = new Rectangle(45, 420, 450, 235);
            Bitmap bitmap = new(captureRect.Width, captureRect.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(captureRect.Location, System.Drawing.Point.Empty, captureRect.Size);
            }
            Funcs.UpdateProperty(this, "Visible", true);
            OcrAndTransl(Funcs.BitmapToByteArray(bitmap));
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

            Funcs.UpdateProperty(UI_TextBox_Output, "Text", words_join);

            Console.WriteLine("开始翻译");

            var transl_text = "";
            _ = openai_sdk.Send((string character) =>
            {
                transl_text += character;

                string replacedText = transl_text.Replace("\r\n", "\r\n").Replace("\n", "\r\n");

                Funcs.UpdateProperty(UI_TextBox_Output, "Text", replacedText);
            },
            [
                ChatMessage.FromSystem(openai_prompt),
                ChatMessage.FromUser(words_join)
            ], 500, "gpt-3.5-turbo");

            Console.WriteLine("翻译结束");

            return transl_text;
        }
    }// MainWindow 

    public class Funcs
    {
        public static void UpdateProperty<TControl, TProperty>(TControl control, string propertyName, TProperty value) where TControl : System.Windows.Controls.Control
        {
            if (control.Dispatcher.CheckAccess())
            {
                var propertyInfo = control.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.CanWrite && propertyInfo.PropertyType == typeof(TProperty))
                {
                    propertyInfo.SetValue(control, value);
                }
            }
            else
            {
                control.Dispatcher.Invoke(new Action<TControl, string, TProperty>(UpdateProperty), control, propertyName, value);
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
                (stat, msg) = this.Check();
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

            public (bool, string) Check()
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

        public class OpenAI
        {
            private readonly string host;
            private readonly string token;
            private readonly OpenAIService openai_service;
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
                    Console.WriteLine($"OpenAI - 发生异常: {e.Data}");
                    throw;
                }
                return true;
            }
        }
    }
}
