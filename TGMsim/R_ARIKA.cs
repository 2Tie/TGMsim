using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class R_ARIKA : Rotation
    {
        public R_ARIKA()
        {
            type = "ARIKA";
        }

        public override Tetromino rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large)
        {

            Tetromino testTet = tet.clone();

            int yOffset = 0;

            int bigOffset = 1;
            if (large)
                bigOffset = 2;

            int lowY = 22;
            int big = 2;
            if (large)
                big = 1;
            for (int q = 0; q < tet.bits.Count; q++)
            {
                if (tet.bits[q].y < lowY)
                    lowY = tet.bits[q].y;
            }

            switch (tet.id)
            {
                case 1: //I has two rotation states; KICKS ONLY IN NEW RULES
                    //check current rotation
                    //check positions based on p and rotation, if abovescreen or offscreen to the sides then add an offset
                    switch (tet.rotation)
                    {
                        case 0:
                            yOffset = 1 - bigOffset;

                            testTet.bits[0].move(2, 2 + yOffset);
                            testTet.bits[1].move(1, 1 + yOffset);
                            testTet.bits[2].move(0, yOffset);
                            testTet.bits[3].move(-1, -1 + yOffset);

                            testTet.rotation = 1;

                            if (rule > 3)
                                for (int i = 0; i < 2; i++)
                                {
                                    if (!checkUnder(testTet, p, gameField, large))
                                    {
                                        if (!tet.floored)
                                            return tet;
                                        testTet.move(0, -1);
                                    }
                                }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;

                        case 1:
                            yOffset = bigOffset - 1;

                            testTet.bits[0].move(-2, -2 + yOffset);
                            testTet.bits[1].move(-1, -1 + yOffset);
                            testTet.bits[2].move(0, yOffset);
                            testTet.bits[3].move(1, 1 + yOffset);

                            testTet.rotation = 0;

                            if (rule > 3)
                                for (int i = 0; i < 3; i++)
                                {
                                    if (!checkUnder(testTet, p, gameField, large))
                                    {
                                        if (i != 2)
                                            testTet.move(1, 0);
                                        else
                                            testTet.move(-3, 0);
                                    }
                                }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                    }
                    break;
                case 2: //T 
                    switch (p)
                    {
                        case 1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(1, -1 + yOffset);

                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 1:
                                    //UPKICKS
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 0 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 2 + yOffset);
                                    testTet.bits[3].move(-1, 0 + yOffset);

                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large) && rule > 3) //upkick
                                        testTet.move(1, -1 * bigOffset);
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(-1, 0 + yOffset);

                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 3:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, -1 + yOffset);
                                    testTet.bits[3].move(1, 1 + yOffset);

                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(-1, -1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 1:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, 1 + yOffset);
                                    testTet.bits[3].move(-1, 1 + yOffset);
                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(1, 0 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 3:
                                    //UPKICKS
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 2 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 0 + yOffset);
                                    testTet.bits[3].move(1, 0 + yOffset);
                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large) && rule > 3) //upkick
                                        testTet.move(1, -1 * bigOffset);
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                    }
                    break;
                case 3: //L
                    switch (p)
                    {
                        case 1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(2, 0 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 1:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 0 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 2 + yOffset);
                                    testTet.bits[3].move(0, -1 + yOffset);
                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(-2, -1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 3:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, -1 + yOffset);
                                    testTet.bits[3].move(0, 2 + yOffset);
                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(0, -2 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 1:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, 1 + yOffset);
                                    testTet.bits[3].move(-2, 0 + yOffset);
                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(0, 1 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                                case 3:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 2 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 0 + yOffset);
                                    testTet.bits[3].move(2, 1 + yOffset);
                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                    }
                    break;
                case 4: //J
                    switch (p)
                    {
                        case 1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(0, -2 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 1:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 0 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 2 + yOffset);
                                    testTet.bits[3].move(-2, 1 + yOffset);
                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(0, 1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 3:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, -1 + yOffset);
                                    testTet.bits[3].move(2, 0 + yOffset);
                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(-2, 0 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                                case 1:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(-1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(1, 1 + yOffset);
                                    testTet.bits[3].move(0, 2 + yOffset);
                                    testTet.rotation = 0;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(2, -1 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                                case 3:
                                    yOffset = bigOffset - 1;

                                    testTet.bits[0].move(1, 2 + yOffset);
                                    testTet.bits[1].move(0, 1 + yOffset);
                                    testTet.bits[2].move(-1, 0 + yOffset);
                                    testTet.bits[3].move(0, -1 + yOffset);
                                    testTet.rotation = 2;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, p, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                    }
                    break;
                case 5: //S has two rotation states
                    switch (tet.rotation)
                    {
                        case 0:
                            yOffset = 1 - bigOffset;

                            testTet.bits[0].move(1, 0 + yOffset);
                            testTet.bits[1].move(0, -1 + yOffset);
                            testTet.bits[2].move(-1, 0 + yOffset);
                            testTet.bits[3].move(-2, -1 + yOffset);
                            testTet.rotation = 1;

                            for (int i = 1; i < 3; i++)
                            {
                                if (!checkUnder(testTet, p, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                        case 1:
                            yOffset = bigOffset - 1;

                            testTet.bits[0].move(-1, 0 + yOffset);
                            testTet.bits[1].move(0, 1 + yOffset);
                            testTet.bits[2].move(1, 0 + yOffset);
                            testTet.bits[3].move(2, 1 + yOffset);
                            testTet.rotation = 0;

                            for (int i = 1; i < 3; i++)
                            {
                                if (!checkUnder(testTet, p, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                    }
                    break;
                case 6: //Z has two rotation states
                    switch (tet.rotation)
                    {
                        case 0:
                            yOffset = 1 - bigOffset;

                            testTet.bits[0].move(2, -1 + yOffset);
                            testTet.bits[1].move(1, 0 + yOffset);
                            testTet.bits[2].move(0, -1 + yOffset);
                            testTet.bits[3].move(-1, 0 + yOffset);
                            testTet.rotation = 1;

                            for (int i = 1; i < 3; i++)
                            {
                                if (!checkUnder(testTet, p, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                        case 1:
                            yOffset = bigOffset - 1;

                            testTet.bits[0].move(-2, 1 + yOffset);
                            testTet.bits[1].move(-1, 0 + yOffset);
                            testTet.bits[2].move(0, 1 + yOffset);
                            testTet.bits[3].move(1, 0 + yOffset);
                            testTet.rotation = 0;

                            for (int i = 1; i < 3; i++)
                            {
                                if (!checkUnder(testTet, p, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, p, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                    }
                    break;
                case 7: //O has one. do nothing.
                    break;
            }
            return tet;
        }

        public override bool checkUnder(Tetromino tet, int p, List<List<int>> gameField, bool large)
        {
            bool empty = true;

            int lowY = 22;
            int big = 2;
            if (large)
                big = 1;
            for (int q = 0; q < tet.bits.Count; q++)
            {
                if (tet.bits[q].y < lowY)
                    lowY = tet.bits[q].y;
            }

            for (int i = 0; i < 4; i++)
            {
                int tetX, tetY;
                tetX = tet.bits[i].x * (2 / big) - (4 % (6 - big));
                tetY = tet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1));
                //check OoB
                if (tetY > 21)
                    return false;

                if (tetX > 9)
                    return false;
                if (tetX < 0)
                    return false;


                //test the cells
                if (gameField[tetX][tetY] != 0)
                    empty = false;
                if (large)
                {
                    if (gameField[tetX + 1][tetY] != 0)
                        empty = false;
                    if (gameField[tetX][tetY + 1] != 0)
                        empty = false;
                    if (gameField[tetX + 1][tetY + 1] != 0)
                        empty = false;
                }
            }

            return empty;
        }
    }
}
