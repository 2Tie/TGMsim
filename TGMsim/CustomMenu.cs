using System.Drawing;

namespace TGMsim
{
    class CustomMenu
    {
        int pos = 0;
        public bool invPiece = false;
        public bool invPreview = false;
        public bool invStack = false;
        public bool big = false;
        public int grav = 0; //normal, 20G, 0G
        string[] gravnames = new string[] { "normal", "20G", "0G" };
        public int garbage = 0; //none, random, copy
        string[] garbnames = new string[] { "none", "random", "copy" };
        public int game = 0; //sega, tgm1, tap, tgm3, gmx
        string[] gamenames = new string[] { "SEGA", "TGM", "TAP", "TGM3", "GMX" };
        public int rotation = 0; //sega, shimizu, tgm, tgm3
        string[] rotnames = new string[] { "SEGA", "SEMIPRO", "TGM", "TGM3" };
        public int generator = 0; //sega, tgm1, tap, tgm3, tgm3ez
        string[] gennames = new string[] { "SEGA", "TGM", "TAP", "TGM3", "EZPZ" };

        int deltaH = 0, deltaV = 0;

        public void logic(Controller pad)
        {
            if(pad.inputV != deltaV)
            {
                deltaV = pad.inputV;
                if (pad.inputV != 0)
                {
                    pos -= pad.inputV;
                    if (pos < 0)
                        pos = 8;
                    if (pos > 8)
                        pos = 0;
                }
            }
            if (pad.inputH != deltaH)
            {
                deltaH = pad.inputH;
                if (pad.inputH != 0)
                {
                    switch(pos)
                    {
                        case 0:
                            invPiece = !invPiece;
                            break;
                        case 1:
                            invPreview = !invPreview;
                            break;
                        case 2:
                            invStack = !invStack;
                            break;
                        case 3:
                            big = !big;
                            break;
                        case 4:
                            grav += pad.inputH;
                            if (grav > 2)
                                grav = 0;
                            else if (grav < 0)
                                grav = 2;
                            break;
                        case 5:
                            garbage += pad.inputH;
                            if (garbage > 2)
                                garbage = 0;
                            else if (garbage < 0)
                                garbage = 2;
                            break;
                        case 6:
                            game += pad.inputH;
                            if (game > 4)
                                game = 0;
                            else if (game < 0)
                                game = 4;
                            if (game == 0)
                            {
                                generator = 0;
                                rotation = 0;
                            }
                            else if (game == 1)
                            {
                                generator = 1;
                                rotation = 2;
                            }
                            else if (game == 2)
                            {
                                generator = 2;
                                rotation = 2;
                            }
                            else if (game == 3 || game == 4)
                            {
                                generator = 3;
                                rotation = 3;
                            }
                            break;
                        case 7:
                            rotation += pad.inputH;
                            if (rotation > 3)
                                rotation = 0;
                            else if (rotation < 0)
                                rotation = 3;
                            break;
                        case 8:
                            generator += pad.inputH;
                            if (generator > 4)
                                generator = 0;
                            else if (generator < 0)
                                generator = 4;
                            break;
                    }
                }
            }
        }

        public void render()
        {
            Draw.buffer.DrawString("invisible active piece: " + (invPiece?"true":"false"), SystemFonts.DefaultFont, Draw.wb, 150, 250);
            Draw.buffer.DrawString("invisible preview/hold: " + (invPreview?"true":"false"), SystemFonts.DefaultFont, Draw.wb, 150, 262);
            Draw.buffer.DrawString("invisible stack: " + (invStack?"true":"false"), SystemFonts.DefaultFont, Draw.wb, 150, 274);
            Draw.buffer.DrawString("big: " + (big ? "true" : "false"), SystemFonts.DefaultFont, Draw.wb, 150, 286);
            Draw.buffer.DrawString("grav: " + gravnames[grav], SystemFonts.DefaultFont, Draw.wb, 150, 298);
            Draw.buffer.DrawString("garbage: " + garbnames[garbage], SystemFonts.DefaultFont, Draw.wb, 150, 310);
            Draw.buffer.DrawString("game base: " + gamenames[game], SystemFonts.DefaultFont, Draw.wb, 150, 322);
            Draw.buffer.DrawString("rotation system: " + rotnames[rotation], SystemFonts.DefaultFont, Draw.wb, 150, 334);
            Draw.buffer.DrawString("generator: " + gennames[generator], SystemFonts.DefaultFont, Draw.wb, 150, 346);

            Draw.buffer.DrawString("→", SystemFonts.DefaultFont, Draw.wb, 135, 250 + 12 * pos);

            Draw.buffer.DrawString("press start to play", SystemFonts.DefaultFont, Draw.wb, 150, 400);
        }
    }
}
