using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using demos.Concurrency;
using demos.Reflaction;
using demos.Network;

namespace demos
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //  // File concurrency.
            //  Console.Write(FileConcurrency.OpenFile(@"D:\Codes\Demos\_old\ASP.net\CSSFriendly_1.0.zip", 200));
            //  Console.ReadLine();

            //  // Customized attribute.
            //  while (Console.ReadKey().Key != ConsoleKey.Escape)
            //  {
            //      var password = new PasswordCreator().CreatePassword(0xFF, 10);
            //      Console.WriteLine(password);
            //      Clipboard.SetText(password);
            //  }

            // // Concurrent downloading.
            var downloadTarget = "http://download-cf.jetbrains.com/resharper/JetBrains.ReSharperUltimate.2015.1.1.exe"; // ///////////////////
            var localTarget = @"E:\downloadtest.exe";
            var downloader = new HttpDownloader(downloadTarget, localTarget);
            downloader.StartDownload(5);

            // Keep console.
            Console.ReadLine();
        }
    }
}
