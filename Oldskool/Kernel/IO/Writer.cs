using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Reflection.Kernel.IO
{
    public class Writer
    {
        private static TextWriter writer = Console.Out;

        public static void Write(string data, params object[] args)
        {
            writer.Write(data, args);
        }

        public static void Write(object obj)
        {
            writer.Write(obj);
        }

        public static void WriteLine(string data, params object[] args)
        {
            writer.Write(data, args);
            writer.Write(writer.NewLine);
        }

        public static void WriteLine(object obj)
        {
            writer.Write(obj);
            writer.Write(writer.NewLine);
        }
    }
}
