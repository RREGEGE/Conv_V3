using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Math;
using Master.ManagedFile;

namespace Master.Equipment.Port
{
    /// <summary>
    /// [Port -> CIM] Word Memory Map에 사용되는 변수 기재
    /// </summary>
    public partial class Port
    {
        short[] m_ErrorCode = new short[5] { 0, 0, 0, 0, 0 };

        public short[] PortErrorCode
        {
            get { return m_ErrorCode; }
            set { 
                m_ErrorCode = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.ErrorCode_0, m_ErrorCode[0]);
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.ErrorCode_1, m_ErrorCode[1]);
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.ErrorCode_2, m_ErrorCode[2]);
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.ErrorCode_3, m_ErrorCode[3]);
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.ErrorCode_4, m_ErrorCode[4]);
            }
        }

        /// <summary>
        /// X-Axis Data
        /// </summary>
        double m_dX_Axis_CurrentPosition = 0;
        double m_dX_Axis_TargetPosition = 0;
        double m_dX_Axis_CurrentSpeed = 0;
        double m_dX_Axis_CurrentTorque = 0;
        double m_dX_Axis_PeakTorque = 0;
        double m_dX_Axis_AvrTorque = 0;
        short m_nX_Axis_SensorMonitoring = 0;
        double m_dX_Axis_OverloadDetectTorque = 0;
        double m_dX_Axis_OverloadSettingTorque = 0;
        //Sensor
        bool m_bX_Axis_NOTSensor = false;
        bool m_bX_Axis_POTSensor = false;
        bool m_bX_Axis_HOMESensor = false;
        bool m_bX_Axis_POSSensor = false;
        bool m_bX_Axis_Busy = false;
        bool m_bX_Axis_OriginOK = false;
        bool m_bX_Axis_WaitPosSensor = false;

        public double Motion_X_Axis_CurrentPosition
        {
            get { return m_dX_Axis_CurrentPosition; }
            set { 
                m_dX_Axis_CurrentPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentPosition_0, (float)m_dX_Axis_CurrentPosition);
            }
        }
        public double Motion_X_Axis_TargetPosition
        {
            get { return m_dX_Axis_TargetPosition; }
            set { 
                m_dX_Axis_TargetPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_TargetPosition_0, (float)m_dX_Axis_TargetPosition);
            }
        }
        public double Motion_X_Axis_CurrentSpeed
        {
            get { return m_dX_Axis_CurrentSpeed; }
            set { 
                m_dX_Axis_CurrentSpeed = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentSpeed, (short)m_dX_Axis_CurrentSpeed);
            }
        }
        public double Motion_X_Axis_CurrentTorque
        {
            get { return m_dX_Axis_CurrentTorque; }
            set { 
                m_dX_Axis_CurrentTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentTorque, (short)m_dX_Axis_CurrentTorque);
            }
        }
        public double Motion_X_Axis_PeakTorque
        {
            get { return m_dX_Axis_PeakTorque; }
            set { 
                m_dX_Axis_PeakTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_PeakTorque, (short)m_dX_Axis_PeakTorque);
            }
        }
        public double Motion_X_Axis_AvrTorque
        {
            get { return m_dX_Axis_AvrTorque; }
            set { 
                m_dX_Axis_AvrTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_AverageTorque, (short)m_dX_Axis_AvrTorque);
            }
        }
        public short Motion_X_Axis_SensorMonitoring
        {
            get { return m_nX_Axis_SensorMonitoring; }
            set { 
                m_nX_Axis_SensorMonitoring = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_SensorMonitoring, m_nX_Axis_SensorMonitoring);
            }
        }
        public double Motion_X_Axis_OverloadDetectTorque
        {
            get { return m_dX_Axis_OverloadDetectTorque; }
            set { 
                m_dX_Axis_OverloadDetectTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentDetectedMaxLoad, (short)m_dX_Axis_OverloadDetectTorque);
            }
        }
        public double Motion_X_Axis_OverloadSettingTorque
        {
            get { return m_dX_Axis_OverloadSettingTorque; }
            set { 
                m_dX_Axis_OverloadSettingTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentSettingMaxLoad, (short)m_dX_Axis_OverloadSettingTorque);
            }
        }
        public bool Sensor_X_Axis_NOT
        {
            get { return m_bX_Axis_NOTSensor; }
            set { 
                m_bX_Axis_NOTSensor = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.NOT_Sensor, m_bX_Axis_NOTSensor);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_POT
        {
            get { return m_bX_Axis_POTSensor; }
            set
            {
                m_bX_Axis_POTSensor = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.POT_Sensor, m_bX_Axis_POTSensor);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_HOME
        {
            get { return m_bX_Axis_HOMESensor; }
            set
            {
                m_bX_Axis_HOMESensor = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.HOME_Sensor_LP, m_bX_Axis_HOMESensor);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_POS
        {
            get { return m_bX_Axis_POSSensor; }
            set
            {
                m_bX_Axis_POSSensor = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.POS_Sensor_OP, m_bX_Axis_POSSensor);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_Busy
        {
            get { return m_bX_Axis_Busy; }
            set
            {
                m_bX_Axis_Busy = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.Busy, m_bX_Axis_Busy);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_OriginOK
        {
            get { return m_bX_Axis_OriginOK; }
            set
            {
                m_bX_Axis_OriginOK = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.OriginOK, m_bX_Axis_OriginOK);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_X_Axis_WaitPosSensor
        {
            get { return m_bX_Axis_WaitPosSensor; }
            set
            {
                m_bX_Axis_WaitPosSensor = value;
                short nSensorValue = Motion_X_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)X_Axis_SensorStatusBitMap.WaitPosSensor, m_bX_Axis_WaitPosSensor);
                Motion_X_Axis_SensorMonitoring = nSensorValue;
            }
        }

        /// <summary>
        /// Z-Axis
        /// </summary>
        double m_dZ_Axis_CurrentPosition = 0;
        double m_dZ_Axis_TargetPosition = 0;
        double m_dZ_Axis_CurrentSpeed = 0;
        double m_dZ_Axis_CurrentTorque = 0;
        double m_dZ_Axis_PeakTorque = 0;
        double m_dZ_Axis_AvrTorque = 0;
        short m_nZ_Axis_SensorMonitoring = 0;
        double m_dZ_Axis_OverloadDetectTorque = 0;
        double m_dZ_Axis_OverloadSettingTorque = 0;
        //Sensor
        bool m_bZ_Axis_NOTSensor = false;
        bool m_bZ_Axis_POTSensor = false;
        bool m_bZ_Axis_HOMESensor = false;
        bool m_bZ_Axis_POSSensor = false;
        bool m_bZ_Axis_Busy = false;
        bool m_bZ_Axis_OriginOK = false;
        bool m_bZ_Axis_BWDSensor = false;
        bool m_bZ_Axis_FWDSensor = false;

        public double Motion_Z_Axis_CurrentPosition
        {
            get { return m_dZ_Axis_CurrentPosition; }
            set { 
                m_dZ_Axis_CurrentPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentPosition_0, (float)m_dZ_Axis_CurrentPosition);
            }
        }
        public double Motion_Z_Axis_TargetPosition
        {
            get { return m_dZ_Axis_TargetPosition; }
            set { 
                m_dZ_Axis_TargetPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_TargetPosition_0, (float)m_dZ_Axis_TargetPosition);
            }
        }
        public double Motion_Z_Axis_CurrentSpeed
        {
            get { return m_dZ_Axis_CurrentSpeed; }
            set { 
                m_dZ_Axis_CurrentSpeed = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentSpeed, (short)m_dZ_Axis_CurrentSpeed);
            }
        }
        public double Motion_Z_Axis_CurrentTorque
        {
            get { return m_dZ_Axis_CurrentTorque; }
            set { 
                m_dZ_Axis_CurrentTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentTorque, (short)m_dZ_Axis_CurrentTorque);
            }
        }
        public double Motion_Z_Axis_PeakTorque
        {
            get { return m_dZ_Axis_PeakTorque; }
            set { 
                m_dZ_Axis_PeakTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_PeakTorque, (short)m_dZ_Axis_PeakTorque);
            }
        }
        public double Motion_Z_Axis_AvrTorque
        {
            get { return m_dZ_Axis_AvrTorque; }
            set { 
                m_dZ_Axis_AvrTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_AverageTorque, (short)m_dZ_Axis_AvrTorque);
            }
        }
        public short Motion_Z_Axis_SensorMonitoring
        {
            get { return m_nZ_Axis_SensorMonitoring; }
            set { 
                m_nZ_Axis_SensorMonitoring = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_SensorMonitoring, m_nZ_Axis_SensorMonitoring);
            }
        }
        public double Motion_Z_Axis_OverloadDetectTorque
        {
            get { return m_dZ_Axis_OverloadDetectTorque; }
            set { 
                m_dZ_Axis_OverloadDetectTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentDetectedMaxLoad, (short)m_dZ_Axis_OverloadDetectTorque);
            }
        }
        public double Motion_Z_Axis_OverloadSettingTorque
        {
            get { return m_dZ_Axis_OverloadSettingTorque; }
            set { 
                m_dZ_Axis_OverloadSettingTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentSettingMaxLoad, (short)m_dZ_Axis_OverloadSettingTorque);
            }
        }
        public bool Sensor_Z_Axis_NOT
        {
            get { return m_bZ_Axis_NOTSensor; }
            set
            {
                m_bZ_Axis_NOTSensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.NOT_Sensor, m_bZ_Axis_NOTSensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_POT
        {
            get { return m_bZ_Axis_POTSensor; }
            set
            {
                m_bZ_Axis_POTSensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.POT_Sensor, m_bZ_Axis_POTSensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_HOME
        {
            get { return m_bZ_Axis_HOMESensor; }
            set
            {
                m_bZ_Axis_HOMESensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.HOME_Sensor, m_bZ_Axis_HOMESensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_POS
        {
            get { return m_bZ_Axis_POSSensor; }
            set
            {
                m_bZ_Axis_POSSensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.POS_Sensor, m_bZ_Axis_POSSensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_Busy
        {
            get { return m_bZ_Axis_Busy; }
            set
            {
                m_bZ_Axis_Busy = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.Busy, m_bZ_Axis_Busy);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_OriginOK
        {
            get { return m_bZ_Axis_OriginOK; }
            set
            {
                m_bZ_Axis_OriginOK = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.OriginOK, m_bZ_Axis_OriginOK);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_BWDSensor
        {
            get { return m_bZ_Axis_BWDSensor; }
            set
            {
                m_bZ_Axis_BWDSensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.Cylinder_BWD_Pos, m_bZ_Axis_BWDSensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Z_Axis_FWDSensor
        {
            get { return m_bZ_Axis_FWDSensor; }
            set
            {
                m_bZ_Axis_FWDSensor = value;
                short nSensorValue = Motion_Z_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Z_Axis_SensorStatusBitMap.Cylinder_FWD_Pos, m_bZ_Axis_FWDSensor);
                Motion_Z_Axis_SensorMonitoring = nSensorValue;
            }
        }

        /// <summary>
        /// T-Axis
        /// </summary>
        double m_dT_Axis_CurrentPosition = 0;
        double m_dT_Axis_TargetPosition = 0;
        double m_dT_Axis_CurrentSpeed = 0;
        double m_dT_Axis_CurrentTorque = 0;
        double m_dT_Axis_PeakTorque = 0;
        double m_dT_Axis_AvrTorque = 0;
        short m_nT_Axis_SensorMonitoring = 0;
        double m_dT_Axis_OverloadDetectTorque = 0;
        double m_dT_Axis_OverloadSettingTorque = 0;
        //Sensor
        bool m_bT_Axis_NOTSensor = false;
        bool m_bT_Axis_POTSensor = false;
        bool m_bT_Axis_HOMESensor = false;
        bool m_bT_Axis_POSSensor = false;
        bool m_bT_Axis_Busy = false;
        bool m_bT_Axis_OriginOK = false;
        bool m_bT_Axis_0DegSensor = false;
        bool m_bT_Axis_180DegSensor = false;

        public double Motion_T_Axis_CurrentPosition
        {
            get { return m_dT_Axis_CurrentPosition; }
            set { 
                m_dT_Axis_CurrentPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentPosition_0, (float)m_dT_Axis_CurrentPosition);
            }
        }
        public double Motion_T_Axis_TargetPosition
        {
            get { return m_dT_Axis_TargetPosition; }
            set { 
                m_dT_Axis_TargetPosition = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_TargetPosition_0, (float)m_dT_Axis_TargetPosition);
            }
        }
        public double Motion_T_Axis_CurrentSpeed
        {
            get { return m_dT_Axis_CurrentSpeed; }
            set { 
                m_dT_Axis_CurrentSpeed = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentSpeed, (short)m_dT_Axis_CurrentSpeed);
            }
        }
        public double Motion_T_Axis_CurrentTorque
        {
            get { return m_dT_Axis_CurrentTorque; }
            set { 
                m_dT_Axis_CurrentTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentTorque, (short)m_dT_Axis_CurrentTorque);
            }
        }
        public double Motion_T_Axis_PeakTorque
        {
            get { return m_dT_Axis_PeakTorque; }
            set { 
                m_dT_Axis_PeakTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_PeakTorque, (short)m_dT_Axis_PeakTorque);
            }
        }
        public double Motion_T_Axis_AvrTorque
        {
            get { return m_dT_Axis_AvrTorque; }
            set { 
                m_dT_Axis_AvrTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_AverageTorque, (short)m_dT_Axis_AvrTorque);
            }
        }
        public short Motion_T_Axis_SensorMonitoring
        {
            get { return m_nT_Axis_SensorMonitoring; }
            set { 
                m_nT_Axis_SensorMonitoring = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_SensorMonitoring, m_nT_Axis_SensorMonitoring);
            }
        }
        public double Motion_T_Axis_OverloadDetectTorque
        {
            get { return m_dT_Axis_OverloadDetectTorque; }
            set { 
                m_dT_Axis_OverloadDetectTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentDetectedMaxLoad, (short)m_dT_Axis_OverloadDetectTorque);
            }
        }
        public double Motion_T_Axis_OverloadSettingTorque
        {
            get { return m_dT_Axis_OverloadSettingTorque; }
            set { 
                m_dT_Axis_OverloadSettingTorque = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentSettingMaxLoad, (short)m_dT_Axis_OverloadSettingTorque);
            }
        }
        public bool Sensor_T_Axis_NOT
        {
            get { return m_bT_Axis_NOTSensor; }
            set
            {
                m_bT_Axis_NOTSensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.NOT_Sensor, m_bT_Axis_NOTSensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_POT
        {
            get { return m_bT_Axis_POTSensor; }
            set
            {
                m_bT_Axis_POTSensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.POT_Sensor, m_bT_Axis_POTSensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_HOME
        {
            get { return m_bT_Axis_HOMESensor; }
            set
            {
                m_bT_Axis_HOMESensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.HOME_Sensor, m_bT_Axis_HOMESensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_POS
        {
            get { return m_bT_Axis_POSSensor; }
            set
            {
                m_bT_Axis_POSSensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.POS_Sensor, m_bT_Axis_POSSensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_Busy
        {
            get { return m_bT_Axis_Busy; }
            set
            {
                m_bT_Axis_Busy = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.Busy, m_bT_Axis_Busy);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_OriginOK
        {
            get { return m_bT_Axis_OriginOK; }
            set
            {
                m_bT_Axis_OriginOK = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.OriginOK, m_bT_Axis_OriginOK);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_0DegSensor
        {
            get { return m_bT_Axis_0DegSensor; }
            set
            {
                m_bT_Axis_0DegSensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.Degree_0_Position, m_bT_Axis_0DegSensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_T_Axis_180DegSensor
        {
            get { return m_bT_Axis_180DegSensor; }
            set
            {
                Sensor_T_Axis_POS = value;
                m_bT_Axis_180DegSensor = value;
                short nSensorValue = Motion_T_Axis_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)T_Axis_SensorStatusBitMap.Degree_180_Position, m_bT_Axis_180DegSensor);
                Motion_T_Axis_SensorMonitoring = nSensorValue;
            }
        }

        /// <summary>
        /// Set Function
        /// 특정 축 데이터를 가져옴
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="pos"></param>
        public void Motion_TargetPosition(PortAxis ePortAxis, double pos)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_TargetPosition = pos;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_TargetPosition = pos;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_TargetPosition = pos;
                    break;
            }
        }
        public void Motion_CurrentPosition(PortAxis ePortAxis, double pos)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_CurrentPosition = pos;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_CurrentPosition = pos;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_CurrentPosition = pos;
                    break;
            }
        }
        public void Motion_CurrentSpeed(PortAxis ePortAxis, double spd)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_CurrentSpeed = spd;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_CurrentSpeed = spd;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_CurrentSpeed = spd;
                    break;
            }
        }
        public void Motion_CurrentTorque(PortAxis ePortAxis, double trq)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_CurrentTorque = trq;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_CurrentTorque = trq;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_CurrentTorque = trq;
                    break;
            }
        }
        public void Motion_PeakTorque(PortAxis ePortAxis, double trq)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_PeakTorque = trq;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_PeakTorque = trq;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_PeakTorque = trq;
                    break;
            }
        }
        public void Motion_AvrTorque(PortAxis ePortAxis, double trq)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_AvrTorque = trq;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_AvrTorque = trq;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_AvrTorque = trq;
                    break;
            }
        }
        public void Motion_OverloadDetectTorque(PortAxis ePortAxis, double trq)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_OverloadDetectTorque = trq;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_OverloadDetectTorque = trq;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_OverloadDetectTorque = trq;
                    break;
            }
        }
        public void Motion_OverloadSettingTorque(PortAxis ePortAxis, double trq)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Motion_X_Axis_OverloadSettingTorque = trq;
                    break;
                case PortAxis.Shuttle_Z:
                    Motion_Z_Axis_OverloadSettingTorque = trq;
                    break;
                case PortAxis.Shuttle_T:
                    Motion_T_Axis_OverloadSettingTorque = trq;
                    break;
            }
        }
        public double Motion_TargetPosition(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_TargetPosition;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_TargetPosition;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_TargetPosition;
            }

            return 0;
        }
        public double Motion_CurrentPosition(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_CurrentPosition;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_CurrentPosition;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_CurrentPosition;
            }

            return 0;
        }
        public double Motion_CurrentSpeed(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_CurrentSpeed;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_CurrentSpeed;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_CurrentSpeed;
            }

            return 0;
        }
        public double Motion_CurrentTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_CurrentTorque;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_CurrentTorque;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_CurrentTorque;
            }

            return 0;
        }
        public double Motion_PeakTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_PeakTorque;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_PeakTorque;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_PeakTorque;
            }

            return 0;
        }
        public double Motion_AvrTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_AvrTorque;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_AvrTorque;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_AvrTorque;
            }

            return 0;
        }
        public double Motion_OverloadDetectTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_OverloadDetectTorque;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_OverloadDetectTorque;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_OverloadDetectTorque;
            }

            return 0;
        }
        public double Motion_OverloadSettingTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    return Motion_X_Axis_OverloadSettingTorque;
                case PortAxis.Shuttle_Z:
                    return Motion_Z_Axis_OverloadSettingTorque;
                case PortAxis.Shuttle_T:
                    return Motion_T_Axis_OverloadSettingTorque;
            }

            return 0;
        }






        /// <summary>
        /// Shuttle Status
        /// </summary>
        short m_nShuttle_SensorMonitoring = 0;
        //Sensor
        bool m_bShuttleCST_Detect_Sensor1 = false;
        bool m_bShuttleCST_Detect_Sensor2 = false;

        public short Shuttle_SensorMonitoring
        {
            get { return m_nShuttle_SensorMonitoring; }
            set
            {
                m_nShuttle_SensorMonitoring = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Shuttle_SensorStatus, m_nShuttle_SensorMonitoring);
            }
        }
        public bool Sensor_Shuttle_CSTDetect1
        {
            get { return m_bShuttleCST_Detect_Sensor1; }
            set
            {
                m_bShuttleCST_Detect_Sensor1 = value;
                short nSensorValue = Shuttle_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Shuttle_SensorStatusBitMap.CST_InPlace_1, m_bShuttleCST_Detect_Sensor1);
                Shuttle_SensorMonitoring = nSensorValue;
            }
        }
        public bool Sensor_Shuttle_CSTDetect2
        {
            get { return m_bShuttleCST_Detect_Sensor2; }
            set
            {
                m_bShuttleCST_Detect_Sensor2 = value;
                short nSensorValue = Shuttle_SensorMonitoring;
                BitOperation.SetBit(ref nSensorValue, (int)Shuttle_SensorStatusBitMap.CST_InPlace_2, m_bShuttleCST_Detect_Sensor2);
                Shuttle_SensorMonitoring = nSensorValue;
            }
        }



        /// <summary>
        /// Buffer 1 -> OP
        /// OP의 Data는 Buffer1 메모리맵에 맵핑
        /// </summary>
        string m_OP_CarrierID = string.Empty;
        short m_nOP_AutoStep = 0;

        short m_nOP_SensorStatus1 = 0;
        bool m_bOP_CST_Detect_Sensor1 = false;
        bool m_bOP_CST_Detect_Sensor2 = false;
        bool m_bOP_CST_Presence_Sensor = false;
        bool m_bOP_Fork_Detect = false;

        short m_nOP_SensorStatus2 = 0;
        bool m_bOP_CV_IN = false;
        bool m_bOP_CV_SLOW = false;
        bool m_bOP_CV_STOP = false;
        bool m_bOP_Z_NOT = false;
        bool m_bOP_Z_POS1 = false;
        bool m_bOP_Z_POS2 = false;
        bool m_bOP_Z_POT = false;
        bool m_bOP_CV_FWD_Status = false;
        bool m_bOP_CV_BWD_Status = false;
        bool m_bOP_CV_Error = false;
        bool m_bOP_CV_Moving = false;
        bool m_bOP_CV_Reset = false;
        bool m_bOP_Z_Error = false;
        bool m_bOP_Z_Reset = false;

        public string OP_CarrierID
        {
            get
            {
                //m_OP_CarrierID = (string)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer1_Carrier_ID_01);
                return m_OP_CarrierID;
            }
            set {
                if (m_OP_CarrierID != value)
                {
                    if(value != string.Empty)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"OP CST ID : {m_OP_CarrierID} => {value}");
                    else
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"OP CST ID : {m_OP_CarrierID} => Clear");

                    m_OP_CarrierID = value;
                    Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer1_Carrier_ID_01, m_OP_CarrierID);
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.OP_CST_ID, m_OP_CarrierID);
                }
            }
        }
        public short OP_AutoStep
        {
            get
            {
                return m_nOP_AutoStep;
            }
            set
            {
                m_nOP_AutoStep = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer1_AutoStep, m_nOP_AutoStep);
            }
        }
        
        //Sensor 1
        private short Sensor_OP_SensorMonitoring1
        {
            get { return m_nOP_SensorStatus1; }
            set
            {
                m_nOP_SensorStatus1 = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer1_SensorStatus1, m_nOP_SensorStatus1);
            }
        }
        public bool Sensor_OP_CST_Detect1
        {
            get { return m_bOP_CST_Detect_Sensor1; }
            set
            {
                m_bOP_CST_Detect_Sensor1 = value;
                short nSensorValue = Sensor_OP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus1_BitIndex.CST_Detect_1, m_bOP_CST_Detect_Sensor1);
                Sensor_OP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_OP_CST_Detect2
        {
            get { return m_bOP_CST_Detect_Sensor2; }
            set
            {
                m_bOP_CST_Detect_Sensor2 = value;
                short nSensorValue = Sensor_OP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus1_BitIndex.CST_Detect_2, m_bOP_CST_Detect_Sensor2);
                Sensor_OP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_OP_CST_Presence
        {
            get { return m_bOP_CST_Presence_Sensor; }
            set
            {
                m_bOP_CST_Presence_Sensor = value;
                short nSensorValue = Sensor_OP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus1_BitIndex.CST_Presence, m_bOP_CST_Presence_Sensor);
                Sensor_OP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_OP_Fork_Detect
        {
            get { return m_bOP_Fork_Detect; }
            set
            {
                m_bOP_Fork_Detect = value;
                short nSensorValue = Sensor_OP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus1_BitIndex.Fork_Detect, m_bOP_Fork_Detect);
                Sensor_OP_SensorMonitoring1 = nSensorValue;
            }
        }
        
        //Sensor 2
        private short Sensor_OP_SensorMonitoring2
        {
            get { return m_nOP_SensorStatus2; }
            set
            {
                m_nOP_SensorStatus2 = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer1_SensorStatus2, m_nOP_SensorStatus2);
            }
        }
        public bool Sensor_OP_CV_IN
        {
            get { return m_bOP_CV_IN; }
            set
            {
                m_bOP_CV_IN = value;
                short nSensorValue = Sensor_OP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus2_BitIndex.Buffer_CV_IN, m_bOP_CV_IN);
                Sensor_OP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_OP_CV_SLOW
        {
            get { return m_bOP_CV_SLOW; }
            set
            {
                m_bOP_CV_SLOW = value;
                short nSensorValue = Sensor_OP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus2_BitIndex.Buffer_CV_SLOW, m_bOP_CV_SLOW);
                Sensor_OP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_OP_CV_STOP
        {
            get { return m_bOP_CV_STOP; }
            set
            {
                m_bOP_CV_STOP = value;
                short nSensorValue = Sensor_OP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus2_BitIndex.Buffer_CV_OUT, m_bOP_CV_STOP);
                Sensor_OP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_OP_Z_NOT
        {
            get { return m_bOP_Z_NOT; }
            set
            {
                m_bOP_Z_NOT = value;
            }
        }
        public bool Sensor_OP_Z_POS1
        {
            get { return m_bOP_Z_POS1; }
            set
            {
                m_bOP_Z_POS1 = value;
            }
        }
        public bool Sensor_OP_Z_POS2
        {
            get { return m_bOP_Z_POS2; }
            set
            {
                m_bOP_Z_POS2 = value;
            }
        }
        public bool Sensor_OP_Z_POT
        {
            get { return m_bOP_Z_POT; }
            set
            {
                m_bOP_Z_POT = value;
            }
        }
        public bool Sensor_OP_CV_FWD_Status
        {
            get { return m_bOP_CV_FWD_Status; }
            set
            {
                m_bOP_CV_FWD_Status = value;
                short nSensorValue = Sensor_OP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus2_BitIndex.Buffer_CV_Forwarding, m_bOP_CV_FWD_Status);
                Sensor_OP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_OP_CV_BWD_Status
        {
            get { return m_bOP_CV_BWD_Status; }
            set
            {
                m_bOP_CV_BWD_Status = value;
                short nSensorValue = Sensor_OP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer1_SensorStatus2_BitIndex.Buffer_CV_Backwarding, m_bOP_CV_BWD_Status);
                Sensor_OP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_OP_CV_Error
        {
            get { return m_bOP_CV_Error; }
            set
            {
                m_bOP_CV_Error = value;
            }
        }
        public bool Sensor_OP_CV_Moving
        {
            get { return m_bOP_CV_Moving; }
            set
            {
                m_bOP_CV_Moving = value;
            }
        }
        public bool Sensor_OP_CV_Reset
        {
            get { return m_bOP_CV_Reset; }
            set
            {
                m_bOP_CV_Reset = value;
            }
        }
        public bool Sensor_OP_Z_Error
        {
            get { return m_bOP_Z_Error; }
            set
            {
                m_bOP_Z_Error = value;
            }
        }
        public bool Sensor_OP_Z_Reset
        {
            get { return m_bOP_Z_Reset; }
            set
            {
                m_bOP_Z_Reset = value;
            }
        }


        /// <summary>
        /// Buffer 2 -> LP
        /// LP의 Data는 Buffer2 메모리맵에 맵핑
        /// </summary>
        string m_LP_CarrierID = string.Empty;
        short m_nLP_AutoStep = 0;
        short m_nLP_SensorStatus1 = 0;
        bool m_bLP_CST_Detect_Sensor1 = false;
        bool m_bLP_CST_Detect_Sensor2 = false;
        bool m_bLP_CST_Presence_Sensor = false;
        bool m_bLP_Hoist_Detect = false;
        bool m_bLP_Cart_Detect1 = false;
        bool m_bLP_Cart_Detect2 = false;
        bool m_bLP_LED_Bar_Green = false;
        bool m_bLP_LED_Bar_Red     = false;

        short m_nLP_SensorStatus2 = 0;
        bool m_bLP_CV_IN = false;
        bool m_bLP_CV_SLOW = false;
        bool m_bLP_CV_STOP = false;
        bool m_bLP_Z_NOT = false;
        bool m_bLP_Z_POS1 = false;
        bool m_bLP_Z_POS2 = false;
        bool m_bLP_Z_POT = false;
        bool m_bLP_CV_FWD_Status = false;
        bool m_bLP_CV_BWD_Status = false;
        bool m_bLP_CV_Error = false;
        bool m_bLP_CV_Moving = false;
        bool m_bLP_CV_Reset = false;
        bool m_bLP_Z_Error = false;
        bool m_bLP_Z_Reset = false;

        public string LP_CarrierID
        {
            get
            {
                //m_LP_CarrierID = (string)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_Carrier_ID_01);
                return m_LP_CarrierID;
            }
            set
            {
                if (m_LP_CarrierID != value)
                {
                    if(value != string.Empty)
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"LP CST ID : {m_LP_CarrierID} => {value}");
                    else
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"LP CST ID : {m_LP_CarrierID} => Clear");

                    m_LP_CarrierID = value;
                    Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_Carrier_ID_01, m_LP_CarrierID);
                    CassetteInfo.WriteCSTID(GetParam().ID, CassetteInfo.CassetteInfoKey.LP_CST_ID, m_LP_CarrierID);
                }
            }
        }
        public short LP_AutoStep
        {
            get
            {
                return m_nLP_AutoStep;
            }
            set {
                m_nLP_AutoStep = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_AutoStep, m_nLP_AutoStep);
            }
        }
        //Sensor 1
        private short Sensor_LP_SensorMonitoring1
        {
            get { return m_nLP_SensorStatus1; }
            set
            {
                m_nLP_SensorStatus1 = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_SensorStatus1, m_nLP_SensorStatus1);
            }
        }
        public bool Sensor_LP_CST_Detect1
        {
            get { return m_bLP_CST_Detect_Sensor1; }
            set
            {
                m_bLP_CST_Detect_Sensor1 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.CST_Detect_1, m_bLP_CST_Detect_Sensor1);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_CST_Detect2
        {
            get { return m_bLP_CST_Detect_Sensor2; }
            set
            {
                m_bLP_CST_Detect_Sensor2 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.CST_Detect_2, m_bLP_CST_Detect_Sensor2);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_CST_Presence
        {
            get { return m_bLP_CST_Presence_Sensor; }
            set
            {
                m_bLP_CST_Presence_Sensor = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.CST_Presence, m_bLP_CST_Presence_Sensor);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_Hoist_Detect
        {
            get { return m_bLP_Hoist_Detect; }
            set
            {
                m_bLP_Hoist_Detect = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.Hoist_Detect, m_bLP_Hoist_Detect);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_Cart_Detect1
        {
            get { return m_bLP_Cart_Detect1; }
            set
            {
                m_bLP_Cart_Detect1 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.Cart_Detect_1, m_bLP_Cart_Detect1);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_Cart_Detect2
        {
            get { return m_bLP_Cart_Detect2; }
            set
            {
                m_bLP_Cart_Detect2 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.Cart_Detect_2, m_bLP_Cart_Detect2);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_LEDBar_Green
        {
            get { return m_bLP_LED_Bar_Green; }
            set
            {
                m_bLP_LED_Bar_Green = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.LED_Green, m_bLP_LED_Bar_Green);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }
        public bool Sensor_LP_LEDBar_Red
        {
            get { return m_bLP_LED_Bar_Red; }
            set
            {
                m_bLP_LED_Bar_Red = value;
                short nSensorValue = Sensor_LP_SensorMonitoring1;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus1_BitIndex.LED_Red, m_bLP_LED_Bar_Red);
                Sensor_LP_SensorMonitoring1 = nSensorValue;
            }
        }

        //Sensor 2
        private short Sensor_LP_SensorMonitoring2
        {
            get { return m_nLP_SensorStatus2; }
            set
            {
                m_nLP_SensorStatus2 = value;
                Set_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_SensorStatus2, m_nLP_SensorStatus2);
            }
        }
        public bool Sensor_LP_CV_IN
        {
            get { return m_bLP_CV_IN; }
            set
            {
                m_bLP_CV_IN = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_CV_IN, m_bLP_CV_IN);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_CV_SLOW
        {
            get { return m_bLP_CV_SLOW; }
            set
            {
                m_bLP_CV_SLOW = value;
            }
        }
        public bool Sensor_LP_CV_STOP
        {
            get { return m_bLP_CV_STOP; }
            set
            {
                m_bLP_CV_STOP = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_CV_OUT, m_bLP_CV_STOP);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_Z_NOT
        {
            get { return m_bLP_Z_NOT; }
            set
            {
                m_bLP_Z_NOT = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_Z_Axis_NOT, m_bLP_Z_NOT);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_Z_POS1
        {
            get { return m_bLP_Z_POS1; }
            set
            {
                m_bLP_Z_POS1 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_Z_Axis_POS1, m_bLP_Z_POS1);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_Z_POS2
        {
            get { return m_bLP_Z_POS2; }
            set
            {
                m_bLP_Z_POS2 = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_Z_Axis_POS2, m_bLP_Z_POS2);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_Z_POT
        {
            get { return m_bLP_Z_POT; }
            set
            {
                m_bLP_Z_POT = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_Z_Axis_POT, m_bLP_Z_POT);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_CV_FWD_Status
        {
            get { return m_bLP_CV_FWD_Status; }
            set
            {
                m_bLP_CV_FWD_Status = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_CV_Forwarding, m_bLP_CV_FWD_Status);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_CV_BWD_Status
        {
            get { return m_bLP_CV_BWD_Status; }
            set
            {
                m_bLP_CV_BWD_Status = value;
                short nSensorValue = Sensor_LP_SensorMonitoring2;
                BitOperation.SetBit(ref nSensorValue, (int)Buffer2_SensorStatus2_BitIndex.Buffer_CV_Backwarding, m_bLP_CV_BWD_Status);
                Sensor_LP_SensorMonitoring2 = nSensorValue;
            }
        }
        public bool Sensor_LP_CV_Error
        {
            get { return m_bLP_CV_Error; }
            set
            {
                m_bLP_CV_Error = value;
            }
        }
        public bool Sensor_LP_CV_Moving
        {
            get { return m_bLP_CV_Moving; }
            set
            {
                m_bLP_CV_Moving = value;
            }
        }
        public bool Sensor_LP_CV_Reset
        {
            get { return m_bLP_CV_Reset; }
            set
            {
                m_bLP_CV_Reset = value;
            }
        }
        public bool Sensor_LP_Z_Error
        {
            get { return m_bLP_Z_Error; }
            set
            {
                m_bLP_Z_Error = value;
            }
        }
        public bool Sensor_LP_Z_Reset
        {
            get { return m_bLP_Z_Reset; }
            set
            {
                m_bLP_Z_Reset = value;
            }
        }

        
        /// <summary>
        /// Port <-> OMRON CST ID Read, Write
        /// </summary>
        string m_OMRON_To_Port_CarrierID = string.Empty;
        public string OMRON_To_Port_CarrierID
        {
            get
            {

                int StartAddr = 0;
                if (GetParam().ID == "30301")
                    StartAddr = 0;
                else if (GetParam().ID == "30302")
                    StartAddr = 64;
                else if (GetParam().ID == "30303")
                    StartAddr = 128;
                else if (GetParam().ID == "30304")
                    StartAddr = 192;

                int stringSize = 64 * sizeof(short);
                byte[] stringData = new byte[stringSize];
                Buffer.BlockCopy(Master.m_ReadOmronWordMap, StartAddr * sizeof(short), stringData, 0, stringSize);
                string Get_CSTID = (string)Encoding.Default.GetString(stringData).Trim('\0');
                if (m_OMRON_To_Port_CarrierID != Get_CSTID)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"OMRON -> Port CST ID : {Get_CSTID}");
                }

                m_OMRON_To_Port_CarrierID = Get_CSTID;

                return m_OMRON_To_Port_CarrierID;
            }
        }

        string m_Port_To_OMRON_CarrierID = string.Empty;
        public string Port_To_OMRON_CarrierID
        {
            get
            {
                //m_LP_CarrierID = (string)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Buffer2_Carrier_ID_01);
                return m_Port_To_OMRON_CarrierID;
            }
            set
            {
                if (m_Port_To_OMRON_CarrierID != value)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"Port -> OMRON CST ID : {value}");
                }

                m_Port_To_OMRON_CarrierID = value;

                int StartAddr = 0;
                if (GetParam().ID == "30301")
                    StartAddr = 0;
                else if (GetParam().ID == "30302")
                    StartAddr = 64;
                else if (GetParam().ID == "30303")
                    StartAddr = 128;
                else if (GetParam().ID == "30304")
                    StartAddr = 192;

                string str = m_Port_To_OMRON_CarrierID;
                str = str.Replace(" ", string.Empty);
                if (string.IsNullOrEmpty(str))
                {
                    byte[] DataArray = new byte[64 * sizeof(short)];
                    int WriteSize = DataArray.Length;
                    Buffer.BlockCopy(DataArray, 0, Master.m_WriteOmronWordMap, StartAddr * sizeof(short), WriteSize);
                }
                else
                {
                    byte[] DataArray = Encoding.UTF8.GetBytes(str);
                    int WriteSize = DataArray.Length;
                    if (DataArray.Length > 64 * sizeof(short))
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CasseteIDOverFlow, $"{DataArray.Length} > {64 * sizeof(short)}");
                        WriteSize = 64 * sizeof(short); //Max
                    }
                    Buffer.BlockCopy(DataArray, 0, Master.m_WriteOmronWordMap, StartAddr * sizeof(short), WriteSize);
                }
            }
        }


        /// <summary>
        /// CIM 에서 CST Write 명령이 온 경우 Port 쪽 Memory Map에 업데이트
        /// 기능 만 추가하고 사용하지 않음(CST ID는 마스터에서 관리)
        /// </summary>
        public void PortToCIMWordMapUpdate()
        {
            if (CMD_CarrierID_WriteFlag == 1)
            {
                if (GetOperationDirection() == PortDirection.Input)
                {
                    LP_CarrierID = CMD_CIMToPortCarrierID;
                }
                else if (GetOperationDirection() == PortDirection.Output)
                {
                    OP_CarrierID = CMD_CIMToPortCarrierID;
                }
            }

            OP_AutoStep = (short)Get_OP_AutoControlStep();
            LP_AutoStep = (short)Get_LP_AutoControlStep();
        }
    }
}
