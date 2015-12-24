using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace TGMsim
{
    public partial class Form1 : Form
    {

        GameTimer timer = new GameTimer();
        long startTime;
        long interval = (long)TimeSpan.FromSeconds(1.0 / 60).TotalMilliseconds;

        int inputH, inputV, inputStart, inputRot1, inputRot2, inputHold;
        int inputWaitH = 0;
        int inputDelayH = 0, inputDelayV = 0;
        bool inputPressedV = false, inputPressedRot1 = false, inputPressedRot2 = false, inputPressedHold = false;
        int inputPressedStateH = 0;

        Image imgBuffer;
        Graphics graphics, drawBuffer;

        Pen gridPen = new Pen(new SolidBrush(Color.White));

        int menuState = 5; //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings
        bool gameRunning;

        Field field1 = new Field();
        List<Color> tetColors = new List<Color>();
        Tetromino tet1;
        
        
        Rules ruleset = new Rules();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ClientSize = new Size(1280, 780);

            tetColors.Add(Color.Transparent);
            tetColors.Add(Color.Red);
            tetColors.Add(Color.Cyan);
            tetColors.Add(Color.Orange);
            tetColors.Add(Color.Blue);
            tetColors.Add(Color.Purple);
            tetColors.Add(Color.Green);
            tetColors.Add(Color.Yellow);

            imgBuffer = (Image)new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            graphics = this.CreateGraphics();
            drawBuffer = Graphics.FromImage(imgBuffer);
            
        }

        public void gameLoop()
        {
            timer.start();
            //field1.randomize();
            tet1 = generatePiece();
            field1.ghostPiece = tet1;

            if (field1.nextTet.Count == 0 && ruleset.nextNum > 0) //generate nextTet
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    field1.nextTet.Add(generatePiece());
                }
            }

            gameRunning = true;
            //tetrng.delete();

            while (this.Created)
            {
                startTime = timer.elapsedTime;

                gameLogic();
                gameRender();

                Application.DoEvents();
                while (timer.elapsedTime - startTime < interval) ;
            }
        }

        void changeMenu(int newMenu)
        {

            switch (menuState) //clean up the current menu
            {
                case 0:
                    //TitlePanel.Enabled = false;
                    //TitlePanel.Visible = false;
                    break;
            }

            switch (newMenu) //activate the new menu
            {
                case 0:
                    //TitlePanel.Enabled = true;
                    //TitlePanel.Visible = true;
                    break;
            }

            menuState = newMenu;
        }

        void gameLogic()
        {
            //handle inputs
            //reset inputs
            inputH = inputV = inputStart = inputRot1 = inputRot2 = inputHold = 0;

            //up or down = w or s
            if (Keyboard.IsKeyDown(Key.W))
                inputV += 1;

            if (Keyboard.IsKeyDown(Key.S))
                inputV -= 1;

            //left or right = a or d
            if (Keyboard.IsKeyDown(Key.A))
                inputH -= 1;

            if (Keyboard.IsKeyDown(Key.D))
                inputH += 1;

            //start = enter
            if (Keyboard.IsKeyDown(Key.Enter))
                inputStart = 1;

            //rot1 = o or [
            if (Keyboard.IsKeyDown(Key.I) || Keyboard.IsKeyDown(Key.P))
            {
                if (!inputPressedRot1)
                {
                    inputPressedRot1 = true;
                    inputRot1 = 1;
                }
            }
            else
            {
                inputPressedRot1 = false;
            }

            //rot2 = p
            if (Keyboard.IsKeyDown(Key.O))
            {
                if (!inputPressedRot2)
                {
                    inputPressedRot2 = true;
                    inputRot2 = 1;
                }
            }
            else
            {
                inputPressedRot2 = false;
            }

            //hold = K
            if (Keyboard.IsKeyDown(Key.K))
                inputHold = 1;

            //deal with game logic
            switch (menuState)
            {
                case 0: //title

                    break;
                case 5: //ingame
                    logicTetris();
                    break;
            }
        }

        void gameRender()
        {
            //draw temp BG so bleeding doesn't occur
            drawBuffer.FillRectangle(new SolidBrush(Color.Black), this.ClientRectangle);

            //draw the field
            drawBuffer.FillRectangle(new SolidBrush(Color.Gray), field1.x, field1.y + 25, field1.width, field1.height);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    int block = field1.gameField[i][j];
                    if (block != 0)
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[block]), field1.x + 25*i, field1.y + j * 25, 25, 25);
                }
            }

            //draw the current piece
            if (tet1.id != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(tetColors[tet1.id]), field1.x + 25 * tet1.bits[i].x, field1.y + 25 * tet1.bits[i].y, 25, 25);
                }
            }

            if (ruleset.nextNum > 0)
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[field1.nextTet[i].id]), field1.x + 25 * field1.nextTet[i].bits[j].x, field1.y + 25 * field1.nextTet[i].bits[j].y - 75, 25, 25);
                    }
                }
            }

            //draw the ghost piece
            if (field1.level < 100 && field1.ghostPiece != null)
            {
                for(int i = 0; i < 4; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(tetColors[field1.ghostPiece.id]), field1.x + 25 * field1.ghostPiece.bits[i].x, field1.y + 25 * field1.ghostPiece.bits[i].y, 25, 25);
                }
           }

            //draw the grid
            for (int i = 1; i < 11; i++)
                drawBuffer.DrawLine(gridPen, field1.x + 25 * i, field1.y + 25, field1.x + 25 * i, field1.y + field1.height + 25);
            for (int i = 1; i < 22; i++)
                drawBuffer.DrawLine(gridPen, field1.x, field1.y + 25 * i, field1.x + field1.width, field1.y + 25 * i);


#if DEBUG
            SolidBrush debugBrush = new SolidBrush(Color.White);
            //denote debug
            drawBuffer.DrawString("DEBUG", DefaultFont, debugBrush, 20, 710);
            //draw the current inputs
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.W) ? "1" : "0", DefaultFont, debugBrush, 28, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.A) ? "1" : "0", DefaultFont, debugBrush, 20, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.S) ? "1" : "0", DefaultFont, debugBrush, 28, 740);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.D) ? "1" : "0", DefaultFont, debugBrush, 36, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.I) ? "1" : "0", DefaultFont, debugBrush, 50, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.O) ? "1" : "0", DefaultFont, debugBrush, 58, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.P) ? "1" : "0", DefaultFont, debugBrush, 66, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.K) ? "1" : "0", DefaultFont, debugBrush, 50, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(Key.Enter) ? "1" : "0", DefaultFont, debugBrush, 74, 730);

            //tech stats
            drawBuffer.DrawString("Current Timer: " + field1.currentTimer.ToString(), DefaultFont, debugBrush, 20, 750);
            drawBuffer.DrawString("Grav Level: " + field1.gravLevel, DefaultFont, debugBrush, 200, 750);
            drawBuffer.DrawString("Current Gravity: " + field1.gravCounter.ToString(), DefaultFont, debugBrush, 20, 760);
            drawBuffer.DrawString("Current Level: " + field1.level, DefaultFont, debugBrush, 200, 760);

            //game stats
            drawBuffer.DrawString("Score: " + field1.score, DefaultFont, debugBrush, 90, 740);
            if (field1.isGM)
                drawBuffer.DrawString("Grade: GM", DefaultFont, debugBrush, 90, 730);
            else
                drawBuffer.DrawString("Grade: " + ruleset.gradesTGM1[field1.grade], DefaultFont, debugBrush, 90, 730);

            for (int i = 0; i < field1.GMflags.Count; i++ )
            {
                drawBuffer.DrawString("-*".Substring(Convert.ToInt32(field1.GMflags[i]), 1), DefaultFont, debugBrush, 200 + i*8, 730);
            }

            drawBuffer.DrawString("Bravos: " + field1.bravoCounter, DefaultFont, debugBrush, 200, 740);
            
            //time
            drawBuffer.DrawString(string.Format("{0,2:00}:{1,2:00}:{2,2:00}", field1.min, field1.sec, field1.msec10), DefaultFont, debugBrush, 100, 700);

            //tets
            drawBuffer.DrawString(field1.lastTet[0] + " " + field1.lastTet[1] + " " + field1.lastTet[2] + " " + field1.lastTet[3] + " " + tet1.id, DefaultFont, debugBrush, 100, 720);
            for(int i = 0; i < 4; i++)
            {
                drawBuffer.DrawString(tet1.bits[i].x + " " + tet1.bits[i].y, DefaultFont, debugBrush, 160 + (32*i), 720);
            }

            if (field1.ghostPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.DrawString(field1.ghostPiece.bits[i].x + " " + field1.ghostPiece.bits[i].y, DefaultFont, debugBrush, 160 + (32 * i), 710);
                }
            }

#endif

            //draw the buffer, then set to refresh
            this.BackgroundImage = imgBuffer;
            this.Invalidate();

        }




        void logicTetris() {

            if (gameRunning == true)
            {
                //timing logic
                long temptimeVAR = field1.timer.elapsedTime;
                field1.min = (int)Math.Floor((double)temptimeVAR / 60000);
                temptimeVAR -= field1.min * 60000;
                field1.sec = (int)Math.Floor((double)temptimeVAR / 1000);
                temptimeVAR -= field1.sec * 1000;
                field1.msec = (int)temptimeVAR;
                field1.msec10 = (int)(field1.msec/10);

                //check inputs and handle logic pertaining to them

                if (inputH == 1 || inputH == -1)
                {
                    if (inputDelayH > 0)
                    {
                        inputDelayH--;
                    }
                    if (inputDelayH == -1)
                        inputDelayH = ruleset.baseDAS;
                }
                else
                    inputDelayH = -1;
                //check ID of current tetromino.
                if (tet1.id == 0)
                {
                    if (field1.currentTimer == (int)Field.timerType.LineClear)  //if timer is line clear and done, settle pieces and start ARE
                    {
                        if (field1.timerCount == 0)
                        {
                            //settle pieces and start ARE
                            for (int i = 0; i < field1.full.Count; i++ )
                            {
                                for(int j = field1.full[i]; j > 0; j--)
                                {
                                    for(int k = 0; k < 10; k++)
                                    {
                                        field1.gameField[k][j] = field1.gameField[k][j - 1];
                                    }
                                }
                            }
                            field1.full.Clear();
                            field1.currentTimer = (int)Field.timerType.ARE;
                            field1.timerCount = ruleset.baseARE;
                        }
                        else
                        {
                            field1.timerCount--;
                            return;
                        }
                    }
                    //elseif timer is ARE and done, get next tetromino
                    else if (field1.currentTimer == (int)Field.timerType.ARE)
                    {
                        if (field1.timerCount == 0)
                        {
                            //get next tetromino, generate another for "next"
                            if (ruleset.nextNum > 0)
                            {
                                tet1 = field1.nextTet[0];
                                field1.groundTimer = ruleset.baseLock;
                                for (int i = 0; i < field1.nextTet.Count - 1; i++)
                                {
                                    field1.nextTet[i] = field1.nextTet[i + 1];
                                }
                                field1.nextTet[field1.nextTet.Count - 1] = generatePiece();
                            }
                            else
                            {
                                tet1 = generatePiece();
                            }

                            field1.gravCounter = 0;

                            bool blocked = false;

                            blocked = !emptyUnderTet(tet1);

                            if (blocked)
                            {
                                gameRunning = false;
                                //TODO: cleanup and scoring
                                return;
                            }
                            field1.softCounter = 0;
                        }
                        else
                        {
                            field1.timerCount--;
                            return;
                        }
                    }
                }
                else  //else, check collision below
                {
                    bool floored = false;

                    for (int i = 0; i < 4; i++ )
                    {
                        if (tet1.bits[i].y + 1 >= 21)
                        {
                            floored = true;
                            break;
                        } 
                        else if (field1.gameField[tet1.bits[i].x][tet1.bits[i].y + 1] != 0)
                        {
                            floored = true;
                            break;
                        }
                    }

                    if (floored == true)
                    {
                        //check lock delay if grounded
                        if (field1.currentTimer == (int)Field.timerType.LockDelay)
                        {
                            //if lock delay up, place piece.
                            if (field1.groundTimer == 0)
                            {
                                if(field1.level % 100 != 99 && field1.level != 998)
                                    field1.level++;
                                for (int i = 0; i < 4; i++)
                                {
                                    field1.gameField[tet1.bits[i].x][tet1.bits[i].y] = tet1.id;
                                }
                                tet1.id = 0;
                                //check for full rows and screenclears
                                
                                int tetCount = 0;

                                for(int i = 0; i < 20; i++)
                                {
                                    int columnCount = 0;
                                    for(int j = 0; j < 10; j++)
                                    {
                                        if (field1.gameField[j][i+1] != 0)
                                        {
                                            columnCount++;
                                            tetCount++;
                                        }
                                    }
                                    if (columnCount == 10)
                                    {
                                        field1.full.Add(i + 1);
                                        tetCount -= 10;
                                    }
                                }

                                if (field1.full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                {
                                    for(int i = 0; i < field1.full.Count; i++)
                                    {
                                        for (int j = 0; j < 10; j++)
                                            field1.gameField[j][field1.full[i]] = 0;
                                        field1.level++;
                                    }
                                    //calculate combo!
                                    int bravo = 1;
                                    if (tetCount == 0)
                                    {
                                        field1.bravoCounter++;
                                        bravo = 4;
                                    }

                                    field1.combo = field1.combo + (2 * field1.full.Count) - 2;
                                    //give points
                                    field1.score += ((int)Math.Ceiling((double)(field1.level + field1.full.Count) / 4) + field1.softCounter) * field1.full.Count * ((field1.full.Count * 2) - 1) * field1.combo * bravo;

                                    //check GM conditions
                                    if(field1.GMflags.Count == 0 && field1.level >= 300)
                                    {
                                        if (field1.score >= 12000 && field1.timer.elapsedTime <= 255000)
                                            field1.GMflags.Add(true);
                                        else
                                            field1.GMflags.Add(false);
                                    }
                                    else if(field1.GMflags.Count == 1 && field1.level >= 500)
                                    {
                                        if (field1.score >= 40000 && field1.timer.elapsedTime <= 450000)
                                            field1.GMflags.Add(true);
                                        else
                                            field1.GMflags.Add(false);
                                    }
                                    else if(field1.GMflags.Count == 2 && field1.level >= 999)
                                    {
                                        field1.level = 999;
                                        if (field1.score >= 126000 && field1.timer.elapsedTime <= 810000)
                                            field1.GMflags.Add(true);
                                        else
                                            field1.GMflags.Add(false);


                                        //check for awarding GM
                                        if (field1.GMflags[0] && field1.GMflags[1] && field1.GMflags[2])
                                        {
                                            //credits roll in future, for now just gibe GM?
                                            field1.isGM = true;

                                            gameRunning = false;
                                        }
                                    }

                                    //update grade
                                    if(field1.grade < ruleset.gradePointsTGM1.Count - 1)
                                    {
                                        if (field1.score >= ruleset.gradePointsTGM1[field1.grade + 1])
                                            field1.grade++;
                                    }

                                    //start timer
                                    field1.currentTimer = (int)Field.timerType.LineClear;
                                    field1.timerCount = ruleset.baseLineClear;

                                }
                                else //start the ARE, check if new grav level
                                {
                                    field1.currentTimer = (int)Field.timerType.ARE;
                                    field1.timerCount = ruleset.baseARE;

                                    field1.combo = 1;
                                    
                                }

                                if (field1.gravLevel < ruleset.gravLevelsTGM1.Count - 1)
                                {
                                    if (field1.level >= ruleset.gravLevelsTGM1[field1.gravLevel + 1])
                                        field1.gravLevel++;
                                }

                                return;
                                
                            }
                            else
                            {
                                field1.gravCounter = 0;
                                field1.groundTimer--;
                            }
                        }
                        else
                        {
                            field1.currentTimer = (int)Field.timerType.LockDelay;
                            //field1.timerCount = ruleset.baseLock;
                        }
                    }
                    else
                        field1.currentTimer = (int)Field.timerType.ARE;



                    int blockDrop = 0;// make it here so we can drop faster


                    //check saved inputs and act on them accordingly
                    

                    if (inputV == 1 && ruleset.hardDrop == 1)
                    {
                        blockDrop = 19;
                        field1.gravCounter = 0;
                    }
                    else if (inputV == -1 && inputDelayV == 0)
                    {
                        blockDrop = 1;
                        field1.softCounter++;
                        field1.gravCounter = 0;
                        if(field1.currentTimer == (int)Field.timerType.LockDelay)
                            field1.groundTimer = 0;
                    }

                    if (inputHold == 1 && ruleset.hold == true && field1.swappedHeld == false)
                    {
                        Tetromino tempTet;
                        if (field1.heldPiece.id != null)
                        {
                            tempTet = new Tetromino(field1.heldPiece.id);
                            field1.heldPiece = new Tetromino(tet1.id);
                            tet1 = tempTet;
                            field1.groundTimer = ruleset.baseLock;
                        }
                        else
                        {
                            field1.heldPiece = new Tetromino(tet1.id);
                            tet1.id = 0;
                            field1.currentTimer = (int)Field.timerType.ARE;
                            field1.timerCount = ruleset.baseARE;
                        }
                        //field1.heldPiece = new Tetromino(tet1.id);
                    }


                    if (inputRot1 == 1)
                    {
                        rotatePiece(1);
                    }
                    if (inputRot2 == 1)
                    {
                        rotatePiece(2);
                    }

                   

                    if (inputH == 1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                    {
                        bool safe = true;
                        //check to the right of each bit
                        for (int i = 0; i < 4; i++)
                        {
                            if (tet1.bits[i].x + 1 == 10)
                            {
                                safe = false;
                                break;
                            }
                            if (field1.gameField[tet1.bits[i].x + 1][tet1.bits[i].y] != 0)
                            {
                                safe = false;
                                break;
                            }
                        }
                        if (safe) //if it's fine, move them all right one
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tet1.bits[i].x++;
                            }
                        }
                    }
                    else if (inputH == -1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                    {
                        bool safe = true;
                        //check to the right of each bit
                        for (int i = 0; i < 4; i++)
                        {
                            if (tet1.bits[i].x - 1 == -1)
                            {
                                safe = false;
                                break;
                            }
                            if (field1.gameField[tet1.bits[i].x - 1][tet1.bits[i].y] != 0)
                            {
                                safe = false;
                                break;
                            }
                        }
                        if (safe) //if it's fine, move them all right one
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tet1.bits[i].x-=1;
                            }
                        }
                    }

                    //calc gravity LAST sso I-jumps are doable?
                    field1.gravCounter += ruleset.gravTableTGM1[field1.gravLevel]; //add our current gravity strength



                    for (int tempGrav = field1.gravCounter; tempGrav >= 256; tempGrav = tempGrav - 256)
                    {
                        blockDrop++;
                    }
                    if (blockDrop > 0 && field1.currentTimer != (int)Field.timerType.LockDelay)
                    {
                        int i;
                        field1.gravCounter = 0;
                        field1.groundTimer = ruleset.baseLock;
                        for (i = 0; i < blockDrop; i++)
                        {
                            //check collision of each step


                            if (field1.gameField[tet1.bits[0].x][tet1.bits[0].y + i] != 0)
                            {
                                i--;
                                break;
                            }
                            if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y + i] != 0)
                            {
                                i--;
                                break;
                            }
                            if (field1.gameField[tet1.bits[2].x][tet1.bits[2].y + i] != 0)
                            {
                                i--;
                                break;
                            }
                            if (field1.gameField[tet1.bits[3].x][tet1.bits[3].y + i] != 0)
                            {
                                i--;
                                break;
                            }
                            if (tet1.bits[0].y + i >= 20)
                                break;
                            if (tet1.bits[1].y + i >= 20)
                                break;
                            if (tet1.bits[2].y + i >= 20)
                                break;
                            if (tet1.bits[3].y + i >= 20)
                                break;
                        }

                        for (int j = 0; j < 4; j++)
                        {
                            tet1.bits[j].y += i;
                        }


                        

                    }

                    //handle ghost piece logic
                    /*field1.ghostPiece.bits[0] = tet1.bits[0];
                    field1.ghostPiece.bits[0].y = 20;
                    field1.ghostPiece.bits[1] = tet1.bits[1];
                    field1.ghostPiece.bits[1].y = 20;
                    field1.ghostPiece.bits[2] = tet1.bits[2];
                    field1.ghostPiece.bits[2].y = 20;
                    field1.ghostPiece.bits[3] = tet1.bits[3];
                    field1.ghostPiece.bits[3].y = 20;

                    int g = 0;
                    for (g = 0; g < 20; g++)
                    {
                        bool breakout = false;
                        for (int g2 = 0; g2 < 4; g2++)
                        {
                            if (field1.ghostPiece.bits[g2].y + g == 20)
                            {
                                breakout = true;
                                break;
                            }
                            if (field1.gameField[field1.ghostPiece.bits[g2].x][field1.ghostPiece.bits[g2].y + g] != 0)
                            {
                                g--;
                                breakout = true;
                                break;
                            }
                        }
                        if (breakout)
                            break;
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        field1.ghostPiece.bits[j].y += g;
                    }*/

                    //if (!emptyUnderTet(field1.ghostPiece))
                    //{
                    //    throw new NotImplementedException();
                    //}

                    //if (!emptyUnderTet(tet1))
                    //{
                    //    throw new NotImplementedException();
                    //}
                }


            }
        }

        private void rotatePiece(int p)
        {
            int xOffset = 0; //for kicks
            int yOffset = 0;

            switch(tet1.id)
            {
                case 1: //I has two rotation states; KICKS ONLY IN NEW RULES
                    //check current rotation
                    //check positions based on p and rotation, if abovescreen or offscreen to the sides then add an offset
                    switch(tet1.rotation)
                    {
                        case 0:
                            if (tet1.bits[2].y - 1 >= 0 && tet1.bits[2].y + 2 <= 19)
                            {
                                if (field1.gameField[tet1.bits[2].x][tet1.bits[2].y - 1] == 0 && field1.gameField[tet1.bits[2].x][tet1.bits[2].y + 1] == 0 && field1.gameField[tet1.bits[2].x][tet1.bits[2].y +2] == 0)
                                {
                                    tet1.bits[0].x += 2;
                                    tet1.bits[0].y += 2;
                                    tet1.bits[1].x += 1;
                                    tet1.bits[1].y += 1;
                                    tet1.bits[3].x -= 1;
                                    tet1.bits[3].y -= 1;

                                    tet1.rotation = 1;
                                }
                            }
                            break;
                        case 1:
                            if (tet1.bits[2].x - 2 >= 0 && tet1.bits[2].x + 1 <= 9)
                            {
                                if (field1.gameField[tet1.bits[2].x - 2][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[2].x - 1][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[2].x + 1][tet1.bits[2].y] == 0)
                                {
                                    tet1.bits[0].x -= 2;
                                    tet1.bits[0].y -= 2;
                                    tet1.bits[1].x -= 1;
                                    tet1.bits[1].y -= 1;
                                    tet1.bits[3].x += 1;
                                    tet1.bits[3].y += 1;

                                    tet1.rotation = 0;
                                }
                            }
                            break;
                    }
                    //if spaces are open, rotate and place!
                    //else test kicks

                    break;
                case 2: //T 
                    switch(inputRot1 - inputRot2)
                    {
                        case 1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    //test for OoB bump
                                    if(tet1.bits[1].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if(field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y - 1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0 && field1.gameField[tet1.bits[3].x + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += 1 + ((i % 3) - 1);
                                            tet1.bits[0].y += 1 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += yOffset;
                                            tet1.bits[2].x += -1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -1 + yOffset;
                                            tet1.bits[3].x += 1 + ((i % 3) - 1);
                                            tet1.bits[3].y += -1 + yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    //TODO: test for upkick if TGM3
                                    if (ruleset.rotation == 3 && ((field1.gameField[tet1.bits[0].x - 1 + xOffset][tet1.bits[0].y] != 0 || field1.gameField[tet1.bits[0].x + 1 + xOffset][tet1.bits[0].y] != 0) && tet1.kicked == 0))
                                    {
                                        yOffset = -1;
                                        tet1.kicked = 1;
                                    }
                                    
                                    for (int i = 1; i < 4; i++ )
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + yOffset] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x - 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + 2 + yOffset] == 0 && field1.gameField[tet1.bits[3].x - 1 + xOffset + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += yOffset;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1 + yOffset;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 2 + yOffset;
                                            tet1.bits[3].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += yOffset;

                                            tet1.rotation = 2;
                                            
                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if(tet1.bits[3].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if(field1.gameField[tet1.bits[3].x][tet1.bits[3].y-1] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++ )
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + ((i % 3) - 1)][tet1.bits[0].y + -2 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0 && field1.gameField[tet1.bits[3].x + -1 + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += -2 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += yOffset;
                                            tet1.bits[3].x += -1 + ((i % 3) - 1);
                                            tet1.bits[3].y += yOffset;

                                            tet1.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++ )
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + -1] == 0 && field1.gameField[tet1.bits[3].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[3].y + 1] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += -1;
                                            tet1.bits[3].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 1;

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    if(tet1.bits[1].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++ )
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + -1 + ((i % 3) - 1)][tet1.bits[2].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + -1 + ((i % 3) - 1)][tet1.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += 1 + ((i % 3) - 1);
                                            tet1.bits[0].y += -1 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += yOffset;
                                            tet1.bits[2].x += -1 + ((i % 3) - 1);
                                            tet1.bits[2].y += 1 + yOffset;
                                            tet1.bits[3].x += -1 + ((i % 3) - 1);
                                            tet1.bits[3].y += -1 + yOffset;

                                            tet1.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + -1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + xOffset + ((i % 3) - 1)][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += -1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 1;
                                            tet1.bits[3].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 1;

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet1.bits[3].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (field1.gameField[tet1.bits[3].x][tet1.bits[3].y - 1] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x - 1 + ((i % 3) - 1)][tet1.bits[0].y + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y - 2 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + 1 + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -2 + yOffset;
                                            tet1.bits[3].x += 1 + ((i % 3) - 1);
                                            tet1.bits[3].y += yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 2] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0 && field1.gameField[tet1.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[3].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[3].y] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 2;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].x += 1 + xOffset + ((i % 3) - 1);

                                            tet1.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case 3: //L
                    switch(inputRot1 - inputRot2)
                    {
                        case 1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    if (tet1.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if(field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y + 1] != 0)
                                        {
                                            //if (field1.gameField[tet1.bits[0].x][tet1.bits[0].y - 1] == 0) //USE FOR OTHER SPIN
                                            //{
                                                break;
                                            //}
                                        }
                                    }
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + -1 + ((i % 3) - 1)][tet1.bits[2].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + 2 + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += 1 + ((i % 3) - 1);
                                            tet1.bits[0].y += 1 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += yOffset;
                                            tet1.bits[2].x += -1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -1 + yOffset;
                                            tet1.bits[3].x += 2 + ((i % 3) - 1);
                                            tet1.bits[3].y += yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[0].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[0].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0 && field1.gameField[tet1.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + 2] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y + -1] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 2;
                                            tet1.bits[3].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += -1;

                                            tet1.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet1.bits[1].y - 2 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if(field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 2] != 0 || field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + ((i % 3) - 1)][tet1.bits[0].y + -2 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0 && field1.gameField[tet1.bits[3].x + -2 + ((i % 3) - 1)][tet1.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += -2 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += yOffset;
                                            tet1.bits[3].x += -2 + ((i % 3) - 1);
                                            tet1.bits[3].y += -1 + yOffset;

                                            tet1.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + -1] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y + 2] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += -1;
                                            tet1.bits[3].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 2;

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    if (tet1.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y + 1] != 0)
                                        {
                                            if (field1.gameField[tet1.bits[0].x][tet1.bits[0].y - 1] == 0)
                                            {
                                            break;
                                            }
                                        }
                                        for(int i = 1; i < 4; i++)
                                        {
                                            if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                            {
                                                continue;
                                            }
                                            if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + -1 + ((i % 3) - 1)][tet1.bits[2].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + ((i % 3) - 1)][tet1.bits[3].y + -2 + yOffset] == 0)
                                            {
                                                tet1.bits[0].x += 1 + ((i % 3) - 1);
                                                tet1.bits[0].y += -1 + yOffset;
                                                tet1.bits[1].x += ((i % 3) - 1);
                                                tet1.bits[1].y += yOffset;
                                                tet1.bits[2].x += -1 + ((i % 3) - 1);
                                                tet1.bits[2].y += 1 + yOffset;
                                                tet1.bits[3].x += ((i % 3) - 1);
                                                tet1.bits[3].y += -2 + yOffset;

                                                tet1.rotation = 3;

                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + -1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + 1] == 0 && field1.gameField[tet1.bits[3].x + -2 + xOffset + ((i % 3) - 1)][tet1.bits[3].y] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += -1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 1;
                                            tet1.bits[3].x += -2 + xOffset + ((i % 3) - 1);

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet1.bits[1].y - 2 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 2] != 0 || field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + ((i % 3) - 1)][tet1.bits[0].y + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y + -2 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + ((i % 3) - 1)][tet1.bits[3].y + 1 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -2 + yOffset;
                                            tet1.bits[3].x += ((i % 3) - 1);
                                            tet1.bits[3].y += 1 + yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 2] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0 && field1.gameField[tet1.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[3].x + 2 + xOffset + ((i % 3) - 1)][tet1.bits[3].y + 1] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 2;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].x += 2 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 1;

                                            tet1.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case 4: //J
                    switch(inputRot1 - inputRot2)
                    {
                        case 1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    if (tet1.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if(field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y + 1] != 0)
                                        {
                                            if (field1.gameField[tet1.bits[0].x][tet1.bits[0].y - 1] == 0)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + -1 + ((i % 3) - 1)][tet1.bits[2].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + ((i % 3) - 1)][tet1.bits[3].y + -2 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += 1 + ((i % 3) - 1);
                                            tet1.bits[0].y += 1 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += yOffset;
                                            tet1.bits[2].x += -1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -1 + yOffset;
                                            tet1.bits[3].x += ((i % 3) - 1);
                                            tet1.bits[3].y += -2 + yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0 && field1.gameField[tet1.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + 2] == 0 && field1.gameField[tet1.bits[3].x + -2 + xOffset + ((i % 3) - 1)][tet1.bits[3].y + 1] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 2;
                                            tet1.bits[3].x += -2 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 1;

                                            tet1.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet1.bits[1].y - 2 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if(field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 2] != 0 || field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + ((i % 3) - 1)][tet1.bits[0].y + -2 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0 && field1.gameField[tet1.bits[3].x  + ((i % 3) - 1)][tet1.bits[3].y + 1 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += -2 + yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += yOffset;
                                            tet1.bits[3].x += ((i % 3) - 1);
                                            tet1.bits[3].y += 1 + yOffset;

                                            tet1.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for(int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + -1] == 0 && field1.gameField[tet1.bits[3].x + 2 + xOffset + ((i % 3) - 1)][tet1.bits[3].y] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += -1;
                                            tet1.bits[3].x += 2 + xOffset + ((i % 3) - 1);

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case -1:
                            switch(tet1.rotation)
                            {
                                case 0:
                                    if (tet1.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y + 1] != 0)
                                        {
                                           // if (field1.gameField[tet1.bits[0].x][tet1.bits[0].y - 1] == 0) //for the otehr direction
                                            //{
                                                break;
                                            //}
                                        }
                                        for (int i = 1; i < 4; i++)
                                        {
                                            if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                            {
                                                continue;
                                            }
                                            if (field1.gameField[tet1.bits[0].x + 1 + ((i % 3) - 1)][tet1.bits[0].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + -1 + ((i % 3) - 1)][tet1.bits[2].y + 1 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + -2 + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0)
                                            {
                                                tet1.bits[0].x += 1 + ((i % 3) - 1);
                                                tet1.bits[0].y += -1 + yOffset;
                                                tet1.bits[1].x += ((i % 3) - 1);
                                                tet1.bits[1].y += yOffset;
                                                tet1.bits[2].x += -1 + ((i % 3) - 1);
                                                tet1.bits[2].y += 1 + yOffset;
                                                tet1.bits[3].x += -2 + ((i % 3) - 1);
                                                tet1.bits[3].y += yOffset;

                                                tet1.rotation = 3;

                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + -1] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y] == 0 && field1.gameField[tet1.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y + 1] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y + 2] == 0)
                                        {
                                            tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += -1;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[2].y += 1;
                                            tet1.bits[3].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += 2;

                                            tet1.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet1.bits[1].y - 2 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 2] != 0 || field1.gameField[tet1.bits[1].x][tet1.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + -1 + ((i % 3) - 1)][tet1.bits[0].y + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + -1 + yOffset] == 0 && field1.gameField[tet1.bits[2].x + 1 + ((i % 3) - 1)][tet1.bits[2].y + -2 + yOffset] == 0 && field1.gameField[tet1.bits[3].x + 2 + ((i % 3) - 1)][tet1.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet1.bits[0].x += -1 + ((i % 3) - 1);
                                            tet1.bits[0].y += yOffset;
                                            tet1.bits[1].x += ((i % 3) - 1);
                                            tet1.bits[1].y += -1 + yOffset;
                                            tet1.bits[2].x += 1 + ((i % 3) - 1);
                                            tet1.bits[2].y += -2 + yOffset;
                                            tet1.bits[3].x += 2 + ((i % 3) - 1);
                                            tet1.bits[3].y += -1 + yOffset;

                                            tet1.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 2] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0 && field1.gameField[tet1.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y + -1] == 0)
                                        {
                                            tet1.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[0].y += 2;
                                            tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[1].y += 1;
                                            tet1.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet1.bits[3].x += xOffset + ((i % 3) - 1);
                                            tet1.bits[3].y += -1;

                                            tet1.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case 5: //S has two rotation states
                    switch(tet1.rotation)
                    {
                        case 0:
                            if(tet1.bits[0].y - 2 < 0)
                            {
                                //set an offset
                                yOffset = 1;
                            }
                            else
                            {
                                //check if space has a piece there!
                            }

                            for (int i = 1; i < 4; i++)
                            {
                                if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (field1.gameField[tet1.bits[0].x + ((i % 3) - 1)][tet1.bits[0].y - 2 + yOffset] == 0 && field1.gameField[tet1.bits[0].x + ((i % 3) - 1)][tet1.bits[0].y - 1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0)
                                {
                                    tet1.bits[0].x += 1 + ((i % 3) - 1);
                                    tet1.bits[0].y += yOffset;
                                    tet1.bits[1].x += ((i % 3) - 1);
                                    tet1.bits[1].y += yOffset - 1;
                                    tet1.bits[2].x += -1 + ((i % 3) - 1);
                                    tet1.bits[2].y += yOffset;
                                    tet1.bits[3].x += -2 + ((i % 3) - 1);
                                    tet1.bits[3].y += (yOffset - 1);

                                    tet1.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {
                                if (tet1.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (field1.gameField[tet1.bits[0].x - 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y] == 0 && field1.gameField[tet1.bits[0].x + xOffset + ((i % 3) - 1)][tet1.bits[0].y] == 0 && field1.gameField[tet1.bits[0].x + xOffset + ((i % 3) - 1)][tet1.bits[0].y - 1] == 0 && field1.gameField[tet1.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet1.bits[0].y - 1] == 0)
                                {
                                    tet1.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                    tet1.bits[1].y += 1;
                                    tet1.bits[1].x += xOffset + ((i % 3) - 1);
                                    tet1.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                    tet1.bits[3].x += 2 + xOffset + ((i % 3) - 1);
                                    tet1.bits[3].y += 1;

                                    tet1.rotation = 0;

                                    break;
                                }
                            }
                            break;
                    }
                    break;
                case 6: //Z has two rotation states
                    switch (tet1.rotation)
                    {
                        case 0:
                            if (tet1.bits[0].y - 1 < 0)
                            {
                                //set an offset
                                yOffset = 1;
                            }
                            else
                            {
                                //check if space has a piece there!
                            }
                            for (int i = 1; i < 4; i++)
                            {
                                if (tet1.bits[2].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[2].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (field1.gameField[tet1.bits[1].x - 1 + ((i % 3) - 1)][tet1.bits[0].y - 1 + yOffset] == 0 && field1.gameField[tet1.bits[1].x - 1 + ((i % 3) - 1)][tet1.bits[3].y + yOffset] == 0 && field1.gameField[tet1.bits[1].x + ((i % 3) - 1)][tet1.bits[1].y + yOffset] == 0 && field1.gameField[tet1.bits[2].x + ((i % 3) - 1)][tet1.bits[2].y + yOffset] == 0)
                                {
                                    tet1.bits[0].x += 2 + ((i % 3) - 1);
                                    tet1.bits[0].y += yOffset - 1;
                                    tet1.bits[1].x += 1 + ((i % 3) - 1);
                                    tet1.bits[1].y += yOffset;
                                    tet1.bits[2].x += ((i % 3) - 1);
                                    tet1.bits[2].y += yOffset - 1;
                                    tet1.bits[3].x += -1 + ((i % 3) -1);
                                    tet1.bits[3].y += yOffset;

                                    tet1.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i ++)
                            {
                                if (tet1.bits[2].x - 1 + ((i % 3) - 1) < 0 || tet1.bits[2].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (field1.gameField[tet1.bits[0].x - 2 + xOffset + ((i % 3) - 1)][tet1.bits[0].y + 1] == 0 && field1.gameField[tet1.bits[2].x + xOffset + ((i % 3) - 1)][tet1.bits[2].y] == 0 && field1.gameField[tet1.bits[3].x + xOffset + ((i % 3) - 1)][tet1.bits[3].y] == 0 && field1.gameField[tet1.bits[1].x + xOffset + ((i % 3) - 1)][tet1.bits[1].y + 1] == 0)
                                {
                                    tet1.bits[2].x += xOffset + ((i % 3) - 1);
                                    tet1.bits[2].y += 1;
                                    tet1.bits[1].x += xOffset - 1 + ((i % 3) - 1);
                                    tet1.bits[0].x += (xOffset - 2) + ((i % 3) - 1);
                                    tet1.bits[0].y += 1;
                                    tet1.bits[3].x += (xOffset + 1) + ((i % 3) - 1);

                                    tet1.rotation = 0;

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

        public Tetromino generatePiece()
        {
            Random piece = new Random();
            int tempID = 0;
            for (int j = 0; j < 4; j++)
            {
                tempID = piece.Next(7) + 1;
                if (tempID != field1.lastTet[0] && tempID != field1.lastTet[1] && tempID != field1.lastTet[2] && tempID != field1.lastTet[3])
                    break;
            }
            for (int j = 0; j < 3; j++)
            {
                field1.lastTet[j] = field1.lastTet[j + 1];
            }

            Tetromino tempTet = new Tetromino(tempID);
            field1.lastTet[3] = tempTet.id;
            return tempTet;
        }

        public bool emptyUnderTet(Tetromino tet)
        {
            bool status = true;
            for (int i = 0; i < 4; i++ )
            {
                if (field1.gameField[tet.bits[i].x][tet.bits[i].y] != 0)
                {
                    status = false;
                    break;
                }
            }

            return status;
        }
    }
}
