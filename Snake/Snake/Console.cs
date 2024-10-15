using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class ConsoleHelper
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private const int ScreenWidth = 1920; // Ширина экрана
        private const int ScreenHeight = 1080; // Высота экрана

        public void CenterConsoleWindow()
        {
            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow == IntPtr.Zero) return;

            GetWindowRect(consoleWindow, out RECT rect);
            int windowWidth = rect.Right - rect.Left;
            int windowHeight = rect.Bottom - rect.Top;

            int posX = (ScreenWidth - windowWidth) / 2;
            int posY = (ScreenHeight - windowHeight) / 2;

            MoveWindow(consoleWindow, posX, posY, windowWidth, windowHeight, true);
        }
    }
}
