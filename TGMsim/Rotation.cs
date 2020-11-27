using System.Collections.Generic;

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

        public int checkUnder(Tetromino tet, List<List<int>> gameField, bool large)
        {

            int big = 2;
            if (large)
                big = 1;

            for (int i = 1; i < 17; i++)
            {
                int tetX, tetY;
                int p = -1;
                for (int j = 0; j < 4; j++)
                {
                    if ((tet.bits[j].x - tet.x + 4 * (tet.y - tet.bits[j].y)) / (3 - big) + 1 == i)
                        p = j;
                }
                if (p == -1)
                    continue; 
                tetX = tet.bits[p].x;
                tetY = tet.bits[p].y;
                //check OoB
                if (tetY + 2-big > gameField[0].Count - 1)
                    return 2;
                if (tetY < 0)
                    return 8;

                if (tetX + 2-big > 9)
                    return 3;
                if (tetX < 0)
                    return 1;


                //test the cells
                if (gameField[tetX][tetY] != 0)
                    return i; //should return the overlapped cell
                if (large)
                {
                    if (gameField[tetX + 1][tetY] != 0)
                        return i; //yknow what big mode's just broken again for now.
                    if (gameField[tetX][tetY - 1] != 0)
                        return i;
                    if (gameField[tetX + 1][tetY - 1] != 0)
                        return i;
                }
            }

            return 0;
        }
    }
}
