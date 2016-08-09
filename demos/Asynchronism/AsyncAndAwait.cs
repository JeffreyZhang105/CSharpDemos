using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demos.Asynchronism
{
    public class AsyncAndAwait
    {
        public async Task<string> GetStringAsync(string param)
        {
            var result = string.Empty;

            var started = DateTime.Now;
            while ((DateTime.Now - started).TotalSeconds < 5)
            {

            }

            result = Guid.NewGuid().ToString();
            return result;
        }

        public async void TestAsync()
        {
            Debug.WriteLine(">>>>>1");
            string result1 = await GetStringAsync(Guid.NewGuid().ToString());
            Debug.WriteLine(">>>>>2");
            string result2 = await GetStringAsync(Guid.NewGuid().ToString());
            Debug.WriteLine(">>>>>3");

            var task3 = DateTime.Now;
        }

        public void test()
        {
            TestAsync();
            Debug.WriteLine(">>>>>4");
            var a = 1;
        }
    }
}