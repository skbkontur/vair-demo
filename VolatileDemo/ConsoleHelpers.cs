using System;
using System.Runtime.InteropServices;

namespace VolatileDemo
{
    internal class ConsoleHelpers
    {
        public static void SetupConsole()
        {
            Console.WindowHeight = 10;
            Console.WindowWidth = 30;

            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != INVALID_HANDLE_VALUE)
            {
                var fontInfo = new CONSOLE_FONT_INFOEX();
                fontInfo.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFOEX>();
                GetCurrentConsoleFontEx(hnd, false, ref fontInfo);

                fontInfo.dwFontSize.X = 0;
                fontInfo.dwFontSize.Y = 48;

                SetCurrentConsoleFontEx(hnd, false, ref fontInfo);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFOEX consoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int dwType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetCurrentConsoleFontEx(
            IntPtr hOut,
            bool maximumWindow,
            ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx
        );

        private const int STD_OUTPUT_HANDLE = -11;
        private const int LF_FACESIZE = 32;
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct CONSOLE_FONT_INFOEX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal uint FontFamily;
            internal uint FontWeight;
            internal fixed short FaceName[LF_FACESIZE];
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }
    }
}