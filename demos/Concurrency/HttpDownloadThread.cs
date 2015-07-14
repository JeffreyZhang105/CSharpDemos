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

    public class HttpDownloadThread
    {
        private const int BufferSize = 512;
        public event EventHandler<DownloadThreadEventArgs> DownloadStatusChanged;
        public int ThreadId { get; private set; }

        ~HttpDownloadThread()
        {

        }

        public HttpDownloadThread(int threadId)
        {
            ThreadId = threadId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localTarget"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void DownloadFilePart(string url, string localTarget, long from, long to)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            var buffer = new byte[BufferSize];
            var fileStream = new FileStream(localTarget + "_" + ThreadId, FileMode.Create);
            if (request != null)
            {
                request.AllowAutoRedirect = true;
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
            fileStream.Close();

            DownloadStatusChanged?.Invoke(this, new DownloadThreadEventArgs(ThreadId, 1));
        }
    }
}