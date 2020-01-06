using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflection.Kernel.IO
{
    public struct ConsoleScreenBufferInfo
    {
        public Coord dwCursorPosition;
        public Int16 dwMaximumWindowSize;
        public Coord dwSize;
        public SmallRect srWindow;
        public Int16 wAttributes;
    }
}
