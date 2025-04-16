using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using static Synustech.G_Var;
using static Synustech.WMXMotion.AxisParameter;
using WMX3ApiCLR;
using Synustech;

namespace Synustech
{
    public class XMLControl
    {
        public void SaveRectanglesToXML(string strXMLPath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            using (XmlWriter wr = XmlWriter.Create(strXMLPath, settings))
            {
                wr.WriteStartDocument();

                // 루트 요소 생성
                wr.WriteStartElement("Rectangles");

                foreach (var rect in rectangles)
                {
                    // 각 Rectangle 요소 생성
                    wr.WriteStartElement("Rectangle");

                    // 각 Rectangle 속성 추가
                    wr.WriteElementString("ID", rect.ID.ToString());
                    wr.WriteElementString("X", rect.X.ToString());
                    wr.WriteElementString("Y", rect.Y.ToString());
                    wr.WriteElementString("Type", rect.TYPE);

                    // Rectangle 태그 종료
                    wr.WriteEndElement();
                }

                // 루트 태그 종료
                wr.WriteEndElement();

                wr.WriteEndDocument();
            }
        }
        public void LoadRectanglesFromXML(string strXMLPath)
        {
            if (!File.Exists(strXMLPath))
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return; // 파일이 없으면 아무 작업도 하지 않음
            }
            int id = 0;
            int x = 0;
            int y = 0;
            string type = null;
            using (XmlReader reader = XmlReader.Create(strXMLPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "Rectangle")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Rectangle")
                            {
                                break; // Rectangle 요소의 끝에 도달
                            }

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "ID":
                                        id = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "X":
                                        x = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Y":
                                        y = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Type":
                                        type = reader.ReadElementContentAsString();
                                        break;
                                }
                            }
                        }
                        CustomRectangle rect = null;
                        switch (type)
                        {
                            case "Interface":
                                rect = new InterfaceRect(x, y, id);
                                break;
                            case "Normal":
                                rect = new NormalRect(x, y, id);
                                break;
                            case "Turn":
                                rect = new TurnRect(x, y, id);
                                break;
                            case "Long_Vertical":
                                rect = new LongRect_Vertical(x, y, id);
                                break;
                            case "Long_Horizontal":
                                rect = new LongRect_Horizontal(x, y, id);
                                break;
                            //// 다른 타입이 있을 경우 추가 가능
                            default:
                                break;
                        }

                        if (rect != null)
                        {
                            rectangles.Add(rect);
                        }
                    }
                }
            }
        }
        public void SaveConveyorToXML(string strXMLPath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            using (XmlWriter wr = XmlWriter.Create(strXMLPath, settings))
            {
                wr.WriteStartDocument();

                // 루트 요소 생성
                wr.WriteStartElement("Conveyors");

                foreach (var conveyor in conveyors)
                {
                    // 각 Rectangle 요소 생성
                    wr.WriteStartElement("Conveyor");

                    // 각 Rectangle 속성 추가
                    wr.WriteElementString("ID", conveyor.ID.ToString());
                    wr.WriteElementString("Name", conveyor.name);
                    wr.WriteStartElement("Lines");
                    foreach (var line in conveyor.lines)
                    {
                        wr.WriteElementString("Line", line.ID.ToString());
                    }
                    // Rectangle 태그 종료
                    wr.WriteEndElement();
                    wr.WriteElementString("Axis", conveyor.Axis.ToString());
                    wr.WriteElementString("LoadPOS", conveyor.LoadPos.ToString());
                    wr.WriteElementString("LoadSensor", conveyor.LoadLocation.ToString());
                    wr.WriteElementString("UnloadPOS", conveyor.UnloadPos.ToString());
                    wr.WriteElementString("UnloadSensor", conveyor.UnloadLocation.ToString());
                    wr.WriteEndElement();
                }

                // 루트 태그 종료
                wr.WriteEndElement();

                wr.WriteEndDocument();
            }
        }
        public void LoadConveyorFromXML(string strXMLPath)
        {
            if(!File.Exists(strXMLPath))
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return; // 파일이 없으면 아무 작업도 하지 않음
            }
            int id = 0;
            string name = "";
            string line = "";
            double loadpos = 0;
            double unloadpos = 0;
            int loadsensor = 0;
            int unloadsensor = 0;

            List<string> Lines = new List<string>();
            int axis = 0;
            using (XmlReader reader = XmlReader.Create(strXMLPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "Conveyor")
                    {
                        Lines.Clear();
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Conveyor")
                            {
                                break; // Conveyor 요소의 끝에 도달
                            }

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "ID":
                                        id = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Name":
                                        name = reader.ReadElementContentAsString();
                                        break;
                                    case "Axis":
                                        axis = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Line":
                                        line = reader.ReadElementContentAsString();
                                        Lines.Add(line);
                                        break;
                                    case "LoadPOS":
                                        loadpos = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "LoadSensor":
                                        loadsensor = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "UnloadPOS":
                                        unloadpos = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "UnloadSensor":
                                        unloadsensor = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                }
                            }
                        }
                        var matchingConveyor = conveyors.FirstOrDefault(c => c.name == name);
                        if (matchingConveyor != null)
                        {
                            // ID와 Line 값을 업데이트
                            matchingConveyor.ID = id;
                            matchingConveyor.Axis = axis;
                            if (matchingConveyor.type == "Turn")
                            {
                                matchingConveyor.TurnAxis = axis + 1;
                            }
                            foreach (var Line in Lines)
                            {
                                var matchingLine = lines.FirstOrDefault(l => l.ID == Line);
                                matchingConveyor.lines.Add(matchingLine);

                                matchingLine.conveyors.Add(matchingConveyor);
                            }
                            matchingConveyor.LoadPos = loadpos;
                            matchingConveyor.LoadLocation = loadsensor;
                            matchingConveyor.UnloadPos = unloadpos;
                            matchingConveyor.UnloadLocation = unloadsensor;
                        }
                        else
                        {
                            Console.WriteLine($"No Conveyor found with Axis: {axis}");
                        }
                    }
                }
            }
        }
        public void SaveLineToXml(string strXMLPath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            using (XmlWriter wr = XmlWriter.Create(strXMLPath, settings))
            {
                wr.WriteStartDocument();

                // 루트 요소 생성
                wr.WriteStartElement("Lines");

                // lines 리스트에 있는 요소 개수만큼 <line></line> 추가
                foreach (var line in lines)
                {
                    wr.WriteStartElement("Line");
                    // 여기서 필요에 따라 line 속성을 추가할 수 있음
                    wr.WriteFullEndElement();
                }

                wr.WriteEndElement();  // 루트 요소 종료
                wr.WriteEndDocument();
            }
        }
        public void LoadLineFromXml(string strXMLPath)
        {
            if (!File.Exists(strXMLPath))
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return; // 파일이 없으면 아무 작업도 하지 않음
            }
            using (XmlReader reader = XmlReader.Create(strXMLPath))
            {
                Line line = null;

                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "Line")
                    {
                        // Line 개체를 만났을 때 새 Line 객체 생성
                        line = new Line();

                        while (reader.Read())
                        {
                            // Line 끝나면 탈출
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Line")
                            {
                                // 리스트에 Line 객체 추가
                                lines.Add(line);
                                break;
                            }
                        }
                    }
                }
            }
            // 결과 확인용 출력
            foreach (var line in lines)
            {
                Console.WriteLine($"ID: {line.ID}");
            }
        }

        /// <summary>
        /// Opration_Parameter File을 읽어 해당하는 Axis의 Profile을 넣어주는 역할 
        /// </summary>
        /// <param name="strXMLPath">wmx_parameters가 저장된 경로, FullPath </param>
        public void GetProfileParameter(string strXMLPath)
        {

            if (!File.Exists(strXMLPath))
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return; // 파일이 없으면 아무 작업도 하지 않음
            }

            //XmlDocument 인스턴스 생성
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(strXMLPath); // XML 파일을 로드 .. xmlRead 와 달리 문서 자체를 복사 
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message + "by_Opration_Parameter");
                return;
            }


            // AxisParameters 요소만 찾음
            XmlNodeList axisNodes = xmlDoc.SelectNodes("/Parameters/AxisParameters");


            foreach (XmlNode axisNode in axisNodes)
            {
                // 원하는 Axis 값과 비교, 불일치 하면 재실행 
                int axisValue = int.Parse(axisNode.Attributes["Axis"].Value);
                //if (0 != axisValue)
                //{
                //    continue;
                //}

                // AxisParameter를 임시저장할 인스턴스 생성
                //OprationParameter oprationParameter = new OprationParameter();

                //내부 태그 찾기
                foreach (XmlNode paramNode in axisNode.ChildNodes)
                {
                    //내부의 요소들을 파싱
                    foreach (XmlElement under_paramNode in paramNode.ChildNodes)
                    {
                        switch (under_paramNode.Name)
                        {
                            case "Velocity":
                                m_WMXMotion.m_AxisProfile[axisValue].m_velocity = double.Parse(under_paramNode.InnerText);
                                break;

                            case "EndVelocity":
                                m_WMXMotion.m_AxisProfile[axisValue].m_endvelocity = double.Parse(under_paramNode.InnerText);
                                break;

                            case "Acceleration":
                                m_WMXMotion.m_AxisProfile[axisValue].m_acc = double.Parse(under_paramNode.InnerText);
                                break;

                            case "Deceleration":
                                m_WMXMotion.m_AxisProfile[axisValue].m_dec = double.Parse(under_paramNode.InnerText);
                                break;

                            case "JerkRatio":
                                m_WMXMotion.m_AxisProfile[axisValue].m_jerkRatio = double.Parse(under_paramNode.InnerText);
                                break;

                            case "Destination":
                                m_WMXMotion.m_AxisProfile[axisValue].m_dest = double.Parse(under_paramNode.InnerText);
                                break;

                            case "Axis":
                                m_WMXMotion.m_AxisProfile[axisValue].m_axis = int.Parse(under_paramNode.InnerText);
                                Console.WriteLine(m_WMXMotion.m_AxisProfile[axisValue].m_axis);
                                break;

                            case "ProfileType":
                                m_WMXMotion.m_AxisProfile[axisValue].m_profileType = (WMXParam.m_profileType)Enum.Parse(typeof(ProfileType), under_paramNode.InnerText);
                                break;

                                //default:
                                //    throw new Exception($"Unknown element: {reader.Name}");
                        }
                    }

                }
                //w_motion.m_OprationParameter[conveyor.Axis + i].SetOprationParam(oprationParameter);

            }
            // 결과 확인용 출력 
            //Console.WriteLine($"Axis: {conveyor.Axis + i} {w_motion.m_OprationParameter[conveyor.Axis + i].m_Cnv_Auto_Speed} 설정 완료 ");
            Console.WriteLine($"Setting {m_WMXMotion.m_AxisProfile[0].m_velocity} 설정 완료 ");

            return;
        }






        /// <summary>
        /// Opration_Parameter File을 읽어 해당하는 Axis의 Profile을 넣어주는 역할 
        /// </summary>
        /// <param name="strXMLPath">wmx_parameters가 저장된 경로, FullPath </param>
        public void SetProfileParameter(string strXMLPath)
        {

            if (!File.Exists(strXMLPath))
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return; // 파일이 없으면 아무 작업도 하지 않음
            }

            //XmlDocument 인스턴스 생성
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(strXMLPath); // XML 파일을 로드 .. xmlRead 와 달리 문서 자체를 복사 
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message + "by_Opration_Parameter");
                return;
            }


            // AxisParameters 요소만 찾음
            XmlNodeList axisNodes = xmlDoc.SelectNodes("/Parameters/AxisParameters");


            //for (int i = 0; i < 30; ++i)
            //{
            foreach (XmlNode axisNode in axisNodes)
            {

                int axisValue = int.Parse(axisNode.Attributes["Axis"].Value);
                AxisProfile axisProfile = m_WMXMotion.m_AxisProfile[axisValue]; //모든 Axis 값을 참조할 인스턴스 생성


                //// 원하는 Axis 값과 비교, 불일치 하면 재실행 
                //int axisValue = int.Parse(axisNode.Attributes["Axis"].Value);
                //if (0 != axisValue)
                //{
                //    break;
                //}

                // AxisParameter를 임시저장할 인스턴스 생성
                //OprationParameter oprationParameter = new OprationParameter();

                //내부 태그 찾기
                foreach (XmlNode paramNode in axisNode.ChildNodes)
                {
                    //내부의 요소들을 파싱
                    foreach (XmlElement under_paramNode in paramNode.ChildNodes)
                    {
                        switch (under_paramNode.Name)
                        {
                            case "Velocity":
                                under_paramNode.InnerText = axisProfile.m_velocity.ToString();
                                break;

                            case "EndVelocity":
                                under_paramNode.InnerText = axisProfile.m_endvelocity.ToString();
                                break;

                            case "Acceleration":
                                under_paramNode.InnerText = axisProfile.m_acc.ToString();
                                break;

                            case "Deceleration":
                                under_paramNode.InnerText = axisProfile.m_dec.ToString();
                                break;

                            case "JerkRatio":
                                under_paramNode.InnerText = axisProfile.m_jerkRatio.ToString();
                                break;

                            case "Destination":
                                under_paramNode.InnerText = axisProfile.m_dest.ToString();
                                break;

                            case "Axis":
                                under_paramNode.InnerText = axisProfile.m_axis.ToString();
                                break;

                            case "ProfileType":
                                under_paramNode.InnerText = axisProfile.m_profileType.ToString();
                                break;

                                //default:
                                //    throw new Exception($"Unknown element: {reader.Name}");
                        }
                    }
                }

            }

            //}
            // 결과 확인용 출력 

            xmlDoc.Save(strXMLPath);
            Console.WriteLine("XML 파일의 값을 성공적으로 수정하였습니다.");

            return;
        }

    }
}
