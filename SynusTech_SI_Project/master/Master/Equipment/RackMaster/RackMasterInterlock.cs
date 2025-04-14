using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Interface.Alarm;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterInterlock.cs는 STK 명령 수행 시 인터락 처리 작성
    /// 함수 명 : 동작 기능
    /// Check : 인터락 체크 항목 나열
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// 명령 전송 위치를 나타냄
        /// </summary>
        public enum InterlockFrom
        {
            UI_Event,
            ApplicationLoop,
            TCPIP
        }

        /// <summary>
        /// Auto Mode Enable 변경 시 인터락 체크
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AutoModeEnable(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Auto Mode Enable Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeReady(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DoorOpenFlag(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DoorOpenStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MasterKeyManual(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_AutoModeEnable();
        }

        /// <summary>
        /// Auto Mode Disable 변경 시 인터락 체크
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AutoModeDisable(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Auto Mode Disable Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (!Status_AutoMode)
                return;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_AutoModeStop"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                    CMD_AutoModeDisable();
            }
            else
                CMD_AutoModeDisable();
        }

        /// <summary>
        /// CIM <-> Master 제어모드 변경 시 인터락
        /// </summary>
        /// <param name="eControlMode"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetControlMode(ControlMode eControlMode, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Control Mode Interlock Error";

            //if (Check_CurrentMode(eControlMode, ErrorTitleMsg, _InterlockFrom))
            //    return;

            if (Check_AutoModeRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetControlMode(eControlMode);
        }
        
        /// <summary>
        /// STK Power On 명령 시 인터락
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetPowerOn(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Power On Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PowerOnEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PowerOn();
        }
        
        /// <summary>
        /// STK Power Off 명령 시 인터락
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetPowerOff(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Power Off Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PowerOffEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PowerOff();
        }

        /// <summary>
        /// STK Alarm Clear 명령 시 인터락
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetAlarmClear(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Alarm Clear Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeRun(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_AlarmClear();
        }

        /// <summary>
        /// STK Master Cycle 제어 명령 시 인터락
        /// </summary>
        /// <param name="_CycleCount"></param>
        /// <param name="_FromID"></param>
        /// <param name="_ToID"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StartAutoCycleControl(int _CycleCount, int _FromID, int _ToID, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Start Auto Cycle Control Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeIdle(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;


            CMD_AutoCycleRun(_CycleCount, _FromID, _ToID);
        }

        /// <summary>
        /// STK Shelf 정보 요청 명령 시 인터락
        /// </summary>
        /// <param name="_ShelfID"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_GetAutoTeachingShelfInfo(int _ShelfID, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Get Auto Teaching Shelf Info Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeIdle(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_ReadWriteEnableCondition(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_GetShelfInfo(_ShelfID);
        }

        /// <summary>
        /// STK Auto Teaching 명령 시 인터락
        /// </summary>
        /// <param name="eTeachingMode"></param>
        /// <param name="teachingList"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StartAutoTeachingControl(TeachingMode eTeachingMode, List<TeachingParam> teachingList, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Start Auto Teaching Control Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeIdle(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_TeachingModeAndTeachingList(eTeachingMode, teachingList, ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_AutoTeachingRun(teachingList);
        }

        /// <summary>
        /// STK Axis Speed 비율 변경 시 인터락
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="value"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetAxisSpeedRatio(AxisType eAxisType, short value, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eAxisType} Set Speed Ratio Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetAxisSpeedRatio(eAxisType, value);
        }

        /// <summary>
        /// STK Over Load 비율 변경 시 인터락
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="value"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetOverLoadValue(AxisType eAxisType, short value, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eAxisType} Set Over Load Value Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetOverLoadValue(eAxisType, value);
        }

        /// <summary>
        /// STK 과부하 클리어 시 인터락
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetOverClear(AxisType eAxisType, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eAxisType} Set Over Load Clear Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetOverLoadClear(eAxisType);
        }
       
        /// <summary>
        /// STK 시간 동기화 명령 시 인터락
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetTimeSync(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Time Sync Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetTimeSync();
        }
        
        /// <summary>
        /// STK Maint Move 명령 시 인터락
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_MaintMove(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Maint Move Interlock Error";

            if (Check_TCPIPConnection(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DoorOpenStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoCycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoTeachingRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_MaintMoveStatus(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoModeIdle(ErrorTitleMsg, _InterlockFrom))
                return;

            //if (Check_AutoModeRun(ErrorTitleMsg, _InterlockFrom))
            //    return;

            CMD_MaintMoveRun();
        }
        
        /// <summary>
        /// CIM Mode 유무 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_CIMMode(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (m_eControlMode == ControlMode.CIMMode && _InterlockFrom == InterlockFrom.UI_Event)
            {
                string ErrorInfoMsg = $"RackMaster is CIM Mode";
                MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InCIMMode"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Auto Mode 가능 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_AutoModeReady(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_AutoModeReady)
            {
                string ErrorInfoMsg = $"Auto Mode Ready Flag Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_CurrentMode(ControlMode eControlMode, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(m_eControlMode == ControlMode.CIMMode && eControlMode == ControlMode.MasterMode)
            {
                string ErrorInfoMsg = $"RackMaster is CIM Mode";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InCIMMode"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Auto Mode 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_AutoModeRun(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Status_AutoMode)
            {
                string ErrorInfoMsg = $"RackMaster is Auto Mode Status";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoMode"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Idle 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_AutoModeIdle(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_AutoMode)
            {
                string ErrorInfoMsg = $"RackMaster is Not Auto Mode";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InNotAutoMode"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Cycle 제어 동작 여부 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_AutoCycleControlRun(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (IsAutoCycleRun())
            {
                string ErrorInfoMsg = $"RackMaster is Auto Cycle Control Running";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoCycleRun"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Teaching R/W 가능 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_ReadWriteEnableCondition(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_TeachingRWEnable)
            {
                string ErrorInfoMsg = $"RackMastet is not in Teaching R/W Enable Status";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InNotTeachingRW"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Auto Teaching 동작 중 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_AutoTeachingRun(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (IsAutoTeachingRun())
            {
                string ErrorInfoMsg = $"RackMaster is Auto Teaching Control Running";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoTeachingRun"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Maint Move 동작 중 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_MaintMoveStatus(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Status_MaintMove)
            {
                string ErrorInfoMsg = $"RackMaster is Maint Move Running";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_MaintMoveStatus"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Door Open 명령 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_DoorOpenFlag(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Master.CMD_DoorOpen_REQ) //Master.IsDoorOpen_SWFlag()
            {
                string ErrorInfoMsg = $"Door Open Flag";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InDoorOpenFlag"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Door Open 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_DoorOpenStatus(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Master.Sensor_HPDoorOpen && Master.IsValidInputItemMapping(Master.MasterInputItem.HP_Door_Open))
            {
                string ErrorInfoMsg = $"HP Door Open State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_HPDoorOpen"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if (Master.Sensor_OPDoorOpen && Master.IsValidInputItemMapping(Master.MasterInputItem.OP_Door_Open))
            {
                string ErrorInfoMsg = $"OP Door Open State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_OPDoorOpen"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        /// <summary>
        /// HP Master Key Manual 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_MasterKeyManual(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Master.Sensor_HPAutoKey)
            {
                string ErrorInfoMsg = $"Master Key Manual Error";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InMasterKeyManual"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Power Off 가능 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_PowerOffEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_ServoOffEnable)
            {
                string ErrorInfoMsg = $"Power Off Enable State Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Power On 가능 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_PowerOnEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_ServoOnEnable)
            {
                string ErrorInfoMsg = $"Power On Enable State Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// TCP/IP 연결 상태 체크
        /// </summary>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_TCPIPConnection(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!IsConnected())
            {
                string ErrorInfoMsg = $"TCP/IP Disconnection";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_TCPIPDisconnection"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Teaching 진행 시 파일에서 읽어온 티칭 List 체크
        /// </summary>
        /// <param name="eTeachingMode"></param>
        /// <param name="teachingList"></param>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        private bool Check_TeachingModeAndTeachingList(TeachingMode eTeachingMode, List<TeachingParam> teachingList, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(teachingList.Count <= 0)
            {
                string ErrorInfoMsg = $"Target Shelf information does not exist.";

                if (eTeachingMode == TeachingMode.Single && _InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_DonotFindShelfInFileError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if(eTeachingMode == TeachingMode.Continuous && _InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_DonotCheckingInFileError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;

            }

            return false;
        }
        
        /// <summary>
        /// Check 과정에서 부합하지 않는 경우 로그 메세지 출력
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="ErrorInfoMsg"></param>
        private void Interlock_LogMessage(InterlockFrom _InterlockFrom, string ErrorTitleMsg, string ErrorInfoMsg)
        {
            if (_InterlockFrom == InterlockFrom.TCPIP)
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.Interlock, $"[CIM CMD] {ErrorTitleMsg}: {ErrorInfoMsg}");
            else
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.Interlock, $"{ErrorTitleMsg}: {ErrorInfoMsg}");
        }
    }
}
