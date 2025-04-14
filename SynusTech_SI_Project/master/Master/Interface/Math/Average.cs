using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Math
{
    /// <summary>
    /// 평균 값 연산 위한 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Average<T>
    {
        private T AverageSum;
        private int AverageCount;
        private object Lock = new object();

        public void Insert(T Value)
        {
            lock (Lock)
            {
                dynamic dx = AverageSum;
                dynamic dy = Value;
                AverageSum = dx + dy;
                AverageCount++;
            }
        }
        public void Clear()
        {
            lock (Lock)
            {
                AverageSum = default(T);
                AverageCount = 0;
            }
        }
        public T Result()
        {
            dynamic dx = AverageSum;
            dynamic dy = AverageCount;
            return (T)(dx / dy);
        }
        
    }
}
