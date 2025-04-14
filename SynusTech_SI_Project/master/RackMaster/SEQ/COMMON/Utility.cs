using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;

namespace RackMaster.SEQ.COMMON {
    public static class Utility {
        private enum SectionName {
            App,
            UI,
            EtherCAT
        }

        private enum AppKeyName {
            Password,
            LanguageType,
            ModifySyncTime,
        }

        private enum UIKeyName {
            IO,
        }

        public enum PasswordType {
            Admin,
            User,
        }

        public enum UI_IOViewType {
            RawData = 0,
            ConvertData,
        }

        public enum EtherCAT_KeyName {
            AutoRecovery,
        }

        private static int m_userPassword = 5190;
        private static int m_adminPassword = 0710;
        private static UI_IOViewType m_ioViewType = UI_IOViewType.RawData;

        private static bool m_autoRecovery = true;
        private static bool m_modifySyncTime = true;

        private static string filePath = $"{ManagedFileInfo.SettingsDirectory}\\{ManagedFileInfo.UtilitySettingsFileName}";

        public static void InitUtilitSetting() {
            try {
                if (!Ini.IsFileExist($"{ManagedFileInfo.SettingsDirectory}\\{ManagedFileInfo.UtilitySettingsFileName}")) {
                    Ini.CreateIniFile($"{ManagedFileInfo.UtilitySettingsFileName}", $"{ManagedFileInfo.SettingsDirectory}");

                    Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.Password}", $"{m_userPassword}", filePath);
                    Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.LanguageType}", $"{SynusLangPack.LanguageType.Korean}", filePath);
                    Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.ModifySyncTime}", $"{true}", filePath);
                    Ini.SetValueString($"{SectionName.EtherCAT}", $"{EtherCAT_KeyName.AutoRecovery}", $"{m_autoRecovery}", filePath);
                }
                else {
                    int.TryParse(Ini.GetValueString($"{SectionName.App}", $"{AppKeyName.Password}", filePath), out m_userPassword);
                    m_ioViewType = (UI_IOViewType)Ini.GetValueInt($"{SectionName.UI}", $"{UIKeyName.IO}", filePath);

                    SynusLangPack.LanguageType languageType = (SynusLangPack.LanguageType)Enum.Parse(typeof(SynusLangPack.LanguageType), Ini.GetValueString($"{SectionName.App}", $"{AppKeyName.LanguageType}", filePath));
                    SynusLangPack.SetLanguageType(languageType);

                    Boolean.TryParse(Ini.GetValueString($"{SectionName.EtherCAT}", $"{EtherCAT_KeyName.AutoRecovery}", filePath), out m_autoRecovery);
                    Boolean.TryParse(Ini.GetValueString($"{SectionName.App}", $"{AppKeyName.ModifySyncTime}", filePath), out m_modifySyncTime);
                }
            }
            catch(Exception ex) {
                Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.Password}", $"{m_userPassword}", filePath);
                Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.LanguageType}", $"{SynusLangPack.LanguageType.Korean}", filePath);
                Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.ModifySyncTime}", $"{true}", filePath);
                Ini.SetValueString($"{SectionName.EtherCAT}", $"{EtherCAT_KeyName.AutoRecovery}", $"{m_autoRecovery}", filePath);

                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, "Utility Load Fail", ex));
            }
        }
        /// <summary>
        /// 현재 설정된 User Password 로드
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentUserPassword() {
            return m_userPassword;
        }
        /// <summary>
        /// 현재 설정된 Admin Password 로드
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentAdminPassword() {
            return m_adminPassword;
        }
        /// <summary>
        /// 현재 설정된 IO View Type 로드
        /// </summary>
        /// <returns></returns>
        public static UI_IOViewType GetIOViewType() {
            return m_ioViewType;
        }
        /// <summary>
        /// User Password 변경
        /// </summary>
        /// <param name="password"></param>
        public static void SetNewPassword(int password) {
            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.Utility, Log.LogMessage_Main.Utility_PasswordChange));
            m_userPassword = password;
            Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.Password}", $"{m_userPassword}", filePath);
        }
        /// <summary>
        /// 언어팩 설정
        /// </summary>
        /// <param name="lanugage"></param>
        public static void SetLanguageType(SynusLangPack.LanguageType lanugage) {
            SynusLangPack.SetLanguageType(lanugage);
            Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.LanguageType}", $"{lanugage}", filePath);
        }
        /// <summary>
        /// IO View Type 설정
        /// </summary>
        /// <param name="type"></param>
        public static void SetIOViewType(UI_IOViewType type) {
            m_ioViewType = type;
        }
        /// <summary>
        /// 현재 설정된 언어팩 로드
        /// </summary>
        /// <returns></returns>
        public static SynusLangPack.LanguageType GetCurrentLanguageType() {
            SynusLangPack.LanguageType languageType = (SynusLangPack.LanguageType)Enum.Parse(typeof(SynusLangPack.LanguageType), Ini.GetValueString($"{SectionName.App}", $"{AppKeyName.LanguageType}", filePath));
            return languageType;
        }
        /// <summary>
        /// 현재 설정된 자동 리커버리 Enable 값 반환
        /// </summary>
        /// <returns></returns>
        public static bool GetEtherCAT_AutoRecovery() {
            return m_autoRecovery;
        }
        /// <summary>
        /// 자동 리커버리 기능 On/Off
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetEtherCAT_AutoRecovery(bool enabled) {
            m_autoRecovery = enabled;

            Ini.SetValueString($"{SectionName.EtherCAT}", $"{EtherCAT_KeyName.AutoRecovery}", $"{m_autoRecovery}", filePath);
        }
        /// <summary>
        /// CIM으로부터 받은 Sync Time 데이터를 처리할지 말지 결정
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetApp_ModifySyncTime(bool enabled) {
            m_modifySyncTime = enabled;

            Ini.SetValueString($"{SectionName.App}", $"{AppKeyName.ModifySyncTime}", $"{m_modifySyncTime}", filePath);
        }
        /// <summary>
        /// CIM으로부터 받은 Sync Time 데이터를 처리하는지
        /// ture인 경우 CIM으로부터 받은 Sync Time 데이터를 토대로 시스템 시간 설정
        /// false인 경우 Sync Time 데이터 무시
        /// </summary>
        /// <returns></returns>
        public static bool GetApp_ModifySyncTime() {
            return m_modifySyncTime;
        }
    }
}
