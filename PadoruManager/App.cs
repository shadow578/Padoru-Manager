using System;
using System.Windows.Forms;
using PadoruManager.UI;

namespace PadoruManager
{
    static class App
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CollectionManager());
        }
    }
}
