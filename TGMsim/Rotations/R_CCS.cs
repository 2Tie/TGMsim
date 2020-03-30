using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Rotations
{
    class R_CCS : R_ARS1
    {
        public R_CCS()
        {
            type = "CCS";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large, bool spawn)
        {
            Tetromino testTet = tet.clone((tet.rotation + p + 4) % 4);

            if (!spawn) //kicks only work outside of IRS
            {
                //TODO: a big rework of this, possibly all of the rotation systems too.
                //check restrictions
                if (testRestrict(tet, p, gameField, large) == false)
                    //can't rotate here, so return this
                    return tet;

                //we're allowed to rotate here.
                //now we check if the new place is free
                if (!checkUnder(testTet, gameField, large))
                {
                    //if not, determine which way to kick. 
                    //odd rotation kicks left for Z and right for S
                    if (testTet.id == Tetromino.Piece.Z && (testTet.rotation % 2) == 1)
                        testTet.move(-1, 0);
                    else if (testTet.id == Tetromino.Piece.S && (testTet.rotation % 2 == 1))
                        testTet.move(1, 0);
                    //J with R0 prefers left
                    else if (testTet.id == Tetromino.Piece.J && (testTet.rotation == 0))
                    {
                        testTet.move(-1, 0);
                        if (checkUnder(testTet, gameField, large))
                            return testTet;
                        else
                            testTet.move(2, 0);
                    }
                    //J with R2 kicks right if top pip hit, L with R2 kicks left if top pip hit
                    else if (testTet.id == Tetromino.Piece.J && testTet.rotation == 2 && gameField[testTet.x][testTet.y + 1] != 0)
                        testTet.move(1, 0);
                    else if (testTet.id == Tetromino.Piece.L && testTet.rotation == 2 && gameField[testTet.x + 2][testTet.y + 1] != 0)
                        testTet.move(-1, 0);
                    //other J, L, S, and Z kick normally
                    else
                    {
                        testTet.move(1, 0);
                        if (checkUnder(testTet, gameField, large))
                            return testTet;
                        else
                            testTet.move(-2, 0);
                    }
                }
            }
            //if the final spot don't work, return the old tet
            if (!checkUnder(testTet, gameField, large))
                return tet;
            //otherwise return our new one
            return testTet;
        }
    }
}
