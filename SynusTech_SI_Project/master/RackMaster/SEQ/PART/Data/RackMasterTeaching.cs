using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using System.IO;

namespace RackMaster.SEQ.PART
{
    public class TeachingValueData
    {
        public float? valX;             // X 값
        public float? valFork_A;        // Fork 축이 뻗었을 때 Arm 축 값
        public float? valFork_T;        // Fork 축이 뻗었을 때 Turn 축 값
        public float? valT;             // Turn 값
        public float? valZ;             // Z축의 Center 값
        public float? valZDown;         // From 동작 시 Z축의 Center로부터 떨어져 있는 값
        public float? valZUp;           // To 동작 시 Z축의 Center로부터 떨어져 있는 값
        public int? row;                // 해당 포트의 row index
        public int? col;                // 해당 포트의 column index
        public int? id;                 // 해당 포트의 id
        public int? direction;          // 해당 포트가 왼쪽인지 오른쪽인지. 왼쪽 = 0, 오른쪽 = 1, 기준은 HP 기준으로
        public int? portType;           // 해당 포트의 타입
        public int? index;              // XML Node 찾을 Index
        public int? enable;             // 활성화 및 비활성화 여부

        public TeachingValueData() {
            valX            = null;
            valFork_A       = null;
            valFork_T       = null;
            valT            = null;
            valZ            = null;
            valZDown        = null;
            valZUp          = null;
            row             = null;
            col             = null;
            id              = null;
            direction       = null;
            portType        = null;
            index           = null;
            enable          = 1;
        }
    }

    public class MaintTeachingData {
        public float? valX;
        public float? valT;

        public MaintTeachingData() {
            valX = null;
            valT = null;
        }
    }

    public enum PortDirection_HP {
        Left = 0,
        Right = 1,
        MAX = 2,
    }

    public enum PortType {
        SHELF = 100,
        MGV_AGV_PORT = 301,
        OHT_MGV_PORT = 302,
        AUTO_PORT = 303,
        OVEN_PORT = 305,
        SORTER_PORT = 306,
    }

    public enum PortEnabled {
        Disabled = 0,
        Enabled = 1,
    }

    public partial class RackMasterMain {
        public class TeachingData {
            private MaintTeachingData[] m_maintTeaching;
            private List<TeachingValueData> m_teaching;

            private XmlFile m_xml;

            private int m_shelfRows = 0;
            private int m_shelfCols = 0;
            private int m_shelfTotalCount = 0;

            private const int MAINT_TARGET_COUNT = 2;

            private static string m_path = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.PortSettingFileName;

            private const string m_xmlRoot = "PortData";
            private const string m_xmlInfo = "Info";
            private const string m_xmlCount = "Count";
            private const string m_xmlRows = "Rows";
            private const string m_xmlColumns = "Columns";
            private const string m_xmlRow = "Row";
            private const string m_xmlCol = "Column";
            private const string m_xmlPorts = "Ports";
            private const string m_xmlPort = "Port";
            private const string m_xmlId = "Id";
            private const string m_xmlLR = "LR";
            private const string m_xmlType = "Type";
            private const string m_xmlIndex = "Index";
            private const string m_xmlX = "X";
            private const string m_xmlZ = "Z";
            private const string m_xmlZUp = "ZUp";
            private const string m_xmlZDown = "ZDown";
            private const string m_xmlTurn = "Turn";
            private const string m_xmlForkA = "ForkA";
            private const string m_xmlForkT = "ForkT";
            private const string m_xmlEnabled = "Enabled";
            private const string m_xmlMaint_HP = "Maint_HP";
            private const string m_xmlMaint_OP = "Maint_OP";

            private bool m_isFileExist = false;

            public TeachingData() {
                m_teaching = new List<TeachingValueData>();
                m_maintTeaching = new MaintTeachingData[MAINT_TARGET_COUNT];
                for (int i = 0; i < MAINT_TARGET_COUNT; i++) {
                    m_maintTeaching[i] = new MaintTeachingData();
                }

                m_isFileExist = XmlFile.IsFileExist(m_path);
                if (m_isFileExist) {
                    LoadDataFile();
                }
            }

            public bool IsFileExist() {
                return m_isFileExist;
            }

            /// <summary>
            /// 첫 장비 세팅할 때 Port 데이터 생성(Form에서 호출)
            /// </summary>
            /// <param name="row"></param>
            /// <param name="col"></param>
            /// <returns></returns>
            public bool CreatePortData(int row, int col) {
                if (m_isFileExist) {
                    try {
                        File.Delete(m_path);
                    }
                    catch (Exception ex) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.PortDataLoadFail, ex));
                    }
                }

                m_xml = new XmlFile(m_path, m_xmlRoot);

                string xmlParent = m_xmlRoot;
                m_xml.SetNodeVal(xmlParent, m_xmlInfo, null, true);
                m_shelfCols = col;
                m_shelfRows = row;
                m_shelfTotalCount = col * row * (int)PortDirection_HP.MAX;

                xmlParent = m_xmlRoot + "/" + m_xmlInfo;
                m_xml.SetNodeVal(xmlParent, m_xmlCount, $"{m_shelfTotalCount}", true);
                m_xml.SetNodeVal(xmlParent, m_xmlRows, $"{m_shelfRows}", true);
                m_xml.SetNodeVal(xmlParent, m_xmlColumns, $"{m_shelfCols}", true);

                xmlParent = m_xmlRoot;
                m_xml.SetNodeVal(xmlParent, m_xmlPorts, null, true);
                int idx = 0;
                for (int i = 0; i < (int)PortDirection_HP.MAX; i++) {
                    for (int j = 0; j < col; j++) {
                        for (int k = 0; k < row; k++) {
                            int id = ((i + 1) * 10000) + ((j + 1) * 100) + (k + 1);
                            TeachingValueData temp = new TeachingValueData();
                            temp.id = id;
                            temp.direction = i;
                            temp.portType = (int)PortType.SHELF;
                            temp.row = k;
                            temp.col = j;
                            temp.index = idx;

                            m_teaching.Add(temp);

                            xmlParent = m_xmlRoot + "/" + m_xmlPorts;
                            m_xml.SetNodeVal(xmlParent, m_xmlPort, null, true);

                            xmlParent = xmlParent + "/" + m_xmlPort;
                            m_xml.SetNodeAttribute(xmlParent, m_xmlId, $"{temp.id}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlRow, $"{temp.row}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlCol, $"{temp.col}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlLR, $"{temp.direction}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlType, $"{temp.portType}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlIndex, $"{temp.index}", idx);
                            m_xml.SetNodeAttribute(xmlParent, m_xmlEnabled, $"{temp.enable}", idx);

                            m_xml.SetNodeVal(xmlParent, m_xmlX, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlZ, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlZUp, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlZDown, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlTurn, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlForkA, null, idx, true);
                            m_xml.SetNodeVal(xmlParent, m_xmlForkT, null, idx, true);

                            idx++;
                        }
                    }
                }

                xmlParent = m_xmlRoot;
                m_xml.SetNodeVal(xmlParent, m_xmlMaint_HP, null, true);

                xmlParent = $"{m_xmlRoot}/{m_xmlMaint_HP}";

                m_xml.SetNodeVal(xmlParent, m_xmlX, null, true);
                m_xml.SetNodeVal(xmlParent, m_xmlZ, null, true);
                m_xml.SetNodeVal(xmlParent, m_xmlTurn, null, true);

                xmlParent = m_xmlRoot;
                m_xml.SetNodeVal(xmlParent, m_xmlMaint_OP, null, true);

                xmlParent = $"{m_xmlRoot}/{m_xmlMaint_OP}";

                m_xml.SetNodeVal(xmlParent, m_xmlX, null, true);
                m_xml.SetNodeVal(xmlParent, m_xmlZ, null, true);
                m_xml.SetNodeVal(xmlParent, m_xmlTurn, null, true);

                m_xml.Save();

                m_isFileExist = true;

                return true;
            }
            /// <summary>
            /// 티칭 데이터 파일 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveDataFile() {
                if (m_shelfTotalCount <= 0)
                    return false;

                string xmlParent = "";

                for (int idx = 0; idx < m_teaching.Count; idx++) {
                    xmlParent = $"{m_xmlRoot}/{m_xmlPorts}";
                    string nodeName = m_xmlPort;
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlId, $"{m_teaching[idx].id}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlRow, $"{m_teaching[idx].row}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlCol, $"{m_teaching[idx].col}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlLR, $"{m_teaching[idx].direction}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlType, $"{m_teaching[idx].portType}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlIndex, $"{m_teaching[idx].index}", idx);
                    m_xml.SetNodeAttribute(xmlParent, nodeName, m_xmlEnabled, $"{m_teaching[idx].enable}", idx);

                    xmlParent = $"{xmlParent}/{nodeName}";

                    m_xml.SetNodeVal(xmlParent, m_xmlX, Convert.ToString(m_teaching[idx].valX), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlZ, Convert.ToString(m_teaching[idx].valZ), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlZUp, Convert.ToString(m_teaching[idx].valZUp), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlZDown, Convert.ToString(m_teaching[idx].valZDown), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlTurn, Convert.ToString(m_teaching[idx].valT), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlForkA, Convert.ToString(m_teaching[idx].valFork_A), idx);
                    m_xml.SetNodeVal(xmlParent, m_xmlForkT, Convert.ToString(m_teaching[idx].valFork_T), idx);
                }

                xmlParent = $"{m_xmlRoot}/{m_xmlMaint_HP}";

                if (!m_xml.isNodeExist(m_xmlRoot, m_xmlMaint_HP))
                    m_xml.SetNodeVal(m_xmlRoot, m_xmlMaint_HP, null, true);

                m_xml.SetNodeVal(xmlParent, m_xmlX, $"{m_maintTeaching[(int)MaintTarget.HP].valX}");
                m_xml.SetNodeVal(xmlParent, m_xmlTurn, $"{m_maintTeaching[(int)MaintTarget.HP].valT}");

                xmlParent = $"{m_xmlRoot}/{m_xmlMaint_OP}";

                if (!m_xml.isNodeExist(m_xmlRoot, m_xmlMaint_OP))
                    m_xml.SetNodeVal(m_xmlRoot, m_xmlMaint_OP, null, true);

                m_xml.SetNodeVal(xmlParent, m_xmlX, $"{m_maintTeaching[(int)MaintTarget.OP].valX}");
                m_xml.SetNodeVal(xmlParent, m_xmlTurn, $"{m_maintTeaching[(int)MaintTarget.OP].valT}");

                m_xml.Save();

                return true;
            }
            /// <summary>
            /// 저장된 티칭 데이터 파일 로드
            /// </summary>
            /// <returns></returns>
            private bool LoadDataFile() {
                m_xml = new XmlFile(m_path, m_xmlRoot);

                string xmlCount = m_xmlRoot + "/" + m_xmlInfo + "/" + m_xmlCount;
                string xmlRows = m_xmlRoot + "/" + m_xmlInfo + "/" + m_xmlRows;
                string xmlCols = m_xmlRoot + "/" + m_xmlInfo + "/" + m_xmlColumns;

                string count = m_xml.GetNodeVal(xmlCount);
                string rows = m_xml.GetNodeVal(xmlRows);
                string cols = m_xml.GetNodeVal(xmlCols);

                if (!int.TryParse(count, out m_shelfTotalCount) || !int.TryParse(rows, out m_shelfRows) || !int.TryParse(cols, out m_shelfCols)) {
                    return false;
                }

                string xmlPort = m_xmlRoot + "/" + m_xmlPorts + "/" + m_xmlPort;
                int nodeCount = m_xml.GetNodeListLength(xmlPort);
                if (nodeCount == 0) {
                    return false;
                }
                for (int j = 0; j < nodeCount; j++) {
                    TeachingValueData temp = new TeachingValueData();
                    m_teaching.Add(temp);
                }

                try {
                    for (int i = 0; i < nodeCount; i++) {
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlId, i), out m_teaching[i].id);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlRow, i), out m_teaching[i].row);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlCol, i), out m_teaching[i].col);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlType, i), out m_teaching[i].portType);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlLR, i), out m_teaching[i].direction);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlIndex, i), out m_teaching[i].index);
                        NullableFunction.TryParseNullable(m_xml.GetNodeAttribute(xmlPort, m_xmlEnabled, i), out m_teaching[i].enable);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlX, i), out m_teaching[i].valX);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlZ, i), out m_teaching[i].valZ);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlZUp, i), out m_teaching[i].valZUp);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlZDown, i), out m_teaching[i].valZDown);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlTurn, i), out m_teaching[i].valT);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlForkA, i), out m_teaching[i].valFork_A);
                        NullableFunction.TryParseNullable(m_xml.GetNodeVal(xmlPort, m_xmlForkT, i), out m_teaching[i].valFork_T);
                    }

                    string xmlMaint = $"{m_xmlRoot}/{m_xmlMaint_HP}";
                    NullableFunction.TryParseNullable(m_xml.GetNodeVal($"{xmlMaint}/{m_xmlX}"), out m_maintTeaching[(int)MaintTarget.HP].valX);
                    NullableFunction.TryParseNullable(m_xml.GetNodeVal($"{xmlMaint}/{m_xmlTurn}"), out m_maintTeaching[(int)MaintTarget.HP].valT);

                    xmlMaint = $"{m_xmlRoot}/{m_xmlMaint_OP}";
                    NullableFunction.TryParseNullable(m_xml.GetNodeVal($"{xmlMaint}/{m_xmlX}"), out m_maintTeaching[(int)MaintTarget.OP].valX);
                    NullableFunction.TryParseNullable(m_xml.GetNodeVal($"{xmlMaint}/{m_xmlTurn}"), out m_maintTeaching[(int)MaintTarget.OP].valT);
                }
                catch (Exception e) {
                    // Log
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 포트 데이터 추가
            /// 해당 행/열에 Shelf를 Port로 대체, 그 외에는 리스트에 추가
            /// </summary>
            /// <param name="row"></param>
            /// <param name="col"></param>
            /// <param name="dir"></param>
            /// <param name="type"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool AddPortData(int row, int col, int dir, int type, int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        return false;
                    }

                    if ((int)m_teaching[i].row == row && (int)m_teaching[i].col == col && (int)m_teaching[i].direction == dir) {
                        m_teaching[i].id = id;
                        m_teaching[i].portType = type;
                        return true;
                    }
                }

                TeachingValueData temp = new TeachingValueData();

                temp.id = id;
                temp.row = row;
                temp.col = col;
                temp.direction = dir;
                temp.portType = type;
                temp.index = m_teaching.Count;

                m_teaching.Add(temp);

                return true;
            }
            /// <summary>
            /// Maint 티칭 데이터 저장
            /// </summary>
            /// <param name="x"></param>
            /// <param name="turn"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            public bool SetMaintTeaching(float x, float turn, MaintTarget target) {
                m_maintTeaching[(int)target].valX = x;
                m_maintTeaching[(int)target].valT = turn;

                return true;
            }
            /// <summary>
            /// 설정된 Port 데이터 삭제
            /// 해당 행/열에 Port 데이터를 Shelf로 대체하거나 리스트에서 삭제
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool DeletePort(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        if (m_teaching[i].col <= m_shelfCols && m_teaching[i].row <= m_shelfRows && m_teaching[i].col >= 0 && m_teaching[i].row >= 0) {
                            int tempID = (int)(((int)m_teaching[i].direction + 1) * 10000) + (((int)m_teaching[i].col + 1) * 100) + ((int)m_teaching[i].row + 1);
                            m_teaching[i].id = tempID;
                            m_teaching[i].portType = (int)PortType.SHELF;
                            m_teaching[i].valX = null;
                            m_teaching[i].valZ = null;

                            return true;
                        }
                        else {
                            m_teaching.RemoveAt(i);
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// 해당 ID가 존재하는 경우 모든 티칭 데이터 저장
            /// </summary>
            /// <param name="id"></param>
            /// <param name="x"></param>
            /// <param name="t"></param>
            /// <param name="z_center"></param>
            /// <param name="fork_a"></param>
            /// <param name="fork_t"></param>
            /// <returns></returns>
            public bool SetAllValue(int id, float x, float t, float z_center, float fork_a, float fork_t) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        m_teaching[i].valX = x;
                        m_teaching[i].valT = t;
                        m_teaching[i].valZ = z_center;
                        m_teaching[i].valFork_A = fork_a;
                        m_teaching[i].valFork_T = fork_t;

                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 모든 Shelf의 Z축 Up/Down 티칭 데이터 저장
            /// </summary>
            /// <param name="up"></param>
            /// <param name="down"></param>
            public void SetAllValZ_UpDown(float up, float down) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if(m_teaching[i].portType == (int)PortType.SHELF) {
                        m_teaching[i].valZUp = up;
                        m_teaching[i].valZDown = down;
                    }
                }
            }
            /// <summary>
            /// 해당 Index에 해당하는 Z축 Up/Down 티칭 데이터 저장
            /// </summary>
            /// <param name="index"></param>
            /// <param name="up"></param>
            /// <param name="down"></param>
            public void SetValZ_UpDown(int index, float up, float down) {
                m_teaching[index].valZUp = up;
                m_teaching[index].valZDown = down;
            }
            /// <summary>
            /// 해당 Index에 해당하는 X축 Z축 티칭 데이터 저장
            /// </summary>
            /// <param name="index"></param>
            /// <param name="x"></param>
            /// <param name="z"></param>
            /// <returns></returns>
            public bool SetValXZ(int index, float x, float z) {
                m_teaching[index].valX = x;
                m_teaching[index].valZ = z;

                return true;
            }
            /// <summary>
            /// 해당 ID에 해당하는 X/Z 티칭 데이터 저장
            /// </summary>
            /// <param name="id"></param>
            /// <param name="x"></param>
            /// <param name="z"></param>
            /// <returns></returns>
            public bool SetValXZ_ID(int id, float x, float z) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        m_teaching[i].valX = x;
                        m_teaching[i].valZ = z;

                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID에 해당하는 Turn 티칭 데이터 저장
            /// </summary>
            /// <param name="id"></param>
            /// <param name="t"></param>
            /// <returns></returns>
            public bool SetValTurn_ID(int id, float t) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        m_teaching[i].valT = t;

                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID의 Enable 여부 저장
            /// </summary>
            /// <param name="id"></param>
            /// <param name="enable"></param>
            /// <returns></returns>
            public bool SetEnable(int id, bool enable) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        if (enable)
                            m_teaching[i].enable = (int)PortEnabled.Enabled;
                        else
                            m_teaching[i].enable = (int)PortEnabled.Disabled;

                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID가 Enable 상태인지 판단
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsEnablePort(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        if (m_teaching[i].enable == (int)PortEnabled.Enabled)
                            return true;
                        else if (m_teaching[i].enable == (int)PortEnabled.Disabled)
                            return false;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 방향에 존재하는 모든 Shelf에 Fork 티칭 데이터 저장
            /// </summary>
            /// <param name="dir"></param>
            /// <param name="fork_A"></param>
            /// <param name="fork_T"></param>
            public void SetAllFork(int dir, float fork_A, float fork_T) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].direction == dir) {
                        if (m_teaching[i].portType == (int)PortType.SHELF) {
                            m_teaching[i].valFork_A = fork_A;
                            m_teaching[i].valFork_T = fork_T;
                        }
                    }
                }
            }
            /// <summary>
            /// 해당 방향에 존재하는 모든 Shelf/Port의 Turn 티칭 데이터 저장
            /// </summary>
            /// <param name="dir"></param>
            /// <param name="value"></param>
            public void SetAllTurn(int dir, float value) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].direction == dir) {
                        m_teaching[i].valT = value;
                    }
                }
            }
            /// <summary>
            /// 해당 ID의 티칭데이터 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public TeachingValueData GetTeachingData(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id)
                        return m_teaching[i];
                }

                return null;
            }
            /// <summary>
            /// 현재 저장된 모든 티칭데이터 리스트 반환
            /// </summary>
            /// <returns></returns>
            public List<TeachingValueData> GetTeachingDataList() {
                return m_teaching;
            }
            /// <summary>
            /// 현재 저장된 모든 티칭 데이터 리스트의 개수 반환
            /// </summary>
            /// <returns></returns>
            public int GetTeachingDataArrayCount() {
                return m_teaching.Count;
            }
            /// <summary>
            /// 현재 저장된 티칭데이터의 Row 개수 반환
            /// </summary>
            /// <returns></returns>
            public int GetTeachingRow() {
                return m_shelfRows;
            }
            /// <summary>
            /// 현재 저장된 티칭데이터의 Column 개수 반환
            /// </summary>
            /// <returns></returns>
            public int GetTeachingCol() {
                return m_shelfCols;
            }
            /// <summary>
            /// 해당 ID의 Port Type 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public PortType GetPortType(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        if (m_teaching[i].portType != null)
                            return (PortType)m_teaching[i].portType;
                    }
                }

                return PortType.SHELF;
            }
            /// <summary>
            /// 해당 ID에 해당하는 Index 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public Nullable<int> GetTeachingDataIndex(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        return i;
                    }
                }

                return null;
            }
            /// <summary>
            /// 해당 ID의 Shelf/Port의 티칭 데이터들이 전부 저장되어 있는지 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsExistTeachingData(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id) {
                        if (m_teaching[i].valFork_A != null && m_teaching[i].valFork_T != null && m_teaching[i].valT != null
                            && m_teaching[i].valX != null && m_teaching[i].valZ != null && m_teaching[i].valZDown != null
                            && m_teaching[i].valZUp != null && m_teaching[i].col != null && m_teaching[i].id != null
                            && m_teaching[i].direction != null && m_teaching[i].portType != null && m_teaching[i].row != null &&
                            (int)m_teaching[i].valX != 0)
                            return true;

                        return false;
                    }
                }

                return false;
            }
            /// <summary>
            /// 해당 ID가 존재하는지 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsExistPortOrShelf(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id)
                        return true;
                }

                return false;
            }
            /// <summary>
            /// 해당 ID의 X축 티칭 데이터 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public float GetTeachingValueX(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id)
                        return (float)m_teaching[i].valX;
                }

                return 0;
            }
            /// <summary>
            /// 해당 ID의 Z축 티칭 데이터 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public float GetTeachingValueZ(int id) {
                for (int i = 0; i < m_teaching.Count; i++) {
                    if (m_teaching[i].id == id)
                        return (float)m_teaching[i].valZ;
                }

                return 0;
            }
            /// <summary>
            /// 저장된 티칭 데이터 중 Shelf가 아닌 Port에 해당하는 리스트 반환
            /// </summary>
            /// <returns></returns>
            public List<TeachingValueData> GetPortTeachingDataNotShelf() {
                List<TeachingValueData> teaching = new List<TeachingValueData>();

                foreach(TeachingValueData data in m_teaching) {
                    if (data.portType != (int)PortType.SHELF)
                        teaching.Add(data);
                }

                if (teaching.Count <= 0)
                    return null;

                return teaching;
            }
            /// <summary>
            /// Maint 티칭 데이터 반환
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public MaintTeachingData GetMaintTeachingData(MaintTarget target) {
                return m_maintTeaching[(int)target];
            }
            /// <summary>
            /// 해당 Maint 티칭이 완료되었는지 판단
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public bool IsMaintTeachingComplete(MaintTarget target) {
                if (m_maintTeaching[(int)target].valT == null || m_maintTeaching[(int)target].valX == null)
                    return false;

                return true;
            }
            /// <summary>
            /// 모든 Maint 티칭이 완료되었는지 판단
            /// </summary>
            /// <returns></returns>
            public bool IsMaintAllTeachingComplete() {
                return IsMaintTeachingComplete(MaintTarget.HP) && IsMaintTeachingComplete(MaintTarget.OP);
            }
        }
    }
}
