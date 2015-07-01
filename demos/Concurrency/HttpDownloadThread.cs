using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        public event EventHandler<DownloadThreadEventArgs> DownloadStatusChanged;

        ~HttpDownloadThread()
        {

        }

        public bool ReadFile(string url, string localTarget, long from, long to)
        {
            var succeed = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;
            request.AddRange(from, to);
            //request.stream


            return succeed;
        }
    }
}