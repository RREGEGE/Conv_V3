using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Master.Interface.MyFileIO
{
    class MyINI
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileStringW(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string section, byte[] Keys, int nSize, string filePath);
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileSectionNames(byte[] sections, uint size, String filePath);

        /// <summary>
        /// INI File 작성
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="path"></param>
        static public void Write(string section, string key, string val, string path)
        {
            WritePrivateProfileString(section, key, val, path);
        }

        /// <summary>
        /// INI File Read
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        static public StringBuilder Read(string section, string key, string path)
        {
            StringBuilder str = new StringBuilder(409600);
            GetPrivateProfileStringW(section, key, "", str, str.Capacity, path);
            return str;
        }

        /// <summary>
        /// INI File에 저장된 Section 항목을 가져옴
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        static public string[] GetSectionNames(string _filePath)
        {
            byte[] bytes = new byte[409600];
            uint Flag = GetPrivateProfileSectionNames(bytes, 4096, _filePath);
            return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }
        static public string[] GetEntryNames(string section, string _filePath)
        {
            byte[] bytes = new byte[409600];
            GetPrivateProfileSection(section, bytes, 4096, _filePath);

            return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
