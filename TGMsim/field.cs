using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using System.Windows.Forms;

namespace TGMsim
{
    class Field
    {

        public bool gameRunning;

        Tetromino activeTet;
        public List<Tetromino> nextTet = new List<Tetromino>();
        public List<int> lastTet = new List<int>() { 6, 6, 6, 6};

        public List<List<int>> gameField = new List<List<int>>();

        public List<int> full = new List<int>();

        public List<bool> GMflags = new List<bool>();

        public bool isGM = false;

        public Tetromino heldPiece;

        public Tetromino ghostPiece;

        public GameTimer timer = new GameTimer();
        public GameTimer contTime = new GameTimer();
        public GameTimer startTime = new GameTimer();
        public int min, sec, msec, msec10;

        public bool swappedHeld;

        public int x, y, width, height;
        public enum timerType { ARE, DAS, LockDelay, LineClear} ;
        public int currentTimer = 0;
        public int timerCount = 0;
        public int groundTimer = 0;
        public int gravCounter = 0;
        public int gravLevel = 0;
        public int level = 1;
        public int grade = 0;
        public int score = 0;
        public int combo = 1;
        public bool comboing = false;

        public int starting = 1;
        public bool inCredits = false;
        int creditsProgress;
        public bool newHiscore = false;

        public int bravoCounter = 0;

        public int softCounter = 0;

        Rules ruleset;
        Mode mode;
        int curTheme;

        public GameResult results;

        public bool godmode = false;
        public bool bigmode = false;
        public bool g20 = false;

        public bool cont = false;
        public bool exit = false;

        List<System.Drawing.Color> tetColors = new List<System.Drawing.Color>();

        NAudio.Wave.WaveOutEvent soundList = new NAudio.Wave.WaveOutEvent();
        NAudio.Vorbis.VorbisWaveReader vorbisStream;
        System.Windows.Media.MediaPlayer s_Ready = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Go = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet1 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet2 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet3 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet4 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet5 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet6 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet7 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_PreRot = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Contact = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Lock = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Clear = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Impact = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Grade = new System.Windows.Media.MediaPlayer();

        Pen gridPen = new Pen(new SolidBrush(Color.White));

        Controller pad;
        int inputDelayH = 0, inputDelayV = 0;

        public Field(Controller ctlr, Rules rules, Mode m2, NAudio.Vorbis.VorbisWaveReader music)
        {
            x = 275;
            y = 100;
            width = 250;
            height = 500;
            tetColors.Add(Color.Transparent);
            tetColors.Add(Color.Red);
            tetColors.Add(Color.Cyan);
            tetColors.Add(Color.Orange);
            tetColors.Add(Color.Blue);
            tetColors.Add(Color.Purple);
            tetColors.Add(Color.Green);
            tetColors.Add(Color.Yellow);

            s_Ready.Open(new Uri(Environment.CurrentDirectory + @"\Audio\SE\SEP_ready.wav"));
            s_Go.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEP_go.wav"));
            s_Tet1.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino1.wav"));
            s_Tet2.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino2.wav"));
            s_Tet3.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino3.wav"));
            s_Tet4.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino4.wav"));
            s_Tet5.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino5.wav"));
            s_Tet6.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino6.wav"));
            s_Tet7.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_mino7.wav"));
            s_PreRot.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_prerotate.wav"));
            s_Contact.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_fixa.wav"));
            s_Lock.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_instal.wav"));
            s_Clear.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_disappear.wav"));
            s_Impact.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEB_fall.wav"));
            s_Grade.Open(new Uri(Environment.CurrentDirectory + @"/Audio/SE/SEP_levelchange.wav"));

            pad = ctlr;
            ruleset = rules;
            mode = m2;
            vorbisStream = music;

            Random random = new Random();

            activeTet = new Tetromino(0); //first piece cannot be S, Z, or O
            //for (int j = 0; j < lastTet.Count - 1; j++)
            //{
            //    lastTet[j] = lastTet[j + 1];
            //}
            //lastTet[lastTet.Count - 1] = activeTet.id;

            //ghostPiece = activeTet.clone();

            if (nextTet.Count == 0 && ruleset.nextNum > 0) //generate nextTet
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    nextTet.Add(generatePiece());
                }
            }

            gameRunning = true;
            starting = 1;

            timer.start();
            startTime.start();
            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 21; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }
            playMusic("level 1");
            playSound(s_Ready);
        }

        public void randomize()
        {
            Random rng = new Random();
            for (int i = 0; i < 50; i++)
            {
                gameField[rng.Next(10)][rng.Next(20)] = rng.Next(8);
                
            }
        }

        public void draw(Graphics drawBuffer)
        {
            //draw the field
            drawBuffer.FillRectangle(new SolidBrush(Color.Gray), x, y + 25, width, height);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    int block = gameField[i][j];
                    if (block != 0)
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[block]), x + 25 * i, y + j * 25, 25, 25);
                }
            }

            //draw the current piece
            if (activeTet.id != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(tetColors[activeTet.id]), x + 25 * activeTet.bits[i].x, y + 25 * activeTet.bits[i].y, 25, 25);
                }
            }

            //draw the next piece
            if (ruleset.nextNum > 0)
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[nextTet[i].id]), x + i*70 + 15 * nextTet[i].bits[j].x + 40, y + 15 * nextTet[i].bits[j].y - 75, 15, 15);
                    }
                }
            }

            //draw the hold piece
            if(ruleset.hold == true && heldPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(tetColors[heldPiece.id]), x - 75 + 15 * heldPiece.bits[i].x, y - 50 + 15 * heldPiece.bits[i].y, 15, 15);
                }
            }

            //draw the ghost piece
            if (level < mode.themes[0] && ghostPiece != null && activeTet.id == ghostPiece.id)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(100, tetColors[ghostPiece.id])), x + 25 * ghostPiece.bits[i].x, y + 25 * ghostPiece.bits[i].y, 25, 25);
                }
            }

            //draw the grid
            for (int i = 1; i < 11; i++)
                drawBuffer.DrawLine(gridPen, x + 25 * i, y + 25, x + 25 * i, y + height + 25);
            for (int i = 1; i < 22; i++)
                drawBuffer.DrawLine(gridPen, x, y + 25 * i, x + width, y + 25 * i);

            //Starting things
            if (starting == 1)
                drawBuffer.DrawString("Ready", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (starting == 2)
                drawBuffer.DrawString("Go", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);


            //endgame stats
            if (gameRunning == false)
            {
                drawBuffer.DrawString("Grade: " + results.grade, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
                drawBuffer.DrawString("Score: " + results.score, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 210);
                drawBuffer.DrawString("Time: " + results.time, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 220);
                drawBuffer.DrawString("Name: " + results.username, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 230);

                drawBuffer.DrawString("Press start to", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 250);
                drawBuffer.DrawString("restart the field!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 260);

                drawBuffer.DrawString("Press B to", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 280);
                drawBuffer.DrawString("return to menu!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 290);
            }


#if DEBUG
            SolidBrush debugBrush = new SolidBrush(Color.White);

            //tech stats
            drawBuffer.DrawString("Grav Level: " + ruleset.gravTableTGM1[gravLevel].ToString(), SystemFonts.DefaultFont, debugBrush, 20, 760);
            if (!inCredits)
                drawBuffer.DrawString("Current Level: " + level, SystemFonts.DefaultFont, debugBrush, 200, 760);
            else
            {
                drawBuffer.DrawString("Credits", SystemFonts.DefaultFont, debugBrush, 200, 760);
                drawBuffer.DrawString("Credits Timer: " + creditsProgress.ToString(), SystemFonts.DefaultFont, debugBrush, 200, 750);
            }

            //game stats
            drawBuffer.DrawString("Score: " + score, SystemFonts.DefaultFont, debugBrush, 90, 740);
            drawBuffer.DrawString("Combo: " + combo, SystemFonts.DefaultFont, debugBrush, 90, 750);
            if (isGM)
                drawBuffer.DrawString("Grade: GM", SystemFonts.DefaultFont, debugBrush, 90, 730);
            else
                drawBuffer.DrawString("Grade: " + ruleset.gradesTGM1[grade], SystemFonts.DefaultFont, debugBrush, 90, 730);

            for (int i = 0; i < GMflags.Count; i++)
            {
                drawBuffer.DrawString("-*".Substring(Convert.ToInt32(GMflags[i]), 1), SystemFonts.DefaultFont, debugBrush, 200 + i * 8, 730);
            }

            drawBuffer.DrawString("Bravos: " + bravoCounter, SystemFonts.DefaultFont, debugBrush, 200, 740);

            if (newHiscore)
                drawBuffer.DrawString("New Hiscore", SystemFonts.DefaultFont, debugBrush, 200, 700);

            //time
            drawBuffer.DrawString(string.Format("{0,2:00}:{1,2:00}:{2,2:00}", min, sec, msec10), SystemFonts.DefaultFont, debugBrush, 100, 700);

            //tets
            drawBuffer.DrawString(lastTet[0] + " " + lastTet[1] + " " + lastTet[2] + " " + lastTet[3], SystemFonts.DefaultFont, debugBrush, 100, 720);
            for (int i = 0; i < 4; i++)
            {
                drawBuffer.DrawString(activeTet.bits[i].x + " " + activeTet.bits[i].y, SystemFonts.DefaultFont, debugBrush, 160 + (32 * i), 720);
            }

            if (ghostPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    drawBuffer.DrawString(ghostPiece.bits[i].x + " " + ghostPiece.bits[i].y, SystemFonts.DefaultFont, debugBrush, 160 + (32 * i), 710);
                }
            }
#endif
        }

        public void logic()
        {
            if (startTime.elapsedTime > 1000 && starting == 1)
            {
                starting = 2;
                //play GO
                playSound(s_Go);
            }
            if (startTime.elapsedTime > 2000 && starting == 2)
            {
                starting = 0;
            }

            if (starting == 0)
            {

                if (gameRunning == true)
                {
                    //timing logic
                    long temptimeVAR = (long)(timer.elapsedTime * 59.84 / 60);
                    min = (int)Math.Floor((double)temptimeVAR / 60000);
                    temptimeVAR -= min * 60000;
                    sec = (int)Math.Floor((double)temptimeVAR / 1000);
                    temptimeVAR -= sec * 1000;
                    msec = (int)temptimeVAR;
                    msec10 = (int)(msec / 10);



                    //check inputs and handle logic pertaining to them
                    //pad.poll();
                    if (pad.inputH == 1 || pad.inputH == -1)
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

                    if (inCredits)
                    {
                        creditsProgress++;
                        if (pad.inputStart == 1 && ruleset.gameRules == 1)
                            creditsProgress += 3;
                    }

                    //GAME LOGIC

                    //check ID of current tetromino.
                    if (activeTet.id == 0)
                    {
                        if (currentTimer == (int)Field.timerType.LineClear)  //if timer is line clear and done, settle pieces and start ARE
                        {
                            if (timerCount == 0)
                            {
                                //settle pieces and start ARE
                                for (int i = 0; i < full.Count; i++)
                                {
                                    for (int j = full[i]; j > 0; j--)
                                    {
                                        for (int k = 0; k < 10; k++)
                                        {
                                            gameField[k][j] = gameField[k][j - 1];
                                        }
                                    }
                                    for (int k = 0; k < 10; k++)
                                    {
                                        gameField[k][0] = 0;
                                    }
                                }
                                
                                full.Clear();
                                currentTimer = (int)Field.timerType.ARE;
                                timerCount = ruleset.baseARE;
                                playSound(s_Impact);
                            }
                            else
                            {
                                timerCount--;
                                return;
                            }
                        }
                        //elseif timer is ARE and done, get next tetromino
                        else if (currentTimer == (int)Field.timerType.ARE)
                        {
                            if (timerCount == 0)
                            {
                                spawnPiece();
                            }
                            else
                            {
                                timerCount--;
                                return;
                            }
                        }
                    }
                    else  //else, check collision below
                    {
                        bool floored = false;

                        for (int i = 0; i < 4; i++)
                        {
                            if (activeTet.bits[i].y + 1 >= 21)
                            {
                                floored = true;
                                break;
                            }
                            else if (gameField[activeTet.bits[i].x][activeTet.bits[i].y + 1] != 0)
                            {
                                floored = true;
                                break;
                            }
                        }

                        if (floored == true)
                        {
                            //check lock delay if grounded
                            if (currentTimer == (int)Field.timerType.LockDelay)
                            {
                                //if lock delay up, place piece.
                                if (groundTimer == 0)
                                {
                                    if (level % 100 != 99 && level != (mode.endLevel - 1))
                                        level++;

                                    if (inCredits == true && creditsProgress >= ruleset.creditsLength)
                                    {
                                        endGame();
                                    }

                                    for (int i = 0; i < 4; i++)
                                    {
                                        gameField[activeTet.bits[i].x][activeTet.bits[i].y] = activeTet.id;
                                    }
                                    activeTet.id = 0;
                                    //check for full rows and screenclears

                                    int tetCount = 0;

                                    for (int i = 0; i < 20; i++)
                                    {
                                        int columnCount = 0;
                                        for (int j = 0; j < 10; j++)
                                        {
                                            if (gameField[j][i + 1] != 0)
                                            {
                                                columnCount++;
                                                tetCount++;
                                            }
                                        }
                                        if (columnCount == 10)
                                        {
                                            full.Add(i + 1);
                                            tetCount -= 10;
                                        }
                                    }

                                    if (full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                    {
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                                gameField[j][full[i]] = 0;
                                            level++;
                                        }
                                        //calculate combo!
                                        int bravo = 1;
                                        if (tetCount == 0)
                                        {
                                            bravoCounter++;
                                            bravo = 4;
                                        }
                                        combo = combo + (2 * full.Count) - 2;
                                        //give points
                                        if (!inCredits)
                                            score += ((int)Math.Ceiling((double)(level + full.Count) / 4) + softCounter) * full.Count * ((full.Count * 2) - 1) * bravo;
                                        if (comboing)
                                            score *= combo;
                                        else
                                            combo = 1;
                                        comboing = true;

                                        //check GM conditions
                                        if (GMflags.Count == 0 && level >= 300)
                                        {
                                            if (score >= 12000 && timer.elapsedTime <= 255000)
                                                GMflags.Add(true);
                                            else
                                                GMflags.Add(false);
                                        }
                                        else if (GMflags.Count == 1 && level >= 500)
                                        {
                                            if (score >= 40000 && timer.elapsedTime <= 450000)
                                                GMflags.Add(true);
                                            else
                                                GMflags.Add(false);
                                        }
                                        else if (GMflags.Count == 2 && level >= mode.endLevel)
                                        {
                                            level = 999;
                                            if (score >= 126000 && timer.elapsedTime <= 810000)
                                                GMflags.Add(true);
                                            else
                                                GMflags.Add(false);


                                            //check for awarding GM
                                            if (GMflags[0] && GMflags[1] && GMflags[2])
                                            {
                                                isGM = true;
                                            }
                                        }

                                        //update grade
                                        bool checking = true;
                                        while (checking == true)
                                        {
                                            if (grade < ruleset.gradePointsTGM1.Count - 1)
                                            {
                                                if (score >= ruleset.gradePointsTGM1[grade + 1])
                                                {
                                                    grade++;
                                                    playSound(s_Grade);
                                                }
                                                else
                                                    checking = false;
                                            }
                                            else
                                                checking = false;
                                        }

                                        if (level >= mode.endLevel && inCredits == false)
                                        {
                                            triggerCredits();
                                        }

                                        //start timer
                                        currentTimer = (int)Field.timerType.LineClear;
                                        timerCount = ruleset.baseLineClear;
                                        playSound(s_Clear);

                                    }
                                    else //start the ARE, check if new grav level
                                    {
                                        currentTimer = (int)Field.timerType.ARE;
                                        timerCount = ruleset.baseARE;

                                        combo = 1;
                                        comboing = false;

                                    }

                                    if (gravLevel < ruleset.gravLevelsTGM1.Count - 1)
                                    {
                                        if (level >= ruleset.gravLevelsTGM1[gravLevel + 1])
                                            gravLevel++;
                                    }

                                    playSound(s_Contact);

                                    if (mode.themes.Count != curTheme)
                                    if (level > mode.themes[curTheme + 1])
                                    {
                                        curTheme++;
                                        if (curTheme == 3)
                                            playMusic("level 2");
                                        if (curTheme == 5)
                                            playMusic("level 3");
                                        if (curTheme == 8)
                                            playMusic("level 4");
                                        if (curTheme == 10)
                                            playMusic("level 5");
                                    }

                                    return;

                                }
                                else
                                {
                                    gravCounter = 0;
                                    groundTimer--;
                                }
                            }
                            else
                            {
                                currentTimer = (int)Field.timerType.LockDelay;
                                playSound(s_Lock);
                                //timerCount = ruleset.baseLock;
                            }
                        }
                        else
                            currentTimer = (int)Field.timerType.ARE;



                        int blockDrop = 0;// make it here so we can drop faster


                        //check saved inputs and act on them accordingly


                        if (pad.inputV == 1 && ruleset.hardDrop == 1)
                        {
                            blockDrop = 19;
                            gravCounter = 0;
                        }
                        else if (pad.inputV == -1 && inputDelayV == 0)
                        {
                            blockDrop = 1;
                            softCounter++;
                            gravCounter = 0;
                            if (currentTimer == (int)Field.timerType.LockDelay)
                                groundTimer = 0;
                        }

                        if (pad.inputHold == 1 && ruleset.hold == true && swappedHeld == false)
                        {
                            Tetromino tempTet;
                            if (heldPiece != null)
                            {
                                tempTet = new Tetromino(heldPiece.id);
                                heldPiece = new Tetromino(activeTet.id);
                                activeTet = tempTet;
                                groundTimer = ruleset.baseLock;
                            }
                            else
                            {
                                heldPiece = new Tetromino(activeTet.id);
                                spawnPiece();
                                currentTimer = (int)Field.timerType.ARE;
                                timerCount = ruleset.baseARE;
                            }
                            swappedHeld = true;
                        }
                        int rot = (pad.inputRot1 | pad.inputRot3) - pad.inputRot2;
                        if (rot != 0)
                            rotatePiece(activeTet, rot);



                        if (pad.inputH == 1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            //check to the right of each bit
                            for (int i = 0; i < 4; i++)
                            {
                                if (activeTet.bits[i].x + 1 == 10)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x + 1][activeTet.bits[i].y] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    activeTet.bits[i].x++;
                                }
                            }
                        }
                        else if (pad.inputH == -1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            //check to the right of each bit
                            for (int i = 0; i < 4; i++)
                            {
                                if (activeTet.bits[i].x - 1 == -1)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x - 1][activeTet.bits[i].y] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    activeTet.bits[i].x -= 1;
                                }
                            }
                        }

                        //calc gravity LAST sso I-jumps are doable?
                        gravCounter += ruleset.gravTableTGM1[gravLevel]; //add our current gravity strength



                        for (int tempGrav = gravCounter; tempGrav >= 256; tempGrav = tempGrav - 256)
                        {
                            blockDrop++;
                        }
                        if (blockDrop > 0 && currentTimer != (int)Field.timerType.LockDelay)
                        {
                            gravCounter = 0;
                            tetGrav(activeTet, blockDrop);


                        }

                        //handle ghost piece logic
                        ghostPiece = activeTet.clone();

                        tetGrav(ghostPiece, 20);

                    }


                }
                else //gamerunning == false
                {
                    if (pad.inputStart == 1)
                    {
                        cont = true;
                    }
                    if (pad.inputPressedRot2)
                        exit = true;
                }
            }
        }

            private void spawnPiece()
            {
                //get next tetromino, generate another for "next"
                                if (ruleset.nextNum > 0)
                                {
                                    activeTet = nextTet[0];
                                    groundTimer = ruleset.baseLock;
                                    for (int i = 0; i < nextTet.Count - 1; i++)
                                    {
                                        nextTet[i] = nextTet[i + 1];
                                    }
                                    nextTet[nextTet.Count - 1] = generatePiece();
                                    switch(nextTet[nextTet.Count - 1].id)
                                    {
                                        case 1:
                                            playSound(s_Tet1);
                                            break;
                                        case 2:
                                            playSound(s_Tet2);
                                            break;
                                        case 3:
                                            playSound(s_Tet3);
                                            break;
                                        case 4:
                                            playSound(s_Tet4);
                                            break;
                                        case 5:
                                            playSound(s_Tet5);
                                            break;
                                        case 6:
                                            playSound(s_Tet6);
                                            break;
                                        case 7:
                                            playSound(s_Tet7);
                                            break;
                                    }
                                }
                                else
                                {
                                    activeTet = generatePiece();
                                    switch (activeTet.id)
                                    {
                                        case 1:
                                            playSound(s_Tet1);
                                            break;
                                        case 2:
                                            playSound(s_Tet2);
                                            break;
                                        case 3:
                                            playSound(s_Tet3);
                                            break;
                                        case 4:
                                            playSound(s_Tet4);
                                            break;
                                        case 5:
                                            playSound(s_Tet5);
                                            break;
                                        case 6:
                                            playSound(s_Tet6);
                                            break;
                                        case 7:
                                            playSound(s_Tet7);
                                            break;
                                    }
                                }

                                int intRot = 0;
                                if (pad.inputPressedRot1)
                                    intRot += 1;
                                if (pad.inputPressedRot2)
                                    intRot -= 1;
                                if (intRot != 0)
                                {
                                    rotatePiece(activeTet, intRot);
                                    playSound(s_PreRot);
                                }

                                gravCounter = 0;

                                bool blocked = false;

                                blocked = !emptyUnderTet(activeTet);

                                if (blocked)
                                {
                                    endGame();
                                }
                                softCounter = 0;

                                swappedHeld = false;
                            }

        private void endGame()
        {
            if (godmode == true && level != mode.endLevel)
            {
                gameField = new List<List<int>>();
                for (int i = 0; i < 10; i++)
                {
                    List<int> tempList = new List<int>();
                    for (int j = 0; j < 21; j++)
                    {
                        tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                    }
                    gameField.Add(tempList);
                }
            }
            else
            {
                gameRunning = false;
                //in the future, the little fadeout animation goes here!

                stopMusic();
                results = new GameResult();
                results.game = ruleset.gameRules - 1;
                results.username = "TEST";
                results.grade = grade;
                results.score = score;
                results.time = timer.elapsedTime;
                results.level = level;
                contTime.start();
                playMusic("results");
            }
        }

        private void triggerCredits()
        {
            timer.stop();
            inCredits = true;
            playMusic("credits");

        }

        private void rotatePiece(Tetromino tet, int p)
        {
            int xOffset = 0; //for kicks
            int yOffset = 0;

            switch (tet.id)
            {
                case 1: //I has two rotation states; KICKS ONLY IN NEW RULES
                    //check current rotation
                    //check positions based on p and rotation, if abovescreen or offscreen to the sides then add an offset
                    switch (tet.rotation)
                    {
                        case 0:
                            if (tet.bits[2].y - 1 >= 0 && tet.bits[2].y + 2 <= 19)
                            {
                                if (gameField[tet.bits[2].x][tet.bits[2].y - 1] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + 1] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + 2] == 0)
                                {
                                    tet.bits[0].x += 2;
                                    tet.bits[0].y += 2;
                                    tet.bits[1].x += 1;
                                    tet.bits[1].y += 1;
                                    tet.bits[3].x -= 1;
                                    tet.bits[3].y -= 1;

                                    tet.rotation = 1;
                                }
                            }
                            break;
                        case 1:
                            if (tet.bits[2].x - 2 >= 0 && tet.bits[2].x + 1 <= 9)
                            {
                                if (gameField[tet.bits[2].x - 2][tet.bits[2].y] == 0 && gameField[tet.bits[2].x - 1][tet.bits[2].y] == 0 && gameField[tet.bits[2].x + 1][tet.bits[2].y] == 0)
                                {
                                    tet.bits[0].x -= 2;
                                    tet.bits[0].y -= 2;
                                    tet.bits[1].x -= 1;
                                    tet.bits[1].y -= 1;
                                    tet.bits[3].x += 1;
                                    tet.bits[3].y += 1;

                                    tet.rotation = 0;
                                }
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
                                    if (tet.bits[1].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y - 1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + ((i % 3) - 1);
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + ((i % 3) - 1);
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += 1 + ((i % 3) - 1);
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    //TODO: test for upkick if TGM3
                                    if (ruleset.gameRules == 3 && ((gameField[tet.bits[0].x - 1 + xOffset][tet.bits[0].y] != 0 || gameField[tet.bits[0].x + 1 + xOffset][tet.bits[0].y] != 0) && tet.kicked == 0))
                                    {
                                        yOffset = -1;
                                        tet.kicked = 1;
                                    }

                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x - 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + 2 + yOffset] == 0 && gameField[tet.bits[3].x - 1 + xOffset + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1 + yOffset;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 2 + yOffset;
                                            tet.bits[3].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 2;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[3].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[3].x][tet.bits[3].y - 1] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + ((i % 3) - 1)][tet.bits[0].y + -2 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + -1 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += -1 + ((i % 3) - 1);
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[3].y + 1] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += 1 + xOffset + ((i % 3) - 1);
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
                                    if (tet.bits[1].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0) //test no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + ((i % 3) - 1)][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + -1 + ((i % 3) - 1)][tet.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + ((i % 3) - 1);
                                            tet.bits[0].y += -1 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + ((i % 3) - 1);
                                            tet.bits[2].y += 1 + yOffset;
                                            tet.bits[3].x += -1 + ((i % 3) - 1);
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++) //test the three x locations
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + xOffset + ((i % 3) - 1)][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].y += 1;

                                            tet.rotation = 0;

                                            break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (tet.bits[3].y - 1 < 0)
                                    {
                                        yOffset = 1;
                                    }
                                    else
                                    {
                                        if (gameField[tet.bits[3].x][tet.bits[3].y - 1] != 0) //test for no-spin scenarios
                                        {
                                            break;
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x - 1 + ((i % 3) - 1)][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + 1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y - 2 + yOffset] == 0 && gameField[tet.bits[3].x + 1 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += 1 + ((i % 3) - 1);
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 2] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 2;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].x += 1 + xOffset + ((i % 3) - 1);

                                            tet.rotation = 2;

                                            break;
                                        }
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
                                    if (tet.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + 1] != 0)
                                        {
                                            //if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0) //USE FOR OTHER SPIN
                                            //{
                                            break;
                                            //}
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y + 1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + ((i % 3) - 1)][tet.bits[2].y + -1 + yOffset] == 0 && gameField[tet.bits[3].x + 2 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + ((i % 3) - 1);
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + ((i % 3) - 1);
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += 2 + ((i % 3) - 1);
                                            tet.bits[3].y += yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[0].x - 1 + ((i % 3) - 1) < 0 || tet.bits[0].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + 2] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y + -1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 2;
                                            tet.bits[3].x += xOffset + ((i % 3) - 1);
                                            tet.bits[3].y += -1;

                                            tet.rotation = 2;

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
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + ((i % 3) - 1)][tet.bits[0].y + -2 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + -2 + ((i % 3) - 1)][tet.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += -2 + ((i % 3) - 1);
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y + 2] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += xOffset + ((i % 3) - 1);
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
                                    if (tet.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + 1] != 0)
                                        {
                                            if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0)
                                            {
                                                break;
                                            }
                                        }
                                        for (int i = 1; i < 4; i++)
                                        {
                                            if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                            {
                                                continue;
                                            }
                                            if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + ((i % 3) - 1)][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + ((i % 3) - 1)][tet.bits[3].y + -2 + yOffset] == 0)
                                            {
                                                tet.bits[0].x += 1 + ((i % 3) - 1);
                                                tet.bits[0].y += -1 + yOffset;
                                                tet.bits[1].x += ((i % 3) - 1);
                                                tet.bits[1].y += yOffset;
                                                tet.bits[2].x += -1 + ((i % 3) - 1);
                                                tet.bits[2].y += 1 + yOffset;
                                                tet.bits[3].x += ((i % 3) - 1);
                                                tet.bits[3].y += -2 + yOffset;

                                                tet.rotation = 3;

                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + 1] == 0 && gameField[tet.bits[3].x + -2 + xOffset + ((i % 3) - 1)][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += -2 + xOffset + ((i % 3) - 1);

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
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + ((i % 3) - 1)][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y + -2 + yOffset] == 0 && gameField[tet.bits[3].x + ((i % 3) - 1)][tet.bits[3].y + 1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += ((i % 3) - 1);
                                            tet.bits[3].y += 1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 2] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + 2 + xOffset + ((i % 3) - 1)][tet.bits[3].y + 1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 2;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].x += 2 + xOffset + ((i % 3) - 1);
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
                                    if (tet.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + 1] != 0)
                                        {
                                            if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y + 1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + ((i % 3) - 1)][tet.bits[2].y + -1 + yOffset] == 0 && gameField[tet.bits[3].x + ((i % 3) - 1)][tet.bits[3].y + -2 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + ((i % 3) - 1);
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + ((i % 3) - 1);
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += ((i % 3) - 1);
                                            tet.bits[3].y += -2 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + 2] == 0 && gameField[tet.bits[3].x + -2 + xOffset + ((i % 3) - 1)][tet.bits[3].y + 1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 2;
                                            tet.bits[3].x += -2 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].y += 1;

                                            tet.rotation = 2;

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
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + ((i % 3) - 1)][tet.bits[0].y + -2 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + ((i % 3) - 1)][tet.bits[3].y + 1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += -2 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += yOffset;
                                            tet.bits[3].x += ((i % 3) - 1);
                                            tet.bits[3].y += 1 + yOffset;

                                            tet.rotation = 3;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + -1] == 0 && gameField[tet.bits[3].x + 2 + xOffset + ((i % 3) - 1)][tet.bits[3].y] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += -1;
                                            tet.bits[3].x += 2 + xOffset + ((i % 3) - 1);

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
                                    if (tet.bits[1].y - 1 < 0)//test oob for bump
                                    {
                                        yOffset = 1;
                                    }
                                    else //test for special restrictions
                                    {
                                        if (gameField[tet.bits[1].x][tet.bits[1].y - 1] != 0)
                                        {
                                            break;
                                        }
                                        if (gameField[tet.bits[1].x][tet.bits[1].y + 1] != 0)
                                        {
                                            // if (gameField[tet.bits[0].x][tet.bits[0].y - 1] == 0) //for the otehr direction
                                            //{
                                            break;
                                            //}
                                        }
                                        for (int i = 1; i < 4; i++)
                                        {
                                            if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                            {
                                                continue;
                                            }
                                            if (gameField[tet.bits[0].x + 1 + ((i % 3) - 1)][tet.bits[0].y + -1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + -1 + ((i % 3) - 1)][tet.bits[2].y + 1 + yOffset] == 0 && gameField[tet.bits[3].x + -2 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0)
                                            {
                                                tet.bits[0].x += 1 + ((i % 3) - 1);
                                                tet.bits[0].y += -1 + yOffset;
                                                tet.bits[1].x += ((i % 3) - 1);
                                                tet.bits[1].y += yOffset;
                                                tet.bits[2].x += -1 + ((i % 3) - 1);
                                                tet.bits[2].y += 1 + yOffset;
                                                tet.bits[3].x += -2 + ((i % 3) - 1);
                                                tet.bits[3].y += yOffset;

                                                tet.rotation = 3;

                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + -1] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y] == 0 && gameField[tet.bits[2].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[2].y + 1] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y + 2] == 0)
                                        {
                                            tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += -1;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[2].y += 1;
                                            tet.bits[3].x += xOffset + ((i % 3) - 1);
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
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + -1 + ((i % 3) - 1)][tet.bits[0].y + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + -1 + yOffset] == 0 && gameField[tet.bits[2].x + 1 + ((i % 3) - 1)][tet.bits[2].y + -2 + yOffset] == 0 && gameField[tet.bits[3].x + 2 + ((i % 3) - 1)][tet.bits[3].y + -1 + yOffset] == 0)
                                        {
                                            tet.bits[0].x += -1 + ((i % 3) - 1);
                                            tet.bits[0].y += yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += -1 + yOffset;
                                            tet.bits[2].x += 1 + ((i % 3) - 1);
                                            tet.bits[2].y += -2 + yOffset;
                                            tet.bits[3].x += 2 + ((i % 3) - 1);
                                            tet.bits[3].y += -1 + yOffset;

                                            tet.rotation = 1;

                                            break;
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 1; i < 4; i++)
                                    {
                                        if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 2] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0 && gameField[tet.bits[2].x + -1 + xOffset + ((i % 3) - 1)][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y + -1] == 0)
                                        {
                                            tet.bits[0].x += 1 + xOffset + ((i % 3) - 1);
                                            tet.bits[0].y += 2;
                                            tet.bits[1].x += xOffset + ((i % 3) - 1);
                                            tet.bits[1].y += 1;
                                            tet.bits[2].x += -1 + xOffset + ((i % 3) - 1);
                                            tet.bits[3].x += xOffset + ((i % 3) - 1);
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
                            if (tet.bits[0].y - 2 < 0)
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
                                if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x + ((i % 3) - 1)][tet.bits[0].y - 2 + yOffset] == 0 && gameField[tet.bits[0].x + ((i % 3) - 1)][tet.bits[0].y - 1 + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 1 + ((i % 3) - 1);
                                    tet.bits[0].y += yOffset;
                                    tet.bits[1].x += ((i % 3) - 1);
                                    tet.bits[1].y += yOffset - 1;
                                    tet.bits[2].x += -1 + ((i % 3) - 1);
                                    tet.bits[2].y += yOffset;
                                    tet.bits[3].x += -2 + ((i % 3) - 1);
                                    tet.bits[3].y += (yOffset - 1);

                                    tet.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {
                                if (tet.bits[1].x - 1 + ((i % 3) - 1) < 0 || tet.bits[1].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x - 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y] == 0 && gameField[tet.bits[0].x + xOffset + ((i % 3) - 1)][tet.bits[0].y] == 0 && gameField[tet.bits[0].x + xOffset + ((i % 3) - 1)][tet.bits[0].y - 1] == 0 && gameField[tet.bits[0].x + 1 + xOffset + ((i % 3) - 1)][tet.bits[0].y - 1] == 0)
                                {
                                    tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                    tet.bits[1].y += 1;
                                    tet.bits[1].x += xOffset + ((i % 3) - 1);
                                    tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                    tet.bits[3].x += 2 + xOffset + ((i % 3) - 1);
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
                            if (tet.bits[0].y - 1 < 0)
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
                                if (tet.bits[2].x - 1 + ((i % 3) - 1) < 0 || tet.bits[2].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[1].x - 1 + ((i % 3) - 1)][tet.bits[0].y - 1 + yOffset] == 0 && gameField[tet.bits[1].x - 1 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 2 + ((i % 3) - 1);
                                    tet.bits[0].y += yOffset - 1;
                                    tet.bits[1].x += 1 + ((i % 3) - 1);
                                    tet.bits[1].y += yOffset;
                                    tet.bits[2].x += ((i % 3) - 1);
                                    tet.bits[2].y += yOffset - 1;
                                    tet.bits[3].x += -1 + ((i % 3) - 1);
                                    tet.bits[3].y += yOffset;

                                    tet.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {
                                if (tet.bits[2].x - 1 + ((i % 3) - 1) < 0 || tet.bits[2].x + 1 + ((i % 3) - 1) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x - 2 + xOffset + ((i % 3) - 1)][tet.bits[0].y + 1] == 0 && gameField[tet.bits[2].x + xOffset + ((i % 3) - 1)][tet.bits[2].y] == 0 && gameField[tet.bits[3].x + xOffset + ((i % 3) - 1)][tet.bits[3].y] == 0 && gameField[tet.bits[1].x + xOffset + ((i % 3) - 1)][tet.bits[1].y + 1] == 0)
                                {
                                    tet.bits[2].x += xOffset + ((i % 3) - 1);
                                    tet.bits[2].y += 1;
                                    tet.bits[1].x += xOffset - 1 + ((i % 3) - 1);
                                    tet.bits[0].x += (xOffset - 2) + ((i % 3) - 1);
                                    tet.bits[0].y += 1;
                                    tet.bits[3].x += (xOffset + 1) + ((i % 3) - 1);

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

        public Tetromino generatePiece()
        {
            Random piece = new Random();
            int tempID = 0;
            for (int j = 0; j < ruleset.genAttps; j++)
            {
                bool copy = false;
                tempID = piece.Next(7) + 1;
                for (int k = 0; k < lastTet.Count; k++)
                {
                    if (tempID == lastTet[k])
                    {
                        copy = true;
                    }
                }
                if (copy == false)
                    break;
            }
            for (int j = 0; j < lastTet.Count - 1; j++)
            {
                lastTet[j] = lastTet[j + 1];
            }

            Tetromino tempTet = new Tetromino(tempID);
            lastTet[lastTet.Count - 1] = tempTet.id;

            return tempTet;
        }

        public bool emptyUnderTet(Tetromino tet)
        {
            bool status = true;
            for (int i = 0; i < 4; i++)
            {
                if (gameField[tet.bits[i].x][tet.bits[i].y] != 0)
                {
                    status = false;
                    break;
                }
            }

            return status;
        }
        public void tetGrav(Tetromino tet, int i)
        {
            int g = 0;
            for (g = 0; g < i; g++)
            {
                bool breakout = false;
                for (int p = 0; p < 4; p++)
                {
                    if (gameField[tet.bits[p].x][tet.bits[p].y + g] != 0)
                    {
                        g = g - 1;
                        breakout = true;
                        break;
                    }
                }
                for (int p = 0; p < 4; p++)
                {
                    if (tet.bits[p].y + g == 20)
                    {
                        breakout = true;
                        break;
                    }
                }

                if (breakout)
                    break;
            }
            for (int p = 0; p < 4; p++)
            {
                tet.bits[p].y += g;
            }

            //failsafe for now, ugh
            if (!emptyUnderTet(tet))
            {
                for (int p =0; p < 4; p++)
                {
                    tet.bits[p].y--;
                }
            }
        }



        private void playSound(System.Windows.Media.MediaPlayer sound)
        {
            sound.Position = new TimeSpan(0);
            sound.Play();
        }

        private void playMusic(string song)
        {
            try
            {
                vorbisStream = new NAudio.Vorbis.VorbisWaveReader(@"Audio\" + song + ".ogg");
                LoopStream loop = new LoopStream(vorbisStream);
                soundList.Init(loop);
                soundList.Play();

            }
            catch (Exception)
            {
                MessageBox.Show("The file \"" + song + ".ogg\" was not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //throw;
            }
        }

        private void stopMusic()
        {
            soundList.Stop();
            soundList.Dispose();
        }
    }
}
