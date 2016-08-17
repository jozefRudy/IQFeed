using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Threading;

namespace IQFeed
{

    public class IQConnect
    {
        public IQConnect(string product = "", string version = "")
        {
            _product = product;
            _version = version;
        }
        public string Product { get { return _product; } }
        public string Version { get { return _version; } }
        
        public IQCredentials getCredentials()
        {
            var iqc = new IQCredentials();
            // pull the login and password, save login info, and autoconnect settings out of the 
            // registry (if they are already stored)
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            // NOTE: we don't need to check for the virtualized registry key on x64 here since these values are in the HKEY_CURRENT_USER hive.
            if (key != null)
            {
                iqc.LoginId = key.GetValue("Login", "").ToString();
                iqc.Password = key.GetValue("Password", "").ToString();
                iqc.AutoConnect = false;
                object sData = key.GetValue("AutoConnect", "0").ToString();
                if (sData.Equals("1"))
                {
                    iqc.AutoConnect = true;
                }
                iqc.SaveCredentials = false;
                sData = key.GetValue("SaveLoginPassword", "0").ToString();
                if (sData.Equals("1"))
                {
                    iqc.SaveCredentials = true;
                }
                return iqc;
            }
            return null;
        }

        public string getPath()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\DTN\\IQFeed");
            if (key == null)
            {
                // if it isn't in that location, it is possible the user is running and x64 OS.  Check the windows virtualized registry location
                key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\DTN\\IQFeed");
            }
            string sLocation = null;
            if (key != null)
            {
                sLocation = key.GetValue("EXEDIR", "").ToString();
                // close the key since we don't need it anymore
                key.Close();
                // verify there is a \ on the end before we append the exe name
                if (!(sLocation.EndsWith("\\") || sLocation.EndsWith("/")))
                {
                    sLocation += "\\";
                }
                sLocation += "IQConnect.exe";
            }
            return sLocation;
        }
        public string getArguments(IQCredentials iqc)
        {
            string arguments = "";

            if (Product != "") { arguments += "-product " + Product + " "; }
            if (Version != "") { arguments += "-version " + Version + " "; }
            if (iqc.LoginId != "") { arguments += "-login " + iqc.LoginId + " "; }
            if (iqc.Password != "") { arguments += "-password " + iqc.Password + " "; }
            if (iqc.SaveCredentials) { arguments += "-savelogininfo "; }
            if (iqc.AutoConnect) { arguments += "-autoconnect"; }
            arguments.TrimEnd(' ');

            return arguments;
        }

        public bool launch(IQCredentials iqc = null, string arguments = null, int pauseMilliseconds = 6000)
        {
            if (iqc == null) { iqc = getCredentials(); }
            if (iqc == null) { iqc = new IQCredentials(); }
            if (arguments == null) { arguments = getArguments(iqc); }
            System.Diagnostics.Process.Start("IQConnect.exe", arguments);
            Thread.Sleep(pauseMilliseconds);
            return true;
        }

        #region private
        private string _product;
        private string _version;
        #endregion
    }
}
