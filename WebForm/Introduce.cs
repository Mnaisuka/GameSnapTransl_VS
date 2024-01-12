using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WebForm
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 导入和定义 Windows SDK 中关于全局热键函数及常量的静态类。
        /// </summary>
        internal static class NativeMethods
        {
            /// <summary>
            /// 定义使用 <see cref="RegisterHotKey(IntPtr, int, KeyModifiers, VirtualKeys)"/> 注册的热键触发的消息的消息号。
            /// </summary>
            public const int WM_HOTKEY = 0X0312;

            /// <summary>
            /// 注册系统全局热键。
            /// </summary>
            /// <param name="hWnd">关联的窗口句柄。如果此值为零，则与当前县城关联， WM_HOTKEY 消息会放到当前县城的消息队列。</param>
            /// <param name="id">用来标识热键的标识符。</param>
            /// <param name="fsModifiers">修饰键和选项的值。</param>
            /// <param name="vk">虚拟键代码。</param>
            /// <returns>成功返回 true， 失败返回 false。如需错误信息可调用 <see cref="Marshal.GetLastWin32Error"/> 方法。</returns>
            /// <seealso cref="UnregisterHotKey(IntPtr, int)"/>
            /// <remarks>
            /// 当键被按下时，系统会寻找匹配的已注册的全局热键，如果该全局热键与一个窗体关联，则 <see cref="WM_HOTKEY"/> 消息会放到该窗体的消息队列，若未与一个窗体关联，则将 <see cref="WM_HOTKEY"/> 消息发送到对应的线程消息队列。
            /// 该函数无法将全局热键与另一个线程创建的窗体关联。
            /// 如果将要注册的全局热键已被注册，调用该函数将失败。
            /// 如果已注册的全局热键具有与将要注册的全局热键相同的窗体句柄 （hWnd） 和标识符 （id）， 则新注册的全局热键与旧全局热键一起维护。 如果就全局热键需要被新全局热键替换，应该先显示地调用 <see cref="UnregisterHotKey(IntPtr, int)"/> 函数以撤销注册的全局热键， 接着调用该函数注册新的全局热键。
            /// 在 Windows Server 2003 上： 新全局热键与以注册的全局热键具有相同的窗体句柄 （hWnd） 和标识符 （id） 时， 旧全局热键将被新的全局热键替换。
            /// F12 应当保留给调试器使用。
            /// 应用程序必须指定 0x0000 到 0xBFFF之间的值， 共享类库必须指定 0xC000 到 0xFFFF 之间的值给 id 参数。
            /// </remarks>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

            /// <summary>
            /// 撤销已经注册的系统全局热键。
            /// </summary>
            /// <param name="hWnd">关联的窗口句柄。如果没有与任何窗口关联，则必须为零。</param>
            /// <param name="id">需要撤销的热键的标识符。</param>
            /// <returns>成功返回 true， 失败返回 false。如需错误信息可调用 <see cref="Marshal.GetLastWin32Error"/>方法。</returns>
            /// <seealso cref="RegisterHotKey(IntPtr, int, KeyModifiers, VirtualKeys)"/>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        }

        /// <summary>
        /// 为热键提供修饰键选项的枚举。
        /// </summary>
        [Flags]
        public enum KeyModifiers
        {
            /// <summary>
            /// 没有修饰键。
            /// </summary>
            None = 0X00,

            /// <summary>
            /// ALT 键。
            /// </summary>
            Alt = 0X01,

            /// <summary>
            /// CTRL 键。
            /// </summary>
            Control = 0X02,

            /// <summary>
            /// SHIFT 键。
            /// </summary>
            Shift = 0X04,

            /// <summary>
            /// Windows 徽标键。
            /// </summary>
            Windows = 0X08,

            /// <summary>
            /// 热键按下时禁止重复发出消息。
            /// </summary>
            NoRepeat = 0X4000
        }
    }
}
