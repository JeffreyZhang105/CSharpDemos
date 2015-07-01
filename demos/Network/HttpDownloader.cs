using demos.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demos.Network
{
    public class HttpDownloader
    {
        private class downloadStatus
        {
            public long total { get; set; }
            public long completion { get; set; }
        }
        private List<downloadStatus> ThreadsStatus = new List<downloadStatus>();

        // Get size of the file targeted by url, returns size of file by Byte.
        private long getResourceLength(string url)
        {
            var result = 0L;

            var request = WebRequest.CreateDefault(new Uri(url)) as HttpWebRequest;
            request.Method = "HEAD";
            request.Timeout = 5000;
            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.ContentLength;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localTarget"></param>
        /// <param name="threadCount"></param>
        public void StartDownload(string url, string localTarget, int threadCount)
        {
            var targetSize = getResourceLength(url);
            var remainder = (targetSize * threadCount - 1);
            var blockSize = (targetSize - remainder) / (threadCount - 1);

            if (targetSize > 0)
            {
                for (int tNum = 0; tNum < threadCount; tNum++)
                {
                    var from = blockSize * (tNum - 1);
                    var to = from + (tNum == threadCount - 1 ? remainder : blockSize);
                    var thread = new Thread(new ThreadStart(delegate
                      {
                          var filereader = new HttpDownloadThread();
                          filereader.DownloadStatusChanged += DownloadThread_DownloadStatusChanged;
                          new HttpDownloadThread().ReadFile(url, localTarget, from, to);
                      }));
                    thread.Start();
                }
            }
        }

        private void DownloadThread_DownloadStatusChanged(object sender, DownloadThreadEventArgs e)
        {
            if
        }
    }
}
