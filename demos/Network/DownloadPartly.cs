using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace demos.Network
{
    public class DownloadPartly
    {
        delegate void ProcessingCallback(string processing);

        const int bufferSize = 512;
        public int ThreadId { get; set; }
        public string Url { get; private set; }
        public string TargetName { get; private set; }

        ~DownloadPartly()
        {
            //if(!frm)
        }

        public void DownloadFilePart(string url, string localTarget, long from, long to)
        {
            byte[] buffer = new byte[bufferSize];
            int readSize = 0;
            FileStream fs = new FileStream(TargetName, FileMode.Create);
            Stream ns = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AddRange
            }
        }
    }
}
