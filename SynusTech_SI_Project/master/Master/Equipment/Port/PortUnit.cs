using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// PortUnit.cs는 포트 축 사용 시 단위 변환을 위한 기능 기재
    /// WMX 단위 -> Program 기준 단위
    /// Program 단위 -> WMX 단위
    /// </summary>
    public partial class Port
    {
        public enum AxisType
        {
            Linear,
            Rotary
        }

        /// <summary>
        /// WMX의 제어 단위를 Program에서 사용중인 제어 단위로 변경
        /// Linear - WMX um 단위, Program mm 단위 사용
        /// Rotary - WMX 0.001Deg 단위, Program Deg 단위 사용
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="_micrometer"></param>
        /// <returns></returns>
        static public double WMXPosToProgramUnit(AxisType eAxisType, double _micrometer)
        {
            if (eAxisType == AxisType.Linear)
                return _micrometer / 1000.0;            //um -> mm
            else if (eAxisType == AxisType.Rotary)
                return _micrometer / 1000.0;            //0.001 Deg -> Deg

            return _micrometer;
        }

        /// <summary>
        /// WMX의 제어 단위를 Program에서 사용중인 제어 단위로 변경
        /// Linear - WMX um/sec 단위, Program m/min 단위 사용
        /// Rotary - WMX 0.001Deg/sec 단위, Program Deg/min 단위 사용
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="_micrometer"></param>
        /// <returns></returns>
        static public double WMXVelToProgramUnit(AxisType eAxisType, double _micrometer)
        {
            if (eAxisType == AxisType.Linear)
                return _micrometer / 1000000.0 * 60.0;  //um/sec -> m/min
            else if (eAxisType == AxisType.Rotary)
                return _micrometer / 1000.0 * 60.0;     //0.001 Deg/sec -> Deg/min

            return _micrometer;
        }

        /// <summary>
        /// 프로그램 제어 단위를 WMX에서 사용되는 제어 단위로 변경
        /// Linear - WMX um 단위, Program mm 단위 사용
        /// Rotary - WMX 0.001Deg 단위, Program Deg 단위 사용
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="_micrometer"></param>
        /// <returns></returns>
        static public double ProgramUnitToWMXPos(AxisType eAxisType, double _millimeter)
        {
            if (eAxisType == AxisType.Linear)
                return _millimeter * 1000.0;            //mm -> um
            else if (eAxisType == AxisType.Rotary)
                return _millimeter * 1000.0;            //Deg -> 0.001 Deg

            return _millimeter;
        }

        /// <summary>
        /// 프로그램 제어 단위를 WMX에서 사용되는 제어 단위로 변경
        /// Linear - WMX um/sec 단위, Program m/min 단위 사용
        /// Rotary - WMX 0.001Deg/sec 단위, Program Deg/min 단위 사용
        /// </summary>
        /// <param name="eAxisType"></param>
        /// <param name="_micrometer"></param>
        /// <returns></returns>
        static public double ProgramUnitToWMXVel(AxisType eAxisType, double _millimeter)
        {
            //m/min -> um/sec
            if (eAxisType == AxisType.Linear)
                return _millimeter * 1000000.0 / 60.0;  //m/min -> um/sec
            else if (eAxisType == AxisType.Rotary)
                return _millimeter * 1000.0 / 60.0;     //Deg/min -> 0.001 Deg/sec

            return _millimeter;
        }
    }
}
