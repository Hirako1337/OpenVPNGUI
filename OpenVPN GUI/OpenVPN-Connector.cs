using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //to use network http-get functions
using System.Reflection.Emit;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpenVPN_GUI
{
    class OpenVPN_Connector
    {
        bool tap_found = false; //boolean of tap adapter installation
        bool is64Bit = true; //auto resolving is not yet available.
        string startIP = "";
        public bool ClientKill = false;
        public void Disconnect(Button Disconnect, Button Connect)
        {
            Disconnect.Enabled = false;
            Connect.Enabled = true;
            Process.GetProcessesByName("openvpn")[0].Kill(); //this will hard shut down the connection.
            ClientKill = true;
        }
        public void Connect(Button Disconnect, Button Connect)
        {
            Disconnect.Enabled = true;
            Connect.Enabled = false;
        }
        public void Connector(Timer ConnectionManager,RichTextBox Log,string config)
        {
            Log.AppendText("[" + System.DateTime.Now.ToString() + "] OpenVPN Connection was started!");
            ProcessStartInfo OpenVPN_TAP = new ProcessStartInfo();
            OpenVPN_TAP.FileName =Application.StartupPath + "/bin/openvpn.exe";
            OpenVPN_TAP.Arguments = Application.StartupPath + "\\configs\\"+config; //our config file to connect
            OpenVPN_TAP.WindowStyle = ProcessWindowStyle.Normal;
            Log.AppendText("\n[" + System.DateTime.Now.ToString() + "] OpenVPN Configuration " + Application.StartupPath + "\\configs\\config.ovpn");
            Process.Start(OpenVPN_TAP);
            
            ConnectionManager.Start(); //run the auto connection manager. (Timer)
        }
        public void isConnected(Button Disconnect,Button Connect, Timer ConnectionManager)
        {
            if (GetRemoteHost() == startIP)
            {
                //Non-Established
                Disconnect.Enabled = false;
                Connect.Enabled = true;
            }
            else
            {
                //Established
                Connect.Enabled = true;
                Disconnect.Enabled = true;
                ConnectionManager.Stop(); //Handler will stop
            }
        }
        public void InstallTAPAdapter()
        {
            if (is64Bit == true)
            {
                new System.Net.WebClient().DownloadFile("https://swupdate.openvpn.org/community/releases/OpenVPN-2.5-beta1-amd64.msi", "OpenVPN-2.5-beta1-amd64.msi");
                Process.Start("OpenVPN-2.5-beta1-amd64.msi"); //this will run the installer auto.
            }
        }
        public string GetRemoteHost()
        {
            return new System.Net.WebClient().DownloadString("http://ifconfig.co/ip"); //an public ip resolver api
        }
        public void IsOpenVPNAvailable()
        {
            var TAP_ADAPTER=Registry.CurrentUser.OpenSubKey(@"Software\OpenVPN\Shortcuts");
            if (TAP_ADAPTER.GetValue("bin.tapctl.exe.create.TAPWindows6") == null)
            {
                tap_found = false; //TAP Adapter was not installed on the system so we will install it automatically.
                DialogResult tap=MessageBox.Show(null, "TAP-Adapter was not found on the system. you need to install to keep going.", "OpenVPN", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(tap==DialogResult.Yes)
                {
                    InstallTAPAdapter(); //installer.
                }
                else
                {
                    //denied the installation. the vpn will not work.
                }

            }
            else
            {
                //TAP Adapter is already installed.
            }
            startIP = GetRemoteHost(); //to get the base ip address of the user.
        }
    }
}
