using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demos.Asynchronism
{
    public class AsyncAndAwait
    {
        public async static Task<string> GetStringAsync()
        {
            var result = string.Empty;

            Thread.Sleep(10000);

            result = Guid.NewGuid().ToString();
            return result;
        }

        public string TestAsync()
        {
            var result = string.Empty;

            var task = GetStringAsync();
            var awaiter = task.GetAwaiter();
            result = awaiter.GetResult();

            return result;
        }
    }
}