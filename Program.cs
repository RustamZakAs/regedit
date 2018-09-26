using System;
using System.IO;
using System.Net;
using System.Text;
//using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net.Sockets;
//using System.Threading.Tasks;
using System.Collections.Generic;

namespace regedit
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов

        static void Main(string[] args)
        {

            Metanit();


            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = "gpedit.msc";
            //Process.Start(startInfo);

            //cmdProcess();
            //Console.ReadLine();
            //Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            //Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            //Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\ActiveDesktop");

            //--DisableTaskManager(false);
            //--NoChangingWallPaper(false);
            //Console.ReadKey();
        }

        static void cmdProcess()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                Process p = new Process();
                p.StartInfo = startInfo;
                p.Start();
                using (StreamWriter writer = p.StandardInput)
                {
                    // If the streamwriter is able to write
                    if (writer.BaseStream.CanWrite)
                    {
                        // Write the command that was passed into the method
                        string cmd = @"printui /Xs /n ""Printer Name"" ClientSideRender enabled";
                        writer.WriteLine(cmd);

                    }
                    // close the StreamWriter
                    writer.Close();
                }
            }
            catch { }
        }


        //You can find these settings in HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\ in the Registry.

        //For example, the following code can enable/disable the TaskManager:

        //Code Block
        static void DisableTaskManager(bool disable)
        {
            Microsoft.Win32.RegistryKey HKCU = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey key = HKCU.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            key.SetValue("DisableTaskMgr", disable ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
        }

        static void NoChangingWallPaper(bool disable)
        {
            Microsoft.Win32.RegistryKey HKCU = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey key = HKCU.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
            key.SetValue("NoChangingWallPaper", disable ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);

            key = HKCU.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            key.SetValue("Wallpaper", @"D:\VN_logo.jpg", Microsoft.Win32.RegistryValueKind.String);

            string name = key.GetValue("Wallpaper").ToString();
            Console.WriteLine(name);

            key = HKCU.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            key.SetValue("WallpaperStyle", "0", Microsoft.Win32.RegistryValueKind.String);

        }

        public static bool SetMachineName(string newName)
        {
            RegistryKey key = Registry.LocalMachine;

            string activeComputerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName";
            RegistryKey activeCmpName = key.CreateSubKey(activeComputerName);
            activeCmpName.SetValue("ComputerName", newName);
            activeCmpName.Close();
            string computerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
            RegistryKey cmpName = key.CreateSubKey(computerName);
            cmpName.SetValue("ComputerName", newName);
            cmpName.Close();
            string _hostName = "SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\";
            RegistryKey hostName = key.CreateSubKey(_hostName);
            hostName.SetValue("Hostname", newName);
            hostName.SetValue("NV Hostname", newName);
            hostName.Close();
            return true;
        }
        /*
            [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System]
            "NoDispCPL"=dword:00000001

            [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System]
            "NoDispCPL"=- 
        */

        /*
        как запретить менять разрешение экрана для пользователя с правами администратора в винхр

        windows registry editor version 5.00 

        [hkey_current_user\software\microsoft\windows\currentversion\policies\activedesktop]
        "nodispsettingspage"=dword:00000001

        сохраните как "tweak.REG" и запустите.


        ver2
        Windows Registry Editor Version 5.00

        [HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System]
        "NoDispCPL"=dword:00000001

        [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System]
        "NoDispCPL"=-
        */

        void ss()
        {
            Microsoft.Win32.RegistryKey subKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Office");
            if (checkIfKeyExists(subKey))
            {
                subKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Office\\11.0");
                if (checkIfKeyExists(subKey))
                {
                    subKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Office\\11.0\\Outlook\\Options");
                    if (!checkIfKeyExists(subKey))
                    {
                        Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Office\\11.0\\Outlook\\Options");
                    }
                    subKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Office\\11.0\\Outlook\\Options\\Mail");
                    if (!checkIfKeyExists(subKey))
                    {
                        Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Office\\11.0\\Outlook\\Options\\Mail");
                    }

                    Microsoft.Win32.Registry.LocalMachine.SetValue("AllowTNEFtoCreateProps", "00000000", RegistryValueKind.DWord);
                    Microsoft.Win32.Registry.LocalMachine.SetValue("AllowMSGFilestoCreateProps", "00000001", RegistryValueKind.DWord);
                }
            }
        }

        private static bool checkIfKeyExists(Microsoft.Win32.RegistryKey subKey)
        {
            bool status = true;
            if (subKey == null)
            {
                status = false;
            }
            return status;
        }

        public static void Metanit()
        {
            /*
            IPHostEntry host1 = Dns.GetHostEntry("www.microsoft.com");
            Console.WriteLine(host1.HostName);
            foreach (IPAddress ip in host1.AddressList)
                Console.WriteLine(ip.ToString());

            Console.WriteLine();

            IPHostEntry host2 = Dns.GetHostEntry("google.com");
            Console.WriteLine(host2.HostName);
            foreach (IPAddress ip in host2.AddressList)
            Console.WriteLine(ip.ToString());
            */


            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    class MyTcpListener
    {
        public static void MyTcpListener1()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
