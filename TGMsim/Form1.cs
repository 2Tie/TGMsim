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


        Controller pad1 = new Controller();

        Image imgBuffer;
        Graphics graphics, drawBuffer;

        

        int menuState = 5; //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings

        Field field1;
        
        Tetromino tet1;
        

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ClientSize = new Size(1280, 780);

            

            imgBuffer = (Image)new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            graphics = this.CreateGraphics();
            drawBuffer = Graphics.FromImage(imgBuffer);
            
        }

        public void gameLoop()
        {
            timer.start();
            //field1.randomize();
            
            //tetrng.delete();
            field1 = new Field(pad1);

            while (this.Created)
            {
                startTime = timer.elapsedTime;
                //pad1.poll();
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
            //deal with game logic
            switch (menuState)
            {
                case 0: //title

                    break;
                case 5: //ingame
                    field1.logic();
                    break;
            }
        }

        void gameRender()
        {
            //draw temp BG so bleeding doesn't occur
            drawBuffer.FillRectangle(new SolidBrush(Color.Black), this.ClientRectangle);

            field1.draw(drawBuffer);

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
