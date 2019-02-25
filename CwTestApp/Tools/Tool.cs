using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CwTestApp.Tools
{
    /// <summary>
    /// 工具
    /// </summary>
    public class Tool
    {
        public static void WriteLog(string ErrorType, string ErrorMessage)
        {
            string docPath = Environment.CurrentDirectory;
            string logPath = Path.Combine(docPath, "Log.txt");
            using (StreamWriter outputFile = new StreamWriter(logPath, true))
            {
                outputFile.WriteLine(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + "  " + ErrorType + " :" + ErrorMessage);
            }
        }

        public static void SaveJson<T>(List<T> list)
        {
            string docPath = Environment.CurrentDirectory;
            string logPath = Path.Combine(docPath, "Weather.Json");
            using (StreamWriter outputFile = new StreamWriter(logPath, true))
            {
                outputFile.WriteLine(JsonConvert.SerializeObject(list));
            }
        }
    }
}
