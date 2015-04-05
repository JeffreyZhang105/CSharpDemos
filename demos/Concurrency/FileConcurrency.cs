using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace demos.Concurrency
{
    public static class FileConcurrency
    {
        public static string OpenFile(string path, int threadCount)
        {
            var results = new string[threadCount];
            var finished = 0;
            var succeed = 0;
            var lockerFinished = new object();
            var lockerSucceed = new object();

            var start = false; // to control threads
            for (var i = 0; i < threadCount; i++)
            {
                var i1 = i;
                var thread = new Thread(new ThreadStart(delegate
                {
                    while (!start) Thread.Sleep(1);

                    FileStream fs = null;
                    var fileResult = "";
                    try
                    {
                        using (fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            var buff = new byte[fs.Length];
                            fs.Read(buff, 0, buff.Length);
                            fileResult += "size: " + buff.Length;
                            lock (lockerSucceed)
                            {
                                succeed++;
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        fileResult += "err: " + e.Message.Substring(0, 6) + "...";
                    }
                    finally
                    {
                        lock (lockerFinished)
                        {
                            finished++;
                        }
                        if (fs != null) fs.Close();
                    }

                    // print thread infomation and result.
                    results[i1] = string.Format("time: {0}, num: {1:D3}, id: {2:D3}, {3}\r\n",
                        DateTime.Now.ToString("mm-ss.fff"), i1, Thread.CurrentThread.ManagedThreadId, fileResult);
                }));
                thread.Start();
            }
            Thread.Sleep(200);
            start = true; // all threads start to read file.

// ReSharper disable LoopVariableIsNeverChangedInsideLoop
            while (finished < threadCount)
// ReSharper restore LoopVariableIsNeverChangedInsideLoop
            {
                Thread.Sleep(1);
            }

            // append all results together.
            var resultBuilder = new StringBuilder();
            foreach (var s in results)
            {
                resultBuilder.Append(s);
            }

            resultBuilder.Append(succeed + "/" + threadCount + " succeeded, ratio: " +
                                 ((double) succeed/threadCount)*100 + "%");
            return resultBuilder.ToString();
        }
    }
}
