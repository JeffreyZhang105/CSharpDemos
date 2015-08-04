using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using demos.Concurrency;
using demos.Reflaction;

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
            var downloadTarget = @"http://ftp.jaist.ac.jp/pub/eclipse/technology/epp/downloads/release/mars/R/eclipse-jee-mars-R-win32-x86_64.zip"; // ///////////////////
            var localTarget = @"E:\";
            var downloader = new HttpDownloadController(downloadTarget, localTarget);
            downloader.StartDownload(5);

            // Keep console.
            Console.ReadLine();
        }
    }
}
