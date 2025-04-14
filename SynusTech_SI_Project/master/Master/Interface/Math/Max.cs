using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Math
{
    /// <summary>
    /// 최대 값 연산을 위한 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Max<T>
    {
        private T MaxVal;

        public void Insert(T Value)
        {
            dynamic dx = MaxVal;

            if (dx < Value)
                MaxVal = Value;
        }
        public void Clear()
        {
            MaxVal = default(T);
        }
        public T Result()
        {
            return MaxVal;
        }

    }
}
