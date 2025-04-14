using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.CLS;
using MovenCore;

namespace RackMaster.SEQ.PART {
    public partial class RackMasterMain{
        public partial class RackMasterParam {
            private AxisParam[] m_axisParam;
            private MotionParam m_motionParam;
            private AutoTeachingParam m_autoTeachingParam;
            private TimerParameter m_timerParam;
            private ScaraParameter m_scara;
            private Axis m_axis;
            private List<IoParameter> m_inputParam;
            private List<IoParameter> m_outputParam;
            private List<PortParameter> m_portParam;
            private TeachingData m_teaching;

            private static string SettingParameterPath      = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.RackMasterSettingParametersFileName;
            private static string AxisParameterPath         = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.RackMasterAxisParametersFileName;
            private static string IOParameterPath           = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.IOParameterFileName;
            private static string WMXParameterPath          = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.WMXParameterFileName;
            private static string PortParameterPath         = ManagedFileInfo.SettingsDirectory + "\\" + ManagedFileInfo.PortParametersFileName;
            private static string m_ioParameterRootName     = "IOParameter";

            private XmlFile m_ioXml;

            private const int m_MaxAxisCount = 4;

            private double m_PGain = 1.0;
            private double m_IGain = 0.001;

            private bool m_isSettingSuccess_WMX;
            private bool m_isSettingSuccess_RM;
            private bool m_isSettingSuccess_Axis;
            private bool m_isSettingSuccess_IO;
            private bool m_isSettingSuccess_Port;

            public RackMasterParam(RackMasterMain m_main) {
                m_motionParam = new MotionParam();
                m_autoTeachingParam = new AutoTeachingParam();
                m_timerParam = new TimerParameter();
                m_axisParam = new AxisParam[m_MaxAxisCount];
                m_scara = new ScaraParameter();
                for (int i = 0; i < m_MaxAxisCount; i++) {
                    m_axisParam[i] = new AxisParam();
                }
                m_inputParam = new List<IoParameter>();
                m_outputParam = new List<IoParameter>();
                foreach(InputList intput in Enum.GetValues(typeof(InputList))) {
                    IoParameter instance = new IoParameter();
                    m_inputParam.Add(instance);
                }
                foreach(OutputList output in Enum.GetValues(typeof(OutputList))) {
                    IoParameter instance = new IoParameter();
                    m_outputParam.Add(instance);
                }
                m_portParam = new List<PortParameter>();
                m_axis = Axis.Instance;
                m_teaching = m_main.m_teaching;

                m_ioXml = new XmlFile(IOParameterPath, m_ioParameterRootName);

                m_isSettingSuccess_WMX      = true;
                m_isSettingSuccess_RM       = true;
                m_isSettingSuccess_IO       = true;
                m_isSettingSuccess_Axis     = true;

                if (!LoadSettingParameterFile())
                    m_isSettingSuccess_RM = false;
                if (!LoadAxisParameterFile())
                    m_isSettingSuccess_Axis = false;
                if (!LoadIoParameterFile())
                    m_isSettingSuccess_IO = false;
                if (!LoadWMXParameterFile())
                    m_isSettingSuccess_WMX = false;
                if (!LoadPortParameterFile())
                    m_isSettingSuccess_Port = false;

                //RefreshWMXParameter();
            }
        }
    }
}
