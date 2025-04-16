using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WMX3ApiCLR;
using static Synustech.G_Var;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace Synustech
{
    /// <summary>
    /// Conveyor.cs는 이니셜라이징 및 주요 처리 부 작성
    /// </summary>
    public abstract partial class Conveyor
    {
        public enum ConveyorType
        {
            Normal,
            T,
            Z,
            ZT,
            Shuttle
        }
        public enum CnvRun
        {
            Stop,
            Run
        }
        public enum TurnStatus
        {
            Load,
            Busy,
            Unload,
        }
        public enum LineDirection
        {
            Input,
            Output
        }
        /// <summary>
        /// Conveyor의 제어 모드
        /// </summary>
        public enum ControlMode
        {
            MasterMode,
            CIMMode
        }
        /// <summary>
        /// T축 서보 사용 시 티칭 리스트
        /// </summary>
        public enum Teaching_T_Pos
        {
            Degree0_Pos,
            Degree90_Pos,
            Degree180_Pos,
            Degree270_Pos,
        }
        /// <summary>
        /// Common
        /// </summary>
        // Conveyor Parameter 변수
        public ControlMode m_eControlMode = ControlMode.MasterMode;   //포트의 제어 모드

        //public CSTIDReader m_TagReader_Interface;   //Port에 구성된 CST ID를 읽는 태그 장비(보통 In port에 구성되며 PortNetworkParam에 정보 존재)

        public static delMonitorLogView del_ELogSender_Conveyor;
        public int axis { get; set; } = -1;
        public int ID { get; set; }
        public string type { get; set; }
        public double autoVelocity { get; set; }
        public double autoAcc { get; set; }
        public double autoDec { get; set; }
        public double slowVelocity { get; set; }
        public double initVelocity { get; set; }
        public bool isInterface { get; private set; } = false;
        public bool isPIORun { get; private set; } = false;
        //public AlarmList m_ConveyorAlarmList;

        // Conveyor 상태 관련 변수
        public CnvRun run = CnvRun.Stop;
        public ServoOnOff servo = ServoOnOff.Off;
        public ServoOnOff previousServo = ServoOnOff.Off;
        public Mode mode = Mode.Manual;
        public Mode previousMode;
        public AlarmStatus alarmstatus = AlarmStatus.OK;

        // CST Load or Unload를 위한 양 옆 Conveyor 인스턴스
        public Conveyor beforeConv = null;
        public Conveyor nextConv = null;

        // CST 감지 관련 변수
        public bool isCSTEmpty = false;
        public bool isCSTInPosition;

        // Sensor 변수
        public SensorOnOff[] CSTDetect = new SensorOnOff[2];

        /// <summary>
        /// Turn
        /// </summary>
        // T-Axis 변수
        public bool LoadDetect;
        public bool UnloadDetect;

        // POS Sensor 변수
        public SensorOnOff[] POS = new SensorOnOff[4];

        // Load/Unload Position 관련 변수
        public int loadLocation = 0;
        public int unloadLocation = 0;
        public double loadPOS;
        public double unloadPOS;
        
        // HomeDone 변수
        public bool IsHomeDone = false;

        /// <summary>
        /// Conveyor Type이 셔틀이 제어되는 타입인 경우
        /// </summary>
        /// <returns></returns>
        public bool IsShuttleControlPort()
        {
            return false;
        }
    }
}
