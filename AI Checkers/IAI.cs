using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICheckers
{
    interface IAI
    {
        CheckerColour Colour { get; set; }
        Move Process(Square[,] Board);
    }
}
