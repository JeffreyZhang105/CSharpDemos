using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demos.Concurrency;

namespace demos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write(FileConcurrency.OpenFile(@"D:\Codes\Demos\_old\ASP.net\CSSFriendly_1.0.zip", 200));

            Console.ReadLine();
        }
    }
}
