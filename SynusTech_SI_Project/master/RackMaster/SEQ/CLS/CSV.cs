using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace RackMaster.SEQ.CLS
{
    public class CSV
    {
        private string m_folderPath;
        private string m_fileName;
        private int m_rowCount;
        private List<string[]> m_data;
        private string m_exceptionMsg;
        private Exception m_exception;

        public CSV()
        {
            m_rowCount = 0;
            m_data = new List<string[]>();
            m_exceptionMsg = "";
        }
        /// <summary>
        /// CSV 파일을 저장/로드할 경로와 파일명 설정 함수
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void SetPath(string folderPath, string fileName) {
            m_folderPath = folderPath;
            m_fileName = fileName;
        }
        /// <summary>
        /// 설정된 경로의 CSV 파일을 로드하는 함수
        /// </summary>
        /// <returns></returns>
        public bool LoadCSVFile() {
            try {
                string path = m_folderPath + "\\" + m_fileName;
                StreamReader sr = new StreamReader(path);

                while (!sr.EndOfStream) {
                    string line = sr.ReadLine();

                    string[] data = line.Split(',');
                    m_rowCount = data.Length;
                    m_data.Add(data);
                }
            }catch(Exception e) {
                m_exceptionMsg = e.Message;
                m_exception = e;
                return false;
            }

            return true;
        }
        /// <summary>
        /// 로드한 파일 데이터의 특정 행/열의 값을 얻는 함수
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetData(int row, int column) {
            return m_data[column][row];
        }
        /// <summary>
        /// 로드한 파일 데이터의 총 열 개수
        /// </summary>
        /// <returns></returns>
        public int GetColumnCount() {
            return m_data.Count;
        }
        /// <summary>
        /// 로드한 파일 데이터의 총 행 개수
        /// </summary>
        /// <returns></returns>
        public int GetRowCount() {
            return m_rowCount;
        }
        /// <summary>
        /// 로드시 발생한 Exception 메시지를 얻는 함수
        /// </summary>
        /// <returns></returns>
        public string GetExceptionMessage() {
            return m_exceptionMsg;
        }
        /// <summary>
        /// 로드시 발생한 Exception 객체를 얻는 함수
        /// </summary>
        /// <returns></returns>
        public Exception GetException() {
            return m_exception;
        }
    }
}
