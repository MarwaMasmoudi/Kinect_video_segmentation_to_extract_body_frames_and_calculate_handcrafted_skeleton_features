using System;
using System.IO;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public class Log
    {
        static string Unknown = "Administrator";
        private static string ErrorFile = string.Format(@"{0}\tmp\{1}" ,Environment.CurrentDirectory, "Log_Err.txt");
        private static string InfoFile = string.Format(@"{0}\tmp\{1}", Environment.CurrentDirectory, "Log_Info.txt");
        private static string SuccessFile = string.Format(@"{0}\tmp\{1}", Environment.CurrentDirectory, "Log_Sucess.txt");
        private static string LogFile = string.Format(@"{0}\tmp\{1}", Environment.CurrentDirectory, "Log.txt");

        static void WriteLogToFile(string strMessage , Log_Type type , string function)
        {
            string line = string.Format("{1} | {0} | {3} | {4}", DateTime.Now.ToString(),type, Unknown, function  , strMessage);
            string fileName;
            switch (type)
            {
                case Log_Type.Eror:
                    fileName = ErrorFile;
                    break;
                case Log_Type.Info:
                    fileName = InfoFile;
                    break;
                case Log_Type.Sucs:
                    fileName = SuccessFile;
                    break;
                default:
                    fileName = InfoFile;
                    break;
            }
            FileStream fs = new FileStream(fileName , FileMode.Append, FileAccess.Write, FileShare.None);
            StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.WriteLine(line);
            streamWriter.Flush();
            streamWriter.Close();

            FileStream fslog = new FileStream(LogFile, FileMode.Append, FileAccess.Write, FileShare.None);
            StreamWriter streamWriterlog = new StreamWriter(fslog);
            streamWriterlog.WriteLine(line);
            streamWriterlog.Flush();
            streamWriterlog.Close();
        }

        public static void WriteErrorLogToFile(string strMessage, string function)
        {
            WriteLogToFile(strMessage, Log_Type.Eror, function);
        }

        public static void WriteInfoLogToFile(string strMessage, string function)
        {
            WriteLogToFile(strMessage, Log_Type.Info, function);
        }

        public static void WriteSucessLogToFile(string strMessage, string function)
        {
            WriteLogToFile(strMessage, Log_Type.Sucs , function);
        }
    }
    enum Log_Type
    {
        Eror,
        Info,
        Sucs
    }
}
