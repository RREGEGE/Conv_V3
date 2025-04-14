using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.CLS {
    class Average<T> {
        private T AverageSum;
        private int AverageCount;
        private object Lock = new object();
        /// <summary>
        /// 평균 값을 얻기 위해 추가 데이터 삽입
        /// </summary>
        /// <param name="Value"></param>
        public void Insert(T Value) {
            lock (Lock) {
                dynamic dx = AverageSum;
                dynamic dy = Value;
                AverageSum = dx + dy;
                AverageCount++;
            }
        }
        /// <summary>
        /// 현재까지의 데이터 클리어
        /// </summary>
        public void Clear() {
            lock (Lock) {
                AverageSum = default(T);
                AverageCount = 0;
            }
        }
        /// <summary>
        /// 현재까지 삽입된 데이터의 평균 반환
        /// </summary>
        /// <returns></returns>
        public T Result() {
            dynamic dx = AverageSum;
            dynamic dy = AverageCount;
            return (T)(dx / dy);
        }
    }

    class Max<T> {
        private T MaxVal;
        /// <summary>
        /// 최대 값을 얻기 위한 데이터 삽입
        /// </summary>
        /// <param name="Value"></param>
        public void Insert(T Value) {
            dynamic dx = MaxVal;

            if (dx < Value)
                MaxVal = Value;
        }
        /// <summary>
        /// 현재까지의 데이터 클리어
        /// </summary>
        public void Clear() {
            MaxVal = default(T);
        }
        /// <summary>
        /// 현재까지 삽입된 데이터 중 최대값 반환
        /// </summary>
        /// <returns></returns>
        public T Result() {
            return MaxVal;
        }
    }

    public static class MathInterface {
        public static double GetMax(params double[] numbers) {
            if (numbers.Length == 0) {
                return 0;
            }

            double maxNumber = numbers[0];

            for (int i = 1; i < numbers.Length; i++) {
                if (numbers[i] > maxNumber) {
                    maxNumber = numbers[i];
                }
            }

            return maxNumber;
        }

        public static double GetAverage(params double[] numbers) {
            if(numbers.Length == 0) {
                return 0;
            }

            double sum = 0;

            foreach(double number in numbers) {
                sum += number;
            }

            return sum / numbers.Length;
        }
    }
}
