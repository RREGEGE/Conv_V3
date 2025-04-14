using System;
using System.IO;
using MYWMX3API.Classes;
using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MYWMX3API
{
    partial class WMXLib
    {
        static public class Datas
        {
            static public void Reset()
            {
                Engine.Reset();
                EtherCAT.Reset();
            }
            static public void Update()
            {
                Engine.Update();

                if (Engine.EngineStatus.State == EngineState.Communicating ||
                    Engine.EngineStatus.State == EngineState.Running)
                {
                    if (WMX3Api.IsDeviceValid())
                        EtherCAT.Update();
                }
                else
                    EtherCAT.EcMasterInfoList = new EcMasterInfoList();
            }

            static public class Engine
            {
                public enum LicenseID
                {
                    Simulation = 1,
                    EtherCAT = 2,
                    CCLink = 5,
                    Mechatrolink4 = 6,
                    CoreMotion = 10,
                    Log = 11,
                    ApiBuffer = 12,
                    CyclicBuffer = 13,
                    IO = 14,
                    Compensation = 15,
                    Event = 16,
                    UserMemory = 18,
                    AdvancedMotion = 17,
                    Kinematics = 28,
                    Coordinate = 26,
                    PMMotion = 19,
                    SenseIT = 33,
                    ForceControlDll = 32
                }
                public enum ModuleList
                {
                    CoreMotion = 0,
                    Log,
                    ApiBuffer,
                    CyclicBuffer,
                    IO,
                    Compensation,
                    Event,
                    AdvancedMotion,
                    UserMemory,
                    PMMotion,
                    Coordinate,
                    Kinematics,
                    SenseIT,
                    ForceControlDll
                }
                public enum PlatformList
                {
                    Simulation,
                    EtherCAT,
                    CCLink,
                    Mecha4,
                    EtherCAT_Simulation,
                    CCLink_Simulation,
                    Mecha4_Simulation,
                    EtherCAT_EtherCAT,
                    CCLink_EtherCAT,
                    Mecha4_EtherCAT,
                    CCLInk_EtherCAT_Simulation,
                    Mecha4_EtherCAT_Simulation
                }

                private enum EngineRepairStep
                {
                    RepairInit,
                    EngineStop,
                    EngineStart,
                    ErrorStateChange
                }
                static private EngineRepairStep eEngineRepairStep = EngineRepairStep.RepairInit;

                static public string EngineVersion = string.Empty;
                static public string IMDllVersion = string.Empty;
                static public int NumOfPlatformModule = 0;
                static public int NumOfFunctionModule = 0;
                static public ModulesInfo ModulesInfo = new ModulesInfo();
                static public EngineStatus EngineStatus = new EngineStatus();
                static public DevicesInfo DevicesInfo = new DevicesInfo();
                static public Queue<string> EngineMessageQ = new Queue<string>();

                static private bool bModuleError = false;
                static private bool bPlatformError = false;
                static private bool bAutoRestart = false;
                static private bool bAPIChannelTimeOut = false;

                static public string WMXInstallDirectory
                {
                    get { return RegistryControls.WMX.GetWMXDirectoryPath(); }
                }

                static public string WMXInstallVersion
                {
                    get { return RegistryControls.WMX.GetInstallProductVersion(); }
                }

                static public bool IsSimuInstalled()
                {

                    string SimulationPlatformDir = WMXInstallDirectory + @"\Platform\Simu\SimuPlatform.rtdll";

                    if (SimulationPlatformDir == null)
                        return false;

                    return File.Exists(SimulationPlatformDir);
                }

                static public bool IsEtherCATInstalled()
                {

                    string EtherCATPlatformDir = RegistryControls.WMX.GetEtherCATDirectoryPath();

                    if (EtherCATPlatformDir == null)
                        return false;

                    return File.Exists(EtherCATPlatformDir + @"\EcPlatform.rtdll") && File.Exists(WMXInstallDirectory + @"\Lib\EcApi_CLRLib.dll");
                }
                static public bool IsCCLinkTSNInstalled()
                {
                    string CCLinkPlatformDir = RegistryControls.WMX.GetCCLinkDirectoryPath();

                    if (CCLinkPlatformDir == null)
                        return false;

                    return File.Exists(CCLinkPlatformDir + @"\CCLinkPlatform.rtdll") && File.Exists(WMXInstallDirectory + @"\Lib\CCLinkApi_CLRLib.dll");
                }
                static public bool IsMechatrolink4Installed()
                {
                    string M4PlatformDir = RegistryControls.WMX.GetM4DirectoryPath();

                    if (M4PlatformDir == null)
                        return false;

                    return File.Exists(M4PlatformDir + @"\M4Platform.rtdll") && File.Exists(WMXInstallDirectory + @"\Lib\M4Api_CLRLib.dll");
                }

                static public bool IsModuleInstalled(ModuleList eModuleList)
                {
                    switch (eModuleList)
                    {
                        case ModuleList.CoreMotion:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "CoreMotion.rtdll");
                        case ModuleList.Log:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "Log.rtdll");
                        case ModuleList.ApiBuffer:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "ApiBuffer.rtdll");
                        case ModuleList.CyclicBuffer:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "CyclicBuffer.rtdll");
                        case ModuleList.IO:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "IO.rtdll");
                        case ModuleList.Compensation:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "Compensation.rtdll");
                        case ModuleList.Event:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "Event.rtdll");
                        case ModuleList.UserMemory:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "UserMemory.rtdll");
                        case ModuleList.AdvancedMotion:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "AdvancedMotion.rtdll");
                        case ModuleList.Kinematics:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "Kinematics.rtdll");
                        case ModuleList.Coordinate:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "Coordinate.rtdll");
                        case ModuleList.PMMotion:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "PMMotion.rtdll");
                        case ModuleList.SenseIT:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "SenseIT.rtdll");
                        case ModuleList.ForceControlDll:
                            return File.Exists(WMXInstallDirectory + @"\Motion\" + "ForceControlDll.rtdll");
                        default:
                            return false;
                    }
                }

                static public bool IsLicensed(LicenseID eLicense)
                {
                    return ModulesInfo.Engine.Options[(int)eLicense];
                }

                static public string PlatformTypeToString(PlatformList ePlatformList)
                {
                    switch (ePlatformList)
                    {
                        case PlatformList.Simulation:
                            return "Simulation";
                        case PlatformList.EtherCAT:
                            return "EtherCAT";
                        case PlatformList.CCLink:
                            return "CC-Link IE TSN";
                        case PlatformList.Mecha4:
                            return "Mechatrolink4";
                        case PlatformList.EtherCAT_Simulation:
                            return "EtherCAT + Simulation";
                        case PlatformList.CCLink_Simulation:
                            return "CC-Link IE TSN + Simulation";
                        case PlatformList.Mecha4_Simulation:
                            return "Mechatrolink4 + Simulation";
                        case PlatformList.EtherCAT_EtherCAT:
                            return "EtherCAT + EtherCAT";
                        case PlatformList.CCLink_EtherCAT:
                            return "CC-Link IE TSN + EtherCAT";
                        case PlatformList.Mecha4_EtherCAT:
                            return "Mechatrolink4 + EtherCAT";
                        case PlatformList.CCLInk_EtherCAT_Simulation:
                            return "CC-Link IE TSN + EtherCAT + Simulation";
                        case PlatformList.Mecha4_EtherCAT_Simulation:
                            return "Mechatrolink4 + EtherCAT + Simulation";
                        default:
                            return string.Empty;
                    }
                }
                static public int GetPlatformNum(PlatformList ePlatformList)
                {
                    switch (ePlatformList)
                    {
                        case PlatformList.Simulation:
                        case PlatformList.EtherCAT:
                        case PlatformList.CCLink:
                        case PlatformList.Mecha4:
                            return 1;
                            break;
                        case PlatformList.EtherCAT_Simulation:
                        case PlatformList.CCLink_Simulation:
                        case PlatformList.Mecha4_Simulation:
                        case PlatformList.EtherCAT_EtherCAT:
                        case PlatformList.CCLink_EtherCAT:
                        case PlatformList.Mecha4_EtherCAT:
                            return 2;
                            break;
                        case PlatformList.CCLInk_EtherCAT_Simulation:
                        case PlatformList.Mecha4_EtherCAT_Simulation:
                            return 3;
                            break;
                        default:
                            return 0;
                            break;
                    }
                }
                static public PlatformList GetCurrentPlatformType(bool simul, bool ethercat, bool cclink, bool mecha4, string numofmaster)
                {
                    if (simul)
                    {
                        if (ethercat)
                        {
                            if (cclink)
                                return PlatformList.CCLInk_EtherCAT_Simulation;
                            else if (mecha4)
                                return PlatformList.Mecha4_EtherCAT_Simulation;
                            else
                                return PlatformList.EtherCAT_Simulation;
                        }
                        else if (cclink)
                            return PlatformList.CCLink_Simulation;
                        else if (mecha4)
                            return PlatformList.Mecha4_Simulation;
                        else
                            return PlatformList.Simulation;
                    }
                    else if (ethercat)
                    {
                        if (cclink)
                            return PlatformList.CCLink_EtherCAT;
                        else if (mecha4)
                            return PlatformList.Mecha4_EtherCAT;
                        else
                        {
                            if (numofmaster == "2")
                                return PlatformList.EtherCAT_EtherCAT;
                            else
                                return PlatformList.EtherCAT;
                        }
                    }
                    else if (cclink)
                    {
                        return PlatformList.CCLink;
                    }
                    else if (mecha4)
                    {
                        return PlatformList.Mecha4;
                    }
                    else
                        return (PlatformList)(-1);
                }

                static public void Reset()
                {
                    EngineVersion = string.Empty;
                    IMDllVersion = string.Empty;
                    NumOfPlatformModule = 0;
                    NumOfFunctionModule = 0;
                    ModulesInfo = new ModulesInfo();
                    DevicesInfo = new DevicesInfo();
                }
                static public void Update()
                {
                    GetEngineMessage();
                    Thread.Sleep(5);

                    if (bAPIChannelTimeOut)
                    {
                        return;
                    }

                    if (bModuleError || bPlatformError)
                    {
                        switch (eEngineRepairStep)
                        {
                            case EngineRepairStep.RepairInit:
                                eEngineRepairStep = EngineRepairStep.EngineStop;
                                break;

                            case EngineRepairStep.EngineStop:
                                if (Datas.Engine.EngineStatus.State == EngineState.Communicating ||
                                    Datas.Engine.EngineStatus.State == EngineState.Running ||
                                    Datas.Engine.EngineStatus.State == EngineState.Idle)
                                {
                                    Controls.Engine.Stop();
                                    Thread.Sleep(1000);
                                }
                                else
                                    eEngineRepairStep = EngineRepairStep.EngineStart;
                                break;

                            case EngineRepairStep.EngineStart:
                                if (Datas.Engine.EngineStatus.State == EngineState.Shutdown ||
                                    Datas.Engine.EngineStatus.State == EngineState.Unknown)
                                {
                                    if (bAutoRestart)
                                    {
                                        Controls.Engine.TryCreateDevice();
                                        Controls.Engine.Start();
                                        Thread.Sleep(1000);
                                    }
                                    else
                                        eEngineRepairStep = EngineRepairStep.ErrorStateChange;
                                }
                                else
                                    eEngineRepairStep = EngineRepairStep.ErrorStateChange;
                                break;

                            case EngineRepairStep.ErrorStateChange:
                                bModuleError = false;
                                bPlatformError = false;
                                break;
                        }
                    }


                    if (Datas.Engine.EngineStatus.State == EngineState.Unknown && !WMX3Api.IsDeviceValid())
                        return;

                    GetEngineStatus();
                    Thread.Sleep(5);

                    if (Datas.Engine.EngineStatus.State == EngineState.Shutdown)
                        return;

                    GetModulesInfo();
                    Thread.Sleep(5);

                    GetAllDevices();
                    Thread.Sleep(5);

                    Datas.Engine.EngineVersion = GetEngineVer();
                    Datas.Engine.IMDllVersion = GetIMDllVersion();
                }
                static private void GetEngineMessage()
                {
                    string EngineMessage = string.Empty;
                    bool isWideChar = false;
                    int MessageLength = 0;
                    WMX3ApiCLR.WMX3Api.GetStdOutStr(ref EngineMessage, 1024, ref isWideChar, ref MessageLength);

                    if (EngineMessage != null)
                    {
                        if (EngineMessage.Contains("Motion module version") && EngineMessage.Contains("mismatches")) //EngineMessage.Contains("Modules load failed:0x00000006")
                        {
                            WMXLib.Datas.ModuleINI.DefaultModuleLoad();
                            bModuleError = true;
                            bAutoRestart = true;
                            eEngineRepairStep = EngineRepairStep.RepairInit;

                            MessageBoxEx.ShowInfoMessage("Modules version이 Engine version과 일치하지 않습니다.\nDefault Module Setting 으로 엔진을 시작합니다.");
                        }
                        if (EngineMessage.Contains("Failed to load Motion Module: coremotion") ||
                            EngineMessage.Contains("Failed to load Motion Module: log") ||
                            EngineMessage.Contains("Failed to load Motion Module: apibuffer") ||
                            EngineMessage.Contains("Failed to load Motion Module: cyclicbuffer") ||
                            EngineMessage.Contains("Failed to load Motion Module: io") ||
                            EngineMessage.Contains("Failed to load Motion Module: event") ||
                            EngineMessage.Contains("Failed to load Motion Module: usermemory"))
                        {
                            Controls.ModuleINI.ModuleINIReadAndSettings();
                            bModuleError = true;
                            bAutoRestart = true;
                            eEngineRepairStep = EngineRepairStep.RepairInit;
                            MessageBoxEx.ShowInfoMessage("Module INI Resetting...\nModule INI 점검 후 엔진을 재 시작합니다.");
                        }
                        if (EngineMessage.Contains("Registered license code does not contain") && (EngineMessage.ToLower().Contains("m4platform") || EngineMessage.ToLower().Contains("ecplatform") || EngineMessage.ToLower().Contains("cclinkplatform")))
                        {
                            WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                            bPlatformError = true;
                            bAutoRestart = true;
                            eEngineRepairStep = EngineRepairStep.RepairInit;

                            if (EngineMessage.ToLower().Contains("m4platform"))
                                MessageBoxEx.ShowInfoMessage("m4platform을 지원하지 않는 라이센스 입니다.\nSimulation platform 으로 엔진을 시작합니다.");
                            else if (EngineMessage.ToLower().Contains("ecplatform"))
                                MessageBoxEx.ShowInfoMessage("ecplatform을 지원하지 않는 라이센스 입니다.\nSimulation platform 으로 엔진을 시작합니다.");
                            else if (EngineMessage.ToLower().Contains("cclinkplatform"))
                                MessageBoxEx.ShowInfoMessage("cclinkplatform을 지원하지 않는 라이센스 입니다.\nSimulation platform 으로 엔진을 시작합니다.");
                        }
                        if (EngineMessage.Contains("Failed to load Platform Module") && (EngineMessage.ToLower().Contains("m4platform") || EngineMessage.ToLower().Contains("ecplatform") || EngineMessage.ToLower().Contains("cclinkplatform") || EngineMessage.ToLower().Contains("simuplatform")))
                        {
                            bAutoRestart = false;

                            if (EngineMessage.ToLower().Contains("m4platform"))
                                MessageBoxEx.ShowInfoMessage("m4platform이 경로에 없어 엔진을 시작 할 수 없습니다.");
                            else if (EngineMessage.ToLower().Contains("ecplatform"))
                                MessageBoxEx.ShowInfoMessage("ecplatform이 경로에 없어 엔진을 시작 할 수 없습니다.");
                            else if (EngineMessage.ToLower().Contains("cclinkplatform"))
                                MessageBoxEx.ShowInfoMessage("cclinkplatform이 경로에 없어 엔진을 시작 할 수 없습니다.");
                            else if (EngineMessage.ToLower().Contains("simuplatform"))
                                MessageBoxEx.ShowInfoMessage("simuplatform이 경로에 없어 엔진을 시작 할 수 없습니다.");
                        }
                        if (EngineMessage.Contains("RtndConfigure error"))
                        {
                            WMXLib.Datas.ModuleINI.DefaultPlatformLoad();
                            bPlatformError = true;
                            bAutoRestart = true;
                            eEngineRepairStep = EngineRepairStep.RepairInit;

                            MessageBoxEx.ShowInfoMessage("Rtnd Configure 설정이 올바르지 않습니다. \nPlatform Network 설정을 다시 진행해주세요.");
                        }
                    }

                    if (MessageLength > 0)
                        WMXLib.Datas.Engine.EngineMessageQ.Enqueue(EngineMessage);

                    if (WMXLib.Datas.Engine.EngineMessageQ.Count > 10000)
                        WMXLib.Datas.Engine.EngineMessageQ.Dequeue();
                }
                static private string GetEngineVer()
                {
                    //engine
                    int Engine_majorVersion = 0;
                    int Engine_minorVersion = 0;
                    int Engine_revisionVersion = 0;
                    int Engine_fixVersion = 0;

                    int err;
                    err = WMX3Api.GetLibVersion(ref Engine_majorVersion, ref Engine_minorVersion, ref Engine_revisionVersion, ref Engine_fixVersion);

                    if (err != ErrorCode.None)
                    {
                        return ErrorCodeToString(err);
                    }

                    return Engine_majorVersion.ToString() + "." + Engine_minorVersion.ToString() + "." + Engine_revisionVersion.ToString() + "." + Engine_fixVersion.ToString();
                }
                static private string GetIMDllVersion()
                {
                    //IMdll
                    int Imdll_Version = 0;
                    int Imdll_Revision = 0;

                    int err;
                    err = WMX3Api.GetIMDllVersion(ref Imdll_Version, ref Imdll_Revision);

                    if (err != ErrorCode.None)
                    {
                        return ErrorCodeToString(err);
                    }

                    return Imdll_Version.ToString() + "." + Imdll_Revision.ToString();
                }
                static private void GetEngineStatus()
                {
                    EngineState state = Datas.Engine.EngineStatus.State;

                    int err = WMX3Api.GetEngineStatus(ref Datas.Engine.EngineStatus);

                    if (err != ErrorCode.None)
                    {
                        if (err == 270)
                        {
                            bAPIChannelTimeOut = true;
                        }
                        if (err == 263)
                        {
                            //IM Dll Operating 불가 상태
                            Datas.Engine.EngineStatus.State = EngineState.Shutdown;
                        }
                        if (err == 301)
                        {
                            //Device가 없어 확인 불가
                            Datas.Engine.EngineStatus.State = EngineState.Unknown;
                        }

                        string ErrorText = ErrorCodeToString(err);
                    }

                    if ((state == EngineState.Idle || state == EngineState.Shutdown || state == EngineState.Unknown) &&
                        (Datas.Engine.EngineStatus.State == EngineState.Running || Datas.Engine.EngineStatus.State == EngineState.Communicating))
                        Datas.Reset(); //엔진이 시작될 때 데이터 클리어
                }
                static private void GetModulesInfo()
                {
                    WMX3Api.GetModulesInfo(ref Datas.Engine.ModulesInfo);

                    int nPlatformModule = 0;
                    int nFunctionModule = 0;

                    foreach (ModuleInfo info in Datas.Engine.ModulesInfo.Modules)
                    {
                        if (info.ModuleName.ToLower().Contains("ethercat") ||
                            info.ModuleName.ToLower().Contains("cclink") ||
                            info.ModuleName.ToLower().Contains("mechatrolink") ||
                            info.ModuleName.ToLower().Contains("simulation"))
                        {
                            nPlatformModule++;
                        }
                    }

                    Datas.Engine.NumOfPlatformModule = nPlatformModule;
                    Datas.Engine.NumOfFunctionModule = Datas.Engine.ModulesInfo.NumOfModule - nPlatformModule;
                }

                static private void GetAllDevices()
                {
                    WMX3Api.GetAllDevices(ref Datas.Engine.DevicesInfo);
                }
            }
            static public class EtherCAT
            {
                public class EcMasterESCInfoList
                {
                    public EcMasterESCInfo[] EcMasterESCInfo;
                }
                public class EcMasterESCInfo
                {
                    public SlaveESCInfo[] SlaveESCInfoList;
                }
                public class SlaveESCInfo
                {
                    public int m_Result = 0;
                    public bool TimeOut = false;
                    public byte[] RegValue = new byte[20];
                }
                static public EcMasterInfoList EcMasterInfoList = new EcMasterInfoList();
                static public EcMasterESCInfoList EcMasterESCInfolist = new EcMasterESCInfoList();

                static public void Reset()
                {
                    EcMasterInfoList = new EcMasterInfoList();
                    EcMasterESCInfolist = new EcMasterESCInfoList();
                }
                static public void Update()
                {
                    GetMasterInfo();

                    if (Controls.EtherCAT.ESC.ESCFlag > 0)
                    {
                        ESCUpdate();
                    }
                }
                static private void GetMasterInfo()
                {
                    foreach (ModuleInfo info in Datas.Engine.ModulesInfo.Modules)
                    {
                        if (info.ModuleName.ToLower().Contains("ethercat"))
                        {
                            if (WMX3Ecat != null)
                            {
                                WMX3Ecat.GetMasterInfoList(EcMasterInfoList);
                            }
                        }
                    }
                }

                static private void ESCUpdate()
                {
                    for (int nMasterNum = 0; nMasterNum < EcMasterInfoList.NumOfMasters; nMasterNum++)
                    {
                        if (EcMasterESCInfolist.EcMasterESCInfo == null ||
                            EcMasterESCInfolist.EcMasterESCInfo.Length != EcMasterInfoList.NumOfMasters)
                        {
                            EcMasterESCInfolist.EcMasterESCInfo = new EcMasterESCInfo[EcMasterInfoList.NumOfMasters];
                        }

                        if (EcMasterESCInfolist.EcMasterESCInfo[nMasterNum] == null)
                            EcMasterESCInfolist.EcMasterESCInfo[nMasterNum] = new EcMasterESCInfo();

                        for (int nSlaveNum = 0; nSlaveNum < EcMasterInfoList.Masters[nMasterNum].NumOfSlaves; nSlaveNum++)
                        {
                            if (EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList == null || 
                                EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList.Length != EcMasterInfoList.Masters[nMasterNum].NumOfSlaves)
                                EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList = new SlaveESCInfo[EcMasterInfoList.Masters[nMasterNum].NumOfSlaves];

                            if (EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList[nSlaveNum] == null)
                                EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList[nSlaveNum] = new SlaveESCInfo();

                            var slaveinfo = EcMasterInfoList.Masters[nMasterNum].Slaves[nSlaveNum];
                            var slaveESCInfo = EcMasterESCInfolist.EcMasterESCInfo[nMasterNum].SlaveESCInfoList[nSlaveNum];

                            if (!slaveinfo.Offline && !slaveinfo.Inaccessible)
                            {
                                Controls.EtherCAT.Register.Read RegRead = new Controls.EtherCAT.Register.Read();

                                if(RegRead.Send(nMasterNum, nSlaveNum, EtherCATSlaveChip.RegisterMap.RX_Error_Counter, 20, 500))
                                {
                                    slaveESCInfo.m_Result = RegRead.m_result;
                                    slaveESCInfo.RegValue = RegRead.m_data;
                                    slaveESCInfo.TimeOut = false;
                                }
                                else
                                {
                                    slaveESCInfo.m_Result = RegRead.m_result;
                                    slaveESCInfo.TimeOut = true;
                                }
                            }
                        }
                    }
                }
            }

            static public class RTX
            {
                static public string Version
                {
                    get { return RegistryControls.RTX.GetRTXVersion(); }
                }
            }
            static public class ModuleINI
            {
                public enum ModuleININoneSectionKey
                {
                    MessageLevel = 0,
                    PrintLog,
                    NumOfInterrupt,
                    InterruptDll,
                    ImaliveTimeout,
                    StdOut,
                    Location,
                    ServoIoInputAddr,
                    ServoIoOutputAddr
                }
                static public class Platform
                {
                    static public int NumOfPlatform = 0;
                    static public string[] PlatformDllName = new string[Global.MAX_Platform_Num];
                    static public string[] Platformdisable = new string[Global.MAX_Platform_Num];
                    static public string[] PlatformLocation = new string[Global.MAX_Platform_Num];
                    static public string[] PlatformMasterNum = new string[Global.MAX_Platform_Num];

                    static public void PlatformInit()
                    {
                        int NumOfPlatform = 0;
                        string[] PlatformDllName = new string[Global.MAX_Platform_Num];
                        string[] Platformdisable = new string[Global.MAX_Platform_Num];
                        string[] PlatformLocation = new string[Global.MAX_Platform_Num];
                        string[] PlatformMasterNum = new string[Global.MAX_Platform_Num];
                    }
                }

                static public class Module
                {
                    static public int NumOfModule = 0;
                    static public string[] ModuleDllName = new string[Global.MAX_Module_Num];
                    static public string[] Moduledisable = new string[Global.MAX_Module_Num];
                    static public string[] ModuleLocation = new string[Global.MAX_Module_Num];

                    static public void ModuleInit()
                    {
                        int NumOfModule = 0;
                        string[] ModuleDllName = new string[Global.MAX_Module_Num];
                        string[] Moduledisable = new string[Global.MAX_Module_Num];
                        string[] ModuleLocation = new string[Global.MAX_Module_Num];
                    }
                }

                static public string ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList eModuleList)
                {
                    switch (eModuleList)
                    {
                        case WMXLib.Datas.Engine.ModuleList.CoreMotion:
                            return "CoreMotion";
                        case WMXLib.Datas.Engine.ModuleList.Log:
                            return "Log";
                        case WMXLib.Datas.Engine.ModuleList.ApiBuffer:
                            return "ApiBuffer";
                        case WMXLib.Datas.Engine.ModuleList.CyclicBuffer:
                            return "CyclicBuffer";
                        case WMXLib.Datas.Engine.ModuleList.IO:
                            return "IO";
                        case WMXLib.Datas.Engine.ModuleList.Compensation:
                            return "Compensation";
                        case WMXLib.Datas.Engine.ModuleList.Event:
                            return "Event";
                        case WMXLib.Datas.Engine.ModuleList.AdvancedMotion:
                            return "AdvancedMotion";
                        case WMXLib.Datas.Engine.ModuleList.UserMemory:
                            return "UserMemory";
                        case WMXLib.Datas.Engine.ModuleList.PMMotion:
                            return "PMMotion";
                        case WMXLib.Datas.Engine.ModuleList.Coordinate:
                            return "Coordinate";
                        case WMXLib.Datas.Engine.ModuleList.Kinematics:
                            return "Kinematics";
                        case WMXLib.Datas.Engine.ModuleList.ForceControlDll:
                            return "ForceControlDll";
                        case WMXLib.Datas.Engine.ModuleList.SenseIT:
                            return "SenseIT";
                        default:
                            return null;
                    }
                }

                static public void DefaultModuleLoad()
                {
                    for (int nCount = 0; nCount < Global.MAX_Module_Num; nCount++)
                    {
                        FileControls.INIFile.Write("Module " + nCount.ToString(), null, null, Global.ModuleINIPath);
                        Thread.Sleep(10);
                    }

                    int nModuleNum = 0;

                    for (int nCount = 0; nCount < 7; nCount++)
                    {
                        switch (nCount)
                        {
                            case 0:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.CoreMotion))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.CoreMotion), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 1:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.Log))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.Log), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 2:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.ApiBuffer))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.ApiBuffer), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 3:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.CyclicBuffer))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.CyclicBuffer), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 4:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.IO))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.IO), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 5:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.Event))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.Event), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                            case 6:
                                if (WMXLib.Datas.Engine.IsModuleInstalled(Engine.ModuleList.UserMemory))
                                {
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "DllName", ModuleListToModuleName(WMXLib.Datas.Engine.ModuleList.UserMemory), Global.ModuleINIPath);
                                    FileControls.INIFile.Write("Module " + nModuleNum.ToString(), "disable", "0", Global.ModuleINIPath);
                                    nModuleNum++;
                                }
                                break;
                        }
                        Thread.Sleep(10);
                    }
                    Module.NumOfModule = 0;
                    Module.ModuleDllName = new string[Global.MAX_Module_Num];
                    Module.Moduledisable = new string[Global.MAX_Module_Num];
                    Module.ModuleLocation = new string[Global.MAX_Module_Num];
                }
                static public void DefaultPlatformLoad()
                {
                    for (int nCount = 0; nCount < Global.MAX_Platform_Num; nCount++)
                    {
                        FileControls.INIFile.Write("Platform " + nCount.ToString(), null, null, Global.ModuleINIPath);
                        Thread.Sleep(10);
                    }

                    if (WMXLib.Datas.Engine.IsSimuInstalled())
                    {
                        FileControls.INIFile.Write("Platform 0", "Location", @".\platform\simu\", Global.ModuleINIPath);
                        FileControls.INIFile.Write("Platform 0", "DllName", "simuplatform", Global.ModuleINIPath);
                        FileControls.INIFile.Write("Platform 0", "disable", "0", Global.ModuleINIPath);
                    }

                    Platform.NumOfPlatform = 0;
                    Platform.PlatformDllName = new string[Global.MAX_Platform_Num];
                    Platform.Platformdisable = new string[Global.MAX_Platform_Num];
                    Platform.PlatformLocation = new string[Global.MAX_Platform_Num];
                    Platform.PlatformMasterNum = new string[Global.MAX_Platform_Num];
                }

                static public void PlatformClear()
                {
                    for (int nCount = 0; nCount < Global.MAX_Platform_Num; nCount++)
                    {
                        FileControls.INIFile.Write("Platform " + nCount.ToString(), null, null, Global.ModuleINIPath);
                        Thread.Sleep(10);
                    }

                    Platform.NumOfPlatform = 0;
                    Platform.PlatformDllName = new string[Global.MAX_Platform_Num];
                    Platform.Platformdisable = new string[Global.MAX_Platform_Num];
                    Platform.PlatformLocation = new string[Global.MAX_Platform_Num];
                    Platform.PlatformMasterNum = new string[Global.MAX_Platform_Num];
                }
            }
        }
    }
}
