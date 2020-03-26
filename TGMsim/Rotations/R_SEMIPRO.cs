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
            p = (tet.rotation + p);
            if (p < 0)
                p += 4;
            else if (p > 3)
                p -= 4;
            Tetromino testTet = tet.clone(p);

            if (!checkUnder(testTet, gameField, large, false)) //did the rotation work?
                return tet;

            return testTet;
        }
    }
}
