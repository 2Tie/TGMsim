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
        public bool large = false;
        public bool bone = false;
        public bool swapped = false;

        public List<BlockBit> bits = new List<BlockBit>(); //first four will always be the "small" version
        

        public Tetromino(int i, bool big)
        {
            int pips = 4;
            if (big)
                pips = 16;
            for (int j = 0; j < pips; j++)
            {
                //bits.Add(new BlockBit());
            }
            id = i;
            large = big;
            switch (i)
            {
                case 1://I
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(6, 2));
                    if (big)
                    {
                        bits.Add(new BlockBit(3, 3));
                        bits.Add(new BlockBit(4, 3));
                        bits.Add(new BlockBit(5, 3));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(2, 2));
                        bits.Add(new BlockBit(2, 3));
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(7, 3));
                        bits.Add(new BlockBit(8, 2));
                        bits.Add(new BlockBit(8, 3));
                        bits.Add(new BlockBit(9, 2));
                        bits.Add(new BlockBit(9, 3));
                    }
                    break;
                case 2://T
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(4, 3));
                    if (big)
                    {
                        bits.Add(new BlockBit(5, 3));
                        bits.Add(new BlockBit(3, 3));
                        bits.Add(new BlockBit(2, 2));
                        bits.Add(new BlockBit(2, 3));
                        bits.Add(new BlockBit(6, 2));
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(7, 3));
                        bits.Add(new BlockBit(4, 4));
                        bits.Add(new BlockBit(5, 4));
                        bits.Add(new BlockBit(4, 5));
                        bits.Add(new BlockBit(5, 5));
                    }
                    break;
                case 3://L
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(3, 3));
                    if (big)
                    {
                        bits.Add(new BlockBit(4, 3));
                        bits.Add(new BlockBit(5, 3));
                        bits.Add(new BlockBit(2, 2));
                        bits.Add(new BlockBit(2, 3));
                        bits.Add(new BlockBit(6, 2));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(7, 3));
                        bits.Add(new BlockBit(2, 4));
                        bits.Add(new BlockBit(3, 4));
                        bits.Add(new BlockBit(2, 5));
                        bits.Add(new BlockBit(3, 5));
                    }
                    break;
                case 4://J
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(5, 3));
                    if (big)
                    {
                        bits.Add(new BlockBit(4, 3));
                        bits.Add(new BlockBit(3, 3));
                        bits.Add(new BlockBit(2, 2));
                        bits.Add(new BlockBit(2, 3));
                        bits.Add(new BlockBit(6, 2));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(7, 3));
                        bits.Add(new BlockBit(6, 4));
                        bits.Add(new BlockBit(7, 4));
                        bits.Add(new BlockBit(6, 5));
                        bits.Add(new BlockBit(7, 5));
                    }
                    break;
                case 5://S
                    bits.Add(new BlockBit(3, 3));
                    bits.Add(new BlockBit(4, 3));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    if (big)
                    {
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(6, 2));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(5, 3));
                        bits.Add(new BlockBit(4, 4));
                        bits.Add(new BlockBit(5, 4));
                        bits.Add(new BlockBit(2, 4));
                        bits.Add(new BlockBit(3, 4));
                        bits.Add(new BlockBit(4, 5));
                        bits.Add(new BlockBit(5, 5));
                        bits.Add(new BlockBit(2, 5));
                        bits.Add(new BlockBit(3, 5));
                    }
                    break;
                case 6://Z
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(4, 3));
                    bits.Add(new BlockBit(5, 3));
                    if (big)
                    {
                        bits.Add(new BlockBit(3, 3));
                        bits.Add(new BlockBit(2, 2));
                        bits.Add(new BlockBit(2, 3));
                        bits.Add(new BlockBit(5, 2));
                        bits.Add(new BlockBit(4, 4));
                        bits.Add(new BlockBit(5, 4));
                        bits.Add(new BlockBit(6, 4));
                        bits.Add(new BlockBit(7, 4));
                        bits.Add(new BlockBit(4, 5));
                        bits.Add(new BlockBit(5, 5));
                        bits.Add(new BlockBit(6, 5));
                        bits.Add(new BlockBit(7, 5));
                    }
                    break;
                case 7://O
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(5, 3));
                    bits.Add(new BlockBit(4, 3));
                    if (big)
                    {
                        bits.Add(new BlockBit(6, 2));
                        bits.Add(new BlockBit(7, 2));
                        bits.Add(new BlockBit(6, 3));
                        bits.Add(new BlockBit(7, 3));
                        bits.Add(new BlockBit(4, 4));
                        bits.Add(new BlockBit(4, 5));
                        bits.Add(new BlockBit(5, 4));
                        bits.Add(new BlockBit(5, 5));
                        bits.Add(new BlockBit(6, 4));
                        bits.Add(new BlockBit(7, 4));
                        bits.Add(new BlockBit(6, 5));
                        bits.Add(new BlockBit(7, 5));
                    }
                    break;
            }
        }

        public Tetromino clone()
        {
            Tetromino newTet = new Tetromino(id, large);
            for (int i = 0; i < bits.Count; i++ )
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
                if (this.bone)
                    newTet.bone = true;
                if (!this.bone)
                    newTet.bone = false;
            }
            return newTet;
        }
    }
}
