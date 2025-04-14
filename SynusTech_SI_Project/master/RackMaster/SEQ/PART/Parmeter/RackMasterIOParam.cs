using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.PART {
    public enum InputList {
        RM_MC_On = 0,
        EMO_HP,
        EMO_OP,
        HP_DTP_EMS_SW,
        HP_DTP_DeadMan_SW,
        HP_DTP_Mode_Select_SW_1,
        HP_DTP_Mode_Select_SW_2,
        OP_DTP_EMS_SW,
        OP_DTP_DeadMan_SW,
        OP_DTP_Mode_Select_SW_1,
        OP_DTP_Mode_Select_SW_2,
        Z_Axis_Maint_Stopper_Sensor_1,
        Z_Axis_Maint_Stopper_Sensor_2,
        Z_Axis_Wire_Cut_Sensor,

        Presense_Detection_1,
        Presense_Detection_2,
        Double_Storage_Sensor_1,
        Double_Storage_Sensor_2,
        Fork_Pick_Sensor_Left,
        Fork_Place_Sensor_Left,
        Fork_Pick_Sensor_Right,
        Fork_Place_Sensor_Right,
        Fork_In_Place_1,
        Fork_In_Place_2,
        StickDetectSensor_1,
        StickDetectSensor_2,
        StickDetectSensor_3,
        StickDetectSensor_4,
        X_Axis_Position_Sensor_1,
        X_Axis_Position_Sensor_2,
        X_Axis_HP_Moving_Speed_Cut_Neg,
        X_Axis_HP_Moving_Speed_Cut_Pos,
        CPS_Regulator_Run,
        CPS_Regulator_Fault,
    }

    public enum OutputList {
        Handy_Touch_EMO_Relay_On = 0,
        Voice_Buzzer_CH1,
        Voice_Buzzer_CH2,
        Voice_Buzzer_CH3,
        Voice_Buzzer_CH4,
    }
    /// <summary>
    /// 아래 Distance Detect Sensor의 리스트들은 거리감지 센서인 AWS338i 제품의 매뉴얼을 토대로 작성
    /// </summary>
    public enum DistanceDetectSensorByteList {
        PositionValue       = 0,
        PositionStatus      = 4,
        VelocityValue       = 8,
        VelocityStatus      = 12,
    }

    public enum DistanceDetectSensorPositionStatus {
        HardwareError       = 0,
        LaserStatus         = 8,
        Intensity           = 13,
        Temperature         = 14,
        Laser               = 15,
        Plausibility        = 16,
    }

    public enum DistanceDetectSensorVelocityStatus {
        MeasurementError = 0,
    }

    public partial class RackMasterMain {
        public partial class RackMasterParam {
            public class IoParameter {
                public short byteAddr;
                public byte bitAddr;
                public bool isBInvert;  // B접점인 경우 해당 값이 True여야 한다.
                public bool isEnabled;
            }

            private enum IoParameterXmlNodeName {
                Input,
                Output,
                ByteAddr,
                BitAddr,
                BInvert,
                Enabled,
            }
            /// <summary>
            /// IO 파라미터 파일 로드
            /// </summary>
            /// <returns></returns>
            private bool LoadIoParameterFile() {
                try {
                    if (!m_ioXml.m_isFileExist) {
                        CreatedIoParameterFile();
                    }
                    else {
                        string nodeName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Input}";
                        if (m_ioXml.GetNodeListLength(nodeName) == 0) {
                            m_ioXml.Delete();
                            CreatedIoParameterFile();
                        }
                        else {
                            string parentName;
                            foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                                parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Input}" + "/" + $"{input}";
                                m_inputParam[(int)input].byteAddr = short.Parse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.ByteAddr}"));
                                m_inputParam[(int)input].bitAddr = byte.Parse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.BitAddr}"));
                                Boolean.TryParse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.BInvert}"), out m_inputParam[(int)input].isBInvert);
                                Boolean.TryParse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.Enabled}"), out m_inputParam[(int)input].isEnabled);
                            }
                            foreach(OutputList output in Enum.GetValues(typeof(OutputList))) {
                                parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Output}" + "/" + $"{output}";
                                m_outputParam[(int)output].byteAddr = short.Parse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.ByteAddr}"));
                                m_outputParam[(int)output].bitAddr = byte.Parse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.BitAddr}"));
                                Boolean.TryParse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.BInvert}"), out m_outputParam[(int)output].isBInvert);
                                Boolean.TryParse(m_ioXml.GetNodeVal($"{parentName + "/" + IoParameterXmlNodeName.Enabled}"), out m_outputParam[(int)output].isEnabled);
                            }
                        }
                    }

                    return true;
                }catch(Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Io Parameter Load Fail!", ex));
                    return false;
                }
            }
            /// <summary>
            /// IO 파라미터 파일이 존재하지 않는 경우 생성
            /// </summary>
            private void CreatedIoParameterFile() {
                string parentName = m_ioParameterRootName;
                m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Input}", null, true);
                foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                    parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Input}";
                    m_ioXml.SetNodeVal(parentName, $"{input}", null, true);
                    parentName = parentName + "/" + $"{input}";
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.ByteAddr}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BitAddr}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BInvert}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Enabled}", null, true);
                }

                parentName = m_ioParameterRootName;
                m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Output}", null, true);
                foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                    parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Output}";
                    m_ioXml.SetNodeVal(parentName, $"{output}", null, true);
                    parentName = parentName + "/" + $"{output}";
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.ByteAddr}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BitAddr}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BInvert}", null, true);
                    m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Enabled}", null, true);
                }

                m_ioXml.Save();
            }
            /// <summary>
            /// IO 파라미터 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveIoParameter() {
                try {
                    string parentName = "";
                    foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                        parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Input}" + "/" + $"{input}";
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.ByteAddr}", $"{GetInputParameter(input).byteAddr}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BitAddr}", $"{GetInputParameter(input).bitAddr}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BInvert}", $"{GetInputParameter(input).isBInvert}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Enabled}", $"{GetInputParameter(input).isEnabled}");
                    }

                    foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                        parentName = m_ioParameterRootName + "/" + $"{IoParameterXmlNodeName.Output}" + "/" + $"{output}";
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.ByteAddr}", $"{GetOutputParameter(output).byteAddr}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BitAddr}", $"{GetOutputParameter(output).bitAddr}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.BInvert}", $"{GetOutputParameter(output).isBInvert}");
                        m_ioXml.SetNodeVal(parentName, $"{IoParameterXmlNodeName.Enabled}", $"{GetOutputParameter(output).isEnabled}");
                    }

                    m_ioXml.Save();

                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "IO Parameter Save Success"));

                    m_isSettingSuccess_IO = true;

                    return true;
                }catch(Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "IO Parameter Save Fail", ex));

                    m_isSettingSuccess_IO = false;

                    return false;
                }
            }
            /// <summary>
            /// 해당 Input에 해당하는 파라미터 반환
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public IoParameter GetInputParameter(InputList input) {
                return m_inputParam[(int)input];
            }
            /// <summary>
            /// 해당 Input이 Enable 상태인지 반환
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public bool IsInputParameterEnabed(InputList input) {
                return m_inputParam[(int)input].isEnabled;
            }
            /// <summary>
            /// 해당 Output에 해당하는 파라미터 반환
            /// </summary>
            /// <param name="output"></param>
            /// <returns></returns>
            public IoParameter GetOutputParameter(OutputList output) {
                return m_outputParam[(int)output];
            }
            /// <summary>
            /// 해당 Output이 Enable 상태인지 반환
            /// </summary>
            /// <param name="output"></param>
            /// <returns></returns>
            public bool IsOutputParameterEnabled(OutputList output) {
                return m_outputParam[(int)output].isEnabled;
            }
            /// <summary>
            /// IO Setting이 성공했는지 여부
            /// </summary>
            /// <returns></returns>
            public bool IsSettingSuccess_IO() {
                return m_isSettingSuccess_IO;
            }
            /// <summary>
            /// IO 파라미터 파일을 다시 로드
            /// </summary>
            /// <returns></returns>
            public bool RefreshIOParameter() {
                return LoadIoParameterFile();
            }
        }
    }
}
