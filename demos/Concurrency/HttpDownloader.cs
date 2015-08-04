using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demos.Concurrency
{
    public class DownloadThreadEventArgs : EventArgs
    {
        public int ThreadNumber { get; private set; }
        public int Completion { get; private set; }

        public DownloadThreadEventArgs(int threadNumber, int completion)
        {
            ThreadNumber = threadNumber;
            Completion = completion;
        }
    }

    public class HttpDownloader
    {
        private const int BufferSize = 512;
        public event EventHandler<DownloadThreadEventArgs> DownloadStatusChanged;
        public int ThreadId { get; private set; }

        ~HttpDownloader()
        {

        }

        public HttpDownloader(int threadId)
        {
            ThreadId = threadId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localTemp"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void DownloadFilePart(string url, string localTemp, long from, long to)
        {
            Console.WriteLine($"thread: {ThreadId} start to download, from: {from}, to: {to}");
            var tryCount = 5;
            while (tryCount > 0)
            {
                tryCount--;
                FileStream fileStream = null;
                try
                {
                    var request = WebRequest.Create(url) as HttpWebRequest;
                    var buffer = new byte[BufferSize];
                    fileStream = new FileStream(localTemp, FileMode.Create);
                    if (request != null)
                    {
                        request.AllowAutoRedirect = true;
                        request.Timeout = 10000;
                        request.AddRange(from, to);
                        var response = request.GetResponse().GetResponseStream();

                        if (response != null)
                        {
                            var read = response.Read(buffer, 0, BufferSize);
                            while (read > 0)
                            {
                                fileStream.Write(buffer, 0, buffer.Length);
                                read = response.Read(buffer, 0, BufferSize);
                            }
                            response.Close();
                        }
                    }
                    Console.WriteLine($"thread: {ThreadId} download finished, from: {from}, to: {to}");
                    fileStream.Close();
                    tryCount = 0;
                }
                catch (Exception ex)
                {
                    fileStream?.Close();
                    if (tryCount == 0) tryCount = -1;
                    Console.WriteLine($"thread: {ThreadId} {ex.Message}, retry");
                }
            }
            DownloadStatusChanged?.Invoke(this, new DownloadThreadEventArgs(ThreadId, 1));
        }
    }
}