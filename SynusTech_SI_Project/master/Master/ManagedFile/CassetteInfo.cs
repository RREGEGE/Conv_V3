using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Master.Interface.MyFileIO;

namespace Master.ManagedFile
{
    public static class CassetteInfo
    {
        /// <summary>
        /// 프로그램 재실행 후에도 마지막 카세트 아이디 유지하기 위한 카세트 정보 파일
        /// </summary>
        public enum CassetteInfoKey
        {
            LP_CST_ID,
            OP_CST_ID,
            BP_CST_ID1,
            BP_CST_ID2,
            BP_CST_ID3,
            BP_CST_ID4
        }

        public static void AutomaticCleanupCSTInfoFile()
        {
            List<string> UsePortID = new List<string>();
            foreach (var port in Master.m_Ports)
            {
                UsePortID.Add(port.Value.GetParam().ID);
            }

            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;
            string filePath = path + "\\" + fileName;

            string[] Sections = MyINI.GetSectionNames(filePath);

            foreach(var Section in Sections)
            {
                if(!UsePortID.Contains(Section))
                    MyINI.Write($"{Section}", null, null, filePath);
            }
        }

        public static string ReadFailCSTID()
        {
            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            if(string.IsNullOrEmpty(MyINI.Read($"Read_Fail_CST_ID", $"ID", filePath).ToString()))
                MyINI.Write($"Read_Fail_CST_ID", $"ID", $"CST_ID_READ_FAIL", filePath);

            return MyINI.Read($"Read_Fail_CST_ID", $"ID", filePath).ToString();
        }

        public static string ReadTryCount()
        {
            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            if (string.IsNullOrEmpty(MyINI.Read($"Read_Try_Count", $"Count", filePath).ToString()))
                MyINI.Write($"Read_Try_Count", $"Count", $"5", filePath);

            return MyINI.Read($"Read_Try_Count", $"Count", filePath).ToString();
        }
        public static string CanTopsReadPageCount()
        {
            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            if (string.IsNullOrEmpty(MyINI.Read($"CanTops_LM21_Page_Count", $"Count", filePath).ToString()))
                MyINI.Write($"CanTops_LM21_Page_Count", $"Count", $"1", filePath);

            return MyINI.Read($"CanTops_LM21_Page_Count", $"Count", filePath).ToString();
        }

        /// <summary>
        /// 특정 포트의 특정 영역의 CST ID Write
        /// </summary>
        /// <param name="PortID"></param>
        /// <param name="eCassetteInfoKey"></param>
        /// <param name="CSTID"></param>
        public static void WriteCSTID(string PortID, CassetteInfoKey eCassetteInfoKey, string CSTID)
        {
            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            MyINI.Write($"{PortID}", $"{eCassetteInfoKey}", $"{CSTID}", filePath);
        }
        
        
        /// <summary>
        /// 특정 포트의 특정 영역의 CST ID Read
        /// </summary>
        /// <param name="PortID"></param>
        /// <param name="eCassetteInfoKey"></param>
        /// <returns></returns>
        public static string ReadCSTID(string PortID, CassetteInfoKey eCassetteInfoKey)
        {
            string path = ManagedFileInfo.CassetteIDDirectory;
            string fileName = ManagedFileInfo.CassetteIDFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            return MyINI.Read($"{PortID}", $"{eCassetteInfoKey}", filePath).ToString();
        }
    }
}
