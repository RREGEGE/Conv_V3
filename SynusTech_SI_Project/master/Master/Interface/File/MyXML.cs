using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.MyFileIO
{
    class MyXML
    {
        //XmlSerializer의 생성자는 먼저 유형의 직렬화를 위해 생성된 클래스를 포함해야 하는[YourAssembly].XmlSerializers.dll이라는 어셈블리를 찾으려고 시도합니다.
        //이러한 DLL은 아직 생성되지 않았으므로(기본적으로 생성되지 않음) FileNotFoundException이 발생합니다.그런 일이 발생하면 XmlSerializer의 생성자가 해당 예외를 포착하고 
        //런타임에 XmlSerializer의 생성자에 의해 DLL이 자동으로 생성됩니다(이 작업은 컴퓨터의 %temp% 디렉터리에 C# 소스 파일을 생성한 다음 C# 컴파일러를 사용하여 컴파일하여 수행됨). 
        //동일한 유형에 대한 XmlSerializer의 추가 생성은 이미 생성된 DLL을 사용합니다.
        
        
        /// <summary>
        /// xml File을 클래스로 변환
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        static public object XmlToClass(string filePath, Type T)
        {
            using (var reader = new StreamReader(filePath))
            {
                XmlSerializer xs = new XmlSerializer(T);
                return xs.Deserialize(reader);
            }
        }


        /// <summary>
        /// 클래스를 xml File로 변환
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="T"></param>
        static public void ClassToXml(string filePath, object obj, Type T)
        {
            using (StreamWriter wr = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(T);
                xs.Serialize(wr, obj);
            }
        }
    }
}
