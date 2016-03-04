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

        public List<int> medals = new List<int>() { 0, 0, 0, 0, 0, 0 };

        public List<int> sectionTimes = new List<int>();
        public List<bool> GMflags = new List<bool>();

        public bool isGM = false;

        public Tetromino heldPiece;

        public Tetromino ghostPiece;

        public GameTimer timer = new GameTimer();
        public GameTimer contTime = new GameTimer();
        public GameTimer startTime = new GameTimer();
        public GameTimer sectionTime = new GameTimer();
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
        public int gm2grade = 0;
        public int score = 0;
        public int combo = 1;
        public bool comboing = false;
        public int gradePoints = 0;
        public int gradeLevel = 0;
        public int gradeTime = 0;
        List<int> gravTable;

        public int starting = 1;
        public bool inCredits = false;
        int creditsProgress;
        public bool newHiscore = false;

        public int bravoCounter = 0;
        public int tetrises = 0;
        public int totalTets = 0;
        public int rotations = 0;
        bool recoverChecking = false;
        public int recoveries = 0;
        public int speedBonus = 0;

        public int softCounter = 0;

        Rules ruleset;
        public Mode mode;
        int curSection;

        public GameResult results;

        public bool cheating = false;
        public bool godmode = false;
        public bool bigmode = false;
        public bool g20 = false;
        public bool g0 = false;

        public List<int> activeGim = new List<int>();
        public int gimIndex = 0;
        int garbTimer = 0;

        public bool cont = false;
        public bool exit = false;

        List<System.Drawing.Color> tetColors = new List<System.Drawing.Color>();
        Color frameColour;

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
        System.Windows.Media.MediaPlayer s_Hold = new System.Windows.Media.MediaPlayer();

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

            s_Ready.Open(new Uri(Environment.CurrentDirectory + @"\Res\Audio\SE\SEP_ready.wav"));
            s_Go.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_go.wav"));
            s_Tet1.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino1.wav"));
            s_Tet2.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino2.wav"));
            s_Tet3.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino3.wav"));
            s_Tet4.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino4.wav"));
            s_Tet5.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino5.wav"));
            s_Tet6.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino6.wav"));
            s_Tet7.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino7.wav"));
            s_PreRot.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_prerotate.wav"));
            s_Contact.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_fixa.wav"));
            s_Lock.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_instal.wav"));
            s_Clear.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_disappear.wav"));
            s_Impact.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_fall.wav"));
            s_Grade.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_levelchange.wav"));
            s_Hold.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_hold.wav"));

            pad = ctlr;
            ruleset = rules;
            mode = m2;
            vorbisStream = music;

            Random random = new Random();

            activeTet = new Tetromino(0, mode.bigmode); //first piece cannot be S, Z, or O
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

            g20 = mode.g20;

            gravTable = ruleset.gravTableTGM1;
            if (ruleset.gameRules >= 4)
                gravTable = ruleset.gravTableTGM3;

            switch (mode.id)
            {
                case 0:
                    frameColour = Color.LightGray;
                    break;
                case 1:
                    frameColour = Color.DarkRed;
                    ruleset.baseARE = ruleset.delayTableDeath[0][0];
                    ruleset.baseARELine = ruleset.delayTableDeath[1][0];
                    ruleset.baseDAS = ruleset.delayTableDeath[2][0];
                    ruleset.baseLock = ruleset.delayTableDeath[3][0];
                    ruleset.baseLineClear = ruleset.delayTableDeath[4][0];
                    break;
                case 2:
                    frameColour = Color.DarkBlue;
                    ruleset.baseARE = ruleset.delayTableShirase[0][0];
                    ruleset.baseARELine = ruleset.delayTableShirase[1][0];
                    ruleset.baseDAS = ruleset.delayTableShirase[2][0];
                    ruleset.baseLock = ruleset.delayTableShirase[3][0];
                    ruleset.baseLineClear = ruleset.delayTableShirase[4][0];
                    break;
                case 3:
                    frameColour = Color.DarkGreen;
                    break;
            }

            if (mode.exam)
                frameColour = Color.Gold;

            speedBonus = mode.lvlBonus;

            gameRunning = true;
            starting = 1;

            
            while (gravLevel < gravTable.Count - 1) //update gravity
            {
                if (level + (speedBonus * 100) >= ruleset.gravLevelsTGM1[gravLevel + 1])
                    gravLevel++;
                else
                    break;
            }

            timer.start();
            startTime.start();
            sectionTime.start();
            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 21; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }

            updateMusic();
            //playMusic("Level 1");
            //playSound(s_Ready);
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
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Gray)), x, y + 25, width, height);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    int block = gameField[i][j];
                    if (block == 10)
                        drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x + 25 * i, y + j * 25, 25, 25); // WIP
                    else if (block == 9)
                        drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x + 25 * i, y + j * 25, 25, 25);
                    else if (block % 8 != 0)
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[block]), x + 25 * i, y + j * 25, 25, 25);

                    //outline
                    if (block % 8 != 0 && block != 10)
                    {
                        if (i > 0)
                            if (gameField[i - 1][j] == 0)//left
                                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(178, Color.White)), x + 25 * i, y + j * 25, 2, 25);
                        if (i < 9)
                            if (gameField[i + 1][j] == 0)//right
                                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(178, Color.White)), x + 25 * i + 23, y + j * 25, 2, 25);
                        if (j > 0)
                            if (gameField[i][j - 1] == 0)//down
                                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(178, Color.White)), x + 25 * i, y + j * 25, 25, 2);
                        if (j < 20)
                            if (gameField[i][j + 1] == 0)//up
                                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(178, Color.White)), x + 25 * i, y + j * 25 + 23, 25, 2);
                    }
                    
                }
            }

            //draw the frame
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y + height + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, 5, height + 10);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x + width, y - 5 + 25, 5, height + 10);

            //draw the current piece
            if (activeTet.id != 0)
            {
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (activeTet.bone == true)
                        drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x + 25 * activeTet.bits[i].x, y + 25 * activeTet.bits[i].y, 25, 25);
                    else
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
                        if (nextTet[i].bone == true)
                            drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x + i * 70 + 15 * nextTet[i].bits[j].x + 40, y + 15 * nextTet[i].bits[j].y - 75, 15, 15);
                        else
                            drawBuffer.FillRectangle(new SolidBrush(tetColors[nextTet[i].id]), x + i*70 + 15 * nextTet[i].bits[j].x + 40, y + 15 * nextTet[i].bits[j].y - 75, 15, 15);
                    }
                }
            }

            //draw the hold piece
            if(ruleset.hold == true && heldPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (heldPiece.bone == true)
                        drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x - 75 + 15 * heldPiece.bits[i].x, y - 50 + 15 * heldPiece.bits[i].y, 15, 15);
                    else
                        drawBuffer.FillRectangle(new SolidBrush(tetColors[heldPiece.id]), x - 75 + 15 * heldPiece.bits[i].x, y - 50 + 15 * heldPiece.bits[i].y, 15, 15);
                }
            }

            //draw the ghost piece
            if (level < mode.sections[0] && ghostPiece != null && activeTet.id == ghostPiece.id)
            {
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(100, tetColors[ghostPiece.id])), x + 25 * ghostPiece.bits[i].x, y + 25 * ghostPiece.bits[i].y, 25, 25);
                }
            }

            //draw ice
            if (checkGimmick(4))
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), x, y + 275, width, 250);


            //GUI
            Color gravColor, gravMeter;
            switch (ruleset.gameRules)
            {
                case 1:
                    gravColor = Color.White;
                    gravMeter = Color.Teal;
                    break;
                case 3:
                    gravColor = Color.Green;
                    gravMeter = Color.Orange;
                    break;
                default:
                    gravColor = Color.White;
                    gravMeter = Color.Orange;
                    break;

            }


            drawBuffer.FillRectangle(new SolidBrush(gravColor), 600, 550, 60, 8);
            drawBuffer.FillRectangle(new SolidBrush(gravMeter), 600, 550, (int)Math.Round(((double)gravTable[gravLevel] * 60) / ((Math.Pow(256, ruleset.gravType + 1) * 20))), 8);
            if (mode.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20)
                drawBuffer.FillRectangle(new SolidBrush(Color.Red), 600, 550, 60, 8);

            //SMALL TEXT
            //levels
            drawBuffer.DrawString(level.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 610, 530);
            if (mode.sections.Count == curSection)
                drawBuffer.DrawString(mode.sections[curSection - 1].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 610, 570);
            else
                drawBuffer.DrawString(mode.sections[curSection].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 610, 570);

            drawBuffer.DrawString(score.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 610, 400);

            if (ruleset.gameRules == 1)
            {
                drawBuffer.DrawString("Next Grade:", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 240);
                if (grade != ruleset.gradePointsTGM1.Count)
                    drawBuffer.DrawString(ruleset.gradePointsTGM1[grade + 1].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 260);
                else
                    drawBuffer.DrawString("999", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 260);
            }

            if (godmode)
                drawBuffer.DrawString("GodMode", SystemFonts.DefaultFont, new SolidBrush(Color.Orange), 20, 680);
            if (g0)
                drawBuffer.DrawString("0G Mode", SystemFonts.DefaultFont, new SolidBrush(Color.Orange), 20, 700);
            if (mode.bigmode)
                drawBuffer.DrawString("Big Mode", SystemFonts.DefaultFont, new SolidBrush(Color.Orange), 20, 720);

            //BIGGER TEXT
            drawBuffer.DrawString("Points", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 380);

            //GRADE TEXT
            if (ruleset.gameRules == 1)
                drawBuffer.DrawString(ruleset.gradesTGM1[grade].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 100);
            else if (ruleset.gameRules != 4)
                drawBuffer.DrawString(ruleset.gradesTGM1[gm2grade].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 100);

            //Starting things
            if (starting == 2)
                drawBuffer.DrawString("Ready", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (starting == 3)
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

            //draw the playfieldgrid
            //for (int i = 1; i < 11; i++)
                //drawBuffer.DrawLine(gridPen, x + 25 * i, y + 25, x + 25 * i, y + height + 25);
            //for (int i = 1; i < 22; i++)
                //drawBuffer.DrawLine(gridPen, x, y + 25 * i, x + width, y + 25 * i);

            SolidBrush debugBrush = new SolidBrush(Color.White);

            //tech stats
            drawBuffer.DrawString("Grav Level: " + gravTable[gravLevel].ToString() + " Bonus: " + speedBonus, SystemFonts.DefaultFont, debugBrush, 20, 760);
            if (!inCredits)
                drawBuffer.DrawString("Current Level: " + level, SystemFonts.DefaultFont, debugBrush, 200, 760);
            else
            {
                drawBuffer.DrawString("Credits", SystemFonts.DefaultFont, debugBrush, 200, 760);
                drawBuffer.DrawString("Credits Timer: " + creditsProgress.ToString(), SystemFonts.DefaultFont, debugBrush, 200, 750);
            }

            //game stats
            if (ruleset.gameRules == 1)
                drawBuffer.DrawString("Score: " + score, SystemFonts.DefaultFont, debugBrush, 90, 740);
            else
                drawBuffer.DrawString("gradeScore: " + gradePoints, SystemFonts.DefaultFont, debugBrush, 90, 740);
            drawBuffer.DrawString("Combo: " + combo, SystemFonts.DefaultFont, debugBrush, 90, 750);
            if (isGM)
                drawBuffer.DrawString("Grade: GM", SystemFonts.DefaultFont, debugBrush, 90, 730);
            else
                drawBuffer.DrawString("Grade: " + grade + "GM2: " + gm2grade, SystemFonts.DefaultFont, debugBrush, 90, 730);

            for (int i = 0; i < GMflags.Count; i++)
            {
                drawBuffer.DrawString("-*".Substring(Convert.ToInt32(GMflags[i]), 1), SystemFonts.DefaultFont, debugBrush, 200 + i * 8, 730);
            }

            if (ruleset.gameRules == 1)
                drawBuffer.DrawString("Bravos: " + bravoCounter, SystemFonts.DefaultFont, debugBrush, 200, 740);
            else
            {
                drawBuffer.DrawString(medals[0] + " " + medals[1] + " " + medals[2] + " " + medals[3] + " " + medals[4] + " " + medals[5], SystemFonts.DefaultFont, debugBrush, 200, 740);
                drawBuffer.DrawString(totalTets.ToString(), SystemFonts.DefaultFont, debugBrush, 400, 740);
            }

            if (newHiscore)
                drawBuffer.DrawString("New Hiscore", SystemFonts.DefaultFont, debugBrush, 200, 700);

            //time
            drawBuffer.DrawString(string.Format("{0,2:00}:{1,2:00}:{2,2:00}", min, sec, msec10), SystemFonts.DefaultFont, debugBrush, 100, 700);

            drawBuffer.DrawString("gimmicks - " + activeGim.Count.ToString(), SystemFonts.DefaultFont, debugBrush, 400, 700);
#endif
        }

        public void logic()
        {
            if (startTime.elapsedTime > 1000 && starting == 1)
            {
                starting = 2;
                //play READY
                playSound(s_Ready);
            }
            if (startTime.elapsedTime > 2000 && starting == 2)
            {
                starting = 3;
                //play GO
                playSound(s_Go);
            }
            if (startTime.elapsedTime > 3000 && starting == 3)
            {
                starting = 0;
            }

            if (starting == 0)
            {

                if (gameRunning == true)
                {
                    //timing logic
                    long temptimeVAR = (long)(timer.elapsedTime * ruleset.FPS / 60);
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

                    gradeTime++;
                    if (gradeTime > ruleset.decayRate[grade] && gradePoints != 0 && !comboing)
                    {
                        gradeTime = 0;
                        gradePoints--;
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
                                if (ruleset.gameRules == 3)
                                    timerCount = ruleset.baseARELine;
                                else
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
                    if (activeTet.id != 0)//else, check collision below
                    {

                        bool floored = false;

                        if (activeTet.id != 0)
                        {
                            for (int i = 0; i < activeTet.bits.Count; i++)
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

                                    //GIMMICKS

                                    if (mode.gimList.Count > 0)
                                    {
                                        if (mode.gimList.Count > gimIndex)
                                            if (mode.gimList[gimIndex].startLvl <= level)
                                            {
                                                activeGim.Add(mode.gimList[gimIndex].type);
                                                gimIndex++;
                                            }
                                        for (int i = 0; i < activeGim.Count; i++)
                                        {
                                            if (mode.gimList[gimIndex - activeGim.Count + i].endLvl <= level)
                                            {
                                                activeGim.RemoveAt(i);
                                            }
                                        }
                                    }

                                    if (checkGimmick(2))
                                        garbTimer += 1;

                                    if (inCredits == true && creditsProgress >= ruleset.creditsLength)
                                    {
                                        endGame();
                                    }

                                    for (int i = 0; i < activeTet.bits.Count; i++)
                                    {
                                        if (checkGimmick(1))
                                            gameField[activeTet.bits[i].x][activeTet.bits[i].y] = 8;
                                        else if (activeTet.bone == true)
                                            gameField[activeTet.bits[i].x][activeTet.bits[i].y] = 10;
                                        else
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
                                            if (!checkGimmick(4) || i < 10)
                                            {
                                                full.Add(i + 1);
                                                tetCount -= 10;
                                            }
                                        }
                                    }

                                    if (tetCount >= 150)
                                        recoverChecking = true;

                                    if (full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                    {
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                                gameField[j][full[i]] = 0;
                                            level++;
                                        }
                                        //calculate combo!

                                        if (full.Count == 4)
                                            tetrises++;

                                        int bravo = 1;
                                        if (tetCount == 0)
                                        {
                                            bravoCounter++;
                                            bravo = 4;
                                        }
                                        //give points
                                        if (ruleset.gameRules == 1)
                                        {
                                            combo = combo + (2 * full.Count) - 2;
                                            if (!inCredits)
                                            {
                                                if (softCounter > 20)
                                                    throw new IndexOutOfRangeException();
                                                int newscore = ((int)Math.Ceiling((double)(level + full.Count) / 4) + softCounter) * full.Count * ((full.Count * 2) - 1) * bravo;
                                                if (comboing)
                                                    newscore *= combo;

                                                score += newscore;
                                            }
                                        }
                                        else
                                        {
                                            if (mode.id == 0)
                                            {
                                                if (full.Count > 1)
                                                    combo++;
                                                if (!inCredits && mode.gradedBy == 1)
                                                    gradePoints += (int)(Math.Ceiling(ruleset.baseGradePts[full.Count - 1][grade] * ruleset.comboTable[full.Count - 1][combo]) * Math.Ceiling((double)level / 250));
                                            }
                                            if (mode.id == 1)
                                                if (level >= 500)
                                                    gradeLevel = 32;

                                        }
                                        comboing = true;

                                        //check GM conditions
                                        long temptime = (long)((timer.elapsedTime * ruleset.FPS) / 60);

                                        if (GMflags.Count == 0 && level >= 300)
                                        {
                                            if (score >= 12000 && temptime <= 255000)
                                                GMflags.Add(true);
                                            else
                                                GMflags.Add(false);
                                        }
                                        else if (GMflags.Count == 1 && level >= 500)
                                        {
                                            if (score >= 40000 && temptime <= 450000)
                                                GMflags.Add(true);
                                            else
                                                GMflags.Add(false);
                                        }
                                        else if (GMflags.Count == 2 && level >= mode.endLevel)
                                        {
                                            level = 999;
                                            if (score >= 126000 && temptime <= 810000)
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

                                        if (ruleset.gameRules == 1)
                                        {
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
                                        }
                                        else
                                        {
                                            if (gradePoints > 99)
                                            {
                                                if (grade < ruleset.gradesTGM1.Count - 1)
                                                {
                                                    grade++;
                                                    gradePoints = 0;
                                                    if (ruleset.gameRules != 4)
                                                        if (ruleset.gradeIntTGM2[grade] != gm2grade)
                                                        {
                                                            gm2grade++;
                                                        playSound(s_Grade);
                                                            }
                                                }
                                            }
                                        }

                                        if (mode.sections.Count != curSection)
                                            if (level >= mode.sections[curSection])
                                            {
                                                curSection++;

                                                //MUSIC
                                                updateMusic();

                                                //DELAYS
                                                if (mode.id == 0 || mode.id == 3)//Master
                                                {
                                                    if (ruleset.gameRules == 2)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTGM2[0][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTGM2[1][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTGM2[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTGM2[3][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    if (ruleset.gameRules == 3)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTAP[0][curSection - 4 + speedBonus];
                                                                ruleset.baseARELine = ruleset.delayTableTAP[1][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTAP[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTAP[3][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTAP[4][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }

                                                    if (ruleset.gameRules == 4)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 17:
                                                            case 16:
                                                            case 15:
                                                            case 14:
                                                            case 13:
                                                            case 12:
                                                            case 11:
                                                            case 10:
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTGM3[0][curSection - 4 + speedBonus];
                                                                ruleset.baseARELine = ruleset.delayTableTGM3[1][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTGM3[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTGM3[3][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTGM3[4][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                }
                                                if (mode.id == 1)//death
                                                {
                                                    switch (curSection)
                                                    {
                                                        case 9:
                                                        case 8:
                                                        case 7:
                                                        case 6:
                                                        case 5:
                                                            ruleset.baseARE = ruleset.delayTableDeath[0][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseARELine = ruleset.delayTableDeath[1][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseDAS = ruleset.delayTableDeath[2][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseLock = ruleset.delayTableDeath[3][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseLineClear = ruleset.delayTableDeath[4][ruleset.delayTableDeath.Count - 1];
                                                            break;
                                                        case 4:
                                                        case 3:
                                                        case 2:
                                                        case 1:
                                                        case 0:
                                                            ruleset.baseARE = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseARELine = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseDAS = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseLock = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseLineClear = ruleset.delayTableDeath[0][curSection];
                                                            break;
                                                    }
                                                }
                                                if (mode.id == 2 || mode.id == 4) //shirase
                                                {
                                                    switch (curSection)
                                                    {
                                                        case 13:
                                                        case 12:
                                                        case 11:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][curSection - 5];
                                                            break;
                                                        case 10:
                                                        case 9:
                                                        case 8:
                                                        case 7:
                                                        case 6:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][5];
                                                            break;
                                                        case 5:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][4];
                                                            break;
                                                        case 4:
                                                        case 3:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][3];
                                                            break;
                                                        case 2:
                                                        case 1:
                                                        case 0:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][curSection];
                                                            break;
                                                    }
                                                }
                                                

                                                //MEDALS
                                                if (ruleset.gameRules != 1)
                                                {
                                                    int sectime = 60000;
                                                    if (mode.id == 1 || mode.id == 2)
                                                        sectime = 45000;
                                                    if (sectionTime.elapsedTime < sectime - 10000 && medals[2] < 1)
                                                        medals[2] = 1;
                                                    if (sectionTime.elapsedTime < sectime - 5000 && medals[2] < 2)
                                                        medals[2] = 2;
                                                    if (sectionTime.elapsedTime < sectime && medals[2] < 3)
                                                        medals[2] = 3;

                                                    if (curSection == 3)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 1;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                        }
                                                    if (curSection == 7)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 2;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                        }
                                                    sectionTime.stop();

                                                    sectionTime.start();
                                                }
                                            }

                                        //check medal conditions!!
                                        if (ruleset.gameRules != 1)
                                        {
                                            //AC
                                            if (bravoCounter == 1 && medals[0] == 0)
                                                medals[0] = 1;
                                            if (bravoCounter == 2 && medals[0] == 1)
                                                medals[0] = 2;
                                            if (bravoCounter == 3 && medals[0] == 2)
                                                medals[0] = 3;

                                            //RO
                                            
                                            //ST


                                            //SK
                                            int tt = 1;
                                            if (mode.id == 1 || mode.id == 2)
                                                tt = 2;
                                            if (mode.bigmode == true)
                                                tt = 10;
                                            if (tetrises == (int)(Math.Ceiling((double)10/tt)) && medals[3] == 0)
                                                medals[3] = 1;

                                            //RE
                                            if (recoverChecking == true && tetCount <= 70)
                                                recoveries++;
                                            if (recoveries == 1 && medals[4] == 0)
                                                medals[4] = 1;
                                            if (recoveries == 2 && medals[4] == 0)
                                                medals[4] = 2;
                                            if (recoveries == 4 && medals[4] == 0)
                                                medals[4] = 3;
                                            //CO
                                            int big = 1;
                                            if (mode.bigmode == true)
                                                big = 2;
                                            if ((int)(Math.Ceiling((double)4 / big)) == combo)
                                                medals[5] = 1;
                                            if ((int)(Math.Ceiling((double)5 / big)) == combo)
                                                medals[5] = 2;
                                            if ((int)(Math.Ceiling((double)7 / big)) == combo)
                                                medals[5] = 3;
                                            
                                        }

                                        if (mode.endLevel != 0 && level >= mode.endLevel && inCredits == false)
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

                                    while (gravLevel < gravTable.Count - 1)
                                    {
                                        if (level + (speedBonus * 100) >= ruleset.gravLevelsTGM1[gravLevel + 1])
                                            gravLevel++;
                                        else
                                            break;
                                    }

                                    playSound(s_Contact);

                                    if (checkGimmick(2) && garbTimer >= 10)
                                    {
                                        raiseGarbage(1);
                                        garbTimer = 0;
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
                                tempTet = new Tetromino(heldPiece.id, mode.bigmode);
                                heldPiece = new Tetromino(activeTet.id, false);
                                activeTet = tempTet;
                                groundTimer = ruleset.baseLock;
                            }
                            else
                            {
                                heldPiece = new Tetromino(activeTet.id, false);
                                spawnPiece();
                                currentTimer = (int)Field.timerType.ARE;
                                timerCount = ruleset.baseARE;
                            }
                            swappedHeld = true;
                            s_Hold.Play();
                        }
                        int rot = (pad.inputRot1 | pad.inputRot3) - pad.inputRot2;
                        if (rot != 0)
                            rotatePiece(activeTet, rot);



                        if (pad.inputH == 1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            int dst = 1;
                            if (mode.bigmode)
                                dst = 2;
                            //check to the right of each bit
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (activeTet.bits[i].x + dst > 9)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x + dst][activeTet.bits[i].y] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    activeTet.bits[i].x += dst;
                                }
                            }
                        }
                        else if (pad.inputH == -1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            int dst = 1;
                            if (mode.bigmode)
                                dst = 2;
                            //check to the right of each bit
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (activeTet.bits[i].x - dst < 0)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x - dst][activeTet.bits[i].y] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    activeTet.bits[i].x -= dst;
                                }
                            }
                        }

                        //calc gravity LAST sso I-jumps are doable?


                        if (!g0)
                            for (int tempGrav = gravCounter; tempGrav >= Math.Pow(256, ruleset.gravType + 1); tempGrav = tempGrav - (int)Math.Pow(256, ruleset.gravType + 1))
                        {
                            blockDrop++;
                        }

                        gravCounter += gravTable[gravLevel]; //add our current gravity strength


                        if (g20 || (gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20))
                        {
                            blockDrop = 19;
                        }
                        

                        if (blockDrop > 0)// && currentTimer != (int)Field.timerType.LockDelay)
                        {
                            gravCounter = 0;
                            tetGrav(activeTet, blockDrop);


                        }



                        //handle ghost piece logic
                        if (activeTet.id != 0)
                        {
                            ghostPiece = activeTet.clone();

                            tetGrav(ghostPiece, 20);
                        }

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
                if (mode.bigmode && activeTet.id == 5)
                    activeTet.bits[0].x = 7;
                groundTimer = ruleset.baseLock;
                for (int i = 0; i < nextTet.Count - 1; i++)
                {
                    nextTet[i] = nextTet[i + 1];
                }
                nextTet[nextTet.Count - 1] = generatePiece();
                if (starting == 0)
                {
                    switch (nextTet[nextTet.Count - 1].id)
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
            }
            else
            {
                activeTet = generatePiece();
                if (starting == 0)
                {
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
            }

            int intRot = 0;
            if (pad.inputPressedRot1)
                intRot += 1;
            if (pad.inputPressedRot2)
                intRot -= 1;
            if (intRot != 0 && activeTet.id != 7)
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
            if (godmode == true && !inCredits)
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
                results.username = "CHEATS";
                if (ruleset.gameRules == 1)
                    results.grade = grade;
                else
                    results.grade = ruleset.gradeIntTGM2[grade];
                results.score = score;
                results.time = (int)((timer.elapsedTime * ruleset.FPS)/60);
                results.level = level;
                results.medals = medals;
                contTime.start();
                stopMusic();
                playMusic("results");
            }
        }

        private void triggerCredits()
        {
            if ((rotations * 5) / (totalTets * 6) >= 1)
            {
                medals[1] = 3;
                rotations = 0;
                totalTets = 0;
            }

            timer.stop();
            inCredits = true;
            stopMusic();
            playMusic("credits");

        }

        private void rotatePiece(Tetromino tet, int p)
        {
            int xOffset = 0; //for kicks
            int yOffset = 0;

            int bigOffset = 1;
            if (mode.bigmode)
                bigOffset = 2;

            rotations++;

            switch (tet.id)
            {
                case 1: //I has two rotation states; KICKS ONLY IN NEW RULES
                    //check current rotation
                    //check positions based on p and rotation, if abovescreen or offscreen to the sides then add an offset
                    switch (tet.rotation)
                    {
                        case 0:
                            if (tet.bits[2].y - (1 * bigOffset) >= 0 && tet.bits[2].y + (2 * bigOffset) <= 19)
                            {
                                if (gameField[tet.bits[2].x][tet.bits[2].y - (1 * bigOffset)] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (1 * bigOffset)] == 0 && gameField[tet.bits[2].x][tet.bits[2].y + (2 * bigOffset)] == 0)
                                {
                                    tet.bits[0].x += 2;
                                    tet.bits[0].y += 2;
                                    tet.bits[1].x += 1;
                                    tet.bits[1].y += 1;
                                    tet.bits[3].x -= 1;
                                    tet.bits[3].y -= 1;

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(1, 0);
                                        tet.bits[1].move(1, 0);
                                        tet.bits[2].move(1, 0);
                                        tet.bits[3].move(1, 0);
                                        tet.bits[4].move(4, 1);
                                        tet.bits[5].move(3, 0);
                                        tet.bits[6].move(2, -1);
                                        tet.bits[7].move(1, -2);
                                        tet.bits[8].move(4, 3);
                                        tet.bits[9].move(5, 2);
                                        tet.bits[10].move(-1, -2);
                                        tet.bits[11].move(0, -3);
                                        tet.bits[12].move(-2, 4);
                                        tet.bits[13].move(-1, 3);
                                        tet.bits[14].move(-3, 5);
                                        tet.bits[15].move(-2, 4);
                                    }

                                    tet.rotation = 1;
                                }
                            }
                            break;
                        case 1:
                            if (tet.bits[2].x - (2 * bigOffset) >= 0 && tet.bits[2].x + (1 * bigOffset) <= 9)
                            {
                                if (gameField[tet.bits[2].x - (2 * bigOffset)][tet.bits[2].y] == 0 && gameField[tet.bits[2].x - (1 * bigOffset)][tet.bits[2].y] == 0 && gameField[tet.bits[2].x + (1 * bigOffset)][tet.bits[2].y] == 0)
                                {
                                    tet.bits[0].x -= 2;
                                    tet.bits[0].y -= 2;
                                    tet.bits[1].x -= 1;
                                    tet.bits[1].y -= 1;
                                    tet.bits[3].x += 1;
                                    tet.bits[3].y += 1;

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(-1, 0);
                                        tet.bits[1].move(-1, 0);
                                        tet.bits[2].move(-1, 0);
                                        tet.bits[3].move(-1, 0);
                                        tet.bits[4].move(-4, -1);
                                        tet.bits[5].move(-3, 0);
                                        tet.bits[6].move(-2, 1);
                                        tet.bits[7].move(-1, 2);
                                        tet.bits[8].move(-4, -3);
                                        tet.bits[9].move(-5, -2);
                                        tet.bits[10].move(1, 2);
                                        tet.bits[11].move(0, 3);
                                        tet.bits[12].move(2, -4);
                                        tet.bits[13].move(1, -3);
                                        tet.bits[14].move(3, -5);
                                        tet.bits[15].move(2, -4);
                                    }

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
                                        if (tet.bits[1].x - ((1 + ((i % 3) - 1)) * bigOffset) < 0 || tet.bits[1].x + ((1 + ((i % 3) - 1)) * bigOffset) > 9)//test for OoB shenanigans
                                        {
                                            continue;
                                        }
                                        if (gameField[tet.bits[0].x + ((1 + ((i % 3) - 1)) * bigOffset)][tet.bits[0].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[1].x + ((((i % 3) - 1)) * bigOffset)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + ((((i % 3) - 1)) * bigOffset)][tet.bits[2].y + yOffset] == 0 && gameField[tet.bits[3].x + ((((i % 3) - 1)) * bigOffset)][tet.bits[3].y + yOffset] == 0)
                                        {
                                            tet.bits[0].x += 1 + ((i % 3) - 1);
                                            tet.bits[0].y += 1 + yOffset;
                                            tet.bits[1].x += ((i % 3) - 1);
                                            tet.bits[1].y += yOffset;
                                            tet.bits[2].x += -1 + ((i % 3) - 1);
                                            tet.bits[2].y += -1 + yOffset;
                                            tet.bits[3].x += 1 + ((i % 3) - 1);
                                            tet.bits[3].y += -1 + yOffset;

                                            if (mode.bigmode)
                                            {
                                                tet.bits[5].move(2, -2);
                                                tet.bits[6].move(2, -2);
                                                tet.bits[7].move(3, -3);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[2].move(0, 1);
                                                tet.bits[4].move(0, -1);
                                                tet.bits[5].move(-2, 4);
                                                tet.bits[6].move(-2, 4);
                                                tet.bits[7].move(-3, 5);
                                                tet.bits[8].move(0, 2);
                                                tet.bits[9].move(0, 2);
                                                tet.bits[10].move(0, 2);
                                                tet.bits[11].move(0, 2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[2].move(0, -1);
                                                tet.bits[3].move(2, 0);
                                                tet.bits[4].move(0, 1);
                                                tet.bits[5].move(2, -4);
                                                tet.bits[6].move(2, -4);
                                                tet.bits[7].move(3, -5);
                                                tet.bits[8].move(-4, -2);
                                                tet.bits[9].move(-4, -2);
                                                tet.bits[10].move(-4, -2);
                                                tet.bits[11].move(-4, -2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[3].move(-2, 0);
                                                tet.bits[5].move(-2, 2);
                                                tet.bits[6].move(-2, 2);
                                                tet.bits[7].move(-3, 3);
                                                tet.bits[8].move(4, 0);
                                                tet.bits[9].move(4, 0);
                                                tet.bits[10].move(4, 0);
                                                tet.bits[11].move(4, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[3].move(2, 0);
                                                tet.bits[5].move(2, -2);
                                                tet.bits[6].move(2, -2);
                                                tet.bits[7].move(3, -3);
                                                tet.bits[8].move(-4, 0);
                                                tet.bits[9].move(-4, 0);
                                                tet.bits[10].move(-4, 0);
                                                tet.bits[11].move(-4, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[5].move(-2, 2);
                                                tet.bits[6].move(-2, 2);
                                                tet.bits[7].move(-3, 3);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[2].move(0, -1);
                                                tet.bits[4].move(0, 1);
                                                tet.bits[5].move(2, -4);
                                                tet.bits[6].move(2, -4);
                                                tet.bits[7].move(3, -5);
                                                tet.bits[8].move(0, -2);
                                                tet.bits[9].move(0, -2);
                                                tet.bits[10].move(0, -2);
                                                tet.bits[11].move(0, -2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[2].move(0, 1);
                                                tet.bits[3].move(-2, 0);
                                                tet.bits[4].move(0, -1);
                                                tet.bits[5].move(-2, 4);
                                                tet.bits[6].move(-2, 4);
                                                tet.bits[7].move(-3, 5);
                                                tet.bits[8].move(4, 2);
                                                tet.bits[9].move(4, 2);
                                                tet.bits[10].move(4, 2);
                                                tet.bits[11].move(4, 2);
                                            }

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


                                            if (mode.bigmode)
                                            {
                                                tet.bits[4].move(1, -1);
                                                tet.bits[5].move(0, -2);
                                                tet.bits[6].move(2, -2);
                                                tet.bits[7].move(3, -3);
                                                tet.bits[8].move(0, 2);
                                                tet.bits[9].move(0, 2);
                                                tet.bits[10].move(0, 2);
                                                tet.bits[11].move(0, 2);
                                                tet.bits[12].move(2, 0);
                                                tet.bits[13].move(2, 0);
                                                tet.bits[14].move(2, 0);
                                                tet.bits[15].move(2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, 2);
                                                tet.bits[1].move(0, 2);
                                                tet.bits[2].move(0, 2);
                                                tet.bits[3].move(0, 2);
                                                tet.bits[4].move(-1, 2);
                                                tet.bits[5].move(-2, 3);
                                                tet.bits[6].move(-2, 5);
                                                tet.bits[7].move(-3, 4);
                                                tet.bits[12].move(2, -2);
                                                tet.bits[13].move(2, -2);
                                                tet.bits[14].move(2, -2);
                                                tet.bits[15].move(2, -2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, -2);
                                                tet.bits[1].move(0, -2);
                                                tet.bits[2].move(0, -2);
                                                tet.bits[3].move(2, -2);
                                                tet.bits[4].move(1, -2);
                                                tet.bits[5].move(2, -1);
                                                tet.bits[6].move(3, -5);
                                                tet.bits[7].move(2, -4);
                                                tet.bits[8].move(-2, 0);
                                                tet.bits[9].move(-2, 0);
                                                tet.bits[10].move(-2, 0);
                                                tet.bits[11].move(-2, 0);
                                                tet.bits[12].move(-4, -2);
                                                tet.bits[13].move(-4, -2);
                                                tet.bits[14].move(-4, -2);
                                                tet.bits[15].move(-4, -2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[3].move(-2, 0);
                                                tet.bits[4].move(-1, 1);
                                                tet.bits[6].move(-3, 2);
                                                tet.bits[7].move(-2, 3);
                                                tet.bits[8].move(2, -2);
                                                tet.bits[9].move(2, -2);
                                                tet.bits[10].move(2, -2);
                                                tet.bits[11].move(2, -2);
                                                tet.bits[12].move(0, 4);
                                                tet.bits[13].move(0, 4);
                                                tet.bits[14].move(0, 4);
                                                tet.bits[15].move(0, 4);
                                            }

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

                                                if (mode.bigmode)
                                                {
                                                    tet.bits[3].move(2, 0);
                                                    tet.bits[4].move(1, -1);
                                                    tet.bits[6].move(3, -2);
                                                    tet.bits[7].move(2, -3);
                                                    tet.bits[8].move(-2, 2);
                                                    tet.bits[9].move(-2, 2);
                                                    tet.bits[10].move(-2, 2);
                                                    tet.bits[11].move(-2, 2);
                                                    tet.bits[12].move(0, -4);
                                                    tet.bits[13].move(0, -4);
                                                    tet.bits[14].move(0, -4);
                                                    tet.bits[15].move(0, -4);
                                                }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[4].move(-1, 1);
                                                tet.bits[5].move(0, 2);
                                                tet.bits[6].move(-2, 2);
                                                tet.bits[7].move(-3, 3);
                                                tet.bits[8].move(0, -2);
                                                tet.bits[9].move(0, -2);
                                                tet.bits[10].move(0, -2);
                                                tet.bits[11].move(0, -2);
                                                tet.bits[12].move(-2, 0);
                                                tet.bits[13].move(-2, 0);
                                                tet.bits[14].move(-2, 0);
                                                tet.bits[15].move(-2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, -2);
                                                tet.bits[1].move(0, -2);
                                                tet.bits[2].move(0, -2);
                                                tet.bits[3].move(0, -2);
                                                tet.bits[4].move(1, -2);
                                                tet.bits[5].move(2, -3);
                                                tet.bits[6].move(2, -5);
                                                tet.bits[7].move(3, -4);
                                                tet.bits[12].move(-2, 2);
                                                tet.bits[13].move(-2, 2);
                                                tet.bits[14].move(-2, 2);
                                                tet.bits[15].move(-2, 2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, 2);
                                                tet.bits[1].move(0, 2);
                                                tet.bits[2].move(0, 2);
                                                tet.bits[3].move(-2, 2);
                                                tet.bits[4].move(-1, 2);
                                                tet.bits[5].move(-2, 1);
                                                tet.bits[6].move(-3, 5);
                                                tet.bits[7].move(-2, 4);
                                                tet.bits[8].move(2, 0);
                                                tet.bits[9].move(2, 0);
                                                tet.bits[10].move(2, 0);
                                                tet.bits[11].move(2, 0);
                                                tet.bits[12].move(4, 2);
                                                tet.bits[13].move(4, 2);
                                                tet.bits[14].move(4, 2);
                                                tet.bits[15].move(4, 2);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[4].move(1, -1);
                                                tet.bits[5].move(2, 0);
                                                tet.bits[6].move(2, -2);
                                                tet.bits[7].move(3, -3);
                                                tet.bits[8].move(0, -2);
                                                tet.bits[9].move(0, -2);
                                                tet.bits[10].move(0, -2);
                                                tet.bits[11].move(0, -2);
                                                tet.bits[12].move(-2, 0);
                                                tet.bits[13].move(-2, 0);
                                                tet.bits[14].move(-2, 0);
                                                tet.bits[15].move(-2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, 1);
                                                tet.bits[1].move(0, 1);
                                                tet.bits[4].move(-1, 3);
                                                tet.bits[5].move(0, 2);
                                                tet.bits[6].move(-2, 3);
                                                tet.bits[7].move(-3, 2);
                                                tet.bits[8].move(-4, 4);
                                                tet.bits[9].move(-4, 4);
                                                tet.bits[10].move(-4, 4);
                                                tet.bits[11].move(-4, 4);
                                                tet.bits[12].move(2, 0);
                                                tet.bits[13].move(2, 0);
                                                tet.bits[14].move(2, 0);
                                                tet.bits[15].move(2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, -1);
                                                tet.bits[1].move(0, -1);
                                                tet.bits[3].move(2, 0);
                                                tet.bits[4].move(1, -3);
                                                tet.bits[5].move(0, -4);
                                                tet.bits[6].move(2, -3);
                                                tet.bits[7].move(3, -2);
                                                tet.bits[12].move(-2, 0);
                                                tet.bits[13].move(-2, 0);
                                                tet.bits[14].move(-2, 0);
                                                tet.bits[15].move(-2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[3].move(-2, 0);
                                                tet.bits[4].move(-1, 1);
                                                tet.bits[5].move(-2, 2);
                                                tet.bits[6].move(-2, 2);
                                                tet.bits[7].move(-3, 3);
                                                tet.bits[8].move(4, -2);
                                                tet.bits[9].move(4, -2);
                                                tet.bits[10].move(4, -2);
                                                tet.bits[11].move(4, -2);
                                                tet.bits[12].move(2, 0);
                                                tet.bits[13].move(2, 0);
                                                tet.bits[14].move(2, 0);
                                                tet.bits[15].move(2, 0);
                                            }

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

                                                if (mode.bigmode)
                                                {
                                                    tet.bits[3].move(2, 0);
                                                    tet.bits[4].move(1, -1);
                                                    tet.bits[5].move(2, -2);
                                                    tet.bits[6].move(2, -2);
                                                    tet.bits[7].move(3, -3);
                                                    tet.bits[8].move(-4, 2);
                                                    tet.bits[9].move(-4, 2);
                                                    tet.bits[10].move(-4, 2);
                                                    tet.bits[11].move(-4, 2);
                                                    tet.bits[12].move(-2, 0);
                                                    tet.bits[13].move(-2, 0);
                                                    tet.bits[14].move(-2, 0);
                                                    tet.bits[15].move(-2, 0);
                                                }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[4].move(-1, 1);
                                                tet.bits[5].move(-2, 0);
                                                tet.bits[6].move(-2, 2);
                                                tet.bits[7].move(-3, 3);
                                                tet.bits[8].move(0, 2);
                                                tet.bits[9].move(0, 2);
                                                tet.bits[10].move(0, 2);
                                                tet.bits[11].move(0, 2);
                                                tet.bits[12].move(2, 0);
                                                tet.bits[13].move(2, 0);
                                                tet.bits[14].move(2, 0);
                                                tet.bits[15].move(2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, -1);
                                                tet.bits[1].move(0, -1);
                                                tet.bits[4].move(1, -3);
                                                tet.bits[5].move(0, -2);
                                                tet.bits[6].move(2, -3);
                                                tet.bits[7].move(3, -2);
                                                tet.bits[8].move(4, -4);
                                                tet.bits[9].move(4, -4);
                                                tet.bits[10].move(4, -4);
                                                tet.bits[11].move(4, -4);
                                                tet.bits[12].move(-2, 0);
                                                tet.bits[13].move(-2, 0);
                                                tet.bits[14].move(-2, 0);
                                                tet.bits[15].move(-2, 0);
                                            }

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

                                            if (mode.bigmode)
                                            {
                                                tet.bits[0].move(0, 1);
                                                tet.bits[1].move(0, 1);
                                                tet.bits[3].move(-2, 0);
                                                tet.bits[4].move(-1, 3);
                                                tet.bits[5].move(0, 4);
                                                tet.bits[6].move(-2, 3);
                                                tet.bits[7].move(-3, 2);
                                                tet.bits[12].move(2, 0);
                                                tet.bits[13].move(2, 0);
                                                tet.bits[14].move(2, 0);
                                                tet.bits[15].move(2, 0);
                                            }

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
                            else
                            {
                                //check if space has a piece there!
                            }

                            for (int i = 1; i < 4; i++)
                            {
                                if (tet.bits[1].x - ((1 + ((i % 3) - 1)) * bigOffset) < 0 || tet.bits[1].x + ((1 + ((i % 3) - 1)) * bigOffset) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x + (((i % 3) - 1) * bigOffset)][tet.bits[0].y - (2 * bigOffset) + yOffset] == 0 && gameField[tet.bits[0].x + (((i % 3) - 1) * bigOffset)][tet.bits[0].y - (1 * bigOffset) + yOffset] == 0 && gameField[tet.bits[1].x + (((i % 3) - 1) * bigOffset)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + (((i % 3) - 1) * bigOffset)][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 1 + ((i % 3) - 1);
                                    tet.bits[0].y += yOffset;
                                    tet.bits[1].x += ((i % 3) - 1);
                                    tet.bits[1].y += yOffset - 1;
                                    tet.bits[2].x += -1 + ((i % 3) - 1);
                                    tet.bits[2].y += yOffset;
                                    tet.bits[3].x += -2 + ((i % 3) - 1);
                                    tet.bits[3].y += (yOffset - 1);

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(-5, -3);
                                        tet.bits[4].move(-5, -2);
                                        tet.bits[5].move(-4, -1);
                                        tet.bits[6].move(-4, -1);
                                        tet.bits[7].move(0, -1);
                                        tet.bits[8].move(0, -1);
                                        tet.bits[9].move(0, -1);
                                        tet.bits[10].move(0, -1);
                                        tet.bits[11].move(0, -1);
                                        tet.bits[14].move(2, -1);
                                        tet.bits[15].move(2, -1);
                                    }

                                    tet.rotation = 1;

                                    break;
                                }
                            }
                            break;
                        case 1:
                            for (int i = 1; i < 4; i++)
                            {
                                if (tet.bits[1].x - ((1 + ((i % 3) - 1)) * bigOffset) < 0 || tet.bits[1].x + ((1 + ((i % 3) - 1)) * bigOffset) > 9)//test for OoB shenanigans
                                {
                                    continue;
                                }
                                if (gameField[tet.bits[0].x - ((1 + xOffset + ((i % 3) - 1)) * bigOffset)][tet.bits[0].y] == 0 && gameField[tet.bits[0].x + ((xOffset + ((i % 3) - 1)) * bigOffset)][tet.bits[0].y] == 0 && gameField[tet.bits[0].x + ((xOffset + ((i % 3) - 1)) * bigOffset)][tet.bits[0].y - (1 * bigOffset)] == 0 && gameField[tet.bits[0].x + ((1 + xOffset + ((i % 3) - 1)) * bigOffset)][tet.bits[0].y - (1 * bigOffset)] == 0)
                                {
                                    tet.bits[2].x += 1 + xOffset + ((i % 3) - 1);
                                    tet.bits[1].y += 1;
                                    tet.bits[1].x += xOffset + ((i % 3) - 1);
                                    tet.bits[0].x += -1 + xOffset + ((i % 3) - 1);
                                    tet.bits[3].x += 2 + xOffset + ((i % 3) - 1);
                                    tet.bits[3].y += 1;

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(5, 3);
                                        tet.bits[4].move(5, 2);
                                        tet.bits[5].move(4, 1);
                                        tet.bits[6].move(4, 1);
                                        tet.bits[7].move(0, 1);
                                        tet.bits[8].move(0, 1);
                                        tet.bits[9].move(0, 1);
                                        tet.bits[10].move(0, 1);
                                        tet.bits[11].move(0, 1);
                                        tet.bits[14].move(-2, 1);
                                        tet.bits[15].move(-2, 1);
                                    }

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
                                if (gameField[tet.bits[1].x + 1 + ((i % 3) - 1)][tet.bits[0].y - 1 + yOffset] == 0 && gameField[tet.bits[1].x + 1 + ((i % 3) - 1)][tet.bits[3].y + yOffset] == 0 && gameField[tet.bits[1].x + ((i % 3) - 1)][tet.bits[1].y + yOffset] == 0 && gameField[tet.bits[2].x + ((i % 3) - 1)][tet.bits[2].y + yOffset] == 0)
                                {
                                    tet.bits[0].x += 2 + ((i % 3) - 1);
                                    tet.bits[0].y += yOffset - 1;
                                    tet.bits[1].x += 1 + ((i % 3) - 1);
                                    tet.bits[1].y += yOffset;
                                    tet.bits[2].x += ((i % 3) - 1);
                                    tet.bits[2].y += yOffset - 1;
                                    tet.bits[3].x += -1 + ((i % 3) - 1);
                                    tet.bits[3].y += yOffset;

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(0, 2);
                                        tet.bits[4].move(3, -3);
                                        tet.bits[5].move(5, -2);
                                        tet.bits[6].move(4, -2);
                                        tet.bits[7].move(2, -1);
                                        tet.bits[10].move(0, -2);
                                        tet.bits[11].move(0, -2);
                                        tet.bits[14].move(0, -2);
                                        tet.bits[15].move(0, -2);

                                    }

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

                                    if (mode.bigmode)
                                    {
                                        tet.bits[0].move(0, -2);
                                        tet.bits[4].move(-3, 3);
                                        tet.bits[5].move(-5, 2);
                                        tet.bits[6].move(-4, 2);
                                        tet.bits[7].move(-2, 1);
                                        tet.bits[10].move(0, 2);
                                        tet.bits[11].move(0, 2);
                                        tet.bits[14].move(0, 2);
                                        tet.bits[15].move(0, 2);

                                    }

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
            totalTets++;
            Random piece = new Random();
            int tempID = 0;
            for (int j = 0; j < ruleset.genAttps; j++)
            {
                bool copy = false;
                tempID = piece.Next(7) + 1;

                

                for (int k = 0; k < lastTet.Count; k++)
                {
                    while (mode.easyGen == true && (tempID == 5 || tempID == 6))
                    {
                        tempID = piece.Next(7) + 1;
                    }
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

            Tetromino tempTet = new Tetromino(tempID, mode.bigmode);
            lastTet[lastTet.Count - 1] = tempTet.id;
            if (checkGimmick(3))
                tempTet.bone = true;
            return tempTet;
        }

        private bool checkGimmick(int gim)
        {
            for (int i = 0; i < activeGim.Count; i++)
            {
                if (activeGim[i] == gim)
                    return true;
            }
            return false;
        }

        private void raiseGarbage(int num)
        {
            for(int i = 0; i < num; i++)//skim the top
            {
                for (int j = 0; j < 10; j++ )
                    gameField[j][i] = 0;
            }
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameField[j][i] = gameField[j][i + num];
                }
            }
            for (int i = 0; i < num; i++)//works
                for (int j = 0; j < 10; j++)
                    if (gameField[j][20 - num] != 0)
                        gameField[j][20 - i] = 9;
        }

        public bool emptyUnderTet(Tetromino tet)
        {
            bool status = true;
            for (int i = 0; i < tet.bits.Count; i++)
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
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (gameField[tet.bits[p].x][tet.bits[p].y + g] != 0)
                    {
                        g = g - 1;
                        breakout = true;
                        break;
                    }
                }
                for (int p = 0; p < tet.bits.Count; p++)
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
            for (int p = 0; p < tet.bits.Count; p++)
            {
                tet.bits[p].y += g;
            }

            //failsafe for now, ugh
            if (!emptyUnderTet(tet))
            {
                for (int p =0; p < tet.bits.Count; p++)
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
                vorbisStream = new NAudio.Vorbis.VorbisWaveReader(@"Res\Audio\" + song + ".ogg");
                LoopStream loop = new LoopStream(vorbisStream);
                soundList.Init(loop);
                soundList.Play();

            }
            catch (Exception)
            {
                //MessageBox.Show("The file \"" + song + ".ogg\" was not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //throw;
            }
        }

        private void stopMusic()
        {
            soundList.Stop();
            soundList.Dispose();
        }

        private void updateMusic()
        {
            if (curSection + speedBonus == 0)
            {
                stopMusic();
                playMusic("Level 1");
            }
            if (curSection + speedBonus == 3)
            {
                stopMusic();
                playMusic("Level 2");
            }
            if (curSection + speedBonus == 5)
            {
                stopMusic();
                playMusic("Level 3");
            }
            if (curSection + speedBonus == 8)
            {
                stopMusic();
                playMusic("Level 4");
            }
            if (curSection + speedBonus == 10)
            {
                stopMusic();
                playMusic("Level 5");
            }
            if (curSection + speedBonus == 15)
            {
                stopMusic();
                playMusic("Level 6");
            }
        }
    }
}
