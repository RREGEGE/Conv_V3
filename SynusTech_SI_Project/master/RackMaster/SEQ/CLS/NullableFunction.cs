using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.CLS {
    public static class NullableFunction {
        /// <summary>
        /// string 값을 Nullabe 타입의 int형 값으로 변환하는 함수
        /// </summary>
        /// <param name="text"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        public static bool TryParseNullable(string text, out int? outValue) {
            try {
                int parsedValue;
                bool success = int.TryParse(text, out parsedValue);
                outValue = success ? (int?)parsedValue : null;
                return success;
            }catch(Exception ex) {
                outValue = 0;

                return false;
            }
        }
        /// <summary>
        /// string 값을 Nullable 타입의 float형 값으로 변환하는 함수
        /// </summary>
        /// <param name="text"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        public static bool TryParseNullable(string text, out float? outValue) {
            try {
                float parsedValue;
                bool success = float.TryParse(text, out parsedValue);
                outValue = success ? (float?)parsedValue : null;
                return success;
            }catch(Exception ex) {
                outValue = 0;

                return false;
            }
        }
        /// <summary>
        /// string 값을 Nullable 타입의 double형 값으로 변환하는 함수
        /// </summary>
        /// <param name="text"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        public static bool TryParseNullable(string text, out double? outValue) {
            try {
                float parsedValue;
                bool success = float.TryParse(text, out parsedValue);
                outValue = success ? (float?)parsedValue : null;
                return success;
            }
            catch (Exception ex) {
                outValue = 0;

                return false;
            }
        }
    }
}
