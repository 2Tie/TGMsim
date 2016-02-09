using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    public class BlockBit
    {
        public int x, y;
        public BlockBit(int newX, int newY)
        {
            x = newX;
            y = newY;
        }
        public void move(int offX, int offY)
        {
            x += offX;
            y += offY;
        }
    }
}
