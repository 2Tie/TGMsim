using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    static class Draw
    {
        public static Graphics buffer;
        public static SolidBrush wb;//white brush
        public static SolidBrush bb;//black brush
        public static SolidBrush tb;//flash brush
        public static Font f_Maestro;
        static PrivateFontCollection fonts = new PrivateFontCollection();

        static Draw()
        {
            wb = new SolidBrush(Color.White);
            bb = new SolidBrush(Color.Black);
            tb = new SolidBrush(Color.White);

            fonts.AddFontFile(@"Res\Maestro2.ttf");
            FontFamily fontFam = fonts.Families[0];
            f_Maestro = new Font(fontFam, 16, GraphicsUnit.Pixel);
        }
    }
}
