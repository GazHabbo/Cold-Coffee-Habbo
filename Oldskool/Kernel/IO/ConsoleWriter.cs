using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace Reflection.Kernel.IO
{
    public class ConsoleWriter
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int GetConsoleOutputCP();

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern static IntPtr ConsoleOutput();

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTitle", SetLastError = true, CharSet = CharSet.Unicode)]
        extern static bool SetConsoleTitle(string title);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTextAttribute", SetLastError = true, CharSet = CharSet.Unicode)]
        extern static bool SetConsoleTextAttribute(IntPtr handle, short attribute);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo", SetLastError = true, CharSet = CharSet.Unicode)]
        extern static bool GetConsoleScreenBufferInfo(IntPtr handle, out ConsoleScreenBufferInfo info);

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Unicode)]
        extern static IntPtr GetStdHandle(Handles handle);

        public ConsoleWriter()
        {
        }

        internal void SetTitle(string data)
        {
            SetConsoleTitle(data);
        }

        internal void PrintLine(string Type, string Line)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Writer.Write(" <{0}> ", Type);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Line);
        }

        internal void PrintLine(ConsoleColor Color, string Type, string Line)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Writer.Write(" <{0}> ", Type);
            Console.ForegroundColor = Color;
            Writer.WriteLine(Line);
        }

        internal void PrintLine(string Line)
        {
            PrintLine("GLOBAL", Line);
        }
    }
}
