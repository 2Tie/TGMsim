using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class Rotation
    {
        public string type;

        public Rotation()
        {

        }

        public virtual Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {
            return new Tetromino(0, large);
        }

        public bool checkUnder(Tetromino tet, List<List<int>> gameField, bool large, bool spawn)
        {
            if (spawn)
                return true;

            int lowY = 22;
            int big = 2;
            if (large)
                big = 1;
            for (int q = 0; q < tet.bits.Count; q++)
            {
                if (tet.bits[q].y < lowY)
                    lowY = tet.bits[q].y;
            }

            for (int i = 0; i < 4; i++)
            {
                int tetX, tetY;
                tetX = tet.bits[i].x;// * (2 / big) - (4 % (6 - big));
                tetY = tet.bits[i].y;// * (2 / big) - (lowY * ((2 / big) - 1));
                //check OoB
                if (tetY + 2-big > gameField[0].Count - 1)
                    return false;
                if (tetY < 0)
                    return false;

                if (tetX + 2-big > 9)
                    return false;
                if (tetX < 0)
                    return false;


                //test the cells
                if (gameField[tetX][tetY] != 0)
                    return false;
                if (large)
                {
                    if (gameField[tetX + 1][tetY] != 0)
                        return false;
                    if (gameField[tetX][tetY + 1] != 0)
                        return false;
                    if (gameField[tetX + 1][tetY + 1] != 0)
                        return false;
                }
            }

            return true;
        }
    }
}
