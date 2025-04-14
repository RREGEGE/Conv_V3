using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

static public class SynusLangPack
{
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileSection(string section, byte[] Keys, int nSize, string filePath);
    [DllImport("kernel32.dll")]
    private static extern uint GetPrivateProfileSectionNames(byte[] sections, uint size, String filePath);

    static private StringBuilder Read(string section, string key, string path)
    {
        StringBuilder str = new StringBuilder(409600);
        //GetPrivateProfileString(section, key, "", str, str.Capacity, path);
        GetPrivateProfileString(section, key, "", str, str.Capacity, path);
        return str;
    }

    static private string[] GetSectionNames(string _filePath)
    {
        byte[] bytes = new byte[409600];
        uint Flag = GetPrivateProfileSectionNames(bytes, 409600, _filePath);
        return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
    }
    static private string[] GetEntryNames(string section, string _filePath)
    {
        byte[] bytes = new byte[409600];
        GetPrivateProfileSection(section, bytes, 409600, _filePath);

        return Encoding.Default.GetString(bytes).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public enum LangPackErrorCode
    {
        None,
        FileNotExist,
        NoHaveSection,
        Exception,
        MultiComma,
        KeyDuplicate
    }
    public enum LanguageType
    {
        Korean,
        English,
        Chinese,
        Japanese
    }

    static private LanguageType eLanguageType = LanguageType.Korean;
    static private LangPackErrorCode eLangPackErrorCode = LangPackErrorCode.None;
    static private string LanguagePackVersion = string.Empty;
    static private string LanuageErrorMsg = string.Empty;
    static public Dictionary<string, string[]> StrDic = new Dictionary<string, string[]>();


    static public bool LoadFile(string path, string fileName)
    {
        try
        {
            eLangPackErrorCode = LangPackErrorCode.None;
            LanguagePackVersion = string.Empty;
            LanuageErrorMsg = string.Empty;
            StrDic.Clear();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + "\\" + fileName;

            if (File.Exists(filePath))
            {
                string[] Sections = GetSectionNames(filePath);

                if (Sections == null)
                {
                    LanuageErrorMsg = "No Have section in file";
                    eLangPackErrorCode = LangPackErrorCode.NoHaveSection;
                    return false;
                }

                foreach (var Section in Sections)
                {
                    string[] Keys = GetEntryNames(Section, filePath);

                    if (Keys == null)
                        continue;

                    if (Section.ToLower() == "versioninfo")
                    {
                        LanguagePackVersion = Read(Section, "ver", filePath).ToString();
                    }
                    else
                    {
                        if (!StrDic.ContainsKey(Section.ToLower()))
                            StrDic.Add(Section.ToLower(), new string[4]);
                        else
                            StrDic[Section.ToLower()] = new string[4];

                        StrDic[Section.ToLower()][(int)LanguageType.Korean] = Read(Section, "kr", filePath).ToString();
                        StrDic[Section.ToLower()][(int)LanguageType.English] = Read(Section, "us", filePath).ToString();
                        StrDic[Section.ToLower()][(int)LanguageType.Chinese] = Read(Section, "cn", filePath).ToString();
                        StrDic[Section.ToLower()][(int)LanguageType.Japanese] = Read(Section, "jp", filePath).ToString();
                    }
                }

                return true;
            }
            else
            {
                LanuageErrorMsg = $"File Not Exist";
                eLangPackErrorCode = LangPackErrorCode.FileNotExist;
                return false;
            }
        }
        catch(Exception ex) 
        {
            LanuageErrorMsg = ex.Message;
            eLangPackErrorCode = LangPackErrorCode.Exception;
            return false; 
        }
    }
    static public LangPackErrorCode GetErrorCode()
    {
        return eLangPackErrorCode;
    }
    static public string GetLanguagePackVersion()
    {
        return LanguagePackVersion;
    }
    static public string GetErrorMessage()
    {
        return LanuageErrorMsg;
    }

    static public void SetLanguageType(LanguageType eLangType)
    {
        eLanguageType = eLangType;
    }

    static public string GetLanguage(string key)
    {
        key = key.ToLower();
        if (StrDic.ContainsKey(key) && (int)eLanguageType < StrDic[key].Length)
            return StrDic[key][(int)eLanguageType];
        else
            return string.Empty;
    }
}


