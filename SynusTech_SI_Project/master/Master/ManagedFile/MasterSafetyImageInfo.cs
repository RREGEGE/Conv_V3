using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Master.Interface.MyFileIO;

namespace Master.ManagedFile
{
    public class MasterSafetyImageInfo
    {
        /// <summary>
        /// IO Map에 사용되는 파라미터 용 클래스
        /// </summary>
        public class SafetyItem
        {
            public string Text = string.Empty;
            public bool MapVisible = true;
            public bool GridVisible = true;
            public int X = 0;
            public int Y = 0;
        }


        Image DefaultImage = Properties.Resources.icons8_no_image_96;                                                       //Image가 없다는 표시를 위한 Default Image
        public string WorkZoneImagePath = string.Empty;                                                                     //Main 화면 IOMap에 Load될 Image 주소
        public SafetyItem[] SafetyItems = new SafetyItem[(int)Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)).Length];  //Main 화면 IOMap에 출력되는 Item 항목

        /// <summary>
        /// Master Safety Image Info Item 초기화
        /// </summary>
        public MasterSafetyImageInfo()
        {
            for (int nCount = 0; nCount < SafetyItems.Length; nCount++)
                SafetyItems[nCount] = new SafetyItem();
        }

        /// <summary>
        /// Default Image File을 얻음
        /// </summary>
        /// <returns></returns>
        public Image GetDefaultImage()
        {
            return DefaultImage;
        }

        /// <summary>
        /// 지정 경로의 xml file에 저장된 SafetyImageInfo를 불러옴
        /// </summary>
        /// <param name="_SafetyImageInfo"></param>
        /// <returns></returns>
        public static bool Load(ref MasterSafetyImageInfo _SafetyImageInfo)
        {
            try
            {
                //1. 경로 지정
                string filePath = ManagedFileInfo.MasterSafetyItemInfoDirectory + @"\" + $"{ManagedFileInfo.MasterSafetyItemInfoFileName}";

                //2. 파일 확인
                if (!File.Exists(filePath))
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileIsNotExist, $"Master Safety Image Info Load fail");
                    return false;
                }


                //3. 파일 있는 경우 파일 -> 클래스 변환 및 형 변환 진행
                _SafetyImageInfo = (MasterSafetyImageInfo)MyXML.XmlToClass(filePath, typeof(MasterSafetyImageInfo));

                //4. 객체 정상적인 경우 성공
                if (_SafetyImageInfo != null)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Master Safety Image Info");
                    return true;
                }
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Master Safety Image Info Load Fail");

                return false;
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master Safety Image Info Load Exception Error");
                return false;
            }
        }

        /// <summary>
        /// 지정 경로에 SafetyImageInfo 저장
        /// </summary>
        /// <param name="_SafetyImageInfo"></param>
        /// <param name="bWithBackup"></param>
        /// <returns></returns>
        public static bool Save(MasterSafetyImageInfo _SafetyImageInfo, bool bWithBackup = true)
        {
            try
            {
                //1. 경로 지정
                string filePath = ManagedFileInfo.MasterSafetyItemInfoDirectory + @"\" + $"{ManagedFileInfo.MasterSafetyItemInfoFileName}";

                //2. 파일 유무 확인 및 파일 백업 옵션 확인
                if (File.Exists(filePath) && bWithBackup)
                {
                    //3. 파일 있고 백업 옵션 켜져 있는 경우 .bak 생성
                    if (MyFile.BackupAndRemove(filePath))
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileBackupSuccess, $"Master Safety Image Info");
                }

                //3. 파일 존재하는 경우 파일 속성 변경
                if (File.Exists(filePath))
                {
                    //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                    //File 있는 경우 히든 -> 아카이브로 변경 (편집 가능한 상태로 변환)
                    File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
                }

                //4. 클래스 -> xml 파일화 진행
                MyXML.ClassToXml(filePath, _SafetyImageInfo, typeof(MasterSafetyImageInfo));

                //5. 저장된 파일 옵션 변경(히든)
                File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"Master Safety Image Info");
                return true;
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master Safety Image Info Save Exception Error");
                return false;
            }
        }
    }
}
