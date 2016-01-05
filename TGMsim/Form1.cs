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
        double FPS = 60; //59.84 for TGM1, 61.68 for TGM2, 60.00 for TGM3
        long startTime;
        long interval;

        int menuSelection;

        Controller pad1 = new Controller();
        Rules rules = new Rules();

        Image imgBuffer;
        Graphics graphics, drawBuffer;

        Profile player;
        

        int menuState = 0; //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings

        Field field1;
        Login login;
        
        Tetromino tet1;
        

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ClientSize = new Size(1280, 780);

            interval = (long)TimeSpan.FromSeconds(1.0 / FPS).TotalMilliseconds;

            imgBuffer = (Image)new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            graphics = this.CreateGraphics();
            drawBuffer = Graphics.FromImage(imgBuffer);
            
        }

        public void gameLoop()
        {
            timer.start();
            //field1.randomize();
            
            //tetrng.delete();
            //field1 = new Field(pad1, rules);

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
            {  //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings
                case 0:
                    break;
                case 1:
                    break;
                case 4:
                    break;
            }

            switch (newMenu) //activate the new menu
            {  //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings
                case 0:
                    menuState = 0;
                    FPS = 60.00;
                    break;
                case 1:
                    menuState = 1;
                    login = new Login();
                    break;
                case 2:
                    menuState = 2;
                    menuSelection = 0;
                    break;

                case 4:
                    menuState = 5;
                    FPS = 59.84;
                    field1 = new Field(pad1, rules);
                    break;
            }

            menuState = newMenu;
        }

        void gameLogic()
        {
            pad1.poll();
            //deal with game logic
            switch (menuState)
            {
                case 0: //title
                    titleLogic();
                    break;
                case 1:
                    loginLogic();
                    break;
                case 4: //ingame
                    if (field1.gameRunning == false)
                    {
                        field1.results.username = player.name;
                        //Todo: save a hiscore

                    }
                    if (field1.cont == true)
                        field1 = new Field(pad1, rules);
                    field1.logic();
                    break;
            }
        }

        private void loginLogic()
        {

            login.logic(pad1);

            if (pad1.inputStart == 1 && login.loggedin)
            {
                pad1.inputStart = 0;
                player = login.temp;
                changeMenu(4);
            }
        }

        private void titleLogic()
        {

            if (pad1.inputStart == 1)
            {
                pad1.inputStart = 0;
                changeMenu(1);
            }
        }

        void gameRender()
        {
            //draw temp BG so bleeding doesn't occur
            drawBuffer.FillRectangle(new SolidBrush(Color.Black), this.ClientRectangle);


            switch (menuState)
            {
                case 0:
                    drawBuffer.DrawString("TGM sim title screen thingy", DefaultFont, new SolidBrush(Color.White), 500, 300);
                    drawBuffer.DrawString("Press Start", DefaultFont, new SolidBrush(Color.White), 550, 400);
                    break;
                case 1:
                    drawBuffer.DrawString("login", DefaultFont, new SolidBrush(Color.White), 100, 20);
                    login.render(drawBuffer);
                    break;
                case 4:
                    field1.draw(drawBuffer);
                    break;
            }

#if DEBUG
            SolidBrush debugBrush = new SolidBrush(Color.White);
            //denote debug
            drawBuffer.DrawString("DEBUG", DefaultFont, debugBrush, 20, 710);
            //draw the current inputs
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyUp) ? "1" : "0", DefaultFont, debugBrush, 28, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyLeft) ? "1" : "0", DefaultFont, debugBrush, 20, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyDown) ? "1" : "0", DefaultFont, debugBrush, 28, 740);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRight) ? "1" : "0", DefaultFont, debugBrush, 36, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot1) ? "1" : "0", DefaultFont, debugBrush, 50, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot2) ? "1" : "0", DefaultFont, debugBrush, 58, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot3) ? "1" : "0", DefaultFont, debugBrush, 66, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyHold) ? "1" : "0", DefaultFont, debugBrush, 50, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyStart) ? "1" : "0", DefaultFont, debugBrush, 74, 730);

#endif

            //draw the buffer, then set to refresh
            this.BackgroundImage = imgBuffer;
            this.Invalidate();

        }
        
    }
}
