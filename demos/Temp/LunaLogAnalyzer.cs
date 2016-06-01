using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demos.Temp
{
    public class LunaLogAnalyzer
    {
        string accessLogPath8 = @"E:\Downloads\20160328_aupass_log2\10.24.23.8\httpd\access.lnln.2016032423.log";
        string accessLogPath9 = @"E:\Downloads\20160328_aupass_log2\10.24.23.9\httpd\access.lnln.2016032423.log";
        string cataLogPath8 = @"E:\Downloads\20160328_aupass_tomcat0325\8\catalina.out-20160325";
        string cataLogPath9 = @"E:\Downloads\20160328_aupass_tomcat0325\9\catalina.out-20160325";
        string resultPath = @"E:\20160328\result.log";

        public void AnalyzeLogs()
        {
            using (var access8 = new StringReader(accessLogPath8))
            using (var access9 = new StringReader(accessLogPath9))
            using (var cata8 = new StringReader(cataLogPath8))
            using (var cata9 = new StringReader(cataLogPath9))
            {
                string line8, line9;
                while (!string.IsNullOrWhiteSpace(line8 = access8.ReadLine()))
                {

                }
            }
        }
    }
}
