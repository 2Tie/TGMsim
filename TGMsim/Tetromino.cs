using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    public class Tetromino
    {
        enum piece { I = 1, T, L, J, S, Z, O };

        public int id = 0;
        public int rotation = 0;
        public int x = 0;
        public int y = 0;
        public int kicked = 0;

        public List<BlockBit> bits = new List<BlockBit>();
        

        public Tetromino(int i)
        {
            for (int j = 0; j < 4; j++ )
            {
                bits.Add(new BlockBit());
            }
            id = i;
            switch (i)
            {
                case 1:
                    bits[0].x = 3;
                    bits[0].y = 1;
                    bits[1].x = 4;
                    bits[1].y = 1;
                    bits[2].x = 5;
                    bits[2].y = 1;
                    bits[3].x = 6;
                    bits[3].y = 1;
                    break;
                case 2:
                    bits[0].x = 3;
                    bits[0].y = 1;
                    bits[1].x = 4;
                    bits[1].y = 1;
                    bits[2].x = 5;
                    bits[2].y = 1;
                    bits[3].x = 4;
                    bits[3].y = 2;
                    break;
                case 3:
                    bits[0].x = 3;
                    bits[0].y = 1;
                    bits[1].x = 4;
                    bits[1].y = 1;
                    bits[2].x = 5;
                    bits[2].y = 1;
                    bits[3].x = 3;
                    bits[3].y = 2;
                    break;
                case 4:
                    bits[0].x = 3;
                    bits[0].y = 1;
                    bits[1].x = 4;
                    bits[1].y = 1;
                    bits[2].x = 5;
                    bits[2].y = 1;
                    bits[3].x = 5;
                    bits[3].y = 2;
                    break;
                case 5:
                    bits[0].x = 3;
                    bits[0].y = 2;
                    bits[1].x = 4;
                    bits[1].y = 2;
                    bits[2].x = 4;
                    bits[2].y = 1;
                    bits[3].x = 5;
                    bits[3].y = 1;
                    break;
                case 6:
                    bits[0].x = 3;
                    bits[0].y = 1;
                    bits[1].x = 4;
                    bits[1].y = 1;
                    bits[2].x = 4;
                    bits[2].y = 2;
                    bits[3].x = 5;
                    bits[3].y = 2;
                    break;
                case 7:
                    bits[0].x = 4;
                    bits[0].y = 1;
                    bits[1].x = 5;
                    bits[1].y = 1;
                    bits[2].x = 5;
                    bits[2].y = 2;
                    bits[3].x = 4;
                    bits[3].y = 2;
                    break;
            }
        }

        public Tetromino clone()
        {
            Tetromino newTet = new Tetromino(id);
            for (int i = 0; i < 4; i++ )
            {
                int x, y;
                for (x = 0; x < bits[i].x; x++ )
                {
                    
                }
                for (y = 0; y < bits[i].y; y++ )
                {

                }
                newTet.bits[i].x = x;
                newTet.bits[i].y = y;
            }
            return newTet;
        }
    }
}
