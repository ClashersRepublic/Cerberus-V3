using System;
using System.Runtime.InteropServices;

namespace Magic.ClashOfClans.Extensions
{
    internal enum Menu
    {
        MF_BYCOMMAND = 0x00000000,
        SC_MAXIMIZE = 0xF030,
        SC_SIZE = 0xF000,
        SC_CLOSE = 0xF060,
        SC_MINIMIZE = 0xF020
    }

    internal class NativeCalls
    {
        // IsDebuggerPresent()
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = false)]
        public static extern bool IsDebuggerPresent();

        // GetConsoleWindow()
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        // ShowWindow()
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // DeleteWindow()
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        // GetSystemMenu()
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        ///     Disables console resizing
        /// </summary>
        public static void DisableMenus()
        {
            var handle = GetConsoleWindow();
            var sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
                DeleteMenu(sysMenu, (int)Menu.SC_CLOSE, (int)Menu.MF_BYCOMMAND);
        }
    }
}
