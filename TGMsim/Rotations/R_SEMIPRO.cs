using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Rotations
{
    class R_SEMIPRO : Rotation
    {
        public R_SEMIPRO()
        {
            type = "SEMIPRO";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {
            Tetromino testTet = tet.clone((tet.rotation + p + 4) % 4);

            if (!checkUnder(testTet, gameField, large)) //did the rotation work?
                return tet;

            return testTet;
        }
    }
}
