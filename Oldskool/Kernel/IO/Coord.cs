using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflection.Kernel.IO
{
    public struct Coord
    {
        public short X;
        public short Y;

        public Coord(int x, int y)
        {
            X = (short)x;
            Y = (short)y;
        }
    }

}
