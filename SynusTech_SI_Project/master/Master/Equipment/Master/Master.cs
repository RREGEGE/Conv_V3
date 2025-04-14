using Master.Equipment.CIM;
using Master.Equipment.Port;
using Master.Equipment.RackMaster;
using Master.Equipment.CPS;
using Master.Equipment.Omron;
using Master.ManagedFile;
using MovenCore;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using Master.Interface.Safty;
using System.Windows.Forms;
using System;


namespace Master
{
    /// <summary>
    /// Master 클래스는 전체 장비 및 장비 관련 모션의 이니셜라이즈를 담당 (중심부)
    /// </summary>
    static partial class Master
    {
        /// <summary>
        /// Memory Map Type
        /// </summary>
        public enum MapType
        {
            ReceiveBitMap,
            SendBitMap,
            ReceiveWordMap,
            SendWordMap
        }


        static public Color ErrorIntervalColor  = Color.Red;    //Error용 Color (Red <-> White)
        static public Color FocusIntervalColor  = Color.Lime;   //Focus용 Color (Lime <-> White)
        static public int UIUpdateIntervalTime  = 333;          //Form Update 주기
        static public int StatusUpdateTime      = 10;           //장비 상태 Update 주기 (CPU 사용률에 따른 조정 진행)
        static public int StepUpdateTime        = 10;           //장비 Step Update 주기 (CPU 사용률에 따른 조정 진행)


        static EquipMasterIOParam.MasterIOParameter m_MasterIOParameter = new EquipMasterIOParam.MasterIOParameter(); //Master에 사용되는 IO Parameter (최대 64개 등록)

        static public CPS m_CPS;                                        //Master에 할당되는 CPS 장비 (CPS 사용하는 경우)
        static public CIM m_CIM;                                        //Master에 할당되는 CIM 장비
        static public Omron m_Omron;                                    //Master에 할당되는 Omron 장비 (Port Type -> Diebank 한정 사용)
        static public Dictionary<string, RackMaster> m_RackMasters;     //Master에 할당되는 STK 장비 (ID, Class)
        static public Dictionary<string, Port> m_Ports;                 //Master에 할당되는 Port 장비 (ID, Class)

        static public byte[]    m_CPSByteMap;                           //CPS에서 사용하는 Byte Map, 실리콘박스 98 byte 고정 (변경 시 Packet Receive 영역 수정 및 맵 수정 필요)
        static public bool[]    m_CIM_RecvBitMap, m_CIM_SendBitMap, m_RackMaster_RecvBitMap, m_RackMaster_SendBitMap, m_Port_RecvBitMap, m_Port_SendBitMap;         //CIM, STK, Port Bit Map
        static public short[]   m_CIM_RecvWordMap, m_CIM_SendWordMap, m_RackMaster_RecvWordMap, m_RackMaster_SendWordMap, m_Port_RecvWordMap, m_Port_SendWordMap;   //CIM, STK, Port Word Map

        static public EStop mRM_EStop = new EStop();                    //Master의 Alarm에 따라 스토커 장비에 EMO 상황 전송
        static public EStop mPort_EStop = new EStop();                  //Master의 Alarm에 따라 포트 장비에 EMO 상황 전송

        static public bool[] m_ReadOmronBitMap      = new bool[4 * 64]; //Omron Comm 사용 시 메모리 맵(고정), Omron PLC와 통신 구조가 수정되는 경우 OmronCommunicator도 변경 필요 (메모리 맵 고정)
        static public short[] m_ReadOmronWordMap    = new short[4 * 64];
        static public bool[] m_WriteOmronBitMap     = new bool[4 * 64];
        static public short[] m_WriteOmronWordMap   = new short[4 * 64];

        /// <summary>
        /// Master Initialize 진행 (Parameter File, WMX, 장비 Class, 장비 Parameter File)
        /// </summary>
        /// <returns></returns>
        static public int MasterInit()
        {
            //1. Master에서 사용되는 IO Map 로드
            m_MasterIOParameter = new EquipMasterIOParam.MasterIOParameter();
            EquipMasterIOParam.LoadMasterIOParam(ref m_MasterIOParameter);

            //2. WMX Init
            int err = WMXInit();

            //3. err = 268인 경우 RTX 재실행 필요, 아닌 경우 장비 Initialize 진행
            if(err == 0 || err != 268)
                EquipmentInit();

            return err;
        }

        /// <summary>
        /// Master 종료 (Initialize시 할당 된 객체 삭제)
        /// </summary>
        static public void MasterClose()
        {
            //1. Thread 동작 정지
            WMXIOUpdateStop();
            MasterAlarmUpdateStop();

            //2. CIM 종료 (서버 닫고 소켓 정리)
            if (m_CIM != null)
                m_CIM.CloseCIM();

            //3. STK 종료 (연결 해제, 이벤트 해제)
            if (m_RackMasters != null)
            {
                foreach (var RackMaster in m_RackMasters)
                {
                    RackMaster.Value.CloseRackMaster();
                }
            }

            //4. 포트에 사용되는 WMX Motion Device 해제 및 Tag Reader기 통신 종료
            if (m_Ports != null)
            {
                foreach (var port in m_Ports)
                {
                    port.Value.ClosePort();
                }
            }

            //5. WMX Main Update Device 삭제
            WMX3.CloseDevice();
        }

        /// <summary>
        /// Master Class에서 관리되는 Motion Parameter 객체를 얻어옴
        /// I/O 파라미터
        /// </summary>
        /// <returns></returns>
        static public EquipMasterIOParam.MasterIOParameter GetMotionParam()
        {
            return m_MasterIOParameter;
        }

        /// <summary>
        /// WMX Initialize 진행
        /// 모션을 위해서는 WMX Engine이 구동되어야 함
        /// </summary>
        /// <returns></returns>
        static private int WMXInit()
        {
            //1. WMX에 Update용 Device 생성
            int err = WMX3.CreateWMX3Device("Synustech Master Main Update");

            if (err != WMXParam.ErrorCode_None)
            {
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-CreateWMX3Device", $"{WMX3.ErrorCodeToString(err)}");
                return err;
            }

            //2. Engine 통신 상태가 아닌 경우
            if (!WMX3.IsEngineCommunicating())
            {
                //3. 통신 진행 (통신이 연결되면 ECAT 슬레이브와 데이터를 주고 받음)
                err = WMX3.StartCommunicate();

                if (err != WMXParam.ErrorCode_None)
                    LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-StartCommunicate", $"{WMX3.ErrorCodeToString(err)}");
            }

            return err;
        }

        /// <summary>
        /// 마스터에서 사용되는 장비 Initialize 시퀀스
        /// 프로그램 시작 시 호출
        /// Network 인포 수정 시 호출
        /// </summary>
        static public void EquipmentInit()
        {
            //1. 장비 이니셜 시작 로그 작성
            LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.MasterEquipmentInitializeStart, string.Empty);
            
            //2. Update 스레드 돌고 있는 경우 Stop
            WMXIOUpdateStop();
            MasterAlarmUpdateStop();

            //3. 각각 장비 이니셜 진행
            CPSInit();
            OmronInit();
            CIMInit();
            RackMasterInit();
            PortInit();

            //4. Update 스레드 다시 진행
            WMXIOUpdateStart();
            MasterAlarmUpdateStart();

            //5. 장비 이니셜 앤드 로그 작성
            LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.MasterEquipmentInitializeEnd, string.Empty);
        }

        /// <summary>
        /// CPS 장비 Initialize 진행
        /// </summary>
        static public void CPSInit()
        {
            //1. 기존 할당된 장비가 있다면 연결 해제
            if (m_CPS != null)
                m_CPS.CloseCPS();

            //2. 메모리 맵 사이즈 지정
            int ByteMapSize = EquipNetworkParam.m_CPSNetworkParam.PacketSize >= 0 ? EquipNetworkParam.m_CPSNetworkParam.PacketSize : 0;
            
            //3. 객체 할당
            m_CPSByteMap = new byte[ByteMapSize];
            m_CPS = new CPS(EquipNetworkParam.m_CPSNetworkParam);
        }

        /// <summary>
        /// Omron 장비 Initialize 진행
        /// </summary>
        static public void OmronInit()
        {
            //1. 기존 할당된 장비가 있다면 연결 해제
            if (m_Omron != null)
                m_Omron.CloseOmron();

            //2. 사용하는 포트 중 Omron Type이 있다면 (파라미터 객체에서 읽어야 함, 포트 객체는 할당 전 이므로)
            bool bOmronInit = false;

            foreach (var PortParam in ManagedFile.EquipNetworkParam.m_PortNetworkParams)
            {
                if(PortParam.Value.ePortType == Port.PortType.Conveyor_OMRON)
                {
                    bOmronInit = true;
                    break;
                }
            }

            //3. 객체 할당
            m_Omron = new Omron(bOmronInit);
        }

        /// <summary>
        /// CIM 장비 Initialize 진행
        /// </summary>
        static private void CIMInit()
        {
            //1. 기존 할당된 장비가 있다면 연결 해제
            if (m_CIM != null)
                m_CIM.CloseCIM();

            //2. 메모리 맵 사이즈 지정
            int RecvBitMapStartAddr = EquipNetworkParam.m_CIMNetworkParam.RecvBitMapStartAddr >= 0 ? EquipNetworkParam.m_CIMNetworkParam.RecvBitMapStartAddr : 0;
            int SendBitMapStartAddr = EquipNetworkParam.m_CIMNetworkParam.SendBitMapStartAddr >= 0 ? EquipNetworkParam.m_CIMNetworkParam.SendBitMapStartAddr : 0;
            int RecvWordMapStartAddr = EquipNetworkParam.m_CIMNetworkParam.RecvWordMapStartAddr >= 0 ? EquipNetworkParam.m_CIMNetworkParam.RecvWordMapStartAddr : 0;
            int SendWordMapStartAddr = EquipNetworkParam.m_CIMNetworkParam.SendWordMapStartAddr >= 0 ? EquipNetworkParam.m_CIMNetworkParam.SendWordMapStartAddr : 0;

            int RecvBitMapSize = EquipNetworkParam.m_CIMNetworkParam.RecvBitMapSize >= 0 ? EquipNetworkParam.m_CIMNetworkParam.RecvBitMapSize : 0;
            int SendBitMapSize = EquipNetworkParam.m_CIMNetworkParam.SendBitMapSize >= 0 ? EquipNetworkParam.m_CIMNetworkParam.SendBitMapSize : 0;
            int RecvWordMapSize = EquipNetworkParam.m_CIMNetworkParam.RecvWordMapSize >= 0 ? EquipNetworkParam.m_CIMNetworkParam.RecvWordMapSize : 0;
            int SendWordMapSize = EquipNetworkParam.m_CIMNetworkParam.SendWordMapSize >= 0 ? EquipNetworkParam.m_CIMNetworkParam.SendWordMapSize : 0;

            //3. 객체 할당
            m_CIM_RecvBitMap = new bool[RecvBitMapStartAddr + RecvBitMapSize];
            m_CIM_SendBitMap = new bool[SendBitMapStartAddr + SendBitMapSize];
            m_CIM_RecvWordMap = new short[RecvWordMapStartAddr + RecvWordMapSize];
            m_CIM_SendWordMap = new short[SendWordMapStartAddr + SendWordMapSize];
            m_CIM = new CIM(EquipNetworkParam.m_CIMNetworkParam);
        }

        /// <summary>
        /// STK 장비 Initialize 진행
        /// STK 장비는 N대 할당이 가능하므로 메모리 맵 사이즈 연산 필요
        /// </summary>
        static private void RackMasterInit()
        {
            //1. 기존 할당된 장비가 있다면 연결 해제
            if (m_RackMasters != null)
            {
                foreach (var RackMaster in m_RackMasters)
                {
                    RackMaster.Value.CloseRackMaster();
                }
            }

            //2. 메모리 맵 사이즈 지정
            int RecvBitMapMax = 0, SendBitMapMax = 0, RecvWordMapMax = 0, SendWordMapMax = 0;

            foreach (var RackMasterParam in EquipNetworkParam.m_RackMasterNetworkParams)
            {
                int RecvBitMapStartAddr = RackMasterParam.Value.RecvBitMapStartAddr >= 0 ? RackMasterParam.Value.RecvBitMapStartAddr : 0;
                int SendBitMapStartAddr = RackMasterParam.Value.SendBitMapStartAddr >= 0 ? RackMasterParam.Value.SendBitMapStartAddr : 0;
                int RecvWordMapStartAddr = RackMasterParam.Value.RecvWordMapStartAddr >= 0 ? RackMasterParam.Value.RecvWordMapStartAddr : 0;
                int SendWordMapStartAddr = RackMasterParam.Value.SendWordMapStartAddr >= 0 ? RackMasterParam.Value.SendWordMapStartAddr : 0;

                int RecvBitMapSize = RackMasterParam.Value.RecvBitMapSize >= 0 ? RackMasterParam.Value.RecvBitMapSize : 0;
                int SendBitMapSize = RackMasterParam.Value.SendBitMapSize >= 0 ? RackMasterParam.Value.SendBitMapSize : 0;
                int RecvWordMapSize = RackMasterParam.Value.RecvWordMapSize >= 0 ? RackMasterParam.Value.RecvWordMapSize : 0;
                int SendWordMapSize = RackMasterParam.Value.SendWordMapSize >= 0 ? RackMasterParam.Value.SendWordMapSize : 0;

                if (RecvBitMapMax <= RecvBitMapStartAddr + RecvBitMapSize)
                    RecvBitMapMax = RecvBitMapStartAddr + RecvBitMapSize;
                if (SendBitMapMax <= SendBitMapStartAddr + SendBitMapSize)
                    SendBitMapMax = SendBitMapStartAddr + SendBitMapSize;
                if (RecvWordMapMax <= RecvWordMapStartAddr + RecvWordMapSize)
                    RecvWordMapMax = RecvWordMapStartAddr + RecvWordMapSize;
                if (SendWordMapMax <= SendWordMapStartAddr + SendWordMapSize)
                    SendWordMapMax = SendWordMapStartAddr + SendWordMapSize;
            }


            //3. 객체 할당
            m_RackMaster_RecvBitMap = new bool[RecvBitMapMax];
            m_RackMaster_SendBitMap = new bool[SendBitMapMax];
            m_RackMaster_RecvWordMap = new short[RecvWordMapMax];
            m_RackMaster_SendWordMap = new short[SendWordMapMax];

            m_RackMasters = new Dictionary<string, RackMaster>();

            foreach (var RackMasterParam in EquipNetworkParam.m_RackMasterNetworkParams)
            {
                if (!m_RackMasters.ContainsKey(RackMasterParam.Value.ID))
                {
                    m_RackMasters.Add(RackMasterParam.Value.ID, new RackMaster(RackMasterParam.Value));
                }
            }
        }

        /// <summary>
        /// Port 장비 Initialize 진행
        /// Port 장비는 N대 할당이 가능하므로 메모리 맵 사이즈 연산 필요
        /// </summary>
        static private void PortInit()
        {
            //1. 기존 할당된 장비가 있다면 연결 해제
            if (m_Ports != null)
            {
                foreach (var port in m_Ports)
                {
                    port.Value.ClosePort();
                }
            }

            //2. 메모리 맵 사이즈 지정
            int RecvBitMapMax = 0, SendBitMapMax = 0, RecvWordMapMax = 0, SendWordMapMax = 0;

            foreach (var PortParam in ManagedFile.EquipNetworkParam.m_PortNetworkParams)
            {
                int RecvBitMapStartAddr = PortParam.Value.RecvBitMapStartAddr >= 0 ? PortParam.Value.RecvBitMapStartAddr : 0;
                int SendBitMapStartAddr = PortParam.Value.SendBitMapStartAddr >= 0 ? PortParam.Value.SendBitMapStartAddr : 0;
                int RecvWordMapStartAddr = PortParam.Value.RecvWordMapStartAddr >= 0 ? PortParam.Value.RecvWordMapStartAddr : 0;
                int SendWordMapStartAddr = PortParam.Value.SendWordMapStartAddr >= 0 ? PortParam.Value.SendWordMapStartAddr : 0;

                int RecvBitMapSize = PortParam.Value.RecvBitMapSize >= 0 ? PortParam.Value.RecvBitMapSize : 0;
                int SendBitMapSize = PortParam.Value.SendBitMapSize >= 0 ? PortParam.Value.SendBitMapSize : 0;
                int RecvWordMapSize = PortParam.Value.RecvWordMapSize >= 0 ? PortParam.Value.RecvWordMapSize : 0;
                int SendWordMapSize = PortParam.Value.SendWordMapSize >= 0 ? PortParam.Value.SendWordMapSize : 0;

                if (RecvBitMapMax <= RecvBitMapStartAddr + RecvBitMapSize)
                    RecvBitMapMax = RecvBitMapStartAddr + RecvBitMapSize;
                if (SendBitMapMax <= SendBitMapStartAddr + SendBitMapSize)
                    SendBitMapMax = SendBitMapStartAddr + SendBitMapSize;
                if (RecvWordMapMax <= RecvWordMapStartAddr + RecvWordMapSize)
                    RecvWordMapMax = RecvWordMapStartAddr + RecvWordMapSize;
                if (SendWordMapMax <= SendWordMapStartAddr + SendWordMapSize)
                    SendWordMapMax = SendWordMapStartAddr + SendWordMapSize;
            }

            //3. 객체 할당
            m_Port_RecvBitMap = new bool[RecvBitMapMax];
            m_Port_SendBitMap = new bool[SendBitMapMax];
            m_Port_RecvWordMap = new short[RecvWordMapMax];
            m_Port_SendWordMap = new short[SendWordMapMax];


            m_Ports = new Dictionary<string, Port>();

            foreach (var PortParam in ManagedFile.EquipNetworkParam.m_PortNetworkParams)
            {
                if (!m_Ports.ContainsKey(PortParam.Value.ID))
                {
                    m_Ports.Add(PortParam.Value.ID, new Port(PortParam.Value));
                }
            }
        }
        
        /// <summary>
        /// Master에 할당 된 CIM Class 객체를 얻어 옴 (public이라 큰 의미 X)
        /// </summary>
        /// <returns></returns>
        static public CIM GetCIMPt()
        {
            return m_CIM;
        }

        /// <summary>
        /// Master에 할당 된 STK Class 객체를 얻어 옴 (메모리 주소 위치에 따른 객체를 얻어 옴)
        /// </summary>
        /// <param name="eMapType"></param>
        /// <param name="MapAddress"></param>
        /// <returns></returns>
        static public RackMaster ConvertMemoryAddressToRackMasterPt(MapType eMapType, int MapAddress)
        {
            foreach(var rackMaster in m_RackMasters)
            {
                if(eMapType == MapType.ReceiveBitMap)
                {
                    if (MapAddress >= rackMaster.Value.GetParam().RecvBitMapStartAddr && MapAddress < rackMaster.Value.GetParam().RecvBitMapStartAddr + rackMaster.Value.GetParam().RecvBitMapSize)
                        return m_RackMasters[rackMaster.Key];
                }
                else if (eMapType == MapType.SendBitMap)
                {
                    if (MapAddress >= rackMaster.Value.GetParam().SendBitMapStartAddr && MapAddress < rackMaster.Value.GetParam().SendBitMapStartAddr + rackMaster.Value.GetParam().SendBitMapSize)
                        return m_RackMasters[rackMaster.Key];
                }
                else if (eMapType == MapType.ReceiveWordMap)
                {
                    if (MapAddress >= rackMaster.Value.GetParam().RecvWordMapStartAddr && MapAddress < rackMaster.Value.GetParam().RecvWordMapStartAddr + rackMaster.Value.GetParam().RecvWordMapSize)
                        return m_RackMasters[rackMaster.Key];
                }
                else if (eMapType == MapType.SendWordMap)
                {
                    if (MapAddress >= rackMaster.Value.GetParam().SendWordMapStartAddr && MapAddress < rackMaster.Value.GetParam().SendWordMapStartAddr + rackMaster.Value.GetParam().SendWordMapSize)
                        return m_RackMasters[rackMaster.Key];
                }
            }

            return null;
        }

        /// <summary>
        /// Master에 할당 된 Port Class 객체를 얻어 옴 (메모리 주소 위치에 따른 객체를 얻어 옴)
        /// </summary>
        /// <param name="eMapType"></param>
        /// <param name="MapAddress"></param>
        /// <returns></returns>
        static public Port ConvertMemoryAddressToPortPt(MapType eMapType, int MapAddress)
        {
            foreach (var port in m_Ports)
            {
                if (eMapType == MapType.ReceiveBitMap)
                {
                    if (MapAddress >= port.Value.GetParam().RecvBitMapStartAddr && MapAddress < port.Value.GetParam().RecvBitMapStartAddr + port.Value.GetParam().RecvBitMapSize)
                        return m_Ports[port.Key];
                }
                else if (eMapType == MapType.SendBitMap)
                {
                    if (MapAddress >= port.Value.GetParam().SendBitMapStartAddr && MapAddress < port.Value.GetParam().SendBitMapStartAddr + port.Value.GetParam().SendBitMapSize)
                        return m_Ports[port.Key];
                }
                else if (eMapType == MapType.ReceiveWordMap)
                {
                    if (MapAddress >= port.Value.GetParam().RecvWordMapStartAddr && MapAddress < port.Value.GetParam().RecvWordMapStartAddr + port.Value.GetParam().RecvWordMapSize)
                        return m_Ports[port.Key];
                }
                else if (eMapType == MapType.SendWordMap)
                {
                    if (MapAddress >= port.Value.GetParam().SendWordMapStartAddr && MapAddress < port.Value.GetParam().SendWordMapStartAddr + port.Value.GetParam().SendWordMapSize)
                        return m_Ports[port.Key];
                }
            }

            return null;
        }

        /// <summary>
        /// WMX Engine의 EMO Switch 상태를 가져옴
        /// </summary>
        /// <returns></returns>
        static public bool IsWMXEStopState()
        {
            return WMX3.IsEmergencyStop();
        }

        /// <summary>
        /// Master에서 사용되는 I/O Update 스레드 시작
        /// </summary>
        static private void WMXIOUpdateStart()
        {
            if (!m_IOUpdateThread.IsAlive)
            {
                m_IOUpdateThread = new Thread(IOUpdate);
                m_IOUpdateThread.IsBackground = true;
                m_IOUpdateThread.Name = "Master IO Updater";
                m_IOUpdateThread.Start();
            }
        }

        /// <summary>
        /// Master에서 사용되는 I/O Update 스레드 정지
        /// </summary>
        static private void WMXIOUpdateStop()
        {
            if (m_IOUpdateThread.IsAlive)
            {
                m_IOUpdateThread.Abort();
                m_IOUpdateThread.Join();
            }
        }

        /// <summary>
        /// Master에서 사용되는 Alarm Update 스레드 시작
        /// </summary>
        static private void MasterAlarmUpdateStart()
        {
            if (!m_MasterAlarmUpdateThread.IsAlive)
            {
                m_MasterAlarmUpdateThread = new Thread(MasterAlarmUpdateThread);
                m_MasterAlarmUpdateThread.IsBackground = true;
                m_MasterAlarmUpdateThread.Name = "Synustech Master Alarm Updater";
                m_MasterAlarmUpdateThread.Start();
            }
        }

        /// <summary>
        /// Master에서 사용되는 Alarm Update 스레드 정지
        /// </summary>
        static private void MasterAlarmUpdateStop()
        {
            if (m_MasterAlarmUpdateThread.IsAlive)
            {
                m_MasterAlarmUpdateThread.Abort();
                m_MasterAlarmUpdateThread.Join();
            }
        }

        /// <summary>
        /// Master에서 Alarm 발생 시 WMX 관련 알람인 경우 리커버리 동작 수행
        /// </summary>
        /// <param name="Message"></param>
        static public void Do_MasterRecovery(bool Message = true)
        {
            //1. EtherCAT 통신 상태 알람이 뜬 경우
            if (AlarmContains(MasterAlarm.EtherCAT_Communication_Error))
            {
                //2. 엔진은 구동중이지만 엔진 통신 상태가 아닌 경우 
                if (!MovenCore.WMX3.IsEngineCommunicating() && MovenCore.WMX3.IsEngineRunning())
                {
                    //3. Comm 시작 메세지 출력 (패킷으로 알람 클리어가 오는 경우 무조건 복구 루틴 수행)
                    DialogResult result = !Message ? DialogResult.OK : MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_WMXCommunicationStart"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        int err = MovenCore.WMX3.StartCommunicate();

                        if (err != MovenCore.WMXParam.ErrorCode_None)
                            LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-StartCommunicate", $"{MovenCore.WMX3.ErrorCodeToString(err)}");
                    }
                }
            }

            //1. 보유한 Slave중 OP 상태가 아니면서 Door E Stop 발생의 경우 (Door E Stop은 모든 전원 차단)
            if (AlarmContains(MasterAlarm.Slave_Not_Op_State_Error) && (AlarmContains(MasterAlarm.HP_Door_E_Stop) || AlarmContains(MasterAlarm.OP_Door_E_Stop)))
            {
                //2. 엔진 정상 동작 및 통신 중이며 Slave 중 OP가 아닌 경우 (전원 차단 시나리오) 
                if (MovenCore.WMX3.IsEngineCommunicating() && MovenCore.WMX3.IsEngineRunning() && MovenCore.WMX3.SlaveNotOpState())
                {
                    //3. 전체 Stop Com -> Scan -> Start Com 진행
                    DialogResult result = !Message ? DialogResult.OK : MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_WMXReConnectStart"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        int err = MovenCore.WMX3.StopCommunicate();

                        if (err != MovenCore.WMXParam.ErrorCode_None)
                            LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-StopCommunicate", $"{MovenCore.WMX3.ErrorCodeToString(err)}");

                        err = MovenCore.WMX3.ScanSlave();

                        if (err != MovenCore.WMXParam.ErrorCode_None)
                            LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-ScanSlave", $"{MovenCore.WMX3.ErrorCodeToString(err)}");

                        err = MovenCore.WMX3.StartCommunicate();

                        if (err != MovenCore.WMXParam.ErrorCode_None)
                            LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-StartCommunicate", $"{MovenCore.WMX3.ErrorCodeToString(err)}");
                    }
                }
            }
            else if (AlarmContains(MasterAlarm.Slave_Not_Op_State_Error)) //Door 알람 상태는 아니지만 Slave가 OP State Error인 경우
            {
                //2. 엔진 정상 동작 및 통신 중이며 Slave 중 OP가 아닌 경우 (단순히 Slave 한대가 OP가 아닌 시나리오 => 교체, 불량인 상황) 
                if (MovenCore.WMX3.IsEngineCommunicating() && MovenCore.WMX3.IsEngineRunning() && MovenCore.WMX3.SlaveNotOpState())
                {
                    //3. 핫 커넥션을 통한 연결 복구
                    DialogResult result = !Message ? DialogResult.OK : MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_WMXHotConnectStart"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        int err = MovenCore.WMX3.StartHotConnect();

                        if (err != MovenCore.WMXParam.ErrorCode_None)
                            LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3API-HotConnect", $"{MovenCore.WMX3.ErrorCodeToString(err)}");
                    }
                }
            }
        }

        /// <summary>
        /// Master에 연결된 DTP의 DeadMan 상태에 따라 조작중인 장비를 정지
        /// DO : Dead Man Switch를 누른 후 제어 중 Dead Man Switch를 떼는 경우 정지해야함
        /// </summary>
        static private void EquipStopCMDFromDeadManStatus()
        {
            if (Is_DeadMan_Status_ControlLock)
            {
                foreach (var port in m_Ports)
                {
                    if (port.Value.DeadManControlAxis.Count > 0)
                    {
                        for (int nCount = 0; nCount < port.Value.DeadManControlAxis.Count; nCount++)
                        {
                            var CtrlType = port.Value.DeadManControlAxis[nCount];

                            if (port.Value.GetMotionParam().GetAxisControlType(CtrlType) == Port.AxisCtrlType.Servo)
                            {
                                port.Value.ServoCtrl_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Servo");
                            }
                            else if (port.Value.GetMotionParam().GetAxisControlType(CtrlType) == Port.AxisCtrlType.Inverter)
                            {
                                port.Value.InverterCtrl_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Inverter");
                            }
                            else if (port.Value.GetMotionParam().GetAxisControlType(CtrlType) == Port.AxisCtrlType.Cylinder)
                            {
                                port.Value.CylinderCtrl_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Cylinder");
                            }
                        }

                        port.Value.DeadManControlAxis.Clear();
                    }

                    if (port.Value.DeadManControlBuffer.Count > 0)
                    {
                        for (int nCount = 0; nCount < port.Value.DeadManControlBuffer.Count; nCount++)
                        {
                            var CtrlType = port.Value.DeadManControlBuffer[nCount];

                            if (port.Value.GetMotionParam().IsCenteringEnable(CtrlType))
                            {
                                port.Value.BufferCtrl_Centering_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Centering");
                            }

                            if (port.Value.GetMotionParam().IsStopperEnable(CtrlType))
                            {
                                port.Value.BufferCtrl_Stopper_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Stopper");
                            }

                            if (port.Value.GetMotionParam().IsCVUsed(CtrlType))
                            {
                                port.Value.BufferCtrl_CV_MotionStop(CtrlType);
                                LogMsg.AddMasterLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.DeadManSwitchReleaseStop, $"Port [{port.Value.GetParam().ID}], {CtrlType} Conveyor");
                            }
                        }

                        port.Value.DeadManControlBuffer.Clear();
                    }
                }
            }
        }
    }
}
