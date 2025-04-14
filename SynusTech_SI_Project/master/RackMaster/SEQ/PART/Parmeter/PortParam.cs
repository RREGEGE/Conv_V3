using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.PART {
    public enum PortParameterList {
        Id,
        Type,
        RowIndex,
        ColumnIndex,
        Direction,
        UseSensor,
        FromUpPosition,
        FromDownPosition,
        ToUpPosition,
        ToDownPosition,
        ForkPositionSensorType,
        UseForkPositionSensor,
    }

    public enum PositionSensorType { 
        PositionSensor_1,
        PositionSensor_2,
    }

    public partial class RackMasterMain {
        public partial class RackMasterParam {
            public class PortParameter {
                public int id;
                public PortType type;
                public int rowIndex;
                public int columnIndex;
                public PortDirection_HP direction;
                public bool useSensor;                          // Pick/Place Sensor를 사용할지 여부
                public int fromUpPosition;                      // Shelf와 Port의 Up/Down Position은 개별로 설정
                public int fromDownPosition;
                public int toUpPosition;
                public int toDownPosition;
                public PositionSensorType forkPosSensorType;    // Fork(Arm) 축의 Position Sensor 중 어떤 Sensor를 사용할지, Pos1 or Pos2
                public bool useForkPosSensor;                   // Fork(Arm) 축의 Position Sensor를 인터락에 추가할지 여부

                public PortParameter() {
                    id                  = -1;
                    type                = PortType.SHELF;
                    rowIndex            = 0;
                    columnIndex         = 0;
                    direction           = PortDirection_HP.Left;
                    useSensor           = true;
                    fromUpPosition      = 60;
                    fromDownPosition    = 20;
                    toUpPosition        = 60;
                    toDownPosition      = 20;
                    forkPosSensorType   = PositionSensorType.PositionSensor_1;
                    useForkPosSensor    = true;
                }
            }
            /// <summary>
            /// 포트 파라미터 파일 로드
            /// </summary>
            /// <returns></returns>
            public bool LoadPortParameterFile() {
                if (!Ini.IsFileExist(PortParameterPath)) {
                    Ini.CreateIniFile(ManagedFileInfo.PortParametersFileName, ManagedFileInfo.SettingsDirectory);
                }

                try {
                    byte[] buffer = new byte[Ini.MAX_BUFFER_SIZE];
                    buffer = Ini.GetSectionNames(PortParameterPath);
                    string allSections = System.Text.Encoding.Default.GetString(buffer);
                    string[] sectionNames = allSections.Split('\0');

                    foreach (string section in sectionNames) {
                        if (section == "")
                            continue;

                        PortParameter tempParam = new PortParameter();

                        try {
                            foreach (PortParameterList param in Enum.GetValues(typeof(PortParameterList))) {
                                switch (param) {
                                    case PortParameterList.Id:
                                        tempParam.id = Convert.ToInt32(section);
                                        break;

                                    case PortParameterList.Type:
                                        //m_motionParam.forkType = (ForkType)Enum.Parse(typeof(ForkType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                        tempParam.type = (PortType)Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.RowIndex:
                                        tempParam.rowIndex = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.ColumnIndex:
                                        tempParam.columnIndex = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.Direction:
                                        tempParam.direction = (PortDirection_HP)Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.UseSensor:
                                        //Boolean.TryParse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath), out m_motionParam.useInterpolation);
                                        Boolean.TryParse(Ini.GetValueString(section, $"{param}", PortParameterPath), out tempParam.useSensor);
                                        break;

                                    case PortParameterList.FromUpPosition:
                                        tempParam.fromUpPosition = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.FromDownPosition:
                                        tempParam.fromDownPosition = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.ToUpPosition:
                                        tempParam.toUpPosition = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.ToDownPosition:
                                        tempParam.toDownPosition = Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.ForkPositionSensorType:
                                        tempParam.forkPosSensorType = (PositionSensorType)Ini.GetValueInt(section, $"{param}", PortParameterPath);
                                        break;

                                    case PortParameterList.UseForkPositionSensor:
                                        Boolean.TryParse(Ini.GetValueString(section, $"{param}", PortParameterPath), out tempParam.useForkPosSensor);
                                        break;
                                }
                            }
                        }catch(Exception ex) {

                        }

                        m_portParam.Add(tempParam);
                    }

                    foreach(TeachingValueData teaching in m_teaching.GetPortTeachingDataNotShelf()) {
                        bool isAddParameter = false;

                        foreach(PortParameter portParam in m_portParam) {
                            if(teaching.id == portParam.id) {
                                isAddParameter = true;
                            }
                        }

                        if (!isAddParameter) {
                            PortParameter addLostParameter = new PortParameter();

                            addLostParameter.id = (int)teaching.id;
                            addLostParameter.type = (PortType)teaching.portType;
                            addLostParameter.rowIndex = (int)teaching.row;
                            addLostParameter.columnIndex = (int)teaching.col;
                            addLostParameter.direction = (PortDirection_HP)teaching.direction;

                            m_portParam.Add(addLostParameter);
                        }
                    }

                    foreach(PortParameter portParam in m_portParam) {
                        bool isAddParameter = false;

                        foreach(TeachingValueData teaching in m_teaching.GetPortTeachingDataNotShelf()) {
                            if (portParam.id == teaching.id)
                                isAddParameter = true;
                        }

                        if (!isAddParameter) {
                            m_teaching.AddPortData(portParam.rowIndex, portParam.columnIndex, (int)portParam.direction, (int)portParam.type, portParam.id);
                        }
                    }

                    m_isSettingSuccess_Port = true;

                    return true;
                }catch(Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Port Parameter Load Fail", ex));
                    m_isSettingSuccess_Port = false;
                    return false;
                }
            }
            /// <summary>
            /// 포트 파라미터 저장
            /// </summary>
            /// <returns></returns>
            public bool SavePortParameter() {
                if (!Ini.IsFileExist(PortParameterPath)) {
                    Ini.CreateIniFile(ManagedFileInfo.PortParametersFileName, ManagedFileInfo.SettingsDirectory);
                }

                try {
                    foreach(PortParameter port in m_portParam) {
                        foreach(PortParameterList param in Enum.GetValues(typeof(PortParameterList))) {
                            switch (param) {
                                case PortParameterList.Id:
                                    break;

                                case PortParameterList.Type:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.type}", PortParameterPath);
                                    break;

                                case PortParameterList.RowIndex:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.rowIndex}", PortParameterPath);
                                    break;

                                case PortParameterList.ColumnIndex:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.columnIndex}", PortParameterPath);
                                    break;

                                case PortParameterList.Direction:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.direction}", PortParameterPath);
                                    break;

                                case PortParameterList.UseSensor:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{port.useSensor}", PortParameterPath);
                                    break;

                                case PortParameterList.FromUpPosition:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{port.fromUpPosition}", PortParameterPath);
                                    break;

                                case PortParameterList.FromDownPosition:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{port.fromDownPosition}", PortParameterPath);
                                    break;

                                case PortParameterList.ToUpPosition:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.toUpPosition}", PortParameterPath);
                                    break;

                                case PortParameterList.ToDownPosition:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.toDownPosition}", PortParameterPath);
                                    break;

                                case PortParameterList.ForkPositionSensorType:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{(int)port.forkPosSensorType}", PortParameterPath);
                                    break;

                                case PortParameterList.UseForkPositionSensor:
                                    Ini.SetValueString($"{port.id}", $"{param}", $"{port.useForkPosSensor}", PortParameterPath);
                                    break;
                            }
                        }

                        // 포트 파라미터 기준으로 티칭 데이터와 상이할 경우 포트 파라미터가 기준이 되어 티칭 데이터에 추가가 된다.
                        if (!m_teaching.IsExistPortOrShelf(port.id)) {
                            m_teaching.AddPortData(port.rowIndex, port.columnIndex, (int)port.direction, (int)port.type, port.id);
                        }
                    }

                    m_teaching.SaveDataFile();
                    m_isSettingSuccess_Port = true;
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Port Parameter Save Success"));
                    return true;
                }catch(Exception ex) {
                    m_isSettingSuccess_Port = false;
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Port Parameter Save Fail", ex));
                    return false;
                }
            }
            /// <summary>
            /// 포트 파라미터 세팅
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetPortParameter(PortParameter param) {
                foreach(PortParameter port in m_portParam) {
                    if(port.id == param.id) {
                        port.type               = param.type;
                        port.rowIndex           = param.rowIndex;
                        port.columnIndex        = param.columnIndex;
                        port.direction          = param.direction;
                        port.useSensor          = param.useSensor;
                        port.fromUpPosition     = param.fromUpPosition;
                        port.fromDownPosition   = param.fromDownPosition;
                        port.toUpPosition       = param.toUpPosition;
                        port.toDownPosition     = param.toDownPosition;
                        port.forkPosSensorType  = param.forkPosSensorType;
                        port.useForkPosSensor   = param.useForkPosSensor;

                        return true;
                    }
                }

                m_portParam.Add(param);

                return true;
            }
            /// <summary>
            /// 해당 ID의 포트 파라미터 삭제
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool DeletePortParameter(int id) {
                for(int i = 0; i < m_portParam.Count; i++) {
                    if(m_portParam[i].id == id) {
                        m_portParam.RemoveAt(i);
                        m_teaching.DeletePort(id);
                        Ini.SetValueString($"{id}", null, null, PortParameterPath);
                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID에 포트 파라미터가 존재하는지 여부
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsExistPortParameter(int id) {
                for(int i = 0; i < m_portParam.Count; i++) {
                    if (m_portParam[i].id == id)
                        return true;
                }

                return false;
            }
            /// <summary>
            /// 모든 포트 파라미터 리스트 반환
            /// </summary>
            /// <returns></returns>
            public List<PortParameter> GetPortParameterList() {
                return m_portParam;
            }
            /// <summary>
            /// 해당 ID에 존재하는 포트 파라미터 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public PortParameter GetPortParameter(int id) {
                foreach(PortParameter param in m_portParam) {
                    if (param.id == id)
                        return param;
                }

                return null;
            }
            /// <summary>
            /// 해당 ID의 포트 파라미터 중 Pick/Place Sensor 사용 여부 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsPortSensorEnabled(int id) {
                foreach(PortParameter param in m_portParam) {
                    if(param.id == id) {
                        return param.useSensor;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID의 포트 파라미터 중 Fork(Arm)축의 Position Sensor 타입 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public PositionSensorType GetPortParameter_ForkPositionSensorType(int id) {
                foreach(PortParameter param in m_portParam) {
                    if(param.id == id) {
                        return param.forkPosSensorType;
                    }
                }

                return PositionSensorType.PositionSensor_1;
            }
            /// <summary>
            /// 해당 ID의 포트 파라미터 중 Fork(Arm)축의 Position Sensor 사용 여부 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool GetPortParameter_UseForkPositionSensor(int id) {
                foreach(PortParameter param in m_portParam) {
                    if(param.id == id) {
                        return param.useForkPosSensor;
                    }
                }

                return true;
            }
        }
    }
}
