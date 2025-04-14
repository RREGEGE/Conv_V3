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
using static Synustech.AlarmList;
using System.Diagnostics;
using System.Windows.Forms;

namespace Synustech
{
    public class XMLControl
    {
        public static XmlDocument xmlDoc = new XmlDocument();
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
                    wr.WriteElementString("X", rect.x.ToString());
                    wr.WriteElementString("Y", rect.y.ToString());
                    wr.WriteElementString("Type", rect.type);

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
                    wr.WriteElementString("Type", conveyor.type);
                    wr.WriteElementString("Interface", conveyor.isInterface.ToString());
                    wr.WriteStartElement("Lines");
                    foreach (var line in conveyor.lines)
                    {
                        wr.WriteElementString("Line", line.ID.ToString());
                    }
                    // Rectangle 태그 종료
                    wr.WriteEndElement();
                    wr.WriteElementString("Axis", conveyor.axis.ToString());
                    wr.WriteElementString("LoadPOS", conveyor.loadPOS.ToString());
                    wr.WriteElementString("LoadSensor", conveyor.loadLocation.ToString());
                    wr.WriteElementString("UnloadPOS", conveyor.unloadPOS.ToString());
                    wr.WriteElementString("UnloadSensor", conveyor.unloadLocation.ToString());
                    wr.WriteElementString("Velocity", conveyor.autoVelocity.ToString());
                    wr.WriteElementString("Acceleration", conveyor.autoAcc.ToString());
                    wr.WriteElementString("Deceleration", conveyor.autoDec.ToString());
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
            string type = "";
            string name = "";
            string line = "";
            bool isInterface = false;
            double loadPOS = 0;
            double unLoadPOS = 0;
            int loadSensor = 0;
            int unLoadSensor = 0;
            double velocity = 0;
            double acc = 0;
            double dec = 0;

            List<string> lines = new List<string>();
            int axis = 0;
            using (XmlReader reader = XmlReader.Create(strXMLPath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "Conveyor")
                    {
                        lines.Clear();
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
                                    case "Type":
                                        type = reader.ReadElementContentAsString();
                                        break;
                                    case "Interface":
                                        isInterface = bool.Parse(reader.ReadElementContentAsString()); 
                                        break;
                                    case "Line":
                                        line = reader.ReadElementContentAsString();
                                        lines.Add(line);
                                        break;
                                    case "LoadPOS":
                                        loadPOS = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "LoadSensor":
                                        loadSensor = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "UnloadPOS":
                                        unLoadPOS = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "UnloadSensor":
                                        unLoadSensor = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Velocity":
                                        velocity = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Acceleration":
                                        acc = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "Deceleration":
                                        dec = double.Parse(reader.ReadElementContentAsString());
                                        break;
                                }
                            }
                        }
                        Conveyor conveyor = null;
                        if(type == "Normal")
                        {
                            if (!isInterface)
                            { 
                                conveyor = new NormalConv(id, axis, false); 
                            }
                            else
                            {
                                conveyor = new NormalConv(id, axis, true);
                            }
                        }
                        else if(type == "Turn")
                        {
                            if (!isInterface)
                            {
                                conveyor = new TurnConv(id, axis, false);
                            }
                            else
                            {
                                conveyor = new TurnConv(id, axis, true);
                            }
                            conveyor.turnAxis = axis+ 1;
                        }
                        else if( type == "Long")
                        {
                            if (!isInterface)
                            {
                                conveyor = new LongConv(id, axis, false);
                            }
                            else
                            {
                                conveyor = new LongConv(id, axis, true);
                            }
                        }
                        if(conveyor != null)
                        {
                            conveyor.loadPOS = loadPOS;
                            conveyor.loadLocation = loadSensor;
                            conveyor.unloadPOS = unLoadPOS;
                            conveyor.unloadLocation = unLoadSensor;
                            conveyor.autoVelocity = velocity;
                            Console.WriteLine(conveyor.autoVelocity);
                            conveyor.autoAcc = acc;
                            conveyor.autoDec = dec;
                            conveyors.Add(conveyor);
                            foreach (var Line in lines)
                            {
                                var matchingLine = G_Var.lines.FirstOrDefault(l => l.ID == Line);
                                if (matchingLine != null)
                                {
                                    conveyor.lines.Add(matchingLine);

                                    matchingLine.conveyors.Add(conveyor);
                                }
                                else
                                {
                                    MessageBox.Show("The specified line does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No Conveyor found with Axis: {axis}");
                        }
                        Console.WriteLine(conveyor.ID + "----" + conveyor.isInterface);
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
                    wr.WriteElementString("ID", line.ID.ToString());
                    wr.WriteElementString("StartConv", line.startConvID.ToString());
                    wr.WriteElementString("EndID", line.endConvID.ToString());
                    wr.WriteElementString("ConvEA", line.convEA.ToString());
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
            string id = "";
            int startConv = 0;
            int endConv = 0;
            int convEA = 0;
            using (XmlReader reader = XmlReader.Create(strXMLPath))
            {
                Line line = null;

                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name == "Line")
                    {
                        while (reader.Read())
                        {
                            // Line 끝나면 탈출
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Line")
                            {
                                break;
                            }
                            if(reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "ID":
                                        id = reader.ReadElementContentAsString();
                                        break;
                                    case "StartConv":
                                        startConv = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "EndID":
                                        endConv = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                    case "ConvEA":
                                        convEA = int.Parse(reader.ReadElementContentAsString());
                                        break;
                                }
                            }
                        }
                        line = new Line(id);
                        line.startConvID = startConv;
                        line.endConvID = endConv;
                        line.convEA = convEA;

                        lines.Add(line);
                    }
                }
            }
            // 결과 확인용 출력
            foreach (var line in lines)
            {
                Console.WriteLine($"ID: {line.ID}");
            }
            del_lineAdd?.Invoke(G_Var.lines);
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
                    foreach (XmlElement underParamNode in paramNode.ChildNodes)
                    {
                        switch (underParamNode.Name)
                        {
                            case "Velocity":
                                w_motion.m_AxisProfile[axisValue].m_velocity = double.Parse(underParamNode.InnerText);
                                break;

                            case "EndVelocity":
                                w_motion.m_AxisProfile[axisValue].m_endvelocity = double.Parse(underParamNode.InnerText);
                                break;

                            case "Acceleration":
                                w_motion.m_AxisProfile[axisValue].m_acc = double.Parse(underParamNode.InnerText);
                                break;

                            case "Deceleration":
                                w_motion.m_AxisProfile[axisValue].m_dec = double.Parse(underParamNode.InnerText);
                                break;

                            case "JerkRatio":
                                w_motion.m_AxisProfile[axisValue].m_jerkRatio = double.Parse(underParamNode.InnerText);
                                break;

                            case "Destination":
                                w_motion.m_AxisProfile[axisValue].m_dest = double.Parse(underParamNode.InnerText);
                                break;

                            case "Axis":
                                w_motion.m_AxisProfile[axisValue].m_axis = int.Parse(underParamNode.InnerText);
                                break;

                            case "ProfileType":
                                w_motion.m_AxisProfile[axisValue].m_profileType = (WMXParam.m_profileType)Enum.Parse(typeof(ProfileType), underParamNode.InnerText);
                                break;

                            default:
                                break;
                        }
                    }

                }

            }
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
                AxisProfile axisProfile = w_motion.m_AxisProfile[axisValue]; //모든 Axis 값을 참조할 인스턴스 생성


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
                    foreach (XmlElement underParamNode in paramNode.ChildNodes)
                    {
                        switch (underParamNode.Name)
                        {
                            case "Velocity":
                                underParamNode.InnerText = axisProfile.m_velocity.ToString();
                                break;

                            case "EndVelocity":
                                underParamNode.InnerText = axisProfile.m_endvelocity.ToString();
                                break;

                            case "Acceleration":
                                underParamNode.InnerText = axisProfile.m_acc.ToString();
                                break;

                            case "Deceleration":
                                underParamNode.InnerText = axisProfile.m_dec.ToString();
                                break;

                            case "JerkRatio":
                                underParamNode.InnerText = axisProfile.m_jerkRatio.ToString();
                                break;

                            case "Destination":
                                underParamNode.InnerText = axisProfile.m_dest.ToString();
                                break;

                            case "Axis":
                                underParamNode.InnerText = axisProfile.m_axis.ToString();
                                break;

                            case "ProfileType":
                                underParamNode.InnerText = axisProfile.m_profileType.ToString();
                                break;

                            default:
                                break;
                        }
                    }
                }

            }
            xmlDoc.Save(strXMLPath);
            Console.WriteLine("XML 파일의 값을 성공적으로 수정하였습니다.");

            return;
        }
        public async Task GetList(string strPath)
        {

                if (!File.Exists(strPath))
                {
                    Console.WriteLine("파일이 존재하지 않습니다.");
                    return; // 파일이 없으면 아무 작업도 하지 않음
                }

                xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(strPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/AlarmListParam/AlarmCode");
                foreach (XmlNode alarmListParam in xmlNodeList)
                {
                    AlarmListParam gettingFromXML = new AlarmListParam(); //  CodeListParam 생성

                    //Get_form_XML.AlarmCode = (ConveyorAlarm)Enum.Parse(typeof(ConveyorAlarm), alarmListParam.Attributes["Code"].Value);
                    gettingFromXML.code = int.Parse(alarmListParam.Attributes["Code"].Value);

                    foreach (XmlNode paramElement in alarmListParam.ChildNodes)
                    {

                        switch (paramElement.Name)
                        {
                            case "AlarmComment":
                                gettingFromXML.comment = paramElement.InnerText;
                                break;
                            case "AlarmLevel":
                                gettingFromXML.level = (AlarmLevel)Enum.Parse(typeof(AlarmLevel), paramElement.InnerText);
                                break;
                            case "AlarmType":
                                gettingFromXML.EQ = paramElement.InnerText;
                                break;
                        }

                    }
                    alarmCodes.Add(gettingFromXML);
                }
        }
    }
}
