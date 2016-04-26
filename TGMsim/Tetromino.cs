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
        public bool bone = false;
        public bool swapped = false;
        public bool floored = false;
        public int groundTimer = 0;

        public List<BlockBit> bits = new List<BlockBit>(); //first four will always be the "small" version
        

        public Tetromino(int i)
        {
            id = i;
            switch (i)
            {
                case 1://I
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(6, 2));
                    break;
                case 2://T
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(4, 3));
                    break;
                case 3://L
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(3, 3));
                    break;
                case 4://J
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(5, 3));
                    break;
                case 5://S
                    bits.Add(new BlockBit(3, 3));
                    bits.Add(new BlockBit(4, 3));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    break;
                case 6://Z
                    bits.Add(new BlockBit(3, 2));
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(4, 3));
                    bits.Add(new BlockBit(5, 3));
                    break;
                case 7://O
                    bits.Add(new BlockBit(4, 2));
                    bits.Add(new BlockBit(5, 2));
                    bits.Add(new BlockBit(5, 3));
                    bits.Add(new BlockBit(4, 3));
                    break;
            }
        }

        public void move(int x, int y)
        {
            for(int i = 0; i < 4; i++)
            {
                bits[i].move(x, y);
            }
        }

        public Tetromino clone()
        {
            Tetromino newTet = new Tetromino(id);
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

                
                newTet.groundTimer = groundTimer;
            }
            return newTet;
        }
    }
}
