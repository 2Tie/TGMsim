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
using System.Drawing.Text;

namespace TGMsim
{
    class Field
    {

        public bool gameRunning;


        public struct vanPip
        {
            public long time;
            public int x, y;
        }
        public struct flashPip
        {
            public int time;
            public int x, y;
        }

        Tetromino activeTet;
        public List<Tetromino> nextTet = new List<Tetromino>();
        public Rotation RSYS = new R_ARS1();
        public Generator GEN;
        public Mode MOD;// = new M_Master2();

        public List<List<int>> gameField = new List<List<int>>();

        public List<int> full = new List<int>();

        public List<long> sectionTimes = new List<long>();

        public List<vanPip> vanList = new List<vanPip>();
        public List<flashPip> flashList = new List<flashPip>();
        int fadeout = 0;
        

        public Tetromino heldPiece;

        public Tetromino ghostPiece;
        
        public FrameTimer timer = new FrameTimer();
        public FrameTimer startTime = new FrameTimer();
        public FrameTimer sectionTime = new FrameTimer();
        public FrameTimer creditsPause = new FrameTimer();
        public FrameTimer bravoTime = new FrameTimer();
        public long masterTime = 0;

        public int swappedHeld;
        bool justSpawned;
        bool safelock = false;

        public int x, y, width, height;
        public enum timerType { ARE, DAS, LockDelay, LineClear, tgm1flash, GarbageRaise} ;
        public timerType currentTimer = 0;
        public int timerCount = 0;
        public int gravCounter = 0;
        public int gravLevel = 0;
        public int score = 0;
        public int combo = 1;
        public int gradePoints = 0;
        public int gradeLevel = 0;
        public int gradeTime = 0;
        List<int> gravTable;

        public int starting = 1;
        public bool inCredits = false;
        int creditsProgress;
        public bool newHiscore = false;

        public int bgtimer = 0;
        
        public int creditGrades = 0;//TODO: migrate out
        
        public GameRules ruleset;
        int curSection = 0;

        public GameResult results;
        public int seed = 0;

        public bool cheating = false;
        public bool godmode = false;
        public bool bigmode = false;
        public bool g20 = false;
        public bool g0 = false;
        public bool w4 = false;
        public bool toriless = false;

        public List<Mode.Gimmick> activeGim = new List<Mode.Gimmick>();
        public int gimIndex = 0;

        public bool cont = false;
        public bool exit = false;
        public bool record = false;
        public bool recorded = false;

        public bool isPlayback = false;

        List<Image> tetImgs = new List<Image>();
        List<Image> tetSImgs = new List<Image>();
        List<Image> bgs = new List<Image>();
        Image tetGem;
        Image boneGem;
        Image medalImg;
        Image gradeImg;
        Color frameColour;
        SolidBrush textBrush = new SolidBrush(Color.White);
        int bgAlpha = 180;//120

        PrivateFontCollection fonts = new PrivateFontCollection();
        Font f_Maestro;

        Pen gridPen = new Pen(new SolidBrush(Color.White));

        Controller pad;
        int inputDelayH = 0, inputDelayDir = 0;

        public Field(Controller ctlr, GameRules rules, int startSeed)
        {
            x = 200;
            y = 50;
            width = 250;
            height = 500;

            tetImgs.Add(null);
            tetImgs.Add(Image.FromFile("Res/GFX/t1.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t6.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t5.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t4.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t3.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t7.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t2.png"));
            tetImgs.Add(null); //invisible
            tetImgs.Add(Image.FromFile("Res/GFX/t9.png")); //garbage
            tetImgs.Add(Image.FromFile("Res/GFX/t8.png")); //bone

            tetSImgs.Add(null);
            tetSImgs.Add(Image.FromFile("Res/GFX/s1.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s6.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s5.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s4.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s3.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s7.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s2.png"));
            tetSImgs.Add(null); //invisible
            tetSImgs.Add(Image.FromFile("Res/GFX/s9.png")); //garbage
            tetSImgs.Add(Image.FromFile("Res/GFX/s8.png")); //bone

            bgs.Add(Image.FromFile("Res/GFX/bgs/1.png"));
            bgs.Add(Image.FromFile("Res/GFX/bgs/2.png"));
            bgs.Add(Image.FromFile("Res/GFX/bgs/3.png"));
            bgs.Add(Image.FromFile("Res/GFX/bgs/4.png"));
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);

            tetGem = Image.FromFile("Res/GFX/o8.png");
            boneGem = Image.FromFile("Res/GFX/o9.png");

            medalImg = Image.FromFile("Res/GFX/medals.png");
            gradeImg = Image.FromFile("Res/GFX/grades.png");

            fonts.AddFontFile(@"Res\Maestro.ttf");
            FontFamily fontFam = fonts.Families[0];
            f_Maestro = new System.Drawing.Font(fontFam, 16, GraphicsUnit.Pixel);

            pad = ctlr;
            ruleset = rules;
            MOD = rules.mod;

            if (ruleset.rotation == 0) RSYS = new R_ARS1();
            if (ruleset.rotation == 1) RSYS = new R_ARS3();
            if (ruleset.rotation == 2) RSYS = new R_SEGA();


            if (startSeed != -1)
            {
                seed = startSeed;
                isPlayback = true;
                pad.enablePlayback();
            }
            else
            {
                Random r = new Random();
                seed = r.Next(Int32.MaxValue);
                isPlayback = false;
                pad.enableRecording();
            }

            switch (ruleset.generator)
            {
                case 0:
                    GEN = new Generator(seed);
                    break;
                case 1:
                    GEN = new G_ARS1(seed);
                    break;
                case 2:
                    GEN = new G_ARS2(seed);
                    break;
                case 3:
                case 4:
                    GEN = new G_ARS3(seed);
                    break;
                case 5:
                    GEN = new G_SEGA(seed);
                    break;
                case 6:
                    GEN = new G_ARS3Easy(seed);
                    break;
            }

            activeTet = new Tetromino(0, MOD.bigmode); //first piece cannot be S, Z, or O

            if (nextTet.Count == 0 && ruleset.nextNum > 0) //generate nextTet
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    nextTet.Add(generatePiece());
                }
            }

            g20 = MOD.g20;

            gravTable = ruleset.gravTable;

            MOD.grade = MOD.initialGrade;


            
            frameColour = MOD.border;

            if (ruleset.exam != -1)
                frameColour = Color.Gold;

            
            
            while (gravLevel < gravTable.Count - 1) //update gravity
            {
                if (MOD.level + (MOD.lvlBonus * 100) >= ruleset.gravLevels[gravLevel + 1])
                    gravLevel++;
                else
                    break;
            }

            if ((MOD.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && (int)ruleset.gameRules < 4)
                textBrush = new SolidBrush(Color.Gold);
            
            
            //secTet.Add(0);

            for (int i = 0; i < ruleset.fieldW; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < ruleset.fieldH; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }

            resetField();
            
        }

        public void resetField()
        {
            MOD.baseARE = MOD.delayTable[0][0];
            MOD.baseARELine = MOD.delayTable[1][0];
            MOD.baseDAS = MOD.delayTable[2][0];
            MOD.baseLock = MOD.delayTable[3][0];
            MOD.baseLineClear = MOD.delayTable[4][0];

            gameRunning = true;
            starting = 1;

            startTime.tick();
            sectionTime.tick();

            if (MOD.presetBoards)
            {
                loadBoard();
            }

            if (w4)
                w4ify();

            if (MOD.autoGarbage)
            {
                for (int i = 0; i < 13; i++)
                    raiseGarbage(true);
            }

            if (MOD.startWithRandField)
                randomize();

            currentTimer = 0;
            timerCount = 0;

            flashList = new List<flashPip>();
    }

        public void randomize()
        {
            Random rng = new Random();
            for (int i = 0; i < 80; i++)
            {
                gameField[rng.Next(10)][rng.Next(16)] = 9;
                
            }
        }

        public void loadBoard()
        {
            string b = "RES/Boards/" + MOD.boardsFile + ".brd";
            using (FileStream fs = new FileStream(b, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                br.BaseStream.Position += MOD.boardsProgress * 100;
                byte temp;
                int p;
                for (int i = 0; i < 100; i++)
                {
                    temp = br.ReadByte();
                    p = (temp >> 4) & 0xF;
                    if (p > 7)
                    {
                        p += 3;//shift gem blocks into field index
                        MOD.boardGems++;
                    }
                    gameField[(i * 2) % 10][(int)((i * 2) / 10)] = p;
                    p = temp & 0xF;
                    if (p > 7)
                    {
                        p += 3;//shift gem blocks into field index
                        MOD.boardGems++;
                    }
                    gameField[((i * 2) % 10) + 1][(int)((i * 2) / 10)] = p;
                }
            }
        }

        public void draw(Graphics drawBuffer)
        {
            //draw the background
            int bg = 9;
            if (curSection < 9) bg = curSection;
            if (bgtimer > 0 && bgtimer < 10) bg -= 1;
            if (bgs[bg] != null) drawBuffer.DrawImage(bgs[bg], 0, 0, 800, 600);
            if (bgtimer > 0) drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(255 - (25*Math.Abs(bgtimer-10)), Color.Black)), 0, 0, 800, 600);

            //draw the field
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(bgAlpha, Color.Black)), x, y + 25, width, height);

            //TODO: credits text?

            //draw the info bg
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(bgAlpha, Color.Black)), x + 275, y + 20, 100, height + 10);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    bool flashing = false;
                    for (int k = 0; k < flashList.Count; k++)
                    {
                        if (i == flashList[k].x && j == flashList[k].y)
                            flashing = true;
                    }
                    if (flashing)
                    {
                        drawBuffer.DrawImageUnscaled(tetImgs[9], x + 25 * i, y + height - (j * 25), 25, 25);
                    }
                    else
                    {

                        int block = gameField[i][j];
                        if (block == 8 || block == 0) //empty or invis, don't draw
                            ;
                        else if (block < 11)//garbage or bone
                            drawBuffer.DrawImageUnscaled(tetImgs[block], x + 25 * i, y + height - (j * 25), 25, 25);
                        else if (block == 11)//colourless gem
                            drawBuffer.DrawImageUnscaled(tetGem, x + 25 * i, y + height - (j * 25), 25, 25);
                        else if (block < 19) //gem block
                        {
                            //drawBuffer.FillEllipse(new SolidBrush(Color.FromArgb(130, Color.White)), x + 25 * i, y + height - (j * 25), 25, 25);
                            drawBuffer.DrawImageUnscaled(tetGem, x + 25 * i, y + height - (j * 25), 25, 25);
                        }
                        if (MOD.shadeStack && block != 8)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(130, Color.Black)), x + 25 * i, y + height - (j * 25), 25, 25);

                        //outline
                        if (MOD.outlineStack)
                            if (block % 8 != 0 && block != 10)
                            {
                                if (i > 0)
                                    if (gameField[i - 1][j] == 0)//left
                                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y + height - (j * 25), 3, 25);
                                if (i < 9)
                                    if (gameField[i + 1][j] == 0)//right
                                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i + 22, y + height - (j * 25), 3, 25);
                                if (j > 0)
                                    if (gameField[i][j - 1] == 0)//down
                                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y + height + 22 - (j * 25), 25, 3);
                                if (j < gameField[0].Count - 1)
                                    if (gameField[i][j + 1] == 0)//up
                                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y + height - (j * 25), 25, 3);
                            }
                    }
                }
            }

            //draw the frame
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y + height + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, 5, height + 10);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x + width, y - 5 + 25, 5, height + 10);

            int big = 1;
            if (activeTet.big)
                big = 2;

            //draw the ghost piece
            if (ghostPiece != null && activeTet.id == ghostPiece.id && MOD.showGhost)
            {
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (ghostPiece.bits[i].y < 20)
                    {
                        drawBuffer.DrawImage(tetImgs[ghostPiece.id], x + 25 * ghostPiece.bits[i].x, y + height - (25 * (ghostPiece.bits[i].y)), 25 * big, 25 * big);
                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(130, Color.Black)), x + 25 * ghostPiece.bits[i].x, y + height - (25 * (ghostPiece.bits[i].y)), 25 * big, 25 * big);
                    }
                }
            }

            //draw the current piece
            if (activeTet.id != 0)
            {
                for (int i = 0; i < activeTet.bits.Count; i++)
                {

                    if (activeTet.bone == true)
                        drawBuffer.DrawImage(tetImgs[10], x + 25 * activeTet.bits[i].x, y + height - (25 * activeTet.bits[i].y), 25 * big, 25 * big);
                    else
                    {
                        if (activeTet.groundTimer > 0)
                        {
                            drawBuffer.DrawImage(tetImgs[activeTet.id], x + 25 * activeTet.bits[i].x, y + height - (25 * activeTet.bits[i].y), 25 * big, 25 * big);
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb((MOD.baseLock - activeTet.groundTimer) * 127 / MOD.baseLock, Color.Black)), x + 25 * activeTet.bits[i].x, y + height - (25 * activeTet.bits[i].y), 25 * big, 25 * big);
                        }
                        //else
                            //drawBuffer.DrawImage(tetImgs[9], x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                    }
                }

                if (swappedHeld == 2)//draw held outline
                {
                    for (int i = 0; i < 4; i++)//for each piece
                    {
                        bool line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, left
                        {
                            if (activeTet.bits[i].x - big == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y + height - (25 * activeTet.bits[i].y), 3, 25 * big);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, up
                        {
                            if (activeTet.bits[i].x == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y - big == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y + height + 22 - (25 * (activeTet.bits[i].y - (big - 1))), 25 * big, 3);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, right
                        {
                            if (activeTet.bits[i].x + big == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x + 22 + 25 * (big-1), y + height - (25 * activeTet.bits[i].y), 3, 25 * big);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, down
                        {
                            if (activeTet.bits[i].x == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y + big == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y + height - (25 * activeTet.bits[i].y), 25 * big, 3);
                    }
                }
            }

            //draw the next piece
            if (ruleset.nextNum > 0)
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 0)//1st next drawn at full size
                        {
                            if (nextTet[i].bone == true)
                                drawBuffer.DrawImage(tetImgs[10], x + 25 * nextTet[i].bits[j].x, y - 95 + 25 * (21- nextTet[i].bits[j].y), 25, 25);
                                //drawBuffer.DrawImageUnscaled(tetImgs[10], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
                            else
                                drawBuffer.DrawImage(tetImgs[nextTet[i].id], x + 25 * nextTet[i].bits[j].x, y - 95 + 25 * (21-nextTet[i].bits[j].y), 25, 25);
                                //drawBuffer.DrawImageUnscaled(tetImgs[nextTet[i].id], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
                        }
                        else
                        {
                            if (nextTet[i].bone == true)
                                drawBuffer.DrawImageUnscaled(tetSImgs[10], x + i * 80 + 16 * nextTet[i].bits[j].x + 65, y + 16 * (21-nextTet[i].bits[j].y) - 75);
                            else
                                drawBuffer.DrawImageUnscaled(tetSImgs[nextTet[i].id], x + i * 80 + 16 * nextTet[i].bits[j].x + 65, y + 16 * (21-nextTet[i].bits[j].y) - 75);
                        }
                    }
                }
            }

            //draw the hold piece
            if(ruleset.hold == true && heldPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (heldPiece.bone == true)
                        drawBuffer.DrawImageUnscaled(tetSImgs[10], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * (21-heldPiece.bits[i].y));
                    else
                        if (swappedHeld != 0)
                            drawBuffer.DrawImageUnscaled(tetSImgs[9], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * (21-heldPiece.bits[i].y));
                        else
                            drawBuffer.DrawImageUnscaled(tetSImgs[heldPiece.id], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * (21-heldPiece.bits[i].y));
                }
            }

            //draw ice
            if (checkGimmick(4))
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), x, y + 275, width, 250);


            //GUI
            Color gravColor, gravMeter;
            switch (ruleset.gameRules)
            {
                case GameRules.Games.TGM1:
                    gravColor = Color.White;
                    gravMeter = Color.Teal;
                    break;
                case GameRules.Games.TGM2:
                case GameRules.Games.TGM3:
                    gravColor = Color.Green;
                    gravMeter = Color.Orange;
                    break;
                default:
                    gravColor = Color.White;
                    gravMeter = Color.Orange;
                    break;

            }//////ADJUST AFTER HERE

            if (ruleset.gameRules != GameRules.Games.SEGA) //grav meter
            {
                drawBuffer.FillRectangle(new SolidBrush(gravColor), x + 280, 505, 60, 8);
                drawBuffer.FillRectangle(new SolidBrush(gravMeter), x + 280, 505, (int)Math.Round(((double)gravTable[gravLevel] * 60) / ((Math.Pow(256, ruleset.gravType + 1) * 20))), 8);
                if (MOD.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20)
                    drawBuffer.FillRectangle(new SolidBrush(Color.Red), x + 280, 505, 60, 8);
            }

            //SMALL TEXT
            //levels
            drawBuffer.DrawString(MOD.level.ToString(), f_Maestro, textBrush, x + 290, 485);
            if (MOD.drawSec)
            {
                if (MOD.sections.Count == curSection)
                    drawBuffer.DrawString(MOD.sections[curSection - 1].ToString(), f_Maestro, textBrush, x + 290, 525);
                else
                    drawBuffer.DrawString(MOD.sections[curSection].ToString(), f_Maestro, textBrush, x + 290, 525);
            }
            //score
            drawBuffer.DrawString(MOD.score.ToString(), f_Maestro, textBrush, x + 280, 280);
            if (ruleset.gameRules == GameRules.Games.TGM1 && MOD.modeID == Mode.ModeType.MASTER)
            {
                drawBuffer.DrawString("NEXT GRADE:", f_Maestro, textBrush, x + 280, 140);
                if (MOD.grade != 18)
                    drawBuffer.DrawString(MOD.gradePointsTGM1[MOD.grade + 1].ToString(), f_Maestro, textBrush, x + 280, 160);
                else
                    drawBuffer.DrawString("??????", f_Maestro, textBrush, x + 280, 160);
            }

            if (godmode)
                drawBuffer.DrawString("GODMODE", f_Maestro, new SolidBrush(Color.Orange), 10, 530);
            if (g0)
                drawBuffer.DrawString("0G MODE", f_Maestro, new SolidBrush(Color.Orange), 10, 550);
            if (toriless)
                drawBuffer.DrawString("TORILESS MODE", f_Maestro, new SolidBrush(Color.Orange), 10, 570);
            //if (mode.bigmode)
            //drawBuffer.DrawString("BIG MODE", f_Maestro, new SolidBrush(Color.Orange), 20, 720);

            //BIGGER TEXT
            //if (ruleset.gameRules == 1 && mode.id == 0 )
            drawBuffer.DrawString("POINTS:", f_Maestro, textBrush, x + 280, 260);


            drawBuffer.DrawString(ruleset.GameName, f_Maestro, textBrush, 20, 20);
            drawBuffer.DrawString(MOD.ModeName, f_Maestro, textBrush, 20, 40);


            if (creditsProgress >= ruleset.creditsLength + 180)
                drawBuffer.DrawString("PUT THE BLOCK !!", f_Maestro, textBrush, 260, 585);

            //debug stuff
            //drawBuffer.DrawString(gradePoints.ToString(), SystemFonts.DefaultFont, textBrush, 20, 280);
            //drawBuffer.DrawString(ruleset.baseLineClear.ToString(), SystemFonts.DefaultFont, textBrush, 20, 290);

            /*string cTex = "REGRET!";
            if (ruleset.gameRules == 4 && coolTime.count > 0)
            {
                if (MOD.level % 100 >= 70)//cool
                {
                    cTex = "COOL!";
                }
                drawBuffer.DrawString(cTex, SystemFonts.DefaultFont, textBrush, x + 300, 350);
            }*/

            //if (bravoTime.count > 0)
                //drawBuffer.DrawString("BRAVO! X" + bravoCounter, SystemFonts.DefaultFont, textBrush, x + 280, 400);

            if (ruleset.gameRules == GameRules.Games.GMX || ruleset.gameRules == GameRules.Games.SEGA)
                drawBuffer.DrawString("LEVEL:", f_Maestro, textBrush, x + 280, 465);
            
            if (MOD.limitType == 3)//time limit?
                drawBuffer.DrawString(convertTime((long)((MOD.limit - timer.count) * ruleset.FPS / 60)), SystemFonts.DefaultFont, textBrush, x + 290, 550);
            else
                drawBuffer.DrawString(convertTime((long)(timer.count * ruleset.FPS / 60)), SystemFonts.DefaultFont, textBrush, x + 290, 550);

            //GRADE TEXT
            if (MOD.shiragrades == false)
            {
                if (MOD.showGrade && MOD.grade != -1)
                    drawGrade(drawBuffer, ruleset.grades[MOD.grade]);
            }
            else
                if (MOD.grade != 0)
                    drawGrade(drawBuffer, "S" + MOD.grade);

            if (ruleset.exam != -1)
                drawBuffer.DrawString("EXAM: " + ruleset.grades[ruleset.exam].ToString(), f_Maestro, textBrush, x + 280, 100);


            //DRAW MEDALS
            if ((int)ruleset.gameRules > 1)
                for (int i = 0; i < 6; i++)
                {
                    if (MOD.medals[i] != 0)
                        drawBuffer.DrawImage(medalImg, new Rectangle(x + 280 + (i % 3) * 20, 190 + (30 * (int)(Math.Floor((double)i / 3))), 25, 15), i * 26, (MOD.medals[i] - 1) * 16, 25, 16, GraphicsUnit.Pixel);
                }

            //garbage meter
            if(MOD.garbMeter && checkGimmick(2))
            {
                int segment = getActiveGimmickParameter(2) / 6;
                int n = (MOD.garbTimer / segment) - 1;
                drawBuffer.DrawString(n.ToString(), f_Maestro, textBrush, 20, 400);
                if (n > 6)
                    n = 6;
                if (n > 0)
                    drawBuffer.FillRectangle(new SolidBrush(Color.Red), x + width + 5, y + 20 + (height + 10) / 6 * (6 - n), 10, (height + 10) / 6 * n);
            }

            //fadeout
            if (fadeout > 22)
            {
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb((fadeout - 22) * 3, Color.Black)), x, y + 25, width, height);
            }



            //Starting things
            if (starting == 2)
                drawBuffer.DrawString("Ready", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (starting == 3)
                drawBuffer.DrawString("Go", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);


            //endgame stats
            if (gameRunning == false && fadeout == 92)
            {
                if (MOD.modeID == Mode.ModeType.MASTER)
                {
                    drawBuffer.DrawString("Grade: " + ruleset.grades[results.grade], SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 200);
                }
                drawBuffer.DrawString("Score: " + results.score, SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 210);
                drawBuffer.DrawString("Time: " + convertTime(results.time), SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 220);
                drawBuffer.DrawString("Name: " + results.username, SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 230);
                if (results.username == "CHEATER")
                {
                    throw new DivideByZeroException();
                }

                if (MOD.torikan)
                {
                    drawBuffer.DrawString("Torikan hit!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 250);
                    drawBuffer.DrawString(convertTime((long)(MOD.torDef * ruleset.FPS / 60)) + " over!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 260);
                }

                drawBuffer.DrawString("Press Start to", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 280);
                if (isPlayback)
                    drawBuffer.DrawString("restart the replay!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 290);
                else
                    drawBuffer.DrawString("reset the field!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 290);

                drawBuffer.DrawString("Press B to", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 310);
                drawBuffer.DrawString("return to menu!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 320);

                if (newHiscore)
                    drawBuffer.DrawString("New Hiscore registered!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 440);

                if (!isPlayback)
                {
                    if (!recorded)
                    {
                        drawBuffer.DrawString("Press Hold to", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 340);
                        drawBuffer.DrawString("record the replay!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 350);
                    }
                    else
                        drawBuffer.DrawString("Replay recorded!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 340);
                }

                if (MOD.modeID == Mode.ModeType.SHIRASE && MOD.grade != 0)
                    drawGrade(drawBuffer, "S" + (MOD.grade));
            }

            //replay stuff
            if(isPlayback)
            {
                //BG
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(bgAlpha, Color.Black)), 10, 80, 180, 500);

                //DATA
                drawBuffer.DrawString("REPLAY", f_Maestro, textBrush, 20, 60);
                drawBuffer.DrawString("length: " + pad.replay.Count, SystemFonts.DefaultFont, textBrush, 20, 80);
                drawBuffer.DrawString("progress: " + pad.progress, SystemFonts.DefaultFont, textBrush, 20, 100);

                //CONTROLS
                drawBuffer.FillEllipse(new SolidBrush(Color.Gray), 35, 180, 20, 20);
                drawBuffer.FillEllipse(new SolidBrush(Color.Red), 25 + (pad.inputH*20), 170 - (pad.inputV*20), 40, 40);

                Color unpressed = Color.Gray;
                Color pressed = Color.GreenYellow;

                drawBuffer.FillEllipse(new SolidBrush(pad.inputPressedRot1 ? pressed : unpressed), 90, 170, 20, 20);
                drawBuffer.FillEllipse(new SolidBrush(pad.inputPressedRot2 ? pressed : unpressed), 120, 165, 20, 20);
                drawBuffer.FillEllipse(new SolidBrush(pad.inputPressedRot3 ? pressed : unpressed), 150, 170, 20, 20);
                drawBuffer.FillEllipse(new SolidBrush(pad.inputPressedHold ? pressed : unpressed), 105, 200, 20, 20);

                //INTERNALS
                //drawBuffer.DrawString("gradepoints: " + MOD.gradePoints, SystemFonts.DefaultFont, textBrush, 20, 240);
                //drawBuffer.DrawString("combo: " + MOD.gradeCombo, SystemFonts.DefaultFont, textBrush, 20, 260);
                //drawBuffer.DrawString("internal grade: " + MOD.gm2grade, SystemFonts.DefaultFont, textBrush, 20, 280);
            }
            MOD.draw(drawBuffer, f_Maestro, isPlayback);
            
        }

        public void logic()
        {

            if (starting > 0) startTime.tick();

            if (startTime.count > 1000 && starting == 1)
            {
                starting = 2;
                //play READY
                Audio.playSound(Audio.s_Ready);
            }
            if (startTime.count > 2000 && starting == 2)
            {
                starting = 3;
                //play GO
                Audio.playSound(Audio.s_Go);
            }
            if (startTime.count > 3000 && starting == 3)
            {
                starting = 0;
                startTime.count = 0;
                timer.tick();
                MOD.updateMusic();
            }

            if (starting == 0)
            {


                if (gameRunning == true)
                {
                    //timing logic
                    if (timer.count > 0 && inCredits == false)
                        timer.tick();//16.66.... (17 16 17)

                    if (inCredits)
                        creditsPause.tick();
                    
                    if (bravoTime.count > 0) bravoTime.tick();

                    long temptimeVAR = (long)(timer.count * ruleset.FPS / 60);
                    


                    if (bgtimer > 0) bgtimer += 1;
                    if (bgtimer == 21) bgtimer = 0;

                    if (bravoTime.count > 1000)
                    {
                        bravoTime.count = 0;
                    }
                    justSpawned = false;
                    if (MOD.limit - timer.count <= 0 && MOD.limitType == 3)
                        endGame();

                    //vanishing logic
                    int vpcount = 0;
                    List<int> remCell = new List<int>();
                    foreach (var vP in vanList)
                    {
                        if (vP.time + ruleset.FPS * 5 <= creditsProgress)
                        {
                            gameField[vP.x][vP.y] = 8;
                            remCell.Add(vpcount);
                        }
                        vpcount++;
                    }
                    for (int i = 0; i < remCell.Count; i++ )
                    {
                        vanList.RemoveAt(remCell[remCell.Count - i - 1]);
                    }

                    List<int> remFlash = new List<int>();
                    vpcount = 0;
                    foreach (var fP in flashList)
                    {
                        if (fP.time == 0)
                            remFlash.Add(vpcount);
                        vpcount++;
                    }
                    for (int i = 0; i < remFlash.Count; i++)
                    {
                        flashList.RemoveAt(remFlash[remFlash.Count - i - 1]);
                    }

                    for (int i = 0; i < flashList.Count; i++ )
                    {
                        var fP = flashList[i];
                        fP.time -= 1;
                        flashList[i] = fP;
                    }


                        //check inputs and handle logic pertaining to them

                    //if (ruleset.gameRules == 6 && pad.inputPressedRot3 == true)//gm4 leftover
                        //inputDelayH = 0;
                    if (pad.inputH == 1 || pad.inputH == -1)
                    {
                        if (inputDelayH > 0)
                        {
                            inputDelayH--;
                        }
                        if (inputDelayH == -1 || inputDelayDir != pad.inputH)
                            inputDelayH = MOD.baseDAS;
                    }
                    else
                        inputDelayH = -1;

                    inputDelayDir = pad.inputH;

                    if(creditsProgress == 0 &&(((int)ruleset.gameRules > 1) == (creditsPause.count >= 3000) || MOD.modeID == Mode.ModeType.DEATH) && inCredits)
                    {
                        if (MOD.modeID == Mode.ModeType.TRAINING || MOD.modeID == Mode.ModeType.MINER || (MOD.modeID == Mode.ModeType.DYNAMO && MOD.variant == 0))
                            Audio.playMusic("crdtcas");
                        else if (MOD.creditsType < 2)
                            Audio.playMusic("crdtvanish");
                        else
                            Audio.playMusic("crdtinvis");
                    }

                    if (inCredits && (((int)ruleset.gameRules > 1) == (creditsPause.count >= 3000) || MOD.modeID == Mode.ModeType.DEATH))
                    {
                        creditsProgress++;
                        if (pad.inputStart == 1 && ruleset.gameRules == GameRules.Games.TGM1)
                            creditsProgress += 3;
                    }

                    MOD.onTick(timer.count, currentTimer);

                    //GAME LOGIC

                    //check ID of current tetromino.
                    if (activeTet.id == 0)
                    {
                        if(currentTimer == timerType.GarbageRaise)
                        {
                            --timerCount;
                            if (timerCount == -1) //garbagedelay of zero means the delay is nonexistant
                            {
                                switch (MOD.garbType)
                                {
                                    case Mode.GarbType.COPY:
                                        raiseGarbage(1);
                                        break;
                                    case Mode.GarbType.RANDOM:
                                        raiseGarbage(true);
                                        break;
                                    case Mode.GarbType.FIXED:
                                        raiseGarbageSlice();
                                        break;
                                }
                                if (full.Count > 0)
                                {
                                    currentTimer = timerType.LineClear;
                                    timerCount = MOD.baseLineClear;
                                }
                                else
                                {
                                    currentTimer = timerType.ARE;
                                    timerCount = MOD.baseARE;
                                }
                            }
                        }
                        if (currentTimer == timerType.LineClear)  //if timer is line clear and done, settle pieces and start ARE
                        {
                            if (timerCount == 0)
                            {
                                //settle pieces and start ARE
                                for (int i = 0; i < full.Count; i++)
                                {
                                    for (int j = full[i]; j < gameField[0].Count-1; j++)
                                    {
                                        for (int k = 0; k < 10; k++)
                                        {
                                            gameField[k][j] = gameField[k][j + 1];
                                        }
                                        for (int c = 0; c < vanList.Count; c++ )
                                        {
                                            if (vanList[c].y == j)
                                            {
                                                var vP = new vanPip();
                                                vP.time = vanList[c].time;
                                                vP.x = vanList[c].x;
                                                vP.y = vanList[c].y + 1;
                                                vanList[c] = vP;
                                            }
                                        }
                                        for (int d = 0; d < flashList.Count; d++)//update flash list, juuust in case of an instant lineclear
                                        {
                                            if (flashList[d].y == j)
                                            {
                                                var vP = new flashPip();
                                                vP.time = flashList[d].time;
                                                vP.x = flashList[d].x;
                                                vP.y = flashList[d].y + 1;
                                                flashList[d] = vP;
                                            }
                                        }
                                    }
                                    for (int k = 0; k < 10; k++)
                                    {
                                        gameField[k][gameField[0].Count-1] = 0;
                                    }
                                }

                                if (w4)
                                    w4ify();

                                if (MOD.autoGarbage)//and if no pieces in 9
                                {
                                    bool raise = true;
                                    for (int j = 0; j < 10; j++)
                                    {
                                        if (gameField[j][9] != 0)
                                        {
                                            raise = false;
                                        }
                                    }
                                    if (raise)
                                        raiseGarbage(true);
                                }

                                currentTimer = timerType.ARE;
                                if ((int)ruleset.gameRules >= 3)
                                    timerCount = MOD.baseARELine;
                                else
                                    timerCount = MOD.baseARE;
                                Audio.playSound(Audio.s_Impact);
                            }
                            else
                            {
                                timerCount--;
                                return;
                            }
                        }
                        //if timer is ARE and done, get next tetromino
                        if (currentTimer == timerType.ARE)
                        {
                            if (timerCount <= 0 && ((inCredits == (creditsPause.count > 3000)) || ruleset.gameRules == GameRules.Games.TGM1 || MOD.modeID == Mode.ModeType.DEATH))
                            {
                                full.Clear();
                                swappedHeld = 0;
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

                        //activeTet.floored = false;
                        activeTet.life++;
                        bool f = false;
                        if (activeTet.id != 0)
                        {
                            int big = 0;
                            if (MOD.bigmode)
                                big = 1;
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (activeTet.bits[i].y < 0)
                                    continue;
                                if (activeTet.bits[i].y - big - 1 <= -1)
                                {
                                    f = true;
                                    break;
                                }
                                else if (gameField[activeTet.bits[i].x][activeTet.bits[i].y - big - 1] != 0)
                                {
                                    f = true;
                                    break;
                                }
                            }
                        }

                        if (!activeTet.floored && f)
                            Audio.playSound(Audio.s_Contact);
                        activeTet.floored = f;

                        if (activeTet.floored == true)
                        {
                            //check lock delay if grounded
                            if (currentTimer == timerType.LockDelay)
                            {
                                //if lock delay up, place piece.
                                if (activeTet.groundTimer == 0)
                                {

                                    safelock = true;

                                    Audio.playSound(Audio.s_Lock);
                                    //GIMMICKS

                                    //garbage is handled in mode


                                    if (inCredits == true && creditsProgress >= ruleset.creditsLength)
                                    {
                                        endGame();
                                        return;
                                    }

                                    int lowY = 22;
                                    for (int i = 0; i < activeTet.bits.Count; i++)
                                    {
                                        if (lowY > activeTet.bits[i].y)
                                            lowY = activeTet.bits[i].y;
                                    }

                                    for (int i = 0; i < activeTet.bits.Count; i++) //SET THE PIECES
                                    {
                                        int big = 2;
                                        if (MOD.bigmode)
                                            big = 1;

                                        for (int j = 0; j < (big % 2) + 1; j++)
                                        {
                                            for (int k = 0; k < (big % 2) + 1; k++)
                                            {
                                                if (activeTet.bits[i].y - k < 0)
                                                    continue;
                                                if (checkGimmick(1) || MOD.creditsType == 2)
                                                    gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = 8;
                                                else if (activeTet.bone == true)
                                                    gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = 10;
                                                else
                                                    gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = activeTet.id;

                                                if (MOD.creditsType == 1)//vanishing?
                                                {
                                                    vanPip vP = new vanPip();
                                                    vP.time = creditsProgress;
                                                    vP.x = activeTet.bits[i].x + j;
                                                    vP.y = activeTet.bits[i].y - k;
                                                    vanList.Add(vP);
                                                }

                                                //add to flash, if not bone
                                                if (!activeTet.bone)
                                                {
                                                    flashPip fP = new flashPip();
                                                    fP.time = 2;
                                                    fP.x = activeTet.bits[i].x + j;
                                                    fP.y = activeTet.bits[i].y - k;
                                                    flashList.Add(fP);
                                                }
                                            }
                                        }
                                    }
                                    activeTet.id = 0;

                                    if (MOD.keepFieldSafe)//drop the field
                                    {
                                        int drop = 0;
                                        for (int d = 0; d < 4; d++)
                                            for (int j = 0; j < 10; j++)
                                                if (gameField[j][16 + d] != 0)
                                                    drop = d;
                                        if (drop > 0)
                                            dropField(drop);
                                    }

                                    //check for full rows and screenclears

                                    int tetCount = 0;

                                    for (int i = gameField[0].Count - 1; i >= 0; i--)
                                    {
                                        int columnCount = 0;
                                        for (int j = 0; j < 10; j++)
                                        {
                                            if (gameField[j][i] != 0)
                                            {
                                                columnCount++;
                                                tetCount++;
                                            }
                                        }
                                        if (columnCount == 10)
                                        {
                                            if (!checkGimmick(4) || i > 9)
                                            {
                                                full.Add(i);
                                                tetCount -= 10;
                                                //clear these from vanishing list
                                                int count = 0;
                                                List<int> remcell = new List<int>();
                                                foreach (var vP in vanList)
                                                {
                                                    if (vP.y == i)
                                                        remcell.Add(count);
                                                    count++;
                                                }
                                                for (int c = 0; c < remcell.Count; c++)
                                                {
                                                    vanList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                }

                                                remcell = new List<int>();
                                                foreach (var vP in flashList)
                                                {
                                                    if (vP.y == i)
                                                        remcell.Add(count);
                                                    count++;
                                                }
                                                for (int c = 0; c < remcell.Count; c++)
                                                {
                                                    flashList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                }
                                            }
                                        }
                                    }

                                    if (full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                    {
                                        int bigFull = full.Count;
                                        int oldLvl = MOD.level;
                                        if (MOD.bigmode)
                                            bigFull = bigFull / 2;
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                            {
                                                if (gameField[j][full[i]] > 8)
                                                    --MOD.boardGems;
                                                gameField[j][full[i]] = 0;
                                            }
                                        }

                                        MOD.onPut(activeTet, true);

                                        MOD.onClear(bigFull, activeTet, timer.count, tetCount == 0);

                                        //garbage is handled per mode

                                        //section stuff used to be here

                                        //check finish condition
                                        /*if (MOD.endLevel != 0 && MOD.level >= MOD.endLevel && inCredits == false)
                                        {
                                            triggerCredits();
                                        }*/

                                        //start timer
                                        currentTimer = timerType.LineClear;
                                        timerCount = MOD.baseLineClear;
                                        Audio.playSound(Audio.s_Clear);

                                    }
                                    else //start the ARE, check if new grav level
                                    {
                                        currentTimer = timerType.ARE;
                                        timerCount = MOD.baseARE;

                                        MOD.onPut(activeTet, false);
                                    }

                                    if (MOD.inCredits && !inCredits)
                                        triggerCredits();

                                    if (MOD.modeClear)
                                        if (MOD.continueMode)
                                        {
                                            resetField();
                                            MOD.modeClear = false;
                                            MOD.continueMode = false;
                                        }
                                        else
                                            endGame();

                                    /*if (MOD.sections.Count != curSection)//update section
                                        if (MOD.level >= MOD.sections[curSection])
                                        {
                                            curSection++;

                                            if ((int)ruleset.gameRules > 3)
                                                Audio.playSound(Audio.s_Section);

                                            if (MOD.torikan && !toriless)
                                            {
                                                currentTimer = timerType.LineClear;
                                                timerCount = MOD.baseLineClear;
                                                Audio.playSound(Audio.s_Clear);
                                                if (MOD.toriCredits)
                                                    triggerCredits();
                                                else
                                                    endGame();
                                                return;
                                            }

                                            
                                        }*/
                                    if (curSection != MOD.curSection)
                                    {
                                        curSection = MOD.curSection;
                                        //UPDATE BACKGROUND
                                        bgtimer = 1;
                                    }

                                    //update gimmicks
                                    bool thaw = false;
                                    if (MOD.gimList.Count > 0)
                                    {
                                        if (MOD.gimList.Count > gimIndex)
                                            if (MOD.gimList[gimIndex].startLvl <= MOD.level)
                                            {
                                                activeGim.Add(MOD.gimList[gimIndex]);
                                                gimIndex++;
                                            }
                                        for (int i = 0; i < activeGim.Count; i++)
                                        {
                                            if (MOD.gimList[gimIndex - activeGim.Count + i].endLvl <= MOD.level)
                                            {
                                                if (MOD.gimList[gimIndex - activeGim.Count + i].type == 4)
                                                    thaw = true;
                                                activeGim.RemoveAt(i);

                                            }
                                        }
                                    }

                                    if (checkGimmick(5))
                                        MOD.bigmode = true;

                                    //thaw ice
                                    if (thaw)
                                    {
                                        for (int i = 0; i < 22; i++)
                                        {
                                            int columnCount = 0;
                                            for (int j = 0; j < 10; j++)
                                                if (gameField[j][i] != 0)
                                                    columnCount++;
                                            if (columnCount == 10)
                                            {
                                                if (!checkGimmick(4) || i < 11)
                                                {
                                                    full.Add(i);
                                                    //clear these from vanishing list
                                                    int count = 0;
                                                    List<int> remcell = new List<int>();
                                                    foreach (var vP in vanList)
                                                    {
                                                        if (vP.y == i)
                                                            remcell.Add(count);
                                                        count++;
                                                    }
                                                    for (int c = 0; c < remcell.Count; c++)
                                                    {
                                                        vanList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                    }

                                                    remcell = new List<int>();
                                                    foreach (var vP in flashList)
                                                    {
                                                        if (vP.y == i)
                                                            remcell.Add(count);
                                                        count++;
                                                    }
                                                    for (int c = 0; c < remcell.Count; c++)
                                                    {
                                                        flashList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                    }
                                                }
                                            }
                                        }
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                                gameField[j][full[i]] = 0;
                                        }

                                    }//end section check

                                    //checkFade();

                                    while (gravLevel < gravTable.Count - 1)
                                    {
                                        if (MOD.level + (MOD.lvlBonus * 100) >= ruleset.gravLevels[gravLevel + 1])
                                            gravLevel++;
                                        else
                                            break;
                                    }

                                    if ((MOD.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && (int)ruleset.gameRules < 4 && (int)ruleset.gameRules > 0)
                                        textBrush = new SolidBrush(Color.Gold);

                                    //garbage
                                    if (checkGimmick(2) && MOD.garbTimer >= getActiveGimmickParameter(2) && (MOD.raiseGarbOnClear == true || full.Count == 0))
                                    {
                                        bool check = true;
                                        if (MOD.garbSafeLine != 0)
                                            for (int i = 19; i > MOD.garbSafeLine; --i)
                                            {
                                                for (int x = 0; x < 10; x++)
                                                    if (gameField[x][i] != 0)
                                                    {
                                                        check = false;
                                                        break;
                                                    }
                                                if (!check)
                                                    break;
                                            }
                                        if (check)
                                        {
                                            currentTimer = timerType.GarbageRaise;
                                            timerCount = MOD.garbDelay;
                                        }
                                        MOD.garbTimer = 0;
                                    }

                                    return;

                                }
                                else
                                {
                                    gravCounter = 0;
                                    activeTet.groundTimer--;
                                }
                            }
                            else
                            {
                                currentTimer = timerType.LockDelay;
                            }
                        }
                        //else
                            //currentTimer = timerType.ARE;



                        int blockDrop = 0;// make it here so we can drop faster


                        //check saved inputs and act on them accordingly

                        bool son = false;

                        if (pad.inputStart == 1 && MOD.startEnd)
                            endGame();

                        if (isPlayback == true && pad.superStart)
                            endGame();

                        if (pad.inputV == 1 && ruleset.hardDrop == 1)
                        {
                            blockDrop = 19;
                            gravCounter = 0;
                            son = true;
                        }
                        else if (pad.inputV == -1)
                        {
                            if (!activeTet.floored)
                            {
                                //blockDrop++;
                                activeTet.soft++;
                                MOD.onSoft();
                            }
                            else if (!safelock || !((int)ruleset.gameRules > 3 || MOD.modeID == Mode.ModeType.DEATH || ((int)ruleset.gameRules == 2 && MOD.level > 899) || ((int)ruleset.gameRules == 3 && MOD.level > 899)))
                            {
                                if(activeTet.floored)
                                    gravCounter = 0;
                                activeTet.groundTimer = 0;
                            }
                        }
                        else
                        {
                            safelock = false;
                        }

                        /*if (ruleset.gameRules == 6 && ruleset.hardDrop == 1 && pad.inputV == 1 && pad.inputPressedRot3 == true) //gm4 leftovers
                        {
                            gravCounter = 0;
                            activeTet.groundTimer = 0;
                        }*/

                        if (pad.inputHold == 1 && ruleset.hold == true && swappedHeld == 0)
                        {
                            hold();
                        }

                        int rot;
                        if (ruleset.gameRules != GameRules.Games.TGM3)
                            rot = (pad.inputRot1 | pad.inputRot3) - pad.inputRot2;
                        else
                            rot = pad.inputRot1 - pad.inputRot2;

                        if (rot != 0)
                        {
                            //if (activeTet.kicked != 0)
                                //activeTet.groundTimer = 1;
                            rotatePiece(activeTet, rot, false);
                        }

                        if (!justSpawned)
                        {
                            int big = 0;
                            if (activeTet.big)
                                big = 1;
                            if (pad.inputH != 0 && (inputDelayH < 1 || inputDelayH == MOD.baseDAS))
                            {
                                bool safe = true;

                                //check to the input direction of each bit
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    if (activeTet.bits[i].y < 0)
                                        continue;
                                    if (Math.Abs(activeTet.bits[i].x + ((big + 1) * ((pad.inputH + 1) / 2)) - 5) > 5 - ((ruleset.bigMove * big) + 1 - big))
                                    {
                                        safe = false;
                                        break;
                                    }
                                    if (gameField[activeTet.bits[i].x + (big * ((pad.inputH + 1) / 2)) + (pad.inputH* ((ruleset.bigMove * big) + 1 - big))][activeTet.bits[i].y] != 0)
                                    {
                                        safe = false;
                                        break;
                                    }
                                    if (activeTet.big == true)
                                    {
                                        if (gameField[activeTet.bits[i].x + (big * ((pad.inputH + 1) / 2)) + (pad.inputH* ((ruleset.bigMove * big) + 1 - big))][activeTet.bits[i].y - 1] != 0)
                                        {
                                            safe = false;
                                            break;
                                        }
                                    }
                                }
                                if (safe) //if it's fine, move them all by one
                                {
                                    activeTet.move(pad.inputH*((ruleset.bigMove*big)+1-big), 0);
                                    if (ruleset.lockType == 1) activeTet.groundTimer = MOD.baseLock;
                                }
                            }
                        }

                        //calc gravity LAST so I-jumps are doable?


                        if (!g0)//handle 
                            for (int tempGrav = gravCounter; tempGrav >= Math.Pow(256, ruleset.gravType + 1); tempGrav = tempGrav - (int)Math.Pow(256, ruleset.gravType + 1))
                            {
                                blockDrop++;
                            }

                        if (!activeTet.floored)
                        {
                            if (ruleset.gravType < 2)
                            {
                                if (pad.inputV == -1 && (gravTable[gravLevel] <= Math.Pow(256, ruleset.gravType + 1)))
                                {
                                    blockDrop += 1;
                                }
                                else
                                    gravCounter += gravTable[gravLevel]; //add our current gravity strength
                                if (gravCounter >= Math.Pow(256, ruleset.gravType + 1))
                                {
                                    blockDrop += 1;
                                    gravCounter = 0;
                                }
                            }
                            else
                            {
                                if (pad.inputV == -1 && gravTable[gravLevel] > 1)
                                {
                                    blockDrop = 1;
                                    gravCounter = 0;
                                }
                                else
                                    gravCounter++;
                                if (gravCounter >= gravTable[gravLevel])
                                {
                                    blockDrop = 1;
                                    gravCounter = 0;
                                }
                            }
                        }

                        if (g20 || (gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20))
                        {
                            blockDrop = 19;
                        }
                        

                        if (blockDrop > 0)// && currentTimer != (int)Field.timerType.LockDelay)
                        {
                            tetGrav(activeTet, blockDrop, false, son);
                        }

                        

                        //handle ghost piece logic
                        if (activeTet.id != 0)
                        {
                            ghostPiece = activeTet.clone();

                            tetGrav(ghostPiece, 22, true, false);
                        }

                    }


                }
                else //gamerunning == false
                {
                    if(fadeout < 92)
                    {
                        if (fadeout < ruleset.fieldH)
                        {
                            for (int i = 0; i < 10; i++)
                                if (gameField[i][fadeout] != 0)
                                    gameField[i][fadeout] = 9;
                        }
                        if (fadeout == 91)
                        {
                            pad.recording = false;
                            pad.playback = false;
                            Audio.playMusic("results");
                        }
                        fadeout++;
                    }
                    else
                    {
                        if (pad.inputStart == 1)
                        {
                            cont = true;
                            Audio.stopMusic();
                        }
                        if (pad.inputPressedRot2)
                        {
                            exit = true;
                            Audio.stopMusic();
                        }
                        if (pad.inputPressedHold && !isPlayback && !recorded)
                            record = true;
                    }
                }
            }
        }

        private void spawnPiece()
        {
            //get next tetromino, generate another for "next"
            if (ruleset.nextNum > 0)
            {
                activeTet = nextTet[0].cloneBig(MOD.bigmode);
                activeTet.groundTimer = MOD.baseLock;
                for (int i = 0; i < nextTet.Count - 1; i++)
                {
                    nextTet[i] = nextTet[i + 1];
                }
                nextTet[nextTet.Count - 1] = generatePiece();
                if (pad.inputPressedHold && ruleset.hold == true && swappedHeld == 0)
                {
                    hold();
                    Audio.playSound(Audio.s_Hold);
                }
                else if (starting == 0)
                {
                    switch (nextTet[nextTet.Count - 1].id)
                    {
                        case 1:
                            Audio.playSound(Audio.s_Tet1);
                            break;
                        case 2:
                            Audio.playSound(Audio.s_Tet2);
                            break;
                        case 3:
                            Audio.playSound(Audio.s_Tet3);
                            break;
                        case 4:
                            Audio.playSound(Audio.s_Tet4);
                            break;
                        case 5:
                            Audio.playSound(Audio.s_Tet5);
                            break;
                        case 6:
                            Audio.playSound(Audio.s_Tet6);
                            break;
                        case 7:
                            Audio.playSound(Audio.s_Tet7);
                            break;
                    }
                }
            }

            int intRot = 0;
            if (pad.inputPressedRot1)
                intRot += 1;
            if (pad.inputPressedRot2)
                intRot -= 1;
            if (intRot != 0 && pad.inputRot1 + pad.inputRot2 + pad.inputRot3 == 0)
            {
                if (rotatePiece(activeTet, intRot, true))
                    Audio.playSound(Audio.s_PreRot);
            }

            gravCounter = 0;

            /*if (level % 100 != 99 && level != (ruleset.endLevel - 1) && inCredits == false && ruleset.id != 9)
            {
                level++;
                if (level % 100 == 99 || level == (ruleset.endLevel - 1))
                    Audio.playSound(s_Bell);
            }*/
            MOD.onSpawn();

            bool blocked = false;

            blocked = !emptyUnderTet(activeTet);

            if (blocked)
            {
                endGame();
            }

            justSpawned = true;
            currentTimer = timerType.LockDelay;
        }

        private void hold()
        {

            Tetromino tempTet;
            if (heldPiece != null)
            {
                swappedHeld = 2;
                tempTet = heldPiece.cloneBig(MOD.bigmode);;
                heldPiece = activeTet.clone(false, false);
                activeTet = tempTet;
                activeTet.groundTimer = MOD.baseLock;
            }
            else
            {
                swappedHeld = 1;
                heldPiece = activeTet.clone(false, false);
                spawnPiece();
                timerCount = MOD.baseARE;
            }
        }

        private void endGame()
        {
            if (godmode == true && !inCredits && (!MOD.torikan || toriless))
            {
                clearField();
            }
            else
            {
                gameRunning = false;

                int lowY = 22;
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (lowY > activeTet.bits[i].y)
                        lowY = activeTet.bits[i].y;
                }

                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    int big = 2;
                    if (activeTet.big)
                        big = 1;

                    for (int j = 0; j < (big % 2) + 1; j++)
                    {
                        for (int k = 0; k < (big % 2) + 1; k++)
                        {
                            if (activeTet.bits[i].y - k < 0)
                                continue;
                            if (checkGimmick(1) || MOD.creditsType == 2)
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = 8;
                            else if (activeTet.bone == true)
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = 10;
                            else
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y - k] = activeTet.id;

                        }
                    }
                }
                activeTet.id = 0;

                results = new GameResult();

                if (inCredits)
                {
                    if (creditsProgress >= ruleset.creditsLength)//cleared credits
                    {
                        results.lineC = 1;

                        if (ruleset.gameRules == GameRules.Games.TGM3)
                        {
                            if (MOD.creditsType == 1)
                                creditGrades += 50;
                            if (MOD.creditsType == 2)
                                creditGrades += 160;
                        }

                        //check credits line clears, award orange line if applicable
                        //play game clear jingle?
                    }
                    else//topped out in credits
                    {
                        //if ((ruleset.gameRules == 2 || ruleset.gameRules == 3) && isGM == true)
                            //MOD.grade = 27;
                    }
                }

                for (; creditGrades > 100; creditGrades -= 100)
                {
                    MOD.grade++;
                }

                //handle TGM3 section modifyers
                if (ruleset.gameRules == GameRules.Games.TGM3)
                {
                    if (MOD.modeID == Mode.ModeType.MASTER)
                    {
                        /*foreach (int i in secCools)
                        {
                            MOD.grade += i;
                        }*/

                        if (MOD.grade < 0)
                            MOD.grade = 0;
                        if (MOD.grade > 32)
                            MOD.grade = 32;
                    }
                    /*if(ruleset.id == 2)
                    {
                        MOD.grade += secCools.Count + 1;
                        foreach (int i in secCools)
                        {
                            MOD.grade += i;
                        }
                    }*/
                }
                
                Audio.stopMusic();
                results.game = (int)ruleset.gameRules;
                results.username = "CHEATS";
                results.grade = MOD.grade;
                results.score = score;
                if (ruleset.gameRules == GameRules.Games.TGM1)
                    results.time = (long)((masterTime * ruleset.FPS) / 60);
                else
                    results.time = (long)((timer.count * ruleset.FPS) / 60);
                results.level = MOD.level;
                results.medals = MOD.medals;
                results.delay = pad.lag != 0;
            }
        }

        private void triggerCredits()
        {
            
            inCredits = true;
            Audio.stopMusic();
            if (ruleset.gameRules == GameRules.Games.TGM1 || MOD.modeID == Mode.ModeType.DEATH)
                Audio.playMusic("crdtvanish");
            else
            {
                Audio.playSound(Audio.s_GameClear); //allclear
                clearField();
            }

            if (MOD.modeID == Mode.ModeType.MASTER)
            {
                if ((ruleset.gameRules == GameRules.Games.TGM2 || ruleset.gameRules == GameRules.Games.TAP) && MOD.grade == 32) //tgm2 always has invisible
                    MOD.creditsType = 2;

                /*if (ruleset.gameRules == 4)
                {
                    if (MOD.grade > 26 && secCools.Sum() == 8)
                        MOD.creditsType = 2;
                    else
                        MOD.creditsType = 1;
                }*/
            }
        }

        private bool rotatePiece(Tetromino tet, int p, bool spawn)
        {
            bool success = true;
            tet.rotations++;
            Tetromino tmptet = RSYS.rotate(tet, p, gameField, (int)ruleset.gameRules, MOD.bigmode, spawn);
            if (activeTet == tmptet)
                success = false;
            activeTet = tmptet;
            return success;
        }

        public Tetromino generatePiece()
        {
            int tempID = GEN.pull() + 1;

            Tetromino tempTet = new Tetromino(tempID, false);//force non-big here so they're not stretched out in the queue
            if (checkGimmick(3))
                tempTet.bone = true;
            
            return tempTet;
        }

        private bool checkGimmick(int gim)
        {
            for (int i = 0; i < activeGim.Count; i++)
            {
                if (activeGim[i].type == gim)
                    return true;
            }
            return false;
        }

        private int getActiveGimmickParameter(int gim)
        {
            for(int i = 0; i < activeGim.Count; i++)
            {
                if (activeGim[i].type == gim)
                    return activeGim[i].parameter;
            }
            return -1;
        }

        private void raiseGarbage(int num)
        {
            for(int i = 0; i < num; i++)//skim the top
            {
                for (int j = 0; j < 10; j++ )
                    gameField[j][21-i] = 0;
            }
            for (int i = gameField[0].Count-1; i > 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameField[j][i] = gameField[j][i - num];
                }
            }
            for (int i = 0; i < num; i++)//works
                for (int j = 0; j < 10; j++)
                    if (gameField[j][num] != 0)
                        gameField[j][num - 1 - i] = 9;
        }

        private void raiseGarbage(bool rand)
        {
            for (int i = gameField[0].Count - 1; i > 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameField[j][i] = gameField[j][i - 1];
                }
            }
            if (rand)
            {
                int r = GEN.read() % 10;
                for (int j = 0; j < 10; j++)
                    if (j != r)
                        gameField[j][0] = 9;
                    else
                        gameField[j][0] = 0;
            }
        }

        private void raiseGarbageSlice()
        {
            if (MOD.garbTemplate.Count > 0)
            {
                    for (int j = 0; j < 10; j++)//skim the top
                        gameField[j][gameField[0].Count - 1] = 0;
                for (int i = gameField[0].Count - 1; i > 0; i--)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        gameField[j][i] = gameField[j][i-1];
                    }
                }
                for (int j = 0; j < 10; j++)
                    gameField[j][0] = MOD.garbTemplate[MOD.garbLine][j];
                MOD.garbLine++;
                if (MOD.garbLine >= MOD.garbTemplate.Count)
                    MOD.garbLine = 0;
            }
            else
                throw new Exception("Tried to use empty garbage template");
        }

        private void dropField(int num)
        {
            for (int p = 0; p < num; p++)
            {
                for (int i = 0; i < gameField[0].Count - 1; i++)
                    for (int j = 0; j < 10; j++)
                        gameField[j][i] = gameField[j][i + 1];
                for (int j = 0; j < 10; j++)
                    gameField[j][gameField[0].Count - 1] = 0;
                for (int d = 0; d < flashList.Count; d++)//update flashpips
                {
                    var vP = new flashPip();
                    vP.time = flashList[d].time;
                    vP.x = flashList[d].x;
                    vP.y = flashList[d].y - 1;
                    flashList[d] = vP;
                }
            }
        }

        public bool emptyUnderTet(Tetromino tet)
        {
            bool status = true;
            for (int i = 0; i < tet.bits.Count; i++)
            {
                if (tet.bits[i].y < 0)
                    continue;
                if (gameField[tet.bits[i].x][tet.bits[i].y] != 0)
                {
                    status = false;
                    break;
                }
                if(tet.big)
                {
                    if (tet.bits[i].y - 1 < 0)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x+1][tet.bits[i].y] != 0)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x][tet.bits[i].y-1] != 0)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x+1][tet.bits[i].y-1] != 0)
                    {
                        status = false;
                        break;
                    }
                }
            }

            return status;
        }
        public void tetGrav(Tetromino tet, int i, bool ghost, bool sonic)
        {
            int g = 0;
            int big = 0;
            if (tet.big) big = 1;
            for (g = 0; g < i; g++)
            {
                bool breakout = false;

                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y > gameField[0].Count - 1)
                        throw new Exception("piece over the top");//continue;
                    if (gameField[tet.bits[p].x][tet.bits[p].y - g - big] != 0)
                    {
                        g = g - 1;
                        breakout = true;
                        break;
                    }
                    if (tet.big)
                    {
                        if (gameField[tet.bits[p].x + 1][tet.bits[p].y - g - big] != 0)
                        {
                            g = g - 1;
                            breakout = true;
                            break;
                        }
                    }
                }
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y - g - big == 0)
                    {
                        breakout = true;
                        break;
                    }
                }

                if (breakout)
                    break;
            }
            tet.move(0, 0-g);

            //failsafe
            if (!emptyUnderTet(tet))
            {
                tet.move(0, 1);
                g -= 1;
            }
            if (g != 0 && ghost == false && activeTet.kicked == 0 && ruleset.lockType == 0)
                activeTet.groundTimer = MOD.baseLock;

            if (i == 19 && g20 == false && g > activeTet.sonic && sonic)
                activeTet.sonic = g;
        }

        private string convertTime(long numtime)
        {
            int min, sec, msec, msec10;
            min = (int)Math.Floor((double)numtime / 60000);
            numtime -= min * 60000;
            sec = (int)Math.Floor((double)numtime / 1000);
            numtime -= sec * 1000;
            msec = (int)numtime;
            msec10 = (int)(msec / 10);
            return string.Format("{0,2:00}:{1,2:00}:{2,2:00}", min, sec, msec10);
        }

        

        private void clearField()
        {
            gameField = new List<List<int>>();
            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }
        }

        private void w4ify()
        {
            List<List<int>> newField = new List<List<int>>();
            for (int i = 0; i < 3; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(9); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            for (int i = 0; i < 4; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(gameField[i+3][j]); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            for (int i = 0; i < 3; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(9); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            gameField = newField;
        }

        private void drawGrade(Graphics drawBuffer, string gd)
        {
            //string gd;
            int gold = 0;
            if (textBrush.Color == Color.Gold)
                gold = 78;

            for (int i = 0; i < gd.Length; i++)
            {
                int dex = "0123456789SmMVOKTG".IndexOf(gd.Substring(i, 1));
                drawBuffer.DrawImage(gradeImg, new Rectangle(x + 280 + i * 26, 70, 25, 25), new Rectangle(1 + (dex % 6) * 26, 1 + (int)Math.Floor((double)dex / 6) * 26 + gold, 25, 25), GraphicsUnit.Pixel);
            }
        }

        

        /*private void updateMusic()
        {
            int cools = 0;
            foreach (int i in secCools)
            {
                if (i == 1) cools++;
            }
            int section = curSection + cools;

            if(ruleset.id == 0)
            {
                if (ruleset.gameRules == 4)
                {
                    if (section >= 8 && Audio.song != "Level 3")
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 3");
                        return;
                    }
                }
                if (ruleset.gameRules == 2 || ruleset.gameRules == 3)
                {
                    if (section >= 9 && Audio.song != "Level 4")
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 4");
                        return;
                    }
                    if (section >= 7 && Audio.song != "Level 3")
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 3");
                        return;
                    }
                }
                if (section >= 5 && Audio.song != "Level 2")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                    return;
                }
                if (section >= 0 && Audio.song != "Level 1")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 1");
                    return;
                }
            }

            else if (ruleset.id == 1)//death
            {
                if (curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                    return;
                }
                if (curSection == 3)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                    return;
                }
                if (curSection == 5)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                    return;
                }
            }
            else if (ruleset.id == 2)//shirase
            {
                if (curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                    return;
                }
                if (curSection == 5)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                    return;
                }
                if (curSection == 7)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 5");
                    return;
                }
                if (curSection == 10)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 6");
                    return;
                }
                if (curSection == 14)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Shirase");
                    return;
                }
            }
            else if (ruleset.id == 10) //dynamo
            {
                section += MOD.variant*5;
                if (section >= 20 && Audio.song != "Level 5")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 5");
                    return;
                }
                if (section >= 15 && Audio.song != "Level 4")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                    return;
                }
                if (section >= 10 && Audio.song != "Level 3")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                    return;
                }
                if(section >= 5 && Audio.song != "Level 2")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                    return;
                }
                if(section >= 0 && Audio.song != "Level 1")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 1");
                    return;
                }
            }
            else
            {
                if(curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Casual 1");
                    return;
                }
            }
        }*/

        /*private void checkFade()
        {
            int cools = 0;
            int section = curSection + cools;

            if (MOD.modeID == Mode.ModeType.MASTER)//master
            {
                if (section == 4 && MOD.level % 100 > 84) Audio.stopMusic();
                if ((ruleset.gameRules == GameRules.Games.TGM2 || ruleset.gameRules == GameRules.Games.TAP) && section == 6 && MOD.level % 100 > 84) Audio.stopMusic();
                if ((ruleset.gameRules == GameRules.Games.TGM2 || ruleset.gameRules == GameRules.Games.TAP) && section == 8 && MOD.level % 100 > 84) Audio.stopMusic();
                if (ruleset.gameRules == GameRules.Games.TGM3 && section == 7 && MOD.level % 100 > 84) Audio.stopMusic();
            }
            if(MOD.modeID == Mode.ModeType.DEATH)//death
            {
                if (curSection == 2 && MOD.level % 100 > 84) Audio.stopMusic();
                if (curSection == 4 && MOD.level % 100 > 84) Audio.stopMusic();
            }
            if (MOD.modeID == Mode.ModeType.SHIRASE)//shirase
            {
                if (curSection == 4 && MOD.level % 100 > 84) Audio.stopMusic();
                if (curSection == 6 && MOD.level % 100 > 84) Audio.stopMusic();
                if (curSection == 9 && MOD.level % 100 > 84) Audio.stopMusic();
                if (curSection == 13 && MOD.level % 100 > 84) Audio.stopMusic();
            }
        }*/
    }
}