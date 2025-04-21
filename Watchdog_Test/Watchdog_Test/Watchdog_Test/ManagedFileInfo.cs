using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watchdog_Test
{
    class ManagedFileInfo
    {
        /// <summary>
        /// Master Program에서 관리되는 파일들의 지정 경로를 나타냄
        /// StartupPath -> 실행 프로그램의 위치
        /// </summary>
        static public string StartUpPath { get => Application.StartupPath; }

        static public string LangPackDirectory { get => StartUpPath + "\\Lang"; }
        static public string LangPackFileName { get => "MasterLangPack.ini"; }

        static public string ParamSaveAdminDirectory { get => StartUpPath + "\\Settings"; }
        static public string ParamSaveAdminFileName { get => "ParamSaveAdmin.txt"; }

        static public string AppSettingsDirectory { get => StartUpPath + "\\Settings"; }
        static public string AppSettingsFileName { get => "SysAppInfo.ini"; }

        static public string EquipNetworkParamDirectory { get => StartUpPath + "\\Settings"; }
        static public string EquipNetworkParamFileName { get => "SysNetworkParamInfo.ini"; }

        static public string MasterSafetyItemInfoDirectory { get => StartUpPath + "\\Settings"; }
        static public string MasterSafetyItemInfoFileName { get => "SafetyItemInfo.xml"; }

        static public string EquipMasterIOParamDirectory { get => StartUpPath + "\\Settings"; }
        static public string EquipMasterIOParamFileName { get => "MasterIOParamInfo.xml"; }
        static public string EquipMotionParamDirectory { get => StartUpPath + "\\Settings"; }
        static public string EquipMotionParamFileName { get => "PortMotionParamInfo.xml"; }

        static public string PortUIParamDirectory { get => StartUpPath + "\\Settings"; }
        static public string PortUIParamFileName { get => "PortUIParamInfo.xml"; }

        static public string LogDirectory { get => StartUpPath + "\\Log"; }
        static public string LogFileName { get => "MasterLog.txt"; }

        static public string ExceptionLogDirectory { get => StartUpPath + "\\Exception"; }
        static public string ExceptionLogFileName { get => "ExceptionLog.txt"; }

        static public string STKLogDirectory { get => StartUpPath + "\\STK"; }
        static public string STKLogFileName { get => "STK_Motion_Log.txt"; }

        static public string CassetteIDDirectory { get => StartUpPath + "\\CassetteInfo"; }
        static public string CassetteIDFileName { get => "CassetteID.ini"; }

        static public string TeachingFileDirectory { get => StartUpPath + "\\Teaching"; }
        static public string TeachingFileName { get => $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_TeachingPosList.csv"; }
        static public string TeachingResultFileName { get => $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_TeachingResult.csv"; }
    }
}
