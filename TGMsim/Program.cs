using System;
using System.Windows.Forms;

namespace TGMsim
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            using (Form1 form1 = new Form1())
            {
                form1.Show();
                form1.gameLoop();
            }
        }
    }
}
