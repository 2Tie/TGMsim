using System.Collections.Generic;

namespace TGMsim
{
    class R_SEGA : Rotation
    {
        public R_SEGA()
        {
            type = "SEGA";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {
            Tetromino testTet = tet.clone((tet.rotation + 1) % 4);

            if (!checkUnder(testTet, gameField, large)) //did the rotation work?
                return tet;

            return testTet;
        }
    }
}
