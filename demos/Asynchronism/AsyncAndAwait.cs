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
            Debug.WriteLine("GetStringAsync: var result = string.Empty;");

            var started = DateTime.Now;
            var result = await Task.Factory.StartNew(() =>
              {
                  var result1 = string.Empty;
                  while ((DateTime.Now - started).TotalSeconds < 5)
                  {
                      Debug.WriteLine("await Task.Factory.StartNew(() =>...");
                  }
                  return result1;
              });

            Debug.WriteLine("GetStringAsync:  await Task.Factory.StartNew(() =>...");
            return result;
        }

        public async void TestAsync()
        {
            var task1 = GetStringAsync(Guid.NewGuid().ToString());
            Debug.WriteLine(DateTime.Now + ", TestAsync:  var task1 = GetStringAsync(Guid.NewGuid().ToString());");
            var result1 = await task1;

            Debug.WriteLine("TestAsync:  string result2 = await task2;");
        }


        public void TestAsync1()
        {
            var task1 = GetStringAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
        }
    }
}