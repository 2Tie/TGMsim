using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class R_ARS3 : Rotation
    {
        public R_ARS3()
        {
            type = "ARS 3";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {

            Tetromino testTet = tet.clone((tet.rotation + p + 4)%4);

            int bigOffset = 1;
            if (large)
                bigOffset = 2;

            int lowY = 22;
            for (int q = 0; q < tet.bits.Count; q++)
            {
                if (tet.bits[q].y < lowY)
                    lowY = tet.bits[q].y;
            }

            //testTet = new Tetromino(tet.id, (tet.rotation + p + 4) % 4, tet.x - 3, tet.y - 20, tet.big);
            testTet.groundTimer = tet.groundTimer;
            testTet.bone = tet.bone;

            if (testRestrict(tet, p, gameField, large))//test kick restrictions
            {
                for (int i = 1; i < 3; i++)//test wallkicks in order, stop at first rotation that works
                {
                    if (!checkUnder(testTet, gameField, large, spawn))
                    {
                        if (i != 2)
                            testTet.move(1*bigOffset, 0);
                        else
                            testTet.move(-2*bigOffset, 0);
                    }
                }
                if (tet.id == 1 && tet.rotation%2 == 1 && !checkUnder(testTet, gameField, large, spawn))//final I wallkick
                    testTet.move(3 * bigOffset, 0);
            }

            if(tet.id == 1 && tet.floored && !checkUnder(testTet, gameField, large, spawn))//test I floorkicks
            {
                testTet.move(1 * bigOffset, bigOffset);
                if (!checkUnder(testTet, gameField, large, spawn))
                    testTet.move(0, bigOffset);
            }

            if (tet.id == 2 && tet.floored && !checkUnder(testTet, gameField, large, spawn))//test T floorkicks
            {
                testTet.move(1 * bigOffset, -1 * bigOffset);
            }

            if (!checkUnder(testTet, gameField, large, spawn)) //did any kick of the rotation work?
                return tet;

            return testTet;
        }

        private bool testRestrict(Tetromino tet, int p, List<List<int>> gameField, bool large)
        {
            int lowY = 22;
            int big = 2;
            if (large)
                big = 1;
            for (int q = 0; q < tet.bits.Count; q++)
            {
                if (tet.bits[q].y < lowY)
                    lowY = tet.bits[q].y;
            }

            if (tet.id == 1 || tet.id > 4)//I, S, Z, and O have no kick restrictions
                return true;


            //universal center-testing (two up from bottom center will never kick)
            if (tet.id < 5)
                if (tet.bits[1].x > -1 && tet.bits[1].y - ((1 + (tet.rotation / 2)) * (3 - big)) > -1)
                    if (gameField[tet.bits[1].x][tet.bits[1].y + ((1 + (tet.rotation / 2)) * (3 - big))] != 0)
                        return false;

            for (int i = 0; i < 4; i++)
            {
                int tetX, tetY;
                tetX = tet.bits[i].x * (2 / big) - (4 % (6 - big));
                tetY = tet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1));

                if (i == 1)
                    switch (tet.id)
                    {
                        case 3:
                        case 4:
                            //test other center
                            if (tet.rotation % 2 == 0)
                                if (gameField[tetX][tetY - (1 * (((tet.rotation + 2) % 4) - 1)) * (2 / big)] != 0)
                                {
                                    if (gameField[tetX + (((tet.id - 3) * 2) - 1)][tetY + ((tet.rotation / 2) + 1)] != 0 && tet.rotation + ((((tet.id - 3) * 2) - 1) * p) == 1)
                                        return true;
                                    return false;
                                }
                            continue;
                    }
            }
            return true;
        }
        
    }
}