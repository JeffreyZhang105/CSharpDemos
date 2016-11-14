using System;
using log4net;

namespace Libraries.Log
{
    public class CommonLogger
    {
        public static void Error(object sender, Exception exception)
        {
            LogManager.GetLogger()
        }
    }
}