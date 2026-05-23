using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
namespace CS_GameTime
{
    public class AutoClicker
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);
        //ширина и высота экрана
        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;


        // Структура INPUT для SendInput
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // Типы ввода
        const int INPUT_MOUSE = 0;

        // Флаги мыши
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        // WinAPI функция
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public void MoveCursorToCenter()
        {

            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);
            SetCursorPos(screenWidth / 2, screenHeight / 2);
        }
        public void ClickMouseAtPos(int x, int y)
        {
            SetCursorPos(x, y);
            ClickMouse();
        }
        public void ClickMouse()
        {
            // Клик левой кнопкой
            INPUT[] inputs = new INPUT[2];
            //нажал
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dwFlags = MOUSEEVENTF_LEFTDOWN;

            //отпустил
            inputs[1].type = INPUT_MOUSE;
            inputs[1].mi.dwFlags = MOUSEEVENTF_LEFTUP;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }


        private Thread? _autoClickerThread;
        private bool _isRunning = false;
        public void Start(int? x = null, int? y = null)
        {
            if (_isRunning)
                return;
            if(x != null&& y != null)
            {
                ClickMouseAtPos(x.Value, y.Value);
                Thread.Sleep(100);
                ClickMouseAtPos(2460, 1170);
            }
            else
            {
                MoveCursorToCenter();
            }
            _isRunning = true;
            _autoClickerThread = new Thread(AutoClickerLoop);
            _autoClickerThread.IsBackground = true;
            _autoClickerThread.Start();
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            _autoClickerThread?.Join();
            _autoClickerThread = null;
        }
        private void AutoClickerLoop()
        {
            while (_isRunning)
            {
                ClickMouse();
                Thread.Sleep(10);
            }
        }
    }
}
