using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICheckers
{
    public enum CheckerColour
    {
        Empty,
        Red,
        Black
    }

    class Square
    {
        public CheckerColour Colour = CheckerColour.Empty;
        public bool King = false;
    }
}
