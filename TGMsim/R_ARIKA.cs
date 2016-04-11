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

        public override void rotate(Tetromino tet, int p, List<List<int>> gameField, int rule, bool large)
        {
            int xOffset = 0; //for kicks
            int yOffset = 0;

            int bigOffset = 1;
            if (large)
                bigOffset = 2;

            switch (tet.id)
            {
                case 1: //I has two rotation states; KICKS ONLY IN NEW RULES
                    //check current rotation
                    //check positions based on p and rotation, if abovescreen or offscreen to the sides then add an offset
                    switch (tet.rotation)
                    {
                        case 0:
                            bool turn = false;
                            if (tet.bits[2].y + (2 * bigOffset) > 20 && tet.floored && rule > 3 && tet.kicked == 0)
                            {
                                yOffset = 21 - (tet.bits[2].y + (2 * bigOffset));
                                tet.kicked++;
                                tet.groundTimer = 0;
                                turn = true;
                            }

                            if (tet.bits[2].y - (1 * bigOffset) >= 0 && tet.bits[2].y + (2 * bigOffset) + yOffset <= 21)
                            {


                                if (gameField[tet.bits[2].x][tet.bits[2].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (2 * bigOffset) + yOffset] == 0)
                                {
                                    turn = true;
                                }
                                else if (rule > 3 && tet.floored) //TGM3 kicks
                                {
                                    //set yOffset here
                                    if (gameField[tet.bits[2].x][tet.bits[2].y + yOffset + 1] != 0)
                                        yOffset--;
                                    if (gameField[tet.bits[2].x][tet.bits[2].y + yOffset + 2] != 0)
                                        yOffset--;

                                    if (gameField[tet.bits[2].x][tet.bits[2].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (2 * bigOffset) + yOffset] == 0)
                                    {
                                        turn = true;
                                        tet.kicked++;
                                    }
                                }
                            }

                            if (turn)
                            {
                                tet.bits[0].x += 2;
                                tet.bits[0].y += 2 + yOffset;
                                tet.bits[1].x += 1;
                                tet.bits[1].y += 1 + yOffset;
                                tet.bits[2].y += yOffset;
                                tet.bits[3].x -= 1;
                                tet.bits[3].y += -1 + yOffset;

                                tet.rotation = 1;
                            }
                            break;
                        case 1:
                            turn = false;
                            if (tet.bits[2].x - (2 * bigOffset) >= 0 && tet.bits[2].x + (1 * bigOffset) <= 9)
                            {
                                if (gameField[tet.bits[2].x - (2 * bigOffset)][tet.bits[2].y] == 0 && gameField[tet.bits[2].x - (1 * bigOffset)][tet.bits[2].y] == 0 && gameField[tet.bits[2].x + (1 * bigOffset)][tet.bits[2].y] == 0)
                                {
                                    turn = true;
                                }
                                else if (rule > 3) //tgm3 kicks
                                {
                                    if (gameField[tet.bits[2].x - (1 * bigOffset) + xOffset][tet.bits[2].y] != 0)
                                        xOffset += 1;
                                    if (gameField[tet.bits[2].x - (2 * bigOffset) + xOffset][tet.bits[2].y] != 0)
                                        xOffset += 1;
                                    if (gameField[tet.bits[2].x + (1 * bigOffset) + xOffset][tet.bits[2].y] != 0)
                                        xOffset -= 1;

                                    if (gameField[tet.bits[2].x - (2 * bigOffset) + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[2].x - (1 * bigOffset) + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[2].x + (1 * bigOffset) + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y] == 0)
                                    {
                                        turn = true;
                                    }
                                }
                            }
                            else if (rule > 3) //tgm3 kicks
                            {
                                if (tet.bits[2].x - (2 * bigOffset) <= 0)
                                    xOffset -= tet.bits[2].x - (2 * bigOffset);
                                if (tet.bits[2].x + (1 * bigOffset) >= 9)
                                    xOffset = 9 - (tet.bits[2].x + (1 * bigOffset));
                                turn = true;
                            }

                            if (turn)
                            {
                                tet.bits[0].x += -2 + xOffset;
                                tet.bits[0].y -= 2;
                                tet.bits[1].x += -1 + xOffset;
                                tet.bits[1].y -= 1;
                                tet.bits[2].x += xOffset;
                                tet.bits[3].x += 1 + xOffset;
                                tet.bits[3].y += 1;

                                tet.rotation = 0;
                            }


                            break;
                    }
                    //if spaces are open, rotate and place!
                    //else test kicks

                    break;
                case 2: //T 
                    switch (p)
                    {
                        case 1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    //test for OoB bump
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += 1 + xOffset;
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    //TODO: test for upkick if TGM3
                                    //if (ruleset.gameRules == 4 && ((gameField[tet.bits[0].x + xOffset][tet.bits[0].y - 1] != 0 || gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y] != 0) && tet.kicked == 0))
                                    //{
                                    //    yOffset = -1;
                                    //    tet.kicked = 1;
                                    //}
                                    bool turn = false;
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x - 1 + xOffset][tet.bits[2].y + 2 + yOffset + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x - 1 + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            turn = true;
                                            break;
                                        }
                                    }
                                    if (!turn && rule > 3 && tet.floored == true)//kicks failed, try up one
                                    {
                                        xOffset = 0;
                                        yOffset--;
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x - 1 + xOffset][tet.bits[2].y + 2 + yOffset + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x - 1 + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            turn = true;
                                            tet.kicked++;
                                            tet.groundTimer = 0;
                                        }
                                    }

                                    if (turn)
                                    {
                                        tet.bits[0].x += 1 + xOffset;
                                        tet.bits[0].y += yOffset;
                                        tet.bits[1].x += xOffset;
                                        tet.bits[1].y += 1 + yOffset;
                                        tet.bits[2].x += -1 + xOffset;
                                        tet.bits[2].y += 2 + yOffset;
                                        tet.bits[3].x += -1 + xOffset;
                                        tet.bits[3].y += yOffset;

                                        tet.rotation = 2;
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[3].y - (1 * bigOffset) < 0)
                                    {
                                        yOffset = 1 * bigOffset;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[3].x][tet.bits[3].y - (1 * bigOffset)] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -2 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + yOffset - (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + -1 + xOffset + (2 * (bigOffset - 1))][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += -1 + xOffset;
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + 1 + xOffset - (2 * (bigOffset - 1))][tet.bits[3].y + 1] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += 1 + xOffset;
                                            tet.bits[3].y += 1;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)
                                    {
                                        yOffset = 1 * bigOffset;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + -1 + xOffset + (2 * (bigOffset - 1))][tet.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += -1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += 1 + yOffset;
                                            tet.bits[3].x += -1 + xOffset;
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += -1 + xOffset;
                                            tet.bits[3].y += 1;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[3].y - (1 * bigOffset) < 0)
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[3].x][tet.bits[3].y - (1 * bigOffset)] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x - 1 + xOffset][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y - 2 + yOffset - (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + 1 + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += 1 + xOffset;
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    bool turn = false;
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + 2] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + 1 + xOffset - (2 * (bigOffset - 1))][tet.bits[3].y] == 0)
                                        {
                                            turn = true;
                                            break;
                                        }
                                    }

                                    if (!turn && rule > 3 && tet.floored == true)
                                    {
                                        xOffset = 0;
                                        yOffset--;
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x - 1 + xOffset][tet.bits[2].y + 2 + yOffset + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x - 1 + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            turn = true;
                                            tet.kicked++;
                                            tet.groundTimer = 0;
                                        }
                                    }

                                    if (turn)
                                    {
                                        tet.bits[0].x += 1 + xOffset;
                                        tet.bits[0].y += 2;
                                        tet.bits[1].x += xOffset;
                                        tet.bits[1].y += 1;
                                        tet.bits[2].x += -1 + xOffset;
                                        tet.bits[3].x += 1 + xOffset;

                                        tet.rotation = 2;
                                    }
                                    break;
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
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)//test oob for bump
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + (1 * bigOffset)] != 0)
                                        {
                                            //if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0) //USE FOR OTHER SPIN
                                            //{
                                            break;
                                            //}
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + 1 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + -1 + yOffset] == 0 && gameField[tet.bits[3].x + 2 + xOffset][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += 2 + xOffset;
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[0].x - (1 * bigOffset) + xOffset < 0 || tet.bits[0].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + 2] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + -1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += 2;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += -1;

                                            tet.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[1].y - (2 * bigOffset) < 0)
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (2 * bigOffset)] != 0 || gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -2 + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + -1 + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + -2 + xOffset][tet.bits[3].y + -1 + yOffset - (2 * (bigOffset - 1))] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += -2 + xOffset;
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + xOffset - (2 * (bigOffset - 1))][tet.bits[3].y + 2] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += 2;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)//test oob for bump
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + (1 * bigOffset)] != 0)
                                        {
                                            if (gameField[tet.bits[0].x][tet.bits[0].y - (1 * bigOffset)] == 0)
                                            {
                                                break;
                                            }
                                        }

                                    }

                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + xOffset + (2 * (bigOffset - 1))][tet.bits[3].y + -2 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += -1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += 1 + yOffset;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += -2 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }

                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + 1] == 0 && gameField[tet.bits[3].x + -2 + xOffset][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += -2 + xOffset;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[1].y - (2 * bigOffset) < 0)
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (2 * bigOffset)] != 0 || gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + -1 + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + -2 + yOffset - (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + 1 + yOffset - (2 * (bigOffset - 1))] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += 1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + 2 + (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + (2 * (bigOffset - 1))] == 0 && gameField[tet.bits[3].x + 2 + xOffset - (2 * (bigOffset - 1))][tet.bits[3].y + 1 + (2 * (bigOffset - 1))] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += 2;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[3].x += 2 + xOffset;
                                            tet.bits[3].y += 1;

                                            tet.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
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
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)//test oob for bump
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + (1 * bigOffset)] != 0)
                                        {
                                            if (gameField[tet.bits[0].x][tet.bits[0].y - (1 * bigOffset)] == 0)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - 1 + xOffset < 0 || tet.bits[1].x + 1 + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + 1 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + -1 + yOffset] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + -2 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += -2 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - 1 + xOffset < 0 || tet.bits[1].x + 1 + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + 2] == 0 && gameField[tet.bits[3].x + -2 + xOffset][tet.bits[3].y + 1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += 2;
                                            tet.bits[3].x += -2 + xOffset;
                                            tet.bits[3].y += 1;

                                            tet.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[1].y - (2 + bigOffset) < 0)
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (2 * bigOffset)] != 0 || gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - 1 + xOffset < 0 || tet.bits[1].x + 1 + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -2 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + 1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += 1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + 2 + xOffset + (-2 * (bigOffset - 1))][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += 2 + xOffset;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch (tet.rotation)
                            {
                                case 0:
                                    if (tet.bits[1].y - (1 * bigOffset) < 0)//test oob for bump
                                    {
                                        yOffset = (1 * bigOffset);
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - (1 * bigOffset)] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + (1 * bigOffset)] != 0)
                                        {
                                            // if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0) //for the otehr direction
                                            //{
                                            break;
                                            //}
                                        }
                                    }

                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + -2 + xOffset + (2 * (bigOffset - 1))][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += -1 + yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[2].y += 1 + yOffset;
                                            tet.bits[3].x += -2 + xOffset;
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }

                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + 1] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y + 2] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += 2;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[1].y - 2 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 2] != 0 || gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset][tet.bits[0].y + yOffset + (-1 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + -1 + yOffset + (-1 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + 1 + xOffset][tet.bits[2].y + -2 + yOffset] == 0 && gameField[tet.bits[3].x + 2 + xOffset][tet.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset;
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + xOffset;
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += 2 + xOffset;
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {

                                        xOffset = ((i % 3) - 1) * bigOffset;

                                        if (tet.bits[1].x - (1 * bigOffset) + xOffset < 0 || tet.bits[1].x + (1 * bigOffset) + xOffset > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y + 2 + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + 1 + (1 * (bigOffset - 1))] == 0 && gameField[tet.bits[2].x + -1 + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset - (2 * (bigOffset - 1))][tet.bits[3].y + -1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset;
                                            tet.bits[0].y += 2;
                                            tet.bits[1].x += xOffset;
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset;
                                            tet.bits[3].x += xOffset;
                                            tet.bits[3].y += -1;

                                            tet.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case 5: //S has two rotation states
                    switch (tet.rotation)
                    {
                        case 0:
                            if (tet.bits[0].y - (2 * bigOffset) < 0)
                            {
                                //set an offset
                                yOffset = (1 * bigOffset);
                            }

                            for (int i = 1; i < 4; i++)
                            {

                                xOffset = ((i % 3) - 1) * bigOffset;

                                if (tet.bits[1].x - 1 + xOffset < 0 || tet.bits[1].x + 1 + xOffset > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x + xOffset][tet.bits[0].y - (2 * bigOffset) + yOffset] == 0 && gameField[tet.bits[0].x + xOffset][tet.bits[0].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 1 + xOffset;
                                    tet.bits[0].y += yOffset;
                                    tet.bits[1].x += xOffset;
                                    tet.bits[1].y += yOffset - 1;
                                    tet.bits[2].x += -1 + xOffset;
                                    tet.bits[2].y += yOffset;
                                    tet.bits[3].x += -2 + xOffset;
                                    tet.bits[3].y += (yOffset - 1);

                                    tet.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {

                                xOffset = ((i % 3) - 1) * bigOffset;

                                if (tet.bits[1].x - 1 + xOffset < 0 || tet.bits[1].x + 1 + xOffset > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x - 1 + xOffset][tet.bits[0].y + (3 * (bigOffset - 1))] == 0 && gameField[tet.bits[0].x + xOffset][tet.bits[0].y + (3 * (bigOffset - 1))] == 0 && gameField[tet.bits[0].x + xOffset][tet.bits[0].y - (1 * bigOffset) + (3 * (bigOffset - 1))] == 0 && gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y - (1 * bigOffset) + (3 * (bigOffset - 1))] == 0)
                                {
                                    tet.bits[2].x += 1 + xOffset;
                                    tet.bits[1].y += 1;
                                    tet.bits[1].x += xOffset;
                                    tet.bits[0].x += -1 + xOffset;
                                    tet.bits[3].x += 2 + xOffset;
                                    tet.bits[3].y += 1;

                                    tet.rotation = 0;

                                    break;
                                }
                            }
                            break;
                    }
                    break;
                case 6: //Z has two rotation states
                    switch (tet.rotation)
                    {
                        case 0:
                            if (tet.bits[0].y - (1 * bigOffset) < 0)
                            {
                                //set an offset
                                yOffset = (1 * bigOffset);
                            }
                            for (int i = 1; i < 4; i++)
                            {
                                xOffset = ((i % 3) - 1) * bigOffset;

                                if (tet.bits[2].x - xOffset < 0 || tet.bits[2].x + xOffset > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[1].x + xOffset][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[3].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 2 + xOffset;
                                    tet.bits[0].y += yOffset - 1;
                                    tet.bits[1].x += 1 + xOffset;
                                    tet.bits[1].y += yOffset;
                                    tet.bits[2].x += xOffset;
                                    tet.bits[2].y += yOffset - 1;
                                    tet.bits[3].x += -1 + xOffset;
                                    tet.bits[3].y += yOffset;

                                    tet.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {

                                xOffset = ((i % 3) - 1) * bigOffset;

                                if (tet.bits[2].x - 1 + xOffset < 0 || tet.bits[2].x + 1 + xOffset > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x - 2 + xOffset][tet.bits[0].y + (1 * bigOffset)] == 0 && gameField[tet.bits[2].x + xOffset][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset][tet.bits[3].y] == 0 && gameField[tet.bits[1].x + xOffset][tet.bits[1].y + (1 * bigOffset)] == 0)
                                {
                                    tet.bits[2].x += xOffset;
                                    tet.bits[2].y += 1;
                                    tet.bits[1].x += xOffset - 1;
                                    tet.bits[0].x += (xOffset - 2);
                                    tet.bits[0].y += 1;
                                    tet.bits[3].x += (xOffset + 1);

                                    tet.rotation = 0;

                                    break;
                                }
                            }
                            break;
                    }
                    break;
                case 7: //O has one. do nothing.
                    break;
            }
        }
    }
}
