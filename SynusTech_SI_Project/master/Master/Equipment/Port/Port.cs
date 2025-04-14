using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.ManagedFile;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using MovenCore;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Master.Equipment.Port.TagReader;
using Master.Interface.Alarm;
using Master.Interface.Safty;
using Master.Interface.Watchdog;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port.cs는 이니셜라이징 및 주요 처리 부 작성
    /// </summary>
    public partial class Port
    {
        /// <summary>
        /// Port의 운용 Type이 정의된 Enum
        /// Auto Step과 연관 있고 UI 출력과도 연관 있음
        /// 삭제, 생성 시 유의
        /// </summary>
        public enum PortType
        {
            MGV,
            MGV_AGV,
            AGV,
            MGV_OHT,
            OHT,
            Conveyor_AGV,
            Conveyor_OMRON,
            EQ
        }

        /// <summary>
        /// Port의 운용 방향이 정의된 Enum
        /// </summary>
        public enum PortDirection
        {
            Input,
            Output
        }

        /// <summary>
        /// Shuttle이 제어되는 Type의 경우 Buffer Type
        /// Auto 공정과 연관 있음
        /// 1 Buffer -> 셔틀이 LP에서 자재를 받아 OP로 이, 적재
        /// 2 Buffer -> LP에 이적재 된 자재를 셔틀이 운반하여 OP에 이, 적재
        /// </summary>
        public enum ShuttleCtrlBufferType
        {
            Two_Buffer,
            One_Buffer
        }
        
        /// <summary>
        /// Shuttle or Buffer가 제어되는 경우 제어 타입 지정
        /// Servo : Servo Amp, Motor를 통해 제어
        /// Inverter : I/O 접점 제어 또는 이더캣 Inverter인 경우 주파수 제어를 통해 제어
        /// Cylinder : I/O 접점 제어를 통해 제어
        /// None : 해당 축은 사용하지 않음
        /// </summary>
        public enum AxisCtrlType
        {
            None,
            Servo,
            Inverter,
            Cylinder
        }

        /// <summary>
        /// Conveyor의 제어를 활성화 함
        /// </summary>
        public enum CVCtrlEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// Conveyor의 Stopper 제어를 활성화 함
        /// </summary>
        public enum CVStopperEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// Conveyor의 Centering 제어를 활성화 함
        /// </summary>
        public enum CVCenteringEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// Conveyor의 Slow Sensor 제어를 활성화 함
        /// </summary>
        public enum CVSlowSensorEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// BP 단 연결 Conveyor가 존재하는 경우 CST 감지 센서를 활성화 함
        /// </summary>
        public enum CVCSTDetectSensorEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// 셔틀 제어시 Wait Pos 사용 유무를 활성화 함
        /// </summary>
        public enum WaitPosEnable
        {
            Disable,
            Enable
        }

        /// <summary>
        /// 셔틀 제어시 위치 확인 타입 설정
        /// Servo and Sensor : Servo의 위치 값과 Sensor의 On 유무를 같이 확인
        /// Servo : Servo의 위치 값 만으로 도착 여부 판단
        /// </summary>
        public enum PositionCheckType
        {
            Servo_and_Sensor,
            Servo
        }

        /// <summary>
        /// Inverter 제어 시 타입 설정
        /// IOControl : HighSpeed, LowSpeed, FWD, BWD의 Output 출력 조합으로 제어
        /// FreqControl : EtherCAT Inverter 한정으로 PDO에 구성된 Freq 값을 조정하여 제어 진행 (슬레이브 설정 및 PDO 주소 확인 필요)
        /// </summary>
        public enum InvCtrlMode
        {
            IOControl,
            FreqControl
        }

        /// <summary>
        /// Inverter IO 제어 시 제어 Flag
        /// </summary>
        public enum InvIOCtrlFlag
        {
            HighSpeed,
            LowSpeed,
            FWD,
            BWD
        }

        /// <summary>
        /// Inverter 제어 항목
        /// </summary>
        public enum InvCtrlType
        {
            HighSpeedFWD,
            LowSpeedFWD,
            HighSpeedBWD,
            LowSpeedBWD,
            FreqFWD,
            FreqBWD
        }

        /// <summary>
        /// Cylinder 제어 항목
        /// </summary>
        public enum CylCtrlList
        {
            FWD,
            BWD
        }

        /// <summary>
        /// Port 구동 모드
        /// Port Type과는 별개
        /// Port Type이 MGV_AGV면 AGV or MGV Operation Mode로 구동 가능한 것
        /// </summary>
        public enum PortOperationMode
        {
            MGV,
            AGV,
            OHT,
            Conveyor,
            EQ
        }

        /// <summary>
        /// Port의 제어 축 리스트
        /// Shuttle X, Z, T -> 셔틀 제어 타입의 포트 (MGV, AGV, OHT)
        /// Buffer -> 컨베이어 제어 타입의 포트 (Conveyor_AGV, Conveyor_Omron)
        /// Y축은 실리콘박스 Diebank 기구 문제로 인해 추가 됨
        /// </summary>
        public enum PortAxis : int
        {
            Shuttle_X,
            Shuttle_Z,
            Shuttle_T,
            Buffer_LP_X,
            Buffer_LP_Y,
            Buffer_LP_Z,
            Buffer_LP_T,
            Buffer_OP_X,
            Buffer_OP_Y,
            Buffer_OP_Z,
            Buffer_OP_T
        }

        /// <summary>
        /// Port의 컨베이어 리스트
        /// LP, OP - 물류 양 끝단
        /// BP - 양 끝단 사이의 추가 컨베이어 구성
        /// </summary>
        public enum BufferCV
        {
            Buffer_LP,
            Buffer_OP,
            Buffer_BP1,
            Buffer_BP2,
            Buffer_BP3,
            Buffer_BP4
        }

        /// <summary>
        /// X축 서보 사용 시 티칭 리스트
        /// </summary>
        public enum Teaching_X_Pos
        {
            OP_Pos,
            MGV_LP_Pos,
            Wait_Pos,
            Equip_LP_Pos
        }

        /// <summary>
        /// Y축 서보 사용 시 티칭 리스트
        /// </summary>
        public enum Teaching_Y_Pos
        {
            FWD_Pos,
            BWD_Pos
        }

        /// <summary>
        /// Z축 서보 사용 시 티칭 리스트
        /// </summary>
        public enum Teaching_Z_Pos
        {
            Up_Pos,
            Down_Pos
        }

        /// <summary>
        /// T축 서보 사용 시 티칭 리스트
        /// </summary>
        public enum Teaching_T_Pos
        {
            Degree0_Pos,
            Degree180_Pos
        }

        /// <summary>
        /// Port의 제어 모드
        /// </summary>
        public enum ControlMode
        {
            MasterMode,
            CIMMode
        }

        /// <summary>
        /// Port의 LED Bar Color
        /// </summary>
        enum LEDBar
        {
            Green,
            Red,
            None
        }

        public ControlMode m_eControlMode               = ControlMode.MasterMode;   //포트의 제어 모드
        public PortOperationMode m_ePortOperationMode   = PortOperationMode.MGV;    //포트의 제어 타입

        EquipNetworkParam.PortNetworkParam          m_PortParameter;                //포트의 ID, 타입 및 네트워크 구성 정보(메모리 맵, RFID)
        EquipPortMotionParam.PortMotionParameter    m_PortMotionParameter;          //포트 제어 시 사용되는 Parameter File
        EquipPortMotionParam.Port_UIParam           m_PortUIParameter;              //포트 제어 및 I/O Map 조작 시 기록되는 UI Parameter File

        WMXMotion m_WMXMotion       = null;         //서보 축 제어시 사용되는 WMX Motion Class
        WMXIO m_WMXIO               = null;         //I/O 제어시 사용되는 WMX IO Class 
        WMXMotion.AxisStatus[] m_WMXAxisStatus;     //서보 축 제어시 사용되는 WMX Axis 상태

        EStop mSW_EStop = new EStop();              //Port의 SW EStop 상태 (UI 상 Button 누름 시)
        EStop mHW_EStop = new EStop();              //Port의 HW Estop 상태 (Hardware EMO Switch 누를 시)

        public CSTIDReader m_TagReader_Interface;   //Port에 구성된 CST ID를 읽는 태그 장비(보통 In port에 구성되며 PortNetworkParam에 정보 존재)

        bool[] bPortAxisBusy                            = Enumerable.Repeat(false, Enum.GetValues(typeof(PortAxis)).Length).ToArray();  //Port 축 Busy 상태
        public bool[] bHomeDoneAndReloadEngineParam     = Enumerable.Repeat(false, Enum.GetValues(typeof(PortAxis)).Length).ToArray();  //Port Absolute Encoder Homing 시 Abs Offset 자동 갱신을 위한 변수

        private bool m_bAutoWMXParamInit = false;   //WMX 통신 시작하는 경우 Parameter Initialize 하기 위한 변수 (통신이 끊어졌다 붙는 경우 진행 됨, 엔진 재시작 시 파라미터 자동 적용 위함)

        public bool m_bInterlockOff = false;        //Interlock에 구속되어 아무런 제어가 안되는 경우를 대비해서 만든 비밀 기능(제어 창 축 선택화면 축 버튼 우 클릭 시 팝업)

        /// <summary>
        /// Port 객체 이니셜라이즈
        /// Master 장비 이니셜 과정에서 진행 됨
        /// </summary>
        /// <param name="_PortParameter"></param>
        public Port(EquipNetworkParam.PortNetworkParam _PortParameter)
        {
            //1. 전달 받은 Port 파라미터 맵핑
            m_PortParameter         = _PortParameter;

            //2. Port Type에 따른 Operation Mode 초기화
            m_ePortOperationMode    = GetPortTypeToOperationMode(m_PortParameter.ePortType);

            //3. Port의 UI Parameter Load (마지막 제어 성공 값 I/O Map)
            m_PortUIParameter       = new EquipPortMotionParam.Port_UIParam();
            m_PortUIParameter.Load(m_PortParameter.ID, ref m_PortUIParameter);

            //4. I/O Map 배열 길이가 안맞거나 정보가 없는 경우 초기화
            SafetyInfoSync(m_PortUIParameter);

            //5. WMX 관련 객체 Initialize -> Motion, I/O, Axis 상태
            m_WMXMotion     = new WMXMotion($"Port [{m_PortParameter.ID}] Motion", false);
            m_WMXIO         = new WMXIO();
            m_WMXAxisStatus = Enumerable.Repeat(new WMXMotion.AxisStatus(), Enum.GetValues(typeof(PortAxis)).Length).ToArray();

            //6. Alarm 관련 객체 초기화
            m_PortAlarm = new CodeAlarm();

            //7. 와치독 객체 초기화
            m_Watchdog = Enumerable.Repeat(new Watchdog(), Enum.GetValues(typeof(WatchdogList)).Length).ToArray();

            //8. Motion Parameter Load 진행
            InitMotionParam();

            //9. Port 상태 업데이트 스레드 시작
            PortStatusUpdateStart();
        }
        
        /// <summary>
        /// Port UI Param에 있는 객체가 정의된 사이즈와 다른 경우 Resize 진행 (파라미터 추가하는 경우 Exception 방지)
        /// 정의가 없는 Class는 초기화
        /// </summary>
        /// <param name="_Port_UIParam"></param>
        private void SafetyInfoSync(EquipPortMotionParam.Port_UIParam _Port_UIParam)
        {
            if (_Port_UIParam.port_SafetyImageInfos.Length < Enum.GetValues(typeof(Port_IO_TabPage)).Length)
            {
                Array.Resize(ref _Port_UIParam.port_SafetyImageInfos, Enum.GetValues(typeof(Port_IO_TabPage)).Length);
            }

            foreach (var eSafetyItemList in Enum.GetValues(typeof(Port_IO_TabPage)))
            {
                if (_Port_UIParam.port_SafetyImageInfos[(int)eSafetyItemList] == null)
                    _Port_UIParam.port_SafetyImageInfos[(int)eSafetyItemList] = new EquipPortMotionParam.Port_SafetyImageInfo();
            }
        }

        /// <summary>
        /// Port Type이 셔틀이 제어되는 타입인 경우 (범위)
        /// </summary>
        /// <returns></returns>
        public bool IsShuttleControlPort()
        {
            if (GetParam().ePortType >= PortType.MGV &&
                GetParam().ePortType <= PortType.OHT)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Port Type이 버퍼가 제어되는 타입인 경우 (범위)
        /// </summary>
        /// <returns></returns>
        public bool IsBufferControlPort()
        {
            if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                return true;
            else
                return false;
        }
        
        /// <summary>
        /// Port Type이 EQ 타입인 경우
        /// </summary>
        /// <returns></returns>
        public bool IsEQPort()
        {
            if (GetParam().ePortType == PortType.EQ)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Port 상태 업데이트 함수
        /// </summary>
        public void PortStatusUpdateStart()
        {
            Thread LocalThread = new Thread(delegate ()
            {
                //1. WMX Device가 정상적으로 할당 되어 있는 경우 업데이트 진행 
                while (m_WMXMotion != null && m_WMXMotion.IsDeviceValid() &&
                        m_WMXIO != null)
                {
                    //2. Engine 통신 중 여부 확인
                    bool bEngineCommunicating = WMX3.IsEngineCommunicating();
                    var eAxisGroup = Enum.GetValues(typeof(PortAxis));

                    if (bEngineCommunicating)
                    {
                        //3. 통신중이 아니었다 통신 상태로 변한 경우 Parameter Initialize 진행
                        if(!m_bAutoWMXParamInit) 
                        {
                            m_bAutoWMXParamInit = true;

                            //4. 공정중이 아닌 경우
                            if (!IsAutoControlRun() && !IsAutoManualCycleRun())
                            {
                                //5. Prog Parameter File에 저장 된 Data를 Engine에 Write
                                if (InitWMXParam())
                                {
                                    //6. Write 성공 시 Offset 재 조정(Absolute Home Offset, Home Shift Distance를 반영하여 현재 위치 재 계산)
                                    HomeDoneAndOffsetInitialize();
                                }
                            }
                        }

                        //7. WMX Engine에서 Update 된 Motion Data를 Update
                        WMXMotionStatusUpdate();

                        //8. WMX에서 Update 된 값을 기반으로 포트 상태 업데이트
                        PortStatusUpdate();
                    }
                    else
                        m_bAutoWMXParamInit = false; //9. 통신 중단 상태인 경우 Parameter Initialize Flag 초기화 (통신 연결 시 다시 Write 하기 위함)

                    Thread.Sleep(Master.StatusUpdateTime);
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Name = $"Port[{m_PortParameter.ID}] Port Status Update";
            LocalThread.Start();
        }
        
        /// <summary>
        /// Port Motion Parameter File Load
        /// </summary>
        public void InitMotionParam()
        {
            //1. Motion Parameter File Load
            m_PortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
            m_PortMotionParameter.Load(m_PortParameter.ID, ref m_PortMotionParameter);

            var eAxisGroup = Enum.GetValues(typeof(PortAxis));

            //2. 제어 축 배열 확인 (이전 버전과 이후 버전에서 축 개수가 증가하면서 이전 버전의 파라미터 로드 시 배열 오류 방지)
            if (m_PortMotionParameter.Ctrl_Axis.Length < eAxisGroup.Length)
            {
                Array.Resize(ref m_PortMotionParameter.Ctrl_Axis, eAxisGroup.Length);

                for (int nCount = 0; nCount < m_PortMotionParameter.Ctrl_Axis.Length; nCount++)
                    if (m_PortMotionParameter.Ctrl_Axis[nCount] == null)
                        m_PortMotionParameter.Ctrl_Axis[nCount] = new EquipPortMotionParam.AxisControlParam();
            }

            //3. 티칭 및 Pos Check Type 배열 확인 (이전 버전과 이후 버전에서 타입 수가 증가하면서 이전 버전의 파라미터 로드 시 배열 오류 방지)
            foreach (PortAxis axistype in eAxisGroup)
            {
                if (m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).TeachingPos.Length < 4)
                    Array.Resize(ref m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).TeachingPos, 4);

                if (m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).PositionCheckType.Length < 4)
                    Array.Resize(ref m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).PositionCheckType, 4);
            }

            //4. Watchdog Detect Time 적용
            Watchdog_Refresh_DetectTime();

            //5. OverLoad 제한 설정
            Motion_OverloadSettingTorque(PortAxis.Shuttle_X, GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).MaxLoad);
            Motion_OverloadSettingTorque(PortAxis.Shuttle_Z, GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).MaxLoad);
            Motion_OverloadSettingTorque(PortAxis.Shuttle_T, GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).MaxLoad);

            //6. Tag Reader기 정보 전달
            m_TagReader_Interface = new CSTIDReader(GetParam(), GetMotionParam());

            //8. CassetteInfo 값 이니셜 (Reader 관련 설정 로드하고 마지막 기록된 CST ID를 로드, CST ID는 자재가 없는 경우 자동 삭제됨)
            CassetteInfo.ReadFailCSTID();
            CassetteInfo.ReadTryCount();
            CassetteInfo.CanTopsReadPageCount();
            LP_CarrierID = CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.LP_CST_ID);
            OP_CarrierID = CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.OP_CST_ID);
            Carrier_SetBP_CarrierID(0, CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID1));
            Carrier_SetBP_CarrierID(1, CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID2));
            Carrier_SetBP_CarrierID(2, CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID3));
            Carrier_SetBP_CarrierID(3, CassetteInfo.ReadCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.BP_CST_ID4));
        }

        /// <summary>
        /// WMX Device 삭제 및 TagReader TCP/IP Client 종료
        /// </summary>
        public void ClosePort()
        {
            m_WMXMotion.CloseWMXMotion();
            m_TagReader_Interface.Close();

            int nCount = 0;
            while(m_WMXMotion.IsDeviceValid())
            {
                nCount++;
                Thread.Sleep(10);

                if (nCount > 500 || !m_WMXMotion.IsDeviceValid()) //Device가 삭제될 때까지 대기
                    break;
            }
        }

        /// <summary>
        /// Port 객체에 적용되어 있는 Network Parameter 정보를 리턴
        /// </summary>
        public EquipNetworkParam.PortNetworkParam GetParam()
        {
            return m_PortParameter;
        }

        /// <summary>
        /// Port 객체에 적용되어 있는 Motion Parameter 정보를 리턴
        /// </summary>
        public EquipPortMotionParam.PortMotionParameter GetMotionParam()
        {
            return m_PortMotionParameter;
        }

        /// <summary>
        /// Port 객체에 적용되어 있는 UI Parameter 정보를 리턴
        /// </summary>
        public EquipPortMotionParam.Port_UIParam GetUIParam()
        {
            return m_PortUIParameter;
        }

        /// <summary>
        /// Port 적용되어 있는 Manual 속도 정보를 리턴
        /// Port Class Initialize 과정에서 Motion Parameter File에서 Load된 값
        /// </summary>
        public float GetMotionManualSpeed(PortAxis ePortAxis)
        {
            return GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed * (float)(GetUIManualSpeedRatio() / 100.0);
        }
        
        /// <summary>
        /// Port 적용되어 있는 Manual 가속도 정보를 리턴
        /// Port Class Initialize 과정에서 Motion Parameter File에서 Load된 값
        /// </summary>
        public float GetMotionManualAcc(PortAxis ePortAxis)
        {
            return GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc * (float)(GetUIManualSpeedRatio() / 100.0);
        }
        
        /// <summary>
        /// Port 적용되어 있는 Manual 감속도 정보를 리턴
        /// Port Class Initialize 과정에서 Motion Parameter File에서 Load된 값
        /// </summary>
        public float GetMotionManualDec(PortAxis ePortAxis)
        {
            return GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec * (float)(GetUIManualSpeedRatio() / 100.0);
        }

        /// <summary>
        /// Port의 특정 축이 타겟 위치에서 범위 이내에 있는지 확인
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="TargetPosition"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public bool IsAxisPositionInside(PortAxis ePortAxis, float TargetPosition, float Range)
        {
            if ((Motion_CurrentPosition(ePortAxis) >= (TargetPosition - Range)) &&
                (Motion_CurrentPosition(ePortAxis) <= (TargetPosition + Range)) &&
                GetMotionParam().IsValidServo(ePortAxis))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Port의 특정 축이 타겟 위치에서 범위 밖에 있는지 확인
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="TargetPosition"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public bool IsAxisPositionOutside(PortAxis ePortAxis, float TargetPosition, float Range)
        {
            if ((Motion_CurrentPosition(ePortAxis) < (TargetPosition - Range)) || 
                (Motion_CurrentPosition(ePortAxis) > (TargetPosition + Range)) &&
                GetMotionParam().IsValidServo(ePortAxis))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 포트의 오토 공정 속도를 지정
        /// </summary>
        /// <param name="SpeedValue"></param>
        private void CMD_SetAutoRunSpeed(int SpeedValue)
        {
            GetMotionParam().AutoRun_Ratio = SpeedValue;
        }

        /// <summary>
        /// 포트의 과부하 기준 값을 설정
        /// </summary>
        /// <param name="SpeedValue"></param>
        private void CMD_SetOverLoadValue(PortAxis ePortAxis, short value)
        {
            Motion_OverloadSettingTorque(ePortAxis, value);
            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).MaxLoad = value;
        }

        /// <summary>
        /// 포트의 과부하 검출 값을 클리어
        /// </summary>
        /// <param name="SpeedValue"></param>
        private void CMD_SetOverLoadClear(PortAxis ePortAxis)
        {
            Motion_OverloadDetectTorque(ePortAxis, 0);
        }

        /// <summary>
        /// Port의 Motion 상태 업데이트
        /// WMX에서 값을 얻어옴
        /// </summary>
        private void WMXMotionStatusUpdate()
        {
            var eAxisGroup = Enum.GetValues(typeof(PortAxis));

            //1. EQ가 아닌 경우
            if (GetPortOperationMode() != PortOperationMode.EQ)
            {
                foreach (PortAxis axistype in eAxisGroup)
                {
                    //2. 서보 인 경우
                    if (GetMotionParam().IsValidServo(axistype))
                    {
                        //3. 축 번호 획득
                        int nAxis = GetMotionParam().GetServoAxisNum(axistype);
                        
                        //4. 해당 축 번호의 모션 상태 업데이트 (엔진에 있는 값을 Prog에 갱신)
                        m_WMXMotion.UpdateMotionStatus(nAxis);

                        //5. 상태 값으로 사용중인 객체에 복사
                        m_WMXAxisStatus[(int)axistype] = m_WMXMotion.m_axisStatus[nAxis].Copy();

                        //6. 앱솔루트 엔코더인 경우
                        if (!m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).WMXParam.m_absEncoderMode)
                            continue;

                        //7. HomeDone 상태 확인 및 아닌 경우 복구 시나리오
                        HomeDoneAndOffsetRecovery(axistype);
                    }
                }
            }
        }

        /// <summary>
        /// 통신 연결 시 최초 Abs Home Offset 처리 및 HomeDone 변환
        /// </summary>
        private void HomeDoneAndOffsetInitialize()
        {
            var eAxisGroup = Enum.GetValues(typeof(PortAxis));

            foreach (PortAxis axistype in eAxisGroup)
            {
                if (GetMotionParam().IsValidServo(axistype))
                {
                    if (!m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).WMXParam.m_absEncoderMode)
                    {
                        bHomeDoneAndReloadEngineParam[(int)axistype] = true;
                        continue;
                    }

                    int nAxis = GetMotionParam().GetServoAxisNum(axistype);

                    m_WMXMotion.UpdateMotionStatus(nAxis);
                    m_WMXAxisStatus[(int)axistype] = m_WMXMotion.m_axisStatus[nAxis].Copy();

                    bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(axistype);
                    double CMDEncorderPos = ServoCtrl_GetCMDEncorderPos(axistype);
                    double EncorderHomeOffset = ServoCtrl_GetCMDEncorderOffset(axistype); //엔진에서 가져와서 적용
                    double HomeShiftDistance = ServoCtrl_GetHomeShiftDistance(axistype);
                    int WMXDirection = m_PortMotionParameter.GetShuttleCtrl_ServoParam(axistype).WMXParam.m_motorDirection == WMXParam.m_motorDirection.Positive ? 1 : -1;
                    double WMXPos = ProgramUnitToWMXPos(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ((CMDEncorderPos - EncorderHomeOffset) * WMXDirection) + HomeShiftDistance);


                    ServoCtrl_SetCommandPos(axistype, WMXPos);
                    ServoCtrl_SetHomeDone(axistype);
                    bHomeDoneAndReloadEngineParam[(int)axistype] = true;
                }
            }
        }
        
        /// <summary>
        /// Home Done 잃어버린 경우 복구 시나리오
        /// -> 특정 축이 HotConnect를 통해 재 연결된 경우
        /// </summary>
        /// <param name="ePortAxis"></param>
        private void HomeDoneAndOffsetRecovery(PortAxis ePortAxis)
        {
            if (!ServoCtrl_GetHomeDone(ePortAxis) && !m_WMXAxisStatus[(int)ePortAxis].m_servoOffline)
            {
                WMXMotion.AxisParameter FileWMXParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).WMXParam;
                bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
                double CMDEncorderPos = ServoCtrl_GetCMDEncorderPos(ePortAxis);
                double EncorderHomeOffset = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, FileWMXParam.m_absEncoderHomeOffset * FileWMXParam.m_gearRatioDen / FileWMXParam.m_gearRatioNum); //현재 파일 값 기준으로 적용
                double HomeShiftDistance = ServoCtrl_GetHomeShiftDistance(ePortAxis);
                int WMXDirection = m_PortMotionParameter.GetShuttleCtrl_ServoParam(ePortAxis).WMXParam.m_motorDirection == WMXParam.m_motorDirection.Positive ? 1 : -1;
                double WMXPos = ProgramUnitToWMXPos(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ((CMDEncorderPos - EncorderHomeOffset) * WMXDirection) + HomeShiftDistance);


                ServoCtrl_SetCommandPos(ePortAxis, WMXPos);
                ServoCtrl_SetHomeDone(ePortAxis);
            }
        }

        /// <summary>
        /// 포트 상태 업데이트
        /// </summary>
        private void PortStatusUpdate()
        {
            //1. WMX와 연결된 I/O 업데이트 (센서 정보 업데이트)
            WMX_IO_Update();
            //2. 오토 상태인 경우 공정 시간 업데이트
            AutoRunProgressTimeUpdate();
            //3. CST ID Clear 관리 (카세트 없는 경우 자동 클리어)
            CSTAutoClearCheck();

            //4. 제어 상태에 따른 Bit Map, Word Map 업데이트
            if (GetParam().ePortType != PortType.EQ)
            {
                foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
                {
                    if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Servo)
                        ServoCtrl_BitWordUpdate(ePortAxis);
                    else if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Cylinder)
                        CylinderCtrl_BitWordUpdate(ePortAxis);
                    else if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Inverter)
                        InverterCtrl_BitWordUpdate(ePortAxis);
                }

                if (IsBufferControlPort())
                {
                    foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                    {
                        if (GetMotionParam().GetBufferControlEnable(eBufferCV) == CVCtrlEnable.Enable)
                            BufferCtrl_BitWordUpdate(eBufferCV);
                    }
                }
            }
            else
            {
                //5. EQ Port인 경우 PIO만 전달
                EQPIOPass();
            }

            //6. Port Alarm 상태에 따른 Alarm WordMap 업데이트
            AlarmListWordMapUpdate();

            //6. 기타 상태에 의한 Port -> CIM Bit Map Update
            PortToCIMBitMapUpdate();

            //6. 기타 상태에 의한 Port -> CIM Word Map Update
            PortToCIMWordMapUpdate();

            //7. 기타 상태에 의한 Port -> CIM Word Map Update
            AlarmCheck();


            if (GetParam().ePortType == PortType.EQ)
                return;

            //8. LP 영역 출입구 LED Bar Update
            Output_LightBarUpdate();
        }

        /// <summary>
        /// EQ Sensor에서 인식한 PIO를 Port -> STK PIO에 맵핑하는 함수
        /// </summary>
        private void EQPIOPass()
        { 
            PIOStatus_PortToSTK_Unload_Req      = PIOStatus_EQToRM_Unload_Req;
            PIOStatus_PortToSTK_Load_Req        = PIOStatus_EQToRM_Load_Req;
            PIOStatus_PortToSTK_Ready           = PIOStatus_EQToRM_Ready;
        }

        private void LEDBarControl(LEDBar eLEDBar)
        {
            switch (eLEDBar)
            {
                case LEDBar.Green:
                    Sensor_LP_LEDBar_Green = true;
                    Sensor_LP_LEDBar_Red = false;
                    break;
                case LEDBar.Red:
                    Sensor_LP_LEDBar_Green = false;
                    Sensor_LP_LEDBar_Red = true;
                    break;
                case LEDBar.None:
                default:
                    Sensor_LP_LEDBar_Green = false;
                    Sensor_LP_LEDBar_Red = false;
                    break;
            }
        }

        /// <summary>
        /// Port에 장착된 LED Bar를 공정 상태에 따라 업데이트
        /// </summary>
        private void Output_LightBarUpdate()
        {
            bool bAutoRunning = IsAutoControlRun() || IsAutoManualCycleRun();

            if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                bool LEDGreenStep;

                if (GetOperationDirection() == PortDirection.Input)
                {
                    LEDGreenStep =
                        Get_LP_AutoControlStep() == (int)LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid ||
                        Get_LP_AutoControlStep() == (int)LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load;
                }
                else
                {
                    LEDGreenStep =
                        Get_LP_AutoControlStep() == (int)LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid ||
                        Get_LP_AutoControlStep() == (int)LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload;
                }

                if (bAutoRunning && LEDGreenStep)
                    LEDBarControl(LEDBar.Green);
                else
                    LEDBarControl(LEDBar.Red);
            }
            else if (IsShuttleControlPort())
            {
                if (!bAutoRunning)
                {
                    LEDBarControl(LEDBar.Red);
                    return;
                }

                if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    if (GetOperationDirection() == PortDirection.Input)
                    {
                        if (IsOHT() && Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step200_InMode_Await_PIO_CS && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (IsAGV() && Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step210_InMode_Check_PIO_Valid && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step300_InMode_Await_MGV_CST_Load)
                            LEDBarControl(LEDBar.Green);
                        else
                            LEDBarControl(LEDBar.Red);
                    }
                    else
                    {
                        if (IsOHT() && Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step800_OutMode_Await_PIO_CS && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (IsAGV() && Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step810_OutMode_Check_PIO_Valid && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (Get_LP_AutoControlStep() == (int)LP_2BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload)
                            LEDBarControl(LEDBar.Green);
                        else
                            LEDBarControl(LEDBar.Red);
                    }
                }
                else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    //Cassette Input/Output 가능 상태
                    if (GetOperationDirection() == PortDirection.Input)
                    {
                        if(IsOHT() && Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if(IsAGV() && Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if(Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load)
                            LEDBarControl(LEDBar.Green);
                        else
                            LEDBarControl(LEDBar.Red);
                    }
                    else
                    {
                        if (IsOHT() && Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (IsAGV() && Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid && !IsAutoManualCycleRun())
                            LEDBarControl(LEDBar.Green);
                        else if (Get_LP_AutoControlStep() == (int)Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload)
                            LEDBarControl(LEDBar.Green);
                        else
                            LEDBarControl(LEDBar.Red);
                    }
                }
            }
            else
            {
                LEDBarControl(LEDBar.None);
            }
        }
        
        /// <summary>
        /// Port 공정 상황에 따라 Auto Run 시간 업데이트 진행
        /// Port가 공정중이지 않은 경우 Auto Run 시간 및 Watch dog 정지
        /// </summary>
        private void AutoRunProgressTimeUpdate()
        {
            if (IsAutoControlRun() || IsAutoManualCycleRun())
            {
                if (!m_AutoRunProgressTime.IsRunning)
                {
                    m_AutoRunProgressTime.Reset();
                    m_AutoRunProgressTime.Start();
                }
            }
            else
            {
                m_AutoRunProgressTime.Stop();
                Watchdog_Stop(WatchdogList.OP_Step_Timer, false);
                Watchdog_Stop(WatchdogList.LP_Step_Timer, false);
                Watchdog_Stop(WatchdogList.BP_Step_Timer, false);
                Watchdog_Stop(WatchdogList.EQ_Step_Timer, false);
            }
        }
        

        /// <summary>
        /// Port CST 감지 센서에 상태에 따라 CST ID Clear
        /// </summary>
        private void CSTAutoClearCheck()
        {
            if (!IsAutoControlRun() && !IsAutoManualCycleRun())
            {
                if (IsShuttleControlPort())
                {
                    if (Carrier_CheckOP_ExistProduct(false, false))
                        OP_CarrierID = string.Empty;

                    if (Carrier_CheckShuttle_ExistProduct(false))
                        Carrier_ClearBP_CarrierID(0);

                    if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                    {
                        if (Carrier_CheckLP_ExistProduct(false, false))
                            LP_CarrierID = string.Empty;
                    }
                }
                else if (IsBufferControlPort())
                {
                    //Initialize AutoControl

                    if (GetParam().ePortType == PortType.Conveyor_AGV)
                    {
                        if (Carrier_CheckOP_ExistProduct(false, false))
                            OP_CarrierID = string.Empty;

                        if (Carrier_CheckLP_ExistProduct(false))
                            LP_CarrierID = string.Empty;
                    }
                    else if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    {
                        if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                        {
                            if (!Sensor_OP_CST_Detect1 && !Sensor_OP_CST_Detect2)
                            {
                                OP_CarrierID = string.Empty;
                            }
                        }
                        else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                        {
                            if (Carrier_CheckOP_ExistProduct(false, false))
                                OP_CarrierID = string.Empty;
                        }

                        if (Carrier_CheckLP_ExistProduct(false))
                            LP_CarrierID = string.Empty;
                    }

                    var origin = Enum.GetValues(typeof(BufferCV));
                    foreach (BufferCV eBufferCV in origin)
                    {
                        if (eBufferCV < BufferCV.Buffer_BP1 || eBufferCV > BufferCV.Buffer_BP4)
                            continue;

                        if (GetMotionParam().IsCVUsed(eBufferCV))
                        {
                            if (GetMotionParam().IsCSTDetectSensorEnable(eBufferCV))
                            {
                                int BPIndex = (int)eBufferCV - 2;

                                if (!BufferCtrl_BP_CSTDetect_Status(eBufferCV))
                                {
                                    Carrier_ClearBP_CarrierID(BPIndex);
                                }
                            }
                        }
                    }

                    if (Carrier_CheckLP_ExistProduct(false, false))
                        LP_CarrierID = string.Empty;
                }
            }
        }
 
        /// <summary>
        /// Port Type에 따른 출력 String 지정
        /// </summary>
        /// <param name="ePortType"></param>
        /// <returns></returns>
        public string GetPortTypeToStr(PortType ePortType)
        {
            switch (ePortType)
            {
                case PortType.MGV:
                    return "MGV";
                case PortType.MGV_AGV:
                    return "MGV,AGV";
                case PortType.AGV:
                    return "AGV";
                case PortType.MGV_OHT:
                    return "MGV,OHT";
                case PortType.OHT:
                    return "OHT";
                case PortType.Conveyor_AGV:
                    return "Conveyor_AGV";
                case PortType.Conveyor_OMRON:
                    return "Conveyor_OMRON";
                case PortType.EQ:
                    return "EQ";
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Port Type에 따른 Port 운용 모드 지정 (이니셜 과정에서 사용)
        /// File에 기록된 Port Type Load -> Load된 Port Type 기본으로 Default Port Type 설정
        /// </summary>
        /// <param name="ePortType"></param>
        /// <returns></returns>
        private PortOperationMode GetPortTypeToOperationMode(PortType ePortType)
        {
            switch (ePortType)
            {
                case PortType.MGV_OHT:
                case PortType.MGV_AGV:
                case PortType.MGV:
                    return PortOperationMode.MGV;
                case PortType.AGV:
                    return PortOperationMode.AGV;
                case PortType.OHT:
                    return PortOperationMode.OHT;
                case PortType.Conveyor_AGV:
                case PortType.Conveyor_OMRON:
                    return PortOperationMode.Conveyor;
                case PortType.EQ:
                    return PortOperationMode.EQ;
                default:
                    return PortOperationMode.MGV;
            }
        }

        /// <summary>
        /// 현재 Port의 구동 모드를 가져옴
        /// MGV_AGV Type의 경우 MGV도 운용 가능하고 AGV도 운용 가능 (Auto 공정 Step의 차이)
        /// MGV_OHT Type의 경우 MGV도 운용 가능하고 OHT도 운용 가능 (Auto 공정 Step의 차이)
        /// Conveyor_AGV, Conveyor_OMRON 모두 Conveyor 운용 이지만 Auto 공정 Step에 차이가 있음(PIO 주체가 누군지)
        /// </summary>
        /// <returns></returns>
        public PortOperationMode GetPortOperationMode()
        {
            return m_ePortOperationMode;
        }

        /// <summary>
        /// 현재 Port의 구동 방향을 가져옴
        /// </summary>
        /// <returns></returns>
        public PortDirection GetOperationDirection()
        {
            return GetMotionParam().ePortDirection;
        }
        
        /// <summary>
        /// 현재 Port의 SW EStop 눌림 상태를 가져옴 (UI상 EMO 버튼)
        /// </summary>
        /// <returns></returns>
        public EStopState GetSWEStopState()
        {
            return mSW_EStop.GetEStopState();
        }

        /// <summary>
        /// Port의 상위 운용 모드를 지정함
        /// CIM -> CIM의 명령을 받아 처리 (Form의 버튼을 통한 명령은 조작 안됨)
        /// Master -> Master의 명령을 받아 처리 (Form에 구성된 버튼으로 조작 가능)
        /// </summary>
        /// <param name="econtrolMode"></param>
        private void CMD_PortSetControlMode(ControlMode econtrolMode)
        {
            m_eControlMode = econtrolMode;
        }
        
        /// <summary>
        /// Port의 공정 모드를 지정함
        /// MGV_AGV, MGV_OHT 등 Dual Mode인 경우만 변환 가능
        /// </summary>
        /// <param name="ePortOperationMode"></param>
        private void CMD_PortSetOperationMode(PortOperationMode ePortOperationMode)
        {
            if (GetParam().ePortType == PortType.MGV_AGV && Status_TypeChangeEnable)
            {
                if ((ePortOperationMode == PortOperationMode.AGV && m_ePortOperationMode == PortOperationMode.MGV) ||
                    (ePortOperationMode == PortOperationMode.MGV && m_ePortOperationMode == PortOperationMode.AGV))
                {
                    m_ePortOperationMode = ePortOperationMode;
                }
            }
            else if (GetParam().ePortType == PortType.MGV_OHT && Status_TypeChangeEnable)
            {
                if ((ePortOperationMode == PortOperationMode.OHT && m_ePortOperationMode == PortOperationMode.MGV) ||
                    (ePortOperationMode == PortOperationMode.MGV && m_ePortOperationMode == PortOperationMode.OHT))
                {
                    m_ePortOperationMode = ePortOperationMode;
                }
            }
        }
        
        /// <summary>
        /// Port의 공정 방향을 지정함
        /// Input 포트인지 Output Port인지 (공정 Step에 영향)
        /// </summary>
        /// <param name="ePortDirection"></param>
        private void CMD_PortSetAutoControlDirection(PortDirection ePortDirection)
        {
            if (!IsPortBusy() &&
                !IsAutoManualCycleRun() &&
                !IsAutoControlRun())
            {
                GetMotionParam().ePortDirection = ePortDirection;
                GetMotionParam().Save(GetParam().ID, GetMotionParam());
            }
        }

        /// <summary>
        /// Port의 Tag Reading 실패 시 Option을 지정함
        /// true인 경우 Fail -> 알람 처리
        /// false인 경우 Fail -> 경고 처리 후 ID 자동 부여 및 Step 연속 진행 (부여되는 ID는 CassetteInfo.ini에서 설정 가능)
        /// </summary>
        /// <param name="bEnable"></param>
        private void CMD_PortTagReadFailErrorOption(bool bEnable)
        {
            if (!IsAutoManualCycleRun() &&
                !IsAutoControlRun())
            {
                GetMotionParam().TagReadFailError = bEnable;
                GetMotionParam().Save(GetParam().ID, GetMotionParam());
            }
        }

        /// <summary>
        /// Port의 동작 중 상태를 나타냄
        /// </summary>
        /// <returns></returns>
        public bool IsPortBusy()
        {
            if (!IsEQPort())
            {
                foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
                {
                    if (ePortAxis == PortAxis.Shuttle_X && Sensor_X_Axis_Busy)
                        return true;
                    else if (ePortAxis == PortAxis.Shuttle_Z && Sensor_Z_Axis_Busy)
                        return true;
                    else if (ePortAxis == PortAxis.Shuttle_T && Sensor_T_Axis_Busy)
                        return true;
                    else if (bPortAxisBusy[(int)ePortAxis])
                        return true;
                }

                foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                {
                    if (BufferCtrl_CV_Is_Busy(eBufferCV))
                        return true;
                }
            }
            else
                return PIOStatus_EQToRM_Ready;

            return false;
        }
        
        /// <summary>
        /// Port의 특정 축에 대한 동작 중 상태를 나타냄
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        public bool IsPortAxisBusy(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Sensor_X_Axis_Busy;
                case PortAxis.Shuttle_Z:
                    return Sensor_Z_Axis_Busy;
                case PortAxis.Shuttle_T:
                    return Sensor_T_Axis_Busy;
                default:
                    return bPortAxisBusy[(int)ePortAxis];

            }
        }
        
        /// <summary>
        /// Port를 구성하고 있는 장비의 Power On 상태를 나타냄
        /// </summary>
        /// <returns></returns>
        public bool IsPortPowerOn()
        {
            bool bPowerOn = true;

            if (IsShuttleControlPort())
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(PortAxis)).Length; nRowCount++)
                {
                    PortAxis ePortAxis = (PortAxis)nRowCount;

                    if (GetMotionParam().IsServoType(ePortAxis) && !ServoCtrl_GetServoOn(ePortAxis))
                    {
                        bPowerOn = false;
                        break;
                    }
                }
            }

            return bPowerOn;
        }

        /// <summary>
        /// Port가 Amp Alarm 상태인지를 나타냄
        /// Amp Alarm은 이미 Port Alarm에서 체크 하므로 사용하는 곳 없음
        /// </summary>
        /// <returns></returns>
        public bool IsPortAxisAlarm()
        {
            bool bAlarm = false;

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(PortAxis)).Length; nRowCount++)
            {
                PortAxis ePortAxis = (PortAxis)nRowCount;

                if (GetMotionParam().IsServoType(ePortAxis) && ServoCtrl_GetAlarmStatus(ePortAxis))
                {
                    bAlarm = true;
                    break;
                }
            }

            return bAlarm;
        }
        
        /// <summary>
        /// Port의 Home 위치 완료 상태를 나타냄
        /// </summary>
        /// <returns></returns>
        public bool IsPortHomeDone()
        {
            bool bHomeDone = true;

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(PortAxis)).Length; nRowCount++)
            {
                PortAxis ePortAxis = (PortAxis)nRowCount;

                if (GetMotionParam().IsServoType(ePortAxis) && !ServoCtrl_GetHomeDone(ePortAxis))
                {
                    bHomeDone = false;
                    break;
                }
            }

            return bHomeDone;
        }

        /// <summary>
        /// Port의 전 축에 Stop 명령 전송
        /// </summary>
        public void CMD_PortStop()
        {
            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Servo)
                    ServoCtrl_MotionStop(ePortAxis);
                else if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Cylinder)
                    CylinderCtrl_MotionStop(ePortAxis);
                else if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Inverter)
                    InverterCtrl_MotionStop(ePortAxis);
            }

            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().GetBufferControlEnable(eBufferCV) == CVCtrlEnable.Enable)
                    BufferCtrl_CV_MotionStop(eBufferCV);

                if (GetMotionParam().IsCenteringEnable(eBufferCV))
                    BufferCtrl_Centering_MotionStop(eBufferCV);

                if (GetMotionParam().IsStopperEnable(eBufferCV))
                    BufferCtrl_Stopper_MotionStop(eBufferCV);
            }
        }
        
        /// <summary>
        /// Port를 SW EStop 상태로 변경
        /// </summary>
        public void CMD_PortSetSWEStop()
        {
            mSW_EStop.PushEStop();
        }
        
        /// <summary>
        /// Port의 SW EStop 상태를 해제
        /// </summary>
        public void CMD_PortReleaseSWEStop()
        {
            mSW_EStop.ReleaseEStop();
        }

        /// <summary>
        /// Port의 비상 정지 상태를 나타냄
        /// Port의 EMO와 관련있는 5가지 상태를 조합하여 판단
        /// Master.mPort_EStop : Master Alarm중 Port를 긴급정지 시켜야 하는 경우
        /// Master.IsWMXEStopState : WMX Engine에서 EStop이 발생한 경우
        /// mSW_EStop : UI상 E Stop Button을 누른 경우
        /// mHW_EStop : Hardware에서 EMO Switch를 누른 경우
        /// Master.mPortHandyTouch_EStop : Hand Touch Panel에 장착된 EMO Switch를 누른 경우
        /// </summary>
        /// <returns></returns>
        private bool IsPortEmergencyState()
        {
            if (Master.mPort_EStop.GetEStopState() == EStopState.EStop || 
                Master.IsWMXEStopState() || 
                mSW_EStop.GetEStopState() == EStopState.EStop || 
                mHW_EStop.GetEStopState() == EStopState.EStop ||
                Master.mPortHandyTouch_EStop.GetEStopState() == EStopState.EStop)
                return true;

            return false;
        }

        /// <summary>
        /// Master Program에서 Setting을 통해 지정된 축의 매뉴얼을 오픈
        /// Port - Setting - Shuttle Axis Setting 중 Servo 축의 경우 해당
        /// </summary>
        /// <param name="ePortAxis"></param>
        public void ManualOpen(PortAxis ePortAxis)
        {
            string ManualPath = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).ManualPath;

            if(string.IsNullOrEmpty(ManualPath))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisManualNotDefine"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.FilePathIsEmpty, $"{ePortAxis} Manual Path: {ManualPath}");
                return;
            }

            if(File.Exists(ManualPath))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = ManualPath;
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(ManualPath);

                    Process.Start(processStartInfo);
                }
                catch (Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{GetParam().ID} / {ePortAxis}] Manual Open");
                }
            }
            else
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisManualFileNotExist"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.FileIsNotExist, $"{ePortAxis} Manual File Path: {ManualPath}");
                return;
            }
        }
    
        /// <summary>
        /// I/O 저장 버튼 클릭 시 주소 중복 여부 체크
        /// </summary>
        /// <returns></returns>
        static public bool IsIOParamDupleCheck()
        {
            Dictionary<int, List<string>>  OutputsMap = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>>  InputsMap = new Dictionary<int, List<string>>();

            foreach (var port in Master.m_Ports)
            {
                port.Value.GetMotionParam().IOParamDupleCheck(port.Value.GetParam().ID, ref OutputsMap, ref InputsMap);
            }

            bool bDupleState = false;
            foreach (var value in OutputsMap)
            {
                if (value.Value.Count > 1)
                {
                    bDupleState = true;
                }
            }
            foreach (var value in InputsMap)
            {
                if (value.Value.Count > 1)
                {
                    bDupleState = true;
                }
            }

            if(bDupleState)
            {
                foreach (var value in OutputsMap)
                {
                    if (value.Value.Count > 1)
                    {
                        for (int nCount = 0; nCount < value.Value.Count; nCount++)
                        {
                            int StartAddr = value.Key / 8;
                            int Bit = value.Key % 8;
                            string PortID = value.Value[nCount].Split('_')[0];
                            LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.IODuple, $"Duple Location: {value.Value[nCount].Replace(PortID + "_", string.Empty)} / Output Address: {StartAddr}, {Bit}");
                        }
                    }
                }
                foreach (var value in InputsMap)
                {
                    if (value.Value.Count > 1)
                    {
                        for (int nCount = 0; nCount < value.Value.Count; nCount++)
                        {
                            int StartAddr = value.Key / 8;
                            int Bit = value.Key % 8;
                            string PortID = value.Value[nCount].Split('_')[0];
                            LogMsg.AddPortLog(PortID, LogMsg.LogLevel.Error, LogMsg.MsgList.IODuple, $"Duple Location: {value.Value[nCount].Replace(PortID + "_", string.Empty)} / Input Address: {StartAddr}, {Bit}");
                        }
                    }
                }
            }

            return bDupleState;
        }
    }
}
