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

            int c = checkUnder(testTet, gameField, large);
            if (c == 0)
                return testTet;
            if(!spawn)
            {
                if (c % 4 == 2)
                    return tet; //centre column rule
                if (tet.kicked > 0)
                    testTet.groundTimer = 1;
                if ((tet.id == Tetromino.Piece.I || tet.id == Tetromino.Piece.T) && (c > 8))//floorkick stuff
                {
                    testTet.move(0, 1 * bigOffset);
                    testTet.kicked = 1;
                    c = checkUnder(testTet, gameField, large);
                    if (c == 0)
                        return testTet;
                    if (c > 12)
                    {
                        testTet.move(0, 1 * bigOffset);
                        if (checkUnder(testTet, gameField, large) == 0)
                            return testTet;
                    }
                    return tet;
                }

                testTet.move(1 * bigOffset, 0);
                if (checkUnder(testTet, gameField, large) == 0)
                    return testTet;
                testTet.move(-2 * bigOffset, 0);
                if (checkUnder(testTet, gameField, large) == 0)
                    return testTet;
                if (tet.id == Tetromino.Piece.I) //final I wallkick
                {
                    testTet.move(3 * bigOffset, 0);
                    if (checkUnder(testTet, gameField, large) == 0)
                        return testTet;
                }
            }
            return tet;
        }
    }
}