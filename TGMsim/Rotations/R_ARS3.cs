using System.Collections.Generic;

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

            if (tet.id == Tetromino.Piece.I)//test I floorkicks
            {
                if (tet.rotation % 2 == 0 && tet.floored && !checkUnder(testTet, gameField, large, spawn))
                {
                    testTet.move(0, bigOffset);
                    testTet.kicked++;
                    if (!checkUnder(testTet, gameField, large, spawn))
                    {
                        testTet.move(0, bigOffset);
                        if (!checkUnder(testTet, gameField, large, spawn))
                        {
                            testTet.move(0, -2 * bigOffset);
                            testTet.kicked = 0;
                        }
                    }
                }
                if (tet.rotation % 2 == 1 && tet.kicked > 0)
                    testTet.groundTimer = 1;
            }

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
                if (tet.id == Tetromino.Piece.I && tet.rotation%2 == 1 && !checkUnder(testTet, gameField, large, spawn))//final I wallkick
                    testTet.move(3 * bigOffset, 0);
            }

            

            if (tet.id == Tetromino.Piece.T && tet.floored && !checkUnder(testTet, gameField, large, spawn) && (tet.rotation + p + 4) % 4 == 2)//test T floorkicks
            {
                testTet.move(bigOffset, bigOffset);
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

            if ((int)tet.id < 4 || tet.id == Tetromino.Piece.O)//I, S, Z, and O have no kick restrictions
                return true;


            //universal center-testing (two up from bottom center will never kick, even when considering the exception below)
            if (tet.bits[1].x > -1 && tet.bits[1].y + ((1 + (tet.rotation / 2)) * (3 - big)) < 22)
                if (gameField[tet.bits[1].x][tet.bits[1].y + ((1 + (tet.rotation / 2)) * (3 - big))] != 0)
                    return false;

            if (tet.id == Tetromino.Piece.J || tet.id == Tetromino.Piece.L)
            {
                if (tet.rotation % 2 == 0)
                {
                    if (gameField[tet.bits[1].x][tet.bits[1].y - 1 + (tet.rotation / 2 * (3 - big)) * 2] != 0)//if hooked
                    {
                        if (gameField[tet.bits[1].x + (((int)tet.id - 4) * -2) + 1][tet.bits[1].y + (tet.rotation / 2) + 1] != 0 && tet.rotation - (((((int)tet.id - 4) * 2) - 1) * p) == 1)//if exception blocked
                            return true;
                        return false;
                    }
                }
            }
            return true;
        }
        
    }
}