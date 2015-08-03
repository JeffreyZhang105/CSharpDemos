using demos.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demos.Concurrency
{
    public class HttpDownloader
    {
        // Maximun block size: 10MB.
        private const long BlockSize = 1024*1024*10;

        /// <summary>
        /// download status mark
        /// </summary>
        private readonly DownloadStatus _status;

        private class DownloadStatus
        {
            private readonly object _locker;
            private long _allocated;

            public long Allocated
            {
                get { return _allocated; }
                set
                {
                    lock (_locker)
                    {
                        _allocated = value;
                    }
                }
            }

            private int _threadCount;

            public int ThreadCount
            {
                get { return _threadCount; }
                set
                {
                    lock (_locker)
                    {
                        _threadCount = value;
                    }
                }
            }

            private long _targetSize;

            public long TargetSize
            {
                get { return _targetSize; }
                set
                {
                    lock (_locker)
                    {
                        _targetSize = value;
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Url { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string LocalTarget { get; private set; }

            public DownloadStatus(string url, string localTarget, object locker)
            {
                this.Url = url;
                this.LocalTarget = localTarget;
                _locker = locker;
            }
        }

        // Get size of the file targeted by url, returns size of file in Byte.
        private long getResourceLength(string url)
        {
            var result = 0L;

            var request = WebRequest.CreateDefault(new Uri(url)) as HttpWebRequest;
            if (request != null)
            {
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
            }

            return result;
        }

        private void DownloadNextBlock()
        {
            var threadId = (int) (_status.Allocated/BlockSize);
            var from = _status.Allocated;
            var to = _status.Allocated + BlockSize > _status.TargetSize
                ? _status.TargetSize
                : _status.Allocated + BlockSize;
            var thread = new Thread(new ThreadStart(delegate
            {
                var filereader = new HttpDownloadThread(threadId);
                filereader.DownloadStatusChanged += DownloadThread_DownloadStatusChanged;
                filereader.DownloadFilePart(_status.Url, _status.LocalTarget, from, to);
            }));
            _status.ThreadCount++;
            _status.Allocated += BlockSize;
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localtarget"></param>
        public HttpDownloader(string url, string localtarget)
        {
            this._status = new DownloadStatus(url, localtarget, new object());
        }

        private void DownloadThread_DownloadStatusChanged(object sender, DownloadThreadEventArgs e)
        {
            if (e.Completion == 1 && _status.Allocated < _status.TargetSize)
            {
                DownloadNextBlock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadCount"></param>
        public void StartDownload(int threadCount)
        {
            _status.TargetSize = getResourceLength(_status.Url);

            while (_status.Allocated < _status.TargetSize && _status.ThreadCount < threadCount)
            {
                DownloadNextBlock();
            }
        }
    }
}