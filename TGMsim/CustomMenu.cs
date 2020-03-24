using System.Drawing;

namespace TGMsim
{
    class CustomMenu
    {
        int pos = 0;
        bool invPiece = false;
        bool invPreview = false;
        bool invStack = false;
        bool grav = false;

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
                        pos = 3;
                    if (pos > 3)
                        pos = 0;
                }
            }
            if (pad.inputH != deltaH)
            {
                deltaH = pad.inputH;
                if (pad.inputH != 0)
                {
                    if (pos == 0)
                        invPiece = !invPiece;
                    if (pos == 1)
                        invPreview = !invPreview;
                    if (pos == 2)
                        invStack = !invStack;
                    if (pos == 3)
                        grav = !grav;
                }
            }
        }

        public void render(Graphics drawBuffer)
        {
            drawBuffer.DrawString("invisible active piece: " + (invPiece?"true":"false"), SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 250);
            drawBuffer.DrawString("invisible preview/hold: " + (invPreview?"true":"false"), SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 262);
            drawBuffer.DrawString("invisible stack: " + (invStack?"true":"false"), SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 274);
            drawBuffer.DrawString("grav: " + (grav ? "20G" : "0G"), SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 286);

            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 135, 250 + 12 * pos);

            drawBuffer.DrawString("press start to play", SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 360);
        }
    }
}
