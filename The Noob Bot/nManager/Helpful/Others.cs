﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using nManager.Wow;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Helpful
{
    using System.Security.AccessControl;

    public static class Others
    {
        /// <summary>
        /// Text To Utf8.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToUtf8(string text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    return text;
                }

                byte[] bytes = Encoding.Default.GetBytes(text);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToUtf8(string text): " + exception);
            }
            return "";
        }

        public static bool ToBoolean(string value)
        {
            bool res;
            return Boolean.TryParse(value, out res) && res;
        }

        public static int ToInt32(string value)
        {
            int res;
            return Int32.TryParse(value.Trim(), out res) ? res : 0;
        }

        public static uint ToUInt32(string value)
        {
            uint res;
            return UInt32.TryParse(value.Trim(), out res) ? res : 0;
        }

        public static Int64 ToInt64(string value)
        {
            long res;
            return Int64.TryParse(value.Trim(), out res) ? res : 0;
        }

        public static UInt64 ToUInt64(string value)
        {
            ulong res;
            return UInt64.TryParse(value.Trim(), out res) ? res : 0;
        }

        public static float ToSingle(string value)
        {
            float res;
            return Single.TryParse(value.Trim(), out res) ? res : 0;
        }

        public static char ToChar(string value)
        {
            char res;
            return Char.TryParse(value.Trim(), out res) ? res : '\u0000';
        }

        public static string[] TextToArrayByLine(string text)
        {
            try
            {
                List<string> ret = new List<string>();
                string[] split = text.Split(Environment.NewLine.ToCharArray());

                foreach (string s in split)
                {
                    if (s.Trim() != "")
                        ret.Add(s);
                }
                return ret.ToArray();
            }
            catch (Exception exception)
            {
                Logging.WriteError("TextToArrayByLine(string text): " + exception);
                return new string[0];
            }
        }

        public static string ArrayToTextByLine(string[] array)
        {
            try
            {
                string ret = "";
                foreach (string l in array)
                {
                    ret += l + Environment.NewLine;
                }
                return ret;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ArrayToTextByLine(string[] array): " + exception);
                return "";
            }
        }

        /// <summary>
        /// Open Web Browser
        /// </summary>
        /// <param name="urlOrPath"></param>
        /// <returns></returns>
        public static void OpenWebBrowserOrApplication(string urlOrPath)
        {
            try
            {
                Process p =
                    new Process();
                ProcessStartInfo pi =
                    new ProcessStartInfo
                    {
                        FileName = urlOrPath
                    };
                p.StartInfo = pi;
                p.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("OpenWebBrowser(string url): " + exception);
            }
        }

        private static readonly UTF8Encoding Utf8 = new UTF8Encoding();

        public static string ToUtf8(byte[] bytes)
        {
            try
            {
                string s = Utf8.GetString(bytes, 0, bytes.Length);

                if (s.IndexOf("\0", StringComparison.Ordinal) != -1)
                    s = s.Remove(s.IndexOf("\0", StringComparison.Ordinal),
                        s.Length - s.IndexOf("\0", StringComparison.Ordinal));

                return s;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToUtf8(byte[] bytes): " + exception);
            }
            return "";
        }

        public static int HardDriveID()
        {
            try
            {
                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                dsk.Get();
                string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                return volumeSerial.Sum(c => Convert.ToInt32(c));
            }
            catch (Exception e)
            {
                Logging.WriteError("HardDriveID(): " + e);
                return 0;
            }
        }

        /// <summary>
        /// Return the MD5 checksun of the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMd5CheckSum(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileStream st = null;
                    try
                    {
                        MD5CryptoServiceProvider check = new MD5CryptoServiceProvider();
                        st = File.Open(filePath, FileMode.Open, FileAccess.Read);
                        byte[] somme = check.ComputeHash(st);
                        string ret = "";
                        foreach (byte a in somme)
                        {
                            if ((a < 16))
                            {
                                ret += "0" + a.ToString("X");
                            }
                            else
                            {
                                ret += a.ToString("X");
                            }
                        }
                        return ret;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetFileMd5CheckSum(string filePath)#1: " + exception);
                    }
                    finally
                    {
                        if (st != null) st.Close();
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFileMd5CheckSum(string filePath)#2: " + exception);
            }
            return "";
        }

        public static string DelSpecialChar(string stringSpecialChar)
        {
            try
            {
                List<string> l = new List<string>();
                foreach (char c in stringSpecialChar)
                {
                    if (c < 65 || c > 122 && (c > 90 || c < 97))
                    {
                        // 
                    }
                    else
                    {
                        l.Add(Convert.ToString(c));
                    }
                }
                return String.Concat(l);
            }
            catch (Exception e)
            {
                Logging.WriteError("DelSpecialChar(string stringSpecialChar): " + e);
                return "";
            }
        }

        public static Dictionary<string, long> LUAVariableToDestruct = new Dictionary<string, long>();

        public static void LUAGlobalVarDestructor()
        {
            if (!Usefuls.InGame || Usefuls.IsLoading)
                return;
            string toExec = "";
            if (LUAVariableToDestruct.Count <= 0) return;
            Dictionary<string, long> LUAVariableToDestructLater = new Dictionary<string, long>();
            lock (LUAVariableToDestruct)
            {
                foreach (KeyValuePair<string, long> lua in LUAVariableToDestruct)
                {
                    if (!Regex.IsMatch(lua.Key, @"^[a-zA-Z]+$"))
                        continue;
                    if (lua.Value + 500 < Environment.TickCount)
                        toExec = toExec + lua.Key + " = nil; ";
                    else
                        LUAVariableToDestructLater.Add(lua.Key, lua.Value);
                }
                LUAVariableToDestruct.Clear();
                LUAVariableToDestruct = LUAVariableToDestructLater;
            }
            if (String.IsNullOrWhiteSpace(toExec)) return;
            Lua.LuaDoString(toExec);
        }

        public static void SafeSleep(int sleepMs)
        {
            Memory.WowMemory.GameFrameUnLock();
            Thread.Sleep(sleepMs);
        }


        // https://msdn.microsoft.com/en-us/library/system.io.file.setaccesscontrol(v=vs.110).aspx
        // Adds an ACL entry on the specified file for the specified account.
        public static void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }

        public static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }

        public static string GetRandomString(int maxSize)
        {
            try
            {
                char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(maxSize);
                foreach (byte b in data)
                {
                    result.Append(chars[b%(chars.Length - 1)]);
                }
                lock (LUAVariableToDestruct)
                {
                    if (!LUAVariableToDestruct.ContainsKey(result.ToString()))
                        LUAVariableToDestruct.Add(result.ToString(), Environment.TickCount);
                    else
                        LUAVariableToDestruct[result.ToString()] = Environment.TickCount;
                }
                return result.ToString();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetRandomString(int maxSize): " + e);
            }
            return "abcdef";
        }

        /// <summary>
        /// Shut Down the Computer.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ShutDownPc()
        {
            try
            {
                Process proc = new Process {StartInfo = {FileName = "shutdown.exe", Arguments = " -s -f"}};
                proc.Start();
                proc.Close();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShutDownPc(): " + exception);
            }
        }

        /// <summary>
        /// Return Random number.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int Random(int from, int to)
        {
            try
            {
                Random r = new Random(unchecked((int) DateTime.Now.Ticks));
                return r.Next(@from, to);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Random(int from, int to): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// int Seconde to string HH:MM:SS.
        /// </summary>
        /// <param name ="sec"></param>
        /// <returns></returns>
        public static string SecToHour(int sec)
        {
            try
            {
                string houre = (sec/3600) + "H";
                sec = sec - ((sec/3600)*3600);

                if ((sec/60) < 10)
                    houre = houre + "0" + (sec/60) + "M";
                else
                    houre = houre + (sec/60) + "M";

                sec = sec - ((sec/60)*60);

                if (sec < 10)
                    houre = houre + "0" + sec + "";
                else
                    houre = houre + sec + "";

                return houre;
            }
            catch (Exception e)
            {
                Logging.WriteError("SecToHour(int sec): " + e);
                return "00H00M00";
            }
        }

        /// <summary>
        /// Read file and return array.
        /// </summary>
        /// <param name ="path"></param>
        /// <returns></returns>
        public static string[] ReadFileAllLines(string path)
        {
            try
            {
                StringCollection coll = new StringCollection();

                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        coll.Add(line);
                        if (i > 30)
                        {
                            Application.DoEvents();
                            i = 0;
                        }
                        i++;
                    }
                }


                string[] lines = new string[coll.Count];
                coll.CopyTo(lines, 0);
                return lines;
            }
            catch (Exception e)
            {
                Logging.WriteError("ReadFileAllLines(string path): " + e);
                return new string[0];
            }
        }

        /// <summary>
        /// Time on milisec.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static int Times
        {
            get { return Environment.TickCount; }
        }

        /// <summary>
        /// Time on sec.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static int TimesSec
        {
            get { return Environment.TickCount/1000; }
        }

        private static string _httpUrl = "";
        private static string _fileDest = "";
        private static bool _downloadFinish;
        private static readonly object LockerDownload = new object();

        /// <summary>
        /// Download file from http address, return true if sucess.
        /// </summary>
        /// <param name ="httpUrl"></param>
        /// <param name ="fileDest"></param>
        /// <returns></returns>
        public static bool DownloadFile(string httpUrl, string fileDest)
        {
            try
            {
                lock (LockerDownload)
                {
                    _httpUrl = httpUrl;
                    _fileDest = fileDest;

                    Thread checkUpdateThreadLaunch = new Thread(DownloadThread) {Name = "DownloadFile"};
                    checkUpdateThreadLaunch.Start();

                    Thread.Sleep(200);

                    while (_downloadFinish)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
                    if (ExistFile(fileDest))
                        return true;
                    return false;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownloadFile(string httpUrl, string fileDest): " + exception);
                _downloadFinish = false;
                return false;
            }
        }

        private static void DownloadThread()
        {
            try
            {
                _downloadFinish = true;
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(_httpUrl, _fileDest);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("DownloadThread()#1: " + exception);
                }
                _downloadFinish = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownloadThread()#2: " + exception);
            }
        }

        /// <summary>
        /// Return a list of the file.
        /// </summary>
        /// <param name ="pathDirectory"></param>
        /// <param name ="searchPattern"></param>
        /// <returns></returns>
        public static List<string> GetFilesDirectory(string pathDirectory, string searchPattern = "")
        {
            string path = "";
            if (!pathDirectory.Contains(":"))
                path = Application.StartupPath;
            if (!Directory.Exists(path + pathDirectory))
                return new List<string>();
            return Directory.GetFiles(path + pathDirectory, searchPattern).Select(subfolder =>
            {
                string name = Path.GetFileName(subfolder);
                return name != null ? name.ToString(CultureInfo.InvariantCulture) : null;
            }).ToList();
        }

        /// <summary>
        /// Return the code source of the page. Sample: GetRequest("http://www.google.com/index.php", "a=5&amp;b=10" )
        /// </summary>
        /// <param name ="url"></param>
        /// <param name ="data"></param>
        /// <returns></returns>
        public static string GetRequest(string url, string data)
        {
            HttpWebResponse httpWResponse = null;
            StreamReader sr = null;
            string result;
            try
            {
                if (!String.IsNullOrWhiteSpace(data))
                    url = url + "?" + data;
                HttpWebRequest httpWRequest = (HttpWebRequest) WebRequest.Create(url);
                httpWRequest.UserAgent = "TheNoobBot";
                httpWResponse = (HttpWebResponse) httpWRequest.GetResponse();
                sr = new StreamReader(httpWResponse.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                result = sr.ReadToEnd();
            }

            catch (Exception ex)
            {
                Logging.WriteError("GetRequest(string url, string data): " + ex);
                result = "";
            }

            finally
            {
                if (httpWResponse != null) httpWResponse.Close();
                if (sr != null) sr.Close();
            }

            return result;
        }

        /// <summary>
        /// Return the code source of the page. Sample: PostRequest("http://www.google.com/index.php", "a=5&amp;b=10" )
        /// </summary>
        /// <param name ="url"></param>
        /// <param name ="parameters"></param>
        /// <returns></returns>
        public static string PostRequest(string url, string parameters)
        {
            try
            {
                // parameters: name1=value1&name2=value2	
                WebRequest webRequest = WebRequest.Create(url);
                //string ProxyString = 
                //   System.Configuration.ConfigurationManager.AppSettings
                //   [GetConfigKey("proxy")];
                //webRequest.Proxy = new WebProxy (ProxyString, true);
                //Commenting out above required change to App.Config
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                ((HttpWebRequest) webRequest).UserAgent = "TheNoobBot";
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                Stream os = null;
                try
                {
                    // send the Post
                    webRequest.ContentLength = bytes.Length; //Count bytes to send
                    os = webRequest.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length); //Send it
                }
                catch (WebException ex)
                {
                    Logging.WriteError("PostRequest(string url, string parameters)#1: " + ex);
                }
                finally
                {
                    if (os != null)
                    {
                        os.Close();
                    }
                }

                try
                {
                    // get the response
                    WebResponse webResponse = webRequest.GetResponse();
                    StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                    string name = sr.ReadToEnd().Trim();
                    return name;
                }
                catch (WebException ex)
                {
                    Logging.WriteError("PostRequest(string url, string parameters)#2: " + ex.Message);
                    MessageBox.Show(ex.Message, "HttpPost: Response error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PostRequest(string url, string parameters)#3: " + exception);
            }
            return null;
        }

        /// <summary>
        /// Return the path of bot directory.
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetCurrentDirectory
        {
            get { return Application.StartupPath; }
        }

        /// <summary>
        /// Return MD5.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncrypterMD5(string value)
        {
            try
            {
                byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(value));

                StringBuilder hashedString = new StringBuilder();

                for (int i = 0; i < data.Length; i++)

                    hashedString.Append(data[i].ToString("x2"));

                return hashedString.ToString();
            }
            catch (Exception exception)
            {
                Logging.WriteError("EncrypterMD5(string value): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Open et dialog box Open File and return a path file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public static string DialogBoxOpenFile(string path, string typeFile)
        {
            try
            {
                OpenFileDialog chooseFile = new OpenFileDialog {InitialDirectory = path, Filter = typeFile};
                chooseFile.ShowDialog();
                return chooseFile.FileName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception);
            }
            return "";
        }

        public static string[] DialogBoxOpenFileMultiselect(string path, string typeFile)
        {
            try
            {
                OpenFileDialog chooseFile = new OpenFileDialog {InitialDirectory = path, Filter = typeFile, Multiselect = true};
                chooseFile.ShowDialog();
                return chooseFile.FileNames;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception);
            }
            return new string[0];
        }

        /// <summary>
        /// Open et dialog box Save File and return a path file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public static string DialogBoxSaveFile(string path, string typeFile)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog {InitialDirectory = path, Filter = typeFile};
                saveFile.ShowDialog();
                return saveFile.FileName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxSaveFile(string path, string typeFile): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Gets if Visual Studio 2013 or its redistribuable package is installed.
        /// </summary>
        public static void GetVisualStudioRedistribuable2013()
        {
            try
            {
                if (File.Exists(Environment.SystemDirectory + "\\mfc120.dll"))
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetVisualStudioRedistribuable2013() #1: " + exception);
            }

            try
            {
                DialogResult resulMb = MessageBox.Show(Translate.Get(Translate.Id.VisualStudioRedistribuablePackages), @"Visual Studio 2013 Redistribuable Package " + Translate.Get(Translate.Id.Required),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (resulMb == DialogResult.Yes)
                {
                    Process.Start("http://www.microsoft.com/download/details.aspx?id=40784");
                }
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetVisualStudioRedistribuable2013() #2: " + exception);
            }
        }

        /// <summary>
        /// Return true if File exist.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool ExistFile(string strPath)
        {
            try
            {
                return File.Exists(strPath);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExistFile(string strPath): " + exception);
            }
            return false;
        }

        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static void DeleteFile(string strPath)
        {
            try
            {
                File.Delete(strPath);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeleteFile(string strPath): " + exception);
            }
        }

        /// <summary>
        /// Return a string text ecncrypted.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncryptString(string text)
        {
            try
            {
                string chaineEncrypt = "";
                string chaine = text;
                byte[] test = Encoding.UTF8.GetBytes(chaine);
                int key = HardDriveID();
                for (int i = 0; i <= test.Length - 1; i++)
                {
                    if (chaineEncrypt != "")
                    {
                        chaineEncrypt = chaineEncrypt + "-";
                    }
                    chaineEncrypt = chaineEncrypt + (test[i] + key + 15);
                }
                return chaineEncrypt;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StringToEncryptString(string text): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Return a string text decrypted.
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public static string DecryptString(string encryptedString)
        {
            try
            {
                string[] texte2 = encryptedString.Split('-');
                List<byte> listBytes = new List<byte>();
                int key = HardDriveID();

                for (int i = 0; i <= texte2.Length - 1; i++)
                {
                    listBytes.Add(Convert.ToByte(ToInt32(texte2[i]) - key - 15));
                }
                byte[] arrayBytes = listBytes.ToArray();
                return Encoding.UTF8.GetString(arrayBytes, 0, arrayBytes.Length);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EncryptStringToString(string encryptText): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Read file, Return a string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="writeNewLine"> </param>
        /// <returns></returns>
        public static string ReadFile(string filePath, bool writeNewLine = false)
        {
            try
            {
                StreamReader monStreamReader = new StreamReader(filePath);
                string ligne = monStreamReader.ReadLine();
                string returnText = "";

                while (ligne != null)
                {
                    returnText = returnText + ligne;
                    if (writeNewLine)
                        returnText += Environment.NewLine;
                    ligne = monStreamReader.ReadLine();
                    Application.DoEvents();
                    Thread.Sleep(1);
                }

                monStreamReader.Close();
                return returnText;
            }
            catch (Exception e)
            {
                Logging.WriteError("ReadFile(string filePath, bool writeNewLine = false): " + e);
                return "";
            }
        }

        /// <summary>
        /// Write file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void WriteFile(string filePath, string value)
        {
            try
            {
                StreamWriter monStreamWriter = new StreamWriter(filePath);

                monStreamWriter.Write(value);
                monStreamWriter.Close();
            }
            catch (Exception e)
            {
                Logging.WriteError("WriteFile(string filePath, string value): " + e);
            }
        }

        /// <summary>
        /// Wait.
        /// </summary>
        /// <param name="milsecToWait"></param>
        /// <returns></returns>
        public static void Wait(int milsecToWait)
        {
            try
            {
                int num = GetTickCount() + milsecToWait;
                while (GetTickCount() < num)
                {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Wait(int milsecToWait): " + exception);
            }
        }

        [DllImport("kernel32")]
        private static extern int GetTickCount();

        public static List<string> GetReqWithAuthHeader(string url, String userName, String userPassword)
        {
            try
            {
                WebRequest req = WebRequest.Create(ToUtf8(url));
                if (userName != "" && userPassword != "")
                {
                    string authInfo = ToUtf8(userName) + ":" + ToUtf8(userPassword);
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }
                ((HttpWebRequest) req).UserAgent = "TheNoobBot";

                WebResponse response = req.GetResponse();
                string headerResult = "";
                if (userName != "" && userPassword != "")
                {
                    headerResult = response.Headers.Get("retn");
                }
                Stream stm = response.GetResponseStream();
                if (stm != null)
                {
                    StreamReader r = new StreamReader(stm);
                    string sourceResult = r.ReadToEnd();
                    return new List<string> {ToUtf8(headerResult), ToUtf8(sourceResult)};
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReqWithAuthHeader(string url, String userName, String userPassword): " +
                                   exception);
            }
            return new List<string> {"", ""};
        }

        public static void ProductStatusLog(string productName, uint stepId)
        {
            switch (stepId)
            {
                case 1:
                    if (Logging.Status == productName + " initialized") // do not spam if it was already the last message given
                        return;
                    Logging.Status = productName + " initialized";
                    Logging.Write(productName + " initialized");
                    break;
                case 2:
                    if (Logging.Status == productName + " disposed")
                        return;
                    Logging.Status = productName + " disposed";
                    Logging.Write(productName + " disposed");
                    break;
                case 3:
                    if (Logging.Status == "Start " + productName)
                        return;
                    Logging.Status = "Start " + productName;
                    Logging.Write("Start " + productName);
                    break;
                case 4:
                    if (Logging.Status == productName + " started")
                        return;
                    Logging.Status = productName + " started";
                    Logging.Write(productName + " started");
                    break;
                case 5:
                    if (Logging.Status == productName + " failed to start")
                        return;
                    Logging.Status = productName + " failed to start";
                    Logging.Write(productName + " failed to start");
                    break;
                case 6:
                    if (Logging.Status == productName + " stopped")
                        return;
                    Logging.Status = productName + " stopped";
                    Logging.Write(productName + " stopped");
                    break;
                case 7:
                    if (Logging.Status == "Settings of " + productName + " loaded")
                        return;
                    Logging.Status = "Settings of " + productName + " loaded";
                    Logging.Write("Settings of " + productName + " loaded");
                    break;
            }
        }

        public static void ShowMessageBox(string message, string title = "")
        {
            Thread thread = String.IsNullOrEmpty(title) ? new Thread(() => MessageBox.Show(message)) : new Thread(() => MessageBox.Show(message, title));
            thread.Start();
        }

        public static readonly Dictionary<int, int> ItemStock = new Dictionary<int, int>();
        private static int _oldEventFireCount = -1; // the first call call it with param (0)

        public static EventHandler ItemStockUpdated;

        public static void CheckInventoryForLatestLoot(int eventFireCount)
        {
            lock (ItemStock)
            {
                try
                {
                    if (_oldEventFireCount == eventFireCount)
                        return;
                    _oldEventFireCount = eventFireCount;
                    Dictionary<int, int> newLoots = new Dictionary<int, int>();
                    bool firstCheck = ItemStock.Count == 0;
                    List<WoWItem> objectWoWItems = ObjectManager.GetObjectWoWItem();
                    foreach (WoWItem wowItem in objectWoWItems)
                    {
                        WoWItem item = wowItem;
                        if (!item.IsValid || item.Entry < 1 || newLoots.ContainsKey(item.Entry))
                            continue;
                        if (EquippedItems.IsEquippedItemByGuid(item.Guid))
                            continue;
                        int count = ItemsManager.GetItemCount(item.Entry);
                        if (count == 0)
                            continue;
                        if (!ItemStock.ContainsKey(item.Entry))
                        {
                            newLoots.Add(item.Entry, count);
                            ItemStock.Add(item.Entry, count);
                            continue;
                        }
                        if (ItemStock[item.Entry] == count)
                            continue;
                        if (ItemStock[item.Entry] < count)
                        {
                            newLoots.Add(item.Entry, count - ItemStock[item.Entry]);
                            ItemStock[item.Entry] = count;
                            continue;
                        }
                        if (ItemStock[item.Entry] > count)
                        {
                            // Update our stock if we lost some items.
                            ItemStock[item.Entry] = count;
                        }
                    }
                    if (!firstCheck)
                    {
                        foreach (KeyValuePair<int, int> pair in newLoots)
                        {
                            // Can do anything here, like equip cool items etc.
                            if (ItemStockUpdated != null)
                                ItemStockUpdated(pair, new EventArgs());
                            Logging.Write("You receive loot: " + ItemsManager.GetItemNameById(pair.Key) + "(" + pair.Key + ") x" + pair.Value);
                        }
                    }
                    // When all the processing is done, let's now check if we are missing items completly.
                    var toRemove = new List<int>();
                    foreach (KeyValuePair<int, int> pair in ItemStock)
                    {
                        bool KeepValue = false;
                        foreach (WoWItem item in objectWoWItems)
                        {
                            if (item.Entry == pair.Key)
                                KeepValue = true;
                        }
                        // Update our stock if we lost some items.
                        if (!KeepValue)
                            toRemove.Add(pair.Key);
                    }
                    foreach (int i in toRemove)
                    {
                        ItemStock.Remove(i);
                    }
                    newLoots.Clear();
                    objectWoWItems.Clear();
                    toRemove.Clear();
                }
                catch (Exception e)
                {
                    Logging.WriteError("CheckInventoryForLatestLoot(int eventFireCount): " + e);
                }
            }
        }

        #region FailOver System

        public static bool IsFrameVisible(string frameName)
        {
            string result = GetRandomString(Random(4, 10));
            Lua.LuaDoString(result + " = tostring(" + frameName + " and " + frameName + ":IsVisible())");
            return Lua.GetLocalizedText(result) == "true";
        }

        private static string _cachedAuthServerAddress;
        private static readonly Timer CachedAuthServerTimer = new Timer(300); // Re-try to connect to the prioritized AuthServers every 5 minutes.
        private static readonly string[] FailOversAddress = new[] {"http://tech.thenoobbot.com/" /*, "http://auth2.thenoobbot.com/"*/};

        public static string GetWorkingAuthServerAddress
        {
            get
            {
                if (!String.IsNullOrEmpty(_cachedAuthServerAddress) && !CachedAuthServerTimer.IsReady)
                {
                    return _cachedAuthServerAddress;
                }
                foreach (string failOverAddress in Enumerable.Where(FailOversAddress, server => GetRequest(server + "isOnline.php", "") == "true"))
                {
                    _cachedAuthServerAddress = failOverAddress;
                    CachedAuthServerTimer.Reset();
                    return failOverAddress;
                }
                return FailOversAddress[0];
            }
        }

        public static void LootStatistics(bool startOrStop = true)
        {
            if (startOrStop)
            {
                Logging.Write("Initializing LootStatistics module, may take few seconds.");
                CheckInventoryForLatestLoot(0); // Generate the initial _stockList.
                EventsListener.HookEvent(WoWEventsType.CHAT_MSG_LOOT, callBack => CheckInventoryForLatestLoot((int) callBack), true);
            }
            else
            {
                EventsListener.UnHookEvent(WoWEventsType.CHAT_MSG_LOOT, callBack => CheckInventoryForLatestLoot((int) callBack), true);
            }
        }

        #endregion

        public static string GetAuthScriptLink
        {
            get { return GetWorkingAuthServerAddress + "auth.php"; }
        }

        public static string GetUpdateScriptLink
        {
            get { return GetWorkingAuthServerAddress + "update.php"; }
        }

        public static string GetMonitoringScriptLink
        {
            get { return GetWorkingAuthServerAddress + "isOnline.php"; }
        }

        public static string GetClientIPScriptLink
        {
            get { return GetWorkingAuthServerAddress + "myIp.php"; }
        }

        public static string GetClientIPAddress
        {
            get { return GetRequest(GetClientIPScriptLink, ""); } // We don't auth so we don't get "PLATINIUM" instead of the IP if Plat.
        }

        public static void LoginToWoW()
        {
            Login.SettingsLogin s = new Login.SettingsLogin
            {
                Login = nManagerSetting.AutoStartEmail,
                Password = nManagerSetting.AutoStartPassword,
                Realm = nManagerSetting.AutoStartRealmName,
                Character = nManagerSetting.AutoStartCharacter,
                BNetName = nManagerSetting.AutoStartBattleNet,
            };

            Logging.Write("Begin player logging with informations provided.");
            Login.Pulse(s);
            if (Usefuls.InGame && !Usefuls.IsLoading)
            {
                Thread.Sleep(5000);
                if (Usefuls.InGame && !Usefuls.IsLoading)
                {
                    Logging.Write("Ending player logging with success.");
                    ConfigWowForThisBot.ConfigWow();
                    if (Products.Products.ProductName == "Damage Dealer" && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    if (Products.Products.ProductName == "Heal Bot" && nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    SpellManager.UpdateSpellBook();
                }
            }
        }
    }
}