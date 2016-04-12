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

        public virtual Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large)
        {
            return new Tetromino(0);
        }
        public virtual bool checkUnder(Tetromino tet, int p, List<List<int>> gameField, bool large)
        {
            return false;
        }
    }
}
