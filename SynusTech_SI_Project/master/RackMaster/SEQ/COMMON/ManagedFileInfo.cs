using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RackMaster.SEQ.COMMON {
    class ManagedFileInfo {
        public static string StartUpPath { get => Application.StartupPath; }
        public static string LangPackDirectory { get => $"{StartUpPath}\\Lang"; }
        public static string RackMasterLangFileName { get => $"RackMasterLangPack.ini"; }

        public static string SettingsDirectory { get => $"{StartUpPath}\\Settings"; }
        public static string RackMasterSettingParametersFileName { get => $"RackMasterSettingParameters.ini"; }
        public static string RackMasterAxisParametersFileName { get => $"RackMasterAxisParameters.ini"; }
        public static string WMXParameterFileName { get => $"wmx_parameters.ini"; }
        public static string UtilitySettingsFileName { get => $"UtilitySettings.ini"; }
        public static string IOParameterFileName { get => $"IoParameter.xml"; }
        public static string PortSettingFileName { get => "Port.xml"; }
        public static string PortParametersFileName { get => "PortParameter.ini"; }

        public static string DataDirectory { get => $"{StartUpPath}\\Data"; }
        public static string RackMasterDataFileName { get => $"RackMasterData.ini"; }
        

        public static string LogDirectory { get => $"{StartUpPath}\\Log"; }
        public static string LogFileName { get => $"RackMaster.txt"; }
        public static string ExceptionLogDirectory { get => $"{StartUpPath}\\Exception Log"; }
        public static string ExceptionLogFileName { get => $"ExceptionLog.txt"; }

        public static string LogoFileName { get => "logo.png"; }
    }
}
