using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.PART;
using MovenCore;

namespace RackMaster.SEQ.COMMON
{
    public class Axis {
        private WMXMotion m_WMXMotion;
        private WMXMotion.AxisStatus[] m_status;

        private WMXBarcodeMotion m_barcode;

        private int m_absAlarmCode_0 = 0xFF28;
        private int m_absAlarmCode_1 = 0xFF1D;
        
        private Axis() {
            m_WMXMotion = new WMXMotion("RackMaster");
            m_barcode = new WMXBarcodeMotion("Barcode");
        }

        public void Initialize(int axisCount) {
            m_status = new WMXMotion.AxisStatus[axisCount];
            for (int i = 0; i < m_status.Length; i++) {
                m_status[i] = new WMXMotion.AxisStatus();
            }
        }

        private static readonly Lazy<Axis> instanceHolder = new Lazy<Axis>(() => new Axis());

        public static Axis Instance {
            get {
                return instanceHolder.Value;
            }
        }
        /// <summary>
        /// 서보 온 함수
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool ServoOn(int axis) {
            int ret = m_WMXMotion.ServoOn(axis);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 서보 오프 함수
        /// </summary>
        /// <param name="axis"></param>
        public void ServoOff(int axis) {
            m_WMXMotion.ServoOff(axis);
        }

        /// <summary>
        /// 해당 축이 서보 온이 되어 있는지 판단하는 함수
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsServoOn(int axis) {
            return m_WMXMotion.IsServoOn(axis);
        }
        /// <summary>
        /// 조그 함수
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool Jog(AxisProfile profile) {
            int ret = m_WMXMotion.JogMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 다축 동시 조그 함수
        /// </summary>
        /// <param name="axisCount"></param>
        /// <param name="profiles"></param>
        /// <returns></returns>
        public bool Jog(int axisCount, AxisProfile[] profiles) {
            int ret = m_WMXMotion.JogMove((uint)axisCount, profiles);
            if (ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 절대 위치 제어
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool AbsoluteMove(AxisProfile profile) {
            int ret = m_WMXMotion.AbsoluteMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 트리거 조건을 활용한 절대 위치 제어
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="triggerCondition"></param>
        /// <returns></returns>
        public bool AbsoluteMove(AxisProfile profile, TriggerCondition triggerCondition) {
            int ret = m_WMXMotion.AbsoluteMove(profile, triggerCondition);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 상대 위치 제어
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool RelativeMove(AxisProfile profile) {
            int ret = m_WMXMotion.RelativeMove(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 정지 명령
        /// </summary>
        /// <param name="axis"></param>
        public void Stop(int axis) {
            m_WMXMotion.StopServo(axis);
        }
        /// <summary>
        /// 지정된 감속도로 정지 명령
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="dec"></param>
        public void Stop(int axis, double dec) {
            m_WMXMotion.StopServo(axis, dec);
        }
        /// <summary>
        /// Emergency Stop
        /// </summary>
        /// <param name="axis"></param>
        public void EmergencyStop(int axis) {
            m_WMXMotion.EmergencyStop(axis);
        }
        /// <summary>
        /// 호밍 시작
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool HomeStart(int axis) {
            int ret = m_WMXMotion.HomeStart(axis);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 보간 제어
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool InterpolationMove(AxesProfile profile) {
            int ret = m_WMXMotion.AbsLinearInterpolation(profile);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 축 정보 업데이트
        /// </summary>
        public void UpdateMotorStatus() {
            for(int i = 0; i < m_status.Length; i++) {
                m_status[i] = m_WMXMotion.m_axisStatus[i];
            }
        }
        /// <summary>
        /// 알람 클리어
        /// </summary>
        /// <param name="axis"></param>
        public void AlarmClear(int axis) {
            if(GetAxisStatus(axis).m_alarmCode == m_absAlarmCode_0 || GetAxisStatus(axis).m_alarmCode == m_absAlarmCode_1) {
                AbsoluteAlarmClear(axis);
                int ret = m_WMXMotion.AlarmClear(axis);
                if(ret != WMXParam.ErrorCode_None) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                }
            }
            else {
                int ret = m_WMXMotion.AlarmClear(axis);
                if (ret != WMXParam.ErrorCode_None) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                }
            }
        }
        /// <summary>
        /// 현재 알람이 앱솔루트 관련 알람인지 확인
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsAbsoluteAlarm(int axis) {
            if(GetAxisStatus(axis).m_alarmCode == m_absAlarmCode_0) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 축 정보를 얻어오는 함수
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public WMXMotion.AxisStatus GetAxisStatus(int axis) {
            return m_WMXMotion.m_axisStatus[axis];
        }
        /// <summary>
        /// 현재 설정된 WMX 파라미터를 얻어오는 함수
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public void GetCurrentWMXParameter(int axis, ref WMXMotion.AxisParameter axisParameter) {
            //return m_WMXMotion.m_axisParameter[axis];
            m_WMXMotion.GetAxisParameterFromEngine(axis, ref axisParameter);
        }
        /// <summary>
        /// WMX Parameter Setting
        /// </summary>
        /// <returns></returns>
        public bool SetWMXParameter(int axis, WMXMotion.AxisParameter axisParameter) {
            //int ret = m_WMXMotion.SetAxisParam();
            int ret = m_WMXMotion.AxisParameterApplyToEngine(axis, axisParameter);
            if (ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 축의 Command Position을 설정
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool SetAxisCommandPos(int axis, double position) {
            int ret = m_WMXMotion.SetCommandPos(axis, position);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 특정 경로에 WMX Parameter 파일 저장
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SaveWMXParameter(string path) {
            int ret = m_WMXMotion.GetAndSaveAxisParameter(path);
            if (ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// 현재 축의 AMP 알람 코드를 얻어오는 함수
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAlarmCode(int axis) {
            return m_WMXMotion.m_axisStatus[axis].m_alarmCode;
        }
        /// <summary>
        /// Full Closed 사용 시 Full Closed 관련 파라미터를 설정
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool SetFullClosedParameter(BarcodeParameter param) {
            int ret = m_barcode.SetBarcodeParam(param);
            if(ret != 0) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, $"Barcode Set Param Error={m_barcode.GetErrorToString(ret)}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// Full Closed 사용 시 Full Closed Safety 관련 파라미터를 설정
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool SetFullClosedSafetyParameter(BarcodeSafetyParameter param) {
            int ret = m_barcode.SetBarcodeSafetyParam(param);
            if(ret != 0) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, $"Barcode Set Safety Param Error={m_barcode.GetErrorToString(ret)}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 현재 설정된 Full Closed 파라미터 Get
        /// </summary>
        /// <returns></returns>
        public BarcodeParameter GetFullClosedParameter() {
            return m_barcode.GetBarcodeParameter();
        }
        /// <summary>
        /// 현재 설정된 Full Closed Safety 파라미터 Get
        /// </summary>
        /// <returns></returns>
        public BarcodeSafetyParameter GetFullClosedSafetyParameter() {
            return m_barcode.GetBarcodeSafetyParameter();
        }
        /// <summary>
        /// Full Closed Motion 시작
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool StartFullClosedMotion(BarcodeClosedLoopCommand command) {
            int ret = m_barcode.StartFullClosedMotion(command);
            if(ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, $"Barcode Start Error={m_barcode.GetErrorToString(ret)}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 현재 Full Closed에 관련된 상태 Get
        /// </summary>
        /// <returns></returns>
        public BarcodeStatus GetFullClosedStatus() {
            return m_barcode.m_status;
        }
        /// <summary>
        /// Full Closed Motion Stop
        /// </summary>
        public void StopFullClosed() {
            m_barcode.StopFullClosed();
        }
        /// <summary>
        /// Full Closed 관련 알람 클리어
        /// </summary>
        public void AlarmClearFullClosed() {
            m_barcode.AlarmReset();
        }
        /// <summary>
        /// WMX 상태 중 Homing 완료 플래그인 Home Done 플래그 설정
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="homeDone"></param>
        /// <returns></returns>
        public bool SetHomeDoneFlag(int axis, bool homeDone) {
            int ret = m_WMXMotion.SetHomeDoneFlag(axis, homeDone);
            if (ret != WMXParam.ErrorCode_None) {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"Set Home Done Fail, Error={WMX3.ErrorCodeToString(ret)}"));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 앱솔루트 관련 알람 클리어
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool AbsoluteAlarmClear(int axis) {
            int slaveIndex = 0;

            int index_SF9               = 0x4d01;
            int subIndex_SF9            = 0x0;
            short data_SF9              = 0x31;
            int index_SFStart           = 0x4d00;
            int subIndex_SFStart        = 0x1;
            int data_SFStart            = 0x200;
            int data_SFStart_clear      = 0x0;
            uint errCode                = 0;
            int ret                     = 0;

            for (int i = 0; i < WMX3.GetOnlineSlaveCount(); i++) {
                if (WMX3.GetSlaveInformation(i).axisCount != 0) {
                    for (int j = 0; j < WMX3.GetSlaveInformation(i).axisCount; j++) {
                        if (WMX3.GetSlaveInformation(i).axisNumber[j] == axis) {
                            slaveIndex = i;

                            byte[] byteSF9 = new byte[2];
                            byteSF9 = BitConverter.GetBytes(data_SF9);
                            ret = WMX3.SDODownloadExpedited(slaveIndex, index_SF9, subIndex_SF9, byteSF9.Length, byteSF9, ref errCode);
                            if(ret != WMXParam.ErrorCode_None) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                                return false;
                            }
                            System.Threading.Thread.Sleep(1000);

                            byte[] byteSFStart = new byte[4];
                            byteSFStart = BitConverter.GetBytes(data_SFStart);
                            ret = WMX3.SDODownloadExpedited(slaveIndex, index_SFStart, subIndex_SFStart, byteSFStart.Length, byteSFStart, ref errCode);
                            if(ret != WMXParam.ErrorCode_None) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                                return false;
                            }
                            System.Threading.Thread.Sleep(1000);

                            byteSFStart = BitConverter.GetBytes(data_SFStart_clear);
                            ret = WMX3.SDODownloadExpedited(slaveIndex, index_SFStart, subIndex_SFStart, byteSFStart.Length, byteSFStart, ref errCode);
                            if(ret != WMXParam.ErrorCode_None) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                                return false;
                            }
                            System.Threading.Thread.Sleep(1000);

                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// AMP 온도 Get
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAmpTemperature(int axis) {
            int index = 0x4F62;
            int subindex = 0x0;
            byte[] data = new byte[4];
            uint actSize = 0;
            uint errCode = 0;
            int ret = 0;

            try {
                for (int i = 0; i < WMX3.GetOnlineSlaveCount(); i++) {
                    if(WMX3.GetSlaveInformation(i) == null) {
                        //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, $"Get Slave Informaition is Null"));
                        return 0;
                    }

                    if (WMX3.GetSlaveInformation(i).axisCount != 0) {
                        for (int j = 0; j < WMX3.GetSlaveInformation(i).axisCount; j++) {
                            if (WMX3.GetSlaveInformation(i).axisNumber[j] == axis) {
                                ret = WMX3.SDOUploadExpedited(i, index, subindex, ref data, ref actSize, ref errCode);
                                if (ret != WMXParam.ErrorCode_None) {
                                    //Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}, Code={ret}"));
                                    return 0;
                                }

                                return BitConverter.ToInt32(data, 0);
                            }
                        }
                    }
                }
            }catch(Exception ex) {
                //Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.WMX, "Get Temperature Exception", ex));
            }

            return 0;
        }
    }
}
