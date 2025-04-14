using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.CLS
{
    public static class Ini
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string appName, string keyName, string defaults, StringBuilder retrunedString
            , int size, string fileName);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string appName, string keyName, string value, string fileName);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string appName, string keyName, int defaults, string fileName);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileSectionNames(byte[] sectionName, int size, string filePath);

        public const int MAX_BUFFER_SIZE = 409600;

        public enum LogMessage_Ini {
            IniFileCreateFail,
        }
        /// <summary>
        /// ini 파일을 컨트롤 하는 함수에 발생하는 메시지에 대한 string 값
        /// </summary>
        /// <param name="logMsg"></param>
        /// <returns></returns>
        private static string GetLogMessage(LogMessage_Ini logMsg) {
            switch (logMsg) {
                case LogMessage_Ini.IniFileCreateFail:
                    return $"Ini File Create Fail";
            }

            return "";
        }
        /// <summary>
        /// 해당 경로의 file이 존재 여부를 파악하는 함수
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            if (File.Exists(path))
                return true;

            return false;
        }
        /// <summary>
        /// 해당 경로의 해당 fileName의 ini파일을 생성하는 함수
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool CreateIniFile(string fileName, string folderName)
        {
            try {
                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);

                int count = 0;
                while (true) {
                    if (Directory.Exists(folderName)) {
                        File.Create(fileName);
                        return true;
                    }

                    if (count > 10)
                        return false;

                    System.Threading.Thread.Sleep(100);
                    count++;
                }
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Ini.IniFileCreateFail), ex));
                return false;
            }
        }
        /// <summary>
        /// ini 파일 내의 section 데이터 배열을 얻어오는 함수
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetSectionNames(string path)
        {
            byte[] buffer = new byte[MAX_BUFFER_SIZE];
            GetPrivateProfileSectionNames(buffer, MAX_BUFFER_SIZE, path);
            return buffer;
        }
        /// <summary>
        /// ini 파일 내의 특정 section의 특정 key에 해당하는 int형 값을 얻는 함수
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetValueInt(string sectionName, string keyName, string path)
        {
            int value = GetPrivateProfileInt(sectionName, keyName, 0, path);
            return value;
        }
        /// <summary>
        /// ini 파일 내의 특정 section의 특정 key에 해당하는 string 값을 얻는 함수
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetValueString(string sectionName, string keyName, string path)
        {
            StringBuilder value = new StringBuilder();
            GetPrivateProfileString(sectionName, keyName, "", value, 409600, path);
            return value.ToString();
        }
        /// <summary>
        /// ini 파일 내의 특정 section의 특정 key 값을 작성하는 함수
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void SetValueString(string sectionName, string keyName, string value, string path)
        {
            WritePrivateProfileString(sectionName, keyName, value, path);
        }
    }
}
