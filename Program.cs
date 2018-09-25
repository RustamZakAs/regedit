using System;
using System.IO;
using System.Text;
//using System.Linq;
using System.Diagnostics;
//using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Win32;

namespace regedit
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = "gpedit.msc";
            //Process.Start(startInfo);

            //cmdProcess();
            //Console.ReadLine();
            //Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            //Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            //Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\ActiveDesktop");

            DisableTaskManager(false);
            NoChangingWallPaper(false);
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
    }
}
