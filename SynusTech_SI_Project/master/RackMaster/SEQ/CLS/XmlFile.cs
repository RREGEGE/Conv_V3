using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.CLS
{
    public class XmlFile
    {
        public enum LogMessage_Xml {
            SetNodeValueFail,
            GetNodeValueFail,
            SetNodeAttributeFail,
            GetNodeListLengthFail,
        }

        private static string GetLogMessage(LogMessage_Xml logMsg) {
            switch (logMsg) {
                case LogMessage_Xml.SetNodeValueFail:
                    return "Node Value Setting Fail";

                case LogMessage_Xml.GetNodeValueFail:
                    return "Get Node Value Fail";

                case LogMessage_Xml.SetNodeAttributeFail:
                    return "Node Attribute Setting Fail";

                case LogMessage_Xml.GetNodeListLengthFail:
                    return "Get Node List Length Fail";
            }

            return "";
        }

        private XmlDocument m_xDoc;
        private string m_strPath;

        public bool m_isFileExist = false;

        public XmlFile(string path, string rootName)
        {
            m_xDoc = new XmlDocument();

            m_strPath = Path.GetFullPath(path);
            try
            {
                m_xDoc.Load(m_strPath);
                m_isFileExist = true;
            }catch(FileNotFoundException e)
            {
                XmlDeclaration xmlDecl = m_xDoc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
                m_xDoc.InsertBefore(xmlDecl, m_xDoc.DocumentElement);
                m_isFileExist = false;

                XmlNode root = m_xDoc.CreateElement(rootName);
                m_xDoc.AppendChild(root);
            }
        }
        /// <summary>
        /// 해당 경로의 xml파일이 존재하는지 반환하는 함수
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path) {
            if (File.Exists(path))
                return true;

            return false;
        }
        /// <summary>
        /// 현재까지 갱신된 데이터를 파일에 저장하는 함수
        /// </summary>
        public void Save() {
            m_xDoc.Save(m_strPath);
        }
        /// <summary>
        /// 해당 경로에 존재하는 xml 파일을 삭제하는 함수
        /// </summary>
        public void Delete() {
            File.Delete(m_strPath);
        }
        /// <summary>
        /// 해당 부모 노드에 해당 노드의 이름을 가진 자식노드가 존재하는지 판단하는 함수
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public bool isNodeExist(string parentName, string nodeName) {
            try {
                string selectedNode = $"{parentName}/{nodeName}";

                XmlNode node = m_xDoc.SelectSingleNode(selectedNode);
                if (node == null)
                    return false;
                else
                    return true;
            }catch(Exception ex) {
                return false;
            }
        }
        /// <summary>
        /// 해당 부모 노드의 존재하는 해당 노드의 값을 입력하는 함수
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="nodeName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SetNodeVal(string parentName, string nodeName, string val) {
            try {
                string selectedNode = $"{parentName}/{nodeName}";
                XmlNode node = m_xDoc.SelectSingleNode(selectedNode);

                if(node == null) {
                    XmlNode nodeParent = m_xDoc.SelectSingleNode(parentName);
                    XmlNode nodeCreate = m_xDoc.CreateElement(nodeName);
                    nodeCreate.InnerText = val;
                    nodeParent.AppendChild(nodeCreate);
                }
                else {
                    node.InnerText = val;
                }

                return true;
            }catch(Exception ex) {
                return false;
            }
        }

        public bool SetNodeVal(string parentName, string nodeName, string val, bool appendChild = false)
        {
            if (appendChild) {
                XmlNode nodeParent = m_xDoc.SelectSingleNode(parentName);
                XmlNode node = m_xDoc.CreateElement(nodeName);
                node.InnerText = val;
                nodeParent.AppendChild(node);

                return true;
            }
            else {
                try {
                    string selectedNode = parentName + "/" + nodeName;
                    XmlNode node = m_xDoc.SelectSingleNode(selectedNode);
                    node.InnerText = val;

                    return true;
                }
                catch (Exception ex) {
                    SetNodeVal(parentName, nodeName, val, true);
                    return false;
                }
            }
        }

        public void SetNodeVal(string parentName, string nodeName, string val, int index, bool appendChild = false) {
            if (appendChild) {
                try {
                    XmlNodeList nodes = m_xDoc.SelectNodes(parentName);
                    XmlNode node = m_xDoc.CreateElement(nodeName);
                    node.InnerText = val;
                    nodes[index].AppendChild(node);
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Xml.SetNodeValueFail), ex));
                }
            }
            else {
                try {
                    XmlNodeList nodes = m_xDoc.SelectNodes(parentName);
                    XmlNode node = nodes[index].SelectSingleNode(nodeName);
                    node.InnerText = val;
                }catch(Exception ex) {
                    SetNodeVal(parentName, nodeName, val, index, true);
                }
            }
        }

        public void DeleteNode(string parentName, string nodeName) {
            try {
                XmlNodeList nodes = m_xDoc.SelectNodes(parentName);

            }catch(Exception ex) {

            }
        }

        public string GetNodeVal(string nodeName)
        {
            string val = "";
            try {
                XmlNode node = m_xDoc.SelectSingleNode(nodeName);
                val = node.InnerText;
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Xml.GetNodeValueFail), ex));
            }

            return val;
        }

        public string GetNodeVal(string parentName, string nodeName, int index) {
            string val = "";
            try {
                XmlNodeList nodes = m_xDoc.SelectNodes(parentName);
                if(nodes[index].SelectSingleNode(nodeName).InnerText != null) {
                    val = nodes[index].SelectSingleNode(nodeName).InnerText;
                }
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Xml.GetNodeValueFail), ex));
            }

            return val;
        }

        public void SetNodeAttribute(string nodeName, string attrName, string val)
        {
            try
            {
                XmlNode node = m_xDoc.SelectSingleNode(nodeName);
                XmlAttributeCollection acxNode = node.Attributes;
                acxNode.GetNamedItem(attrName).Value = val;
            }catch(Exception ex)
            {
                XmlNode node = m_xDoc.SelectSingleNode(nodeName);
                XmlAttributeCollection acxNode = node.Attributes;
                XmlAttribute newAttr = m_xDoc.CreateAttribute(attrName);
                newAttr.Value = val;
                acxNode.SetNamedItem(newAttr);
            }
        }

        public void SetNodeAttribute(string nodeName, string attrName, string val, int index) {
            try {
                XmlNodeList nodes = m_xDoc.SelectNodes(nodeName);
                XmlAttribute newAttr = m_xDoc.CreateAttribute(attrName);
                newAttr.Value = val;
                nodes[index].Attributes.SetNamedItem(newAttr);
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Xml.SetNodeAttributeFail), ex));
            }
        }

        public void SetNodeAttribute(string parentName, string nodeName, string attrName, string val, int index) {
            try {
                //string sNodeName = parentName + "/" + nodeName;
                string sNodeName = $"{parentName}/{nodeName}";
                XmlNodeList nodes = m_xDoc.SelectNodes(sNodeName);
                XmlAttribute newAttr = m_xDoc.CreateAttribute(attrName);
                newAttr.Value = val;
                nodes[index].Attributes.SetNamedItem(newAttr);
            }
            catch (Exception ex) {
                SetNodeVal(parentName, nodeName, null, true);

                string sNodeName = $"{parentName}/{ nodeName}";
                XmlNodeList nodes = m_xDoc.SelectNodes(sNodeName);
                XmlAttribute newAttr = m_xDoc.CreateAttribute(attrName);
                newAttr.Value = val;
                nodes[index].Attributes.SetNamedItem(newAttr);
            }
        }

        public string GetNodeAttribute(string nodeName, string attrName)
        {
            XmlNode node = m_xDoc.SelectSingleNode(nodeName);
            XmlAttributeCollection acxNode = node.Attributes;
            return acxNode.GetNamedItem(attrName).Value;
        }

        public string GetNodeAttribute(string nodeName, string attrName, int index) {
            XmlNodeList nodes = m_xDoc.SelectNodes(nodeName);
            return nodes[index].Attributes[attrName].Value;
        }

        public int GetNodeListLength(string strNode) {
            try {
                XmlNodeList nodes = m_xDoc.SelectNodes(strNode);
                return nodes.Count;
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Utility, GetLogMessage(LogMessage_Xml.GetNodeListLengthFail), ex));
            }

            return 0;
        }
    }
}
