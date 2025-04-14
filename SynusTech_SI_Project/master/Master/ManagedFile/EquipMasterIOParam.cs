using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.MyFileIO;
using System.IO;

namespace Master.ManagedFile
{
    public static class EquipMasterIOParam
    {
        public class MasterIOParameter
        {
            public IOMap Ctrl_IO = new IOMap();

            public bool IsValidIO(IOParam IOParam)
            {
                if (IOParam == null)
                    return false;

                if (IOParam.Bit < 0)
                    return false;

                if (IOParam.StartAddr < 0 || IOParam.StartAddr >= 8000)
                    return false;

                return true;
            }
        }
        
        public class IOMap
        {
            public IOParam[] InputMap = new IOParam[64];
            public IOParam[] OutputMap = new IOParam[64];

            public IOMap()
            {
                for (int nCount = 0; nCount < InputMap.Length; nCount++)
                    InputMap[nCount] = new IOParam();
                for (int nCount = 0; nCount < OutputMap.Length; nCount++)
                    OutputMap[nCount] = new IOParam();
            }
        }

        public class IOParam
        {
            public string Name = string.Empty;
            public int StartAddr = -1;
            public int Bit = -1;
            public bool bInvert = false;

            public bool IsValidBitRange()
            {
                if (Bit < -1 || Bit >= 8)
                    return false;

                return true;
            }
            public bool IsValidStartAddrRange()
            {
                if (StartAddr < -1 || StartAddr >= 8000)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// MasterSafetyImageInfo.cs의 Load, Save 과정 참고
        /// </summary>
        /// <param name="masterIOParameter"></param>
        static public void LoadMasterIOParam(ref MasterIOParameter masterIOParameter)
        {
            try
            {
                string filePath = ManagedFileInfo.EquipMasterIOParamDirectory + @"\" + $"{ManagedFileInfo.EquipMasterIOParamFileName}";

                if (!File.Exists(filePath))
                {
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileIsNotExist, $"Master IO Param");
                    return;
                }

                masterIOParameter = (MasterIOParameter)MyXML.XmlToClass(filePath, typeof(MasterIOParameter));

                if (masterIOParameter != null)
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Master IO Param");
                else
                    LogMsg.AddMasterLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Master IO Param");
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master IO Param Load Error");
            }
        }

        static public bool SaveFile(MasterIOParameter masterIOParameter, bool bWithBackup = true)
        {
            try
            {
                string filePath = ManagedFileInfo.EquipMasterIOParamDirectory + @"\" + $"{ManagedFileInfo.EquipMasterIOParamFileName}";

                if (File.Exists(filePath) && bWithBackup)
                {
                    if (MyFile.BackupAndRemove(filePath))
                        LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileBackupSuccess, $"Master Param");
                }

                if (File.Exists(filePath))
                {
                    //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                    File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
                }

                MyXML.ClassToXml(filePath, masterIOParameter, typeof(MasterIOParameter));
                File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);

                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"Master Param");
                return true;
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master IO Param Save Error");
                return false;
            }
        }
    }
}
