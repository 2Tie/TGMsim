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
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (!checkUnder(testTet, gameField, large))
                                    {
                                        if (!tet.floored)
                                            return tet;
                                        testTet.move(0, -1 * bigOffset);
                                        testTet.kicked++;
                                    }
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (!checkUnder(testTet, gameField, large))
                                    {
                                        if (i != 2)
                                            testTet.move(1, 0);
                                        else
                                            testTet.move(-3, 0);
                                    }
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                return tet;
                            
                            if (testTet.kicked > 0 && rule > 3) testTet.groundTimer = 1;
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
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(1, -1 + yOffset);

                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    if (testTet.kicked > 0 && rule > 3) testTet.groundTimer = 1;
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large) && rule > 3) //upkick
                                    {
                                        testTet.move(1, -1 * bigOffset);
                                        testTet.kicked++;
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(-1, 0 + yOffset);

                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    if (testTet.kicked > 0 && rule > 3) testTet.groundTimer = 1;
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(-1, -1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(1, 0 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large) && rule > 3) //upkick
                                    {
                                        testTet.move(1, -1 * bigOffset);
                                        testTet.kicked++;
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(2, 0 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(-2, -1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(0, -2 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(0, 1 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, 1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, -1 + yOffset);
                                    testTet.bits[3].move(0, -2 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;

                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, -2 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, 0 + yOffset);
                                    testTet.bits[3].move(0, 1 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(1, -1 + yOffset);
                                    testTet.bits[1].move(0, 0 + yOffset);
                                    testTet.bits[2].move(-1, 1 + yOffset);
                                    testTet.bits[3].move(-2, 0 + yOffset);
                                    testTet.rotation = 3;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                        return tet;

                                    return testTet;
                                case 2:
                                    //SPECIAL RESTRICTIONS
                                    if (!testRestrict(tet, p, gameField, large))
                                        return tet;

                                    yOffset = 1 - bigOffset;

                                    testTet.bits[0].move(-1, 0 + yOffset);
                                    testTet.bits[1].move(0, -1 + yOffset);
                                    testTet.bits[2].move(1, -2 + yOffset);
                                    testTet.bits[3].move(2, -1 + yOffset);
                                    testTet.rotation = 1;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                        if (!checkUnder(testTet, gameField, large))
                                        {
                                            if (i != 2)
                                                testTet.move(1, 0);
                                            else
                                                testTet.move(-2, 0);
                                        }
                                    }
                                    if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                if (!checkUnder(testTet, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                if (!checkUnder(testTet, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                if (!checkUnder(testTet, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
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
                                if (!checkUnder(testTet, gameField, large))
                                {
                                    if (i != 2)
                                        testTet.move(1, 0);
                                    else
                                        testTet.move(-2, 0);
                                }
                            }
                            if (!checkUnder(testTet, gameField, large)) //will the rotation work?
                                return tet;

                            return testTet;
                    }
                    break;
                case 7: //O has one. do nothing.
                    break;
            }
            return tet;
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


            //universal center-testing
            if (gameField[tet.bits[1].x * (2 / big) - (4 % (6 - big))][(tet.bits[1].y * (2 / big) - (lowY * ((2 / big) - 1))) - ((1 + tet.rotation / 2) * (2 / big))] != 0)
                return false;

            for (int i = 0; i < 4; i++)
            {
                int tetX, tetY;
                tetX = tet.bits[i].x * (2 / big) - (4 % (6 - big));
                tetY = tet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1));

                



                switch(tet.id)
                {
                    case 3:
                    case 4:
                        //test other center
                        if (i == 1)
                            if (gameField[tetX][tetY + (1 * (((tet.rotation + 2) % 4) - 1)) * (2/big)] != 0)
                            {
                                if (gameField[tetX + (((tet.id - 3) * 2) - 1)][tetY - ((tet.rotation / 2) + 1)] != 0)
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
