using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Synustech.Common
{
    public static class ErrorCheck
    {
        //private enum Cmd
        //{
        //    C_START = 100,
        //    C_ERROR = 101,
        //    C_END = 199,
        //}

        //private static Conveyor m_fsmErrorCheck = new NormalConveyor();

        //public static void Init()
        //{
        //    m_fsmErrorCheck.Set((int)Cmd.C_START);
        //}

        //public static void Run()
        //{
        //    switch (m_fsmErrorCheck.Get())
        //    {
        //        case (int)Cmd.C_START:

        //            //Motor();
        //            //EmerSwitch();
        //            //DoorOpen();
        //            break;

        //        case (int)Cmd.C_ERROR:
        //            if (!Global.bErr)
        //                m_fsmErrorCheck.Set((int)Cmd.C_START);
        //            else
        //            {
        //                Stocker.Stop();

        //                Global.bAutoMode_AutoRun = false;


        //                Axis.AllStop();
        //            }
        //            break;

        //        case (int)Cmd.C_END:
        //            m_fsmErrorCheck.Set(FSM.C_IDLE);
        //            break;

        //    }
        //}

        //public static void Stop()
        //{
        //    m_fsmErrorCheck.Set((int)Cmd.C_END);
        //}

        //public static void SetEmoOff()
        //{
        //    m_fsmErrorCheck.Set((int)Cmd.C_START);

        //    return;
        //}

        //public static void SetError(AlarmType eAlarmType, int _Alarm)
        //{
        //    Alarm.AddAlarm(eAlarmType, _Alarm);

        //    m_fsmErrorCheck.Set((int)Cmd.C_ERROR);

        //    Global.bErr = true;
        //}


    }
}