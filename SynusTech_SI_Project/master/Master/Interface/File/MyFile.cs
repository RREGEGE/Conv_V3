using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Master.Interface.MyFileIO
{
    class MyFile
    {
        /// <summary>
        /// 지정 경로의 File의 확장자를 변경하여 Backup File로 저장
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static public bool BackupAndRemove(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string BackupName = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".bak";
                    if(File.Exists(BackupName))
                        File.SetAttributes(BackupName, File.GetAttributes(BackupName) & FileAttributes.Archive);

                    File.Copy(filePath, BackupName, true);
                    File.SetAttributes(BackupName, File.GetAttributes(BackupName) | FileAttributes.Hidden);

                    if (File.Exists(filePath))
                    {
                        File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
                        File.Delete(filePath);
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
