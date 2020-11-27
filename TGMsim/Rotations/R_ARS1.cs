using System.Collections.Generic;

namespace TGMsim
{
    class R_ARS1 : Rotation
    {
        public R_ARS1()
        {
            type = "ARS 1+2";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {
            Tetromino testTet = tet.clone((tet.rotation + p + 4) % 4);

            int bigOffset = 1;
            if (large)
                bigOffset = 2;
            int c = checkUnder(testTet, gameField, large);
            if (c == 0)
                return testTet;
            if (!spawn)
            {
                if (tet.id == Tetromino.Piece.I || c % 4 == 2) //i rule, centre collison
                    return tet;
                testTet.move(1 * bigOffset, 0);
                if (checkUnder(testTet, gameField, large) == 0)
                    return testTet;
                testTet.move(-2 * bigOffset, 0);
                if (checkUnder(testTet, gameField, large) == 0)
                    return testTet;
            }
            return tet;
        }
        
    }
}