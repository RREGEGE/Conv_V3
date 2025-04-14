using System;
using System.Diagnostics;
using System.Threading;
using Master;
using Master.ManagedFile;
using System.Windows.Forms;
using System.Drawing;


/// <summary>
/// Login 제어를 위한 전역 클래스
/// </summary>
static public class LogIn
{
    public delegate void LogInExtendMessage();
    public static event LogInExtendMessage LogInExtendMessageEvent; //로그인 연장 이벤트
    
    /// <summary>
    /// Login Level
    /// </summary>
    public enum LogInLevel : int
    {
        LogOff,
        User,       //User, 1234 로그인 경우 (오토 조작만 가능)
        Maint,      //Maint, safe 로그인 경우 (파라미터 적용 가능, 저장 불가, 조작 가능)
        GOT,        //GOT 꼳은 경우
        Admin       //Admin, Admin 로그인 경우 (파라미터 적용, 저장 가능, 조작 가능)
    }

    static private bool          m_bLogOn       = false;
    static private Stopwatch     m_LogOnTime    = new Stopwatch();
    static private LogInLevel    m_eLogInLevel  = LogInLevel.LogOff;
    static private bool          m_bLogOnRemainingOneMinuteMessage = false;

    /// <summary>
    /// 로그인 진행 및 시간 측정
    /// </summary>
    /// <param name="_eLogInLevel"></param>
    static public void SetLogIn(LogInLevel _eLogInLevel)
    {
        m_bLogOn = true;
        m_eLogInLevel = _eLogInLevel;

        if (_eLogInLevel != LogInLevel.GOT)
        {
            if (!m_LogOnTime.IsRunning)
            {
                m_LogOnTime.Reset();
                m_LogOnTime.Start();

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.AcceptLogIn, $"Level({_eLogInLevel}) / Time({ApplicationParam.m_ApplicationParam.LoginDuration} sec)");

                Thread LocalThread = new Thread(delegate ()
                {
                    while (m_LogOnTime.IsRunning && _eLogInLevel != LogInLevel.GOT)
                    {
                        if (m_LogOnTime.Elapsed.TotalSeconds > ApplicationParam.m_ApplicationParam.LoginDuration && _eLogInLevel != LogInLevel.GOT)
                        {
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.LogInTimeout, $"Level({_eLogInLevel}) / Time({m_LogOnTime.Elapsed.TotalSeconds.ToString("0")} sec)");
                            SetLogout();
                        }
                        Thread.Sleep(500);
                    }
                });
                LocalThread.IsBackground = true;
                LocalThread.Name = "Logout Time Check";
                LocalThread.Start();

                Thread LocalThread2 = new Thread(delegate ()
                {
                    while (m_LogOnTime.IsRunning && _eLogInLevel != LogInLevel.GOT)
                    {
                        if (ApplicationParam.m_ApplicationParam.LoginDuration - m_LogOnTime.Elapsed.TotalSeconds <= 60 && !m_bLogOnRemainingOneMinuteMessage && _eLogInLevel != LogInLevel.GOT)
                        {
                            m_bLogOnRemainingOneMinuteMessage = true;
                            LogInExtendMessageEvent();
                        }
                        Thread.Sleep(1000);
                    }
                });
                LocalThread2.IsBackground = true;
                LocalThread2.Name = "Login Extend Check";
                LocalThread2.Start();
            }
        }
        else
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.AcceptLogIn, $"Level({_eLogInLevel})");
            if (!m_LogOnTime.IsRunning)
            {
                m_LogOnTime.Reset();
                m_LogOnTime.Start();
            }
        }
    }
    
    /// <summary>
    /// 로그아웃 진행 및 시간 측정 초기화
    /// </summary>
    static public void SetLogout()
    {
        if(m_bLogOn)
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.LogOut, $"Level({GetLogInLevel()}), Log On Time({m_LogOnTime.Elapsed.TotalSeconds.ToString("0")} sec)");

        m_bLogOn = false;
        m_eLogInLevel = LogInLevel.LogOff;
        m_bLogOnRemainingOneMinuteMessage = false;

        if (m_LogOnTime.IsRunning)
        {
            m_LogOnTime.Stop();
            m_LogOnTime.Reset();
        }
    }

    /// <summary>
    /// 로그인 연장
    /// </summary>
    static public void SetLogInExtend()
    {
        double currentOnTime = m_LogOnTime.Elapsed.TotalSeconds;
        m_LogOnTime.Reset();
        m_LogOnTime.Start();
        m_bLogOnRemainingOneMinuteMessage = false;
        LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.LogInExtend, $"Log On Time({currentOnTime.ToString("0")} sec)");
    }

    /// <summary>
    /// 로그인 상태
    /// </summary>
    /// <returns></returns>
    static public bool IsLogIn()
    {
        return m_bLogOn;
    }

    /// <summary>
    /// 현재 로그인 레벨
    /// </summary>
    /// <returns></returns>
    static public LogInLevel GetLogInLevel()
    {
        return m_eLogInLevel;
    }

    /// <summary>
    /// 로그인 남은 시간
    /// </summary>
    /// <returns></returns>
    static public TimeSpan GetRemaningTime()
    {
        return TimeSpan.FromSeconds(ApplicationParam.m_ApplicationParam.LoginDuration - m_LogOnTime.Elapsed.TotalSeconds);
    }

    /// <summary>
    /// 로그인 남은 시간을 텍스트로 변환
    /// </summary>
    /// <returns></returns>
    static public string GetRemaningTimeStr()
    {
        return (IsLogIn() && GetLogInLevel() != LogInLevel.GOT) ? $"{GetRemaningTime().Minutes.ToString("00")}:{GetRemaningTime().Seconds.ToString("00")}" : $"-";
    }

    /// <summary>
    /// 로그인 남은 시간이 절반 이하
    /// </summary>
    /// <returns></returns>
    static public bool IsHalfTimeOver()
    {
        return (m_LogOnTime.IsRunning && ApplicationParam.m_ApplicationParam.LoginDuration - m_LogOnTime.Elapsed.TotalSeconds <= (ApplicationParam.m_ApplicationParam.LoginDuration / 2.0));
    }

    /// <summary>
    /// 로그인 남은 시간이 1분 이하
    /// </summary>
    /// <returns></returns>
    static public bool IsRemaining1Min()
    {
        return (m_LogOnTime.IsRunning && ApplicationParam.m_ApplicationParam.LoginDuration - m_LogOnTime.Elapsed.TotalSeconds <= 60.0);
    }

    /// <summary>
    /// 로그인 남은 시간에 따른 라벨 배경 색 변경
    /// </summary>
    /// <param name="label"></param>
    static public void LogOnRemaningTimeLabelUpdate(ref Label label)
    {
        LabelFunc.SetText(label, GetRemaningTimeStr());

        if(IsRemaining1Min())
            LabelFunc.SetBackColor(label, IsHalfTimeOver() ? Master.Master.ErrorIntervalColor == Color.Red ? Color.Red : Color.Transparent : Color.Transparent);
        else if(IsHalfTimeOver())
            LabelFunc.SetBackColor(label, IsHalfTimeOver() ? Master.Master.ErrorIntervalColor == Color.Red ? Color.Orange : Color.Transparent : Color.Transparent);
        else
            LabelFunc.SetBackColor(label, Color.Transparent);
    }

    /// <summary>
    /// 로그인 남은 시간에 따른 버튼 배경색 변경
    /// </summary>
    /// <param name="btn"></param>
    static public void LogOnExtendButtonUpdate(ref Button btn)
    {
        ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_Extend"));
        ButtonFunc.SetEnable(btn, IsLogIn() ? true : false);

        if (IsLogIn())
        {
            if (IsRemaining1Min())
                ButtonFunc.SetBackColor(btn, IsHalfTimeOver() ? Master.Master.ErrorIntervalColor == Color.Red ? Color.Red : Color.White : Color.White);
            else if (IsHalfTimeOver())
                ButtonFunc.SetBackColor(btn, IsHalfTimeOver() ? Master.Master.ErrorIntervalColor == Color.Red ? Color.Orange : Color.White : Color.White);
            else
                ButtonFunc.SetBackColor(btn, Color.White);
        }
        else
        {
            ButtonFunc.SetBackColor(btn, Color.DarkGray);
        }
    }
}

