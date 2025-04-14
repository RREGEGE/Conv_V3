using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

namespace RackMaster.SEQ.CLS {
    public static class BuildTime {
        /// <summary>
        /// 현재 실행된 프로세스의 Build 시간을 가져오는 함수
        /// </summary>
        /// <returns></returns>
        public static DateTime GetBuidTime() {
            //Assembly assembly = Assembly.GetExecutingAssembly();

            //var version = assembly.GetName().Version;
            //DateTime buildData = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);

            //if (TimeZoneInfo.Local.IsDaylightSavingTime(buildData)) {
            //    buildData = buildData.AddHours(1);
            //}

            //return buildData;

            //1. Assembly.GetExecutingAssembly().FullName의 값은
            //'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
            //와 같다.
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터
            //Build된 날짜까지의 총 일(Days) 수 이다.
            int intDays = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            //3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된
            //시간까지의 지나간 초(Second) 값 이다.
            int intSeconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            //4. 시차조정
            DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
                dtBuildDate = dtBuildDate.Add(daylingTime.Delta);
            return dtBuildDate;
        }
    }
}
