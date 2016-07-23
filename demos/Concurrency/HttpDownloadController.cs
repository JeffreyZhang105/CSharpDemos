using demos.Concurrency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demos.Concurrency
{
    public class HttpDownloadController
    {
        // Maximun block size: 10MB.
        private const long BlockSize = 1024*1024*10;

        /// <summary>
        /// download status mark
        /// </summary>
        private DownloadStatus _status;

        /// <summary>
        /// Provides thread-safe values of download status.
        /// </summary>
        private class DownloadStatus
        {
            private long _allocated;
            /// <summary>
            /// Allocated position of download object.
            /// </summary>
            public long Allocated
            {
                get { return _allocated; }
                set { Interlocked.Exchange(ref _allocated, value); }
            }

            private int _threadCount;
            /// <summary>
            /// Count of currently started threads.
            /// </summary>
            public int ThreadCount
            {
                get { return _threadCount; }
                set { Interlocked.Exchange(ref _threadCount, value); }
            }

            public long TargetSize { get; }

            public string Url { get; }

            public string LocalTarget { get; }

            public DownloadStatus(string url, string localTarget, long targetSize)
            {
                Url = url;
                LocalTarget = localTarget;
                TargetSize = targetSize;
            }
        }

        // Get size of the file targeted by url, returns size of file in Byte.
        private void Initialize(string url, string localDir)
        {
            var targetSize = 0L;
            var localFileName = string.Empty;
            var localTarget = string.Empty;

            var request = WebRequest.CreateDefault(new Uri(url)) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "HEAD";
                request.Timeout = 5000;
                request.AllowAutoRedirect = true;
                try
                {
                    var response = request.GetResponse() as HttpWebResponse;
                    if (response != null &&
                        (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Redirect))
                    {
                        localFileName = Regex.Match(response.ResponseUri.ToString(), @"(?<=/)[^/]+$").Value;
                        targetSize = response.ContentLength;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            if (targetSize > 0 && !string.IsNullOrEmpty(localFileName))
            {
                localTarget = localDir.TrimEnd('\\') + localFileName;
                _status = new DownloadStatus(url, localTarget, targetSize);
            }
            Console.WriteLine(
                "Initialized.\r\nurl:{0},\r\nlocal target: {1},\r\ntarget size: {2:F}MB",
                url, localTarget, targetSize/(1024*1024));
        }

        private void DownloadNextBlock()
        {
            var threadId = (int) (_status.Allocated/BlockSize);
            var from = _status.Allocated;
            var to = _status.Allocated + BlockSize > _status.TargetSize
                ? _status.TargetSize
                : _status.Allocated + BlockSize;
            var localTemp = _status.LocalTarget + "_" + threadId;
            var thread = new Thread(new ThreadStart(delegate
            {
                var filereader = new HttpDownloader(threadId);
                filereader.DownloadStatusChanged += DownloadThread_DownloadStatusChanged;
                filereader.DownloadFilePart(_status.Url, localTemp, from, to);
            }));
            _status.ThreadCount++;
            _status.Allocated += BlockSize;
            thread.Start();
        }

        private void DownloadThread_DownloadStatusChanged(object sender, DownloadThreadEventArgs e)
        {
            if (e.Completion == 1)
            {
                if (_status.Allocated < _status.TargetSize)
                {
                    DownloadNextBlock();
                }
                else
                {
                    MergeTempFiles();
                    Console.WriteLine("download finished. ");
                }
            }
        }

        private void MergeTempFiles()
        {
            const int bufferSize = 1024;
            using (var writeStream = new FileStream(_status.LocalTarget, FileMode.OpenOrCreate))
            {
                var mergedTempFile = 0;
                while (mergedTempFile < _status.ThreadCount)
                {
                    mergedTempFile++;
                    var buffer = new byte[bufferSize];
                    using (var reader = new FileStream(_status.LocalTarget + "_" + mergedTempFile, FileMode.Open))
                    {
                        var readMark = 0;
                        var readCount = 0;
                        while ((readCount = reader.Read(buffer, 0, bufferSize)) > 0)
                        {
                            readMark += readCount;
                            writeStream.Write(buffer, readMark, bufferSize);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localDir"></param>
        public HttpDownloadController(string url, string localDir)
        {
            Initialize(url, localDir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadCount"></param>
        public void StartDownload(int threadCount)
        {
            while (_status.Allocated < _status.TargetSize && _status.ThreadCount < threadCount)
            {
                DownloadNextBlock();
            }
        }
    }
}