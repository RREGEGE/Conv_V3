using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Master.ManagedFile
{

    public static class ParamSaveAdmin
    {
        /// <summary>
        /// 파라미터 저장 관련 가능한 사람 항목
        /// </summary>
        static List<string> SaveAdminList = new List<string>()
        {
            "장병찬",
            "오은석",
            "임도훈",
            "이명수",
            "김태우",
            "김시연"
        };

        /// <summary>
        /// List를 Default 인원으로 설정
        /// </summary>
        static public void SetDefaultList()
        {
            SaveAdminList = new List<string>();
            SaveAdminList.Add("장병찬");
            SaveAdminList.Add("오은석");
            SaveAdminList.Add("임도훈");
            SaveAdminList.Add("이명수");
            SaveAdminList.Add("김태우");
            SaveAdminList.Add("김시연");
        }

        /// <summary>
        /// 현재 List에 저장된 인원을 얻어 옴
        /// </summary>
        /// <returns></returns>
        static public List<string> GetAdminList()
        {
            return SaveAdminList;
        }

        /// <summary>
        /// File에 저장된 인원을 불러와 List에 저장
        /// </summary>
        /// <returns></returns>
        static public bool Load()
        {
            try
            {
                string path = ManagedFileInfo.ParamSaveAdminDirectory;
                string fileName = ManagedFileInfo.ParamSaveAdminFileName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + "\\" + fileName;

                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        foreach(var Admin in SaveAdminList)
                        {
                            sw.WriteLine($"{Admin}");
                        }
                    }
                }
                else
                {
                    string[] AdminList = File.ReadAllLines(filePath);
                    SaveAdminList = new List<string>();

                    foreach (var Admin in AdminList)
                        SaveAdminList.Add(Admin);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Parameter Save Admin List Load Error");
                SetDefaultList();
                return false;
            }
        }
    }
}
