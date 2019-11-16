using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using JikanDotNet;

namespace PadoruManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //test jikan
            TestJikan().GetAwaiter().GetResult();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static async Task TestJikan()
        {
            // Initialize JikanWrapper
            IJikan jikan = new Jikan(true);

            // Send request to search character with "spiegel" key word
            CharacterSearchResult characterSearchResult = await jikan.SearchCharacter("002");

            // Print name of the first result
            // Output -> "Spike Spiegel"
            Console.WriteLine(characterSearchResult.Results.First().Name);
        }
    }
}
