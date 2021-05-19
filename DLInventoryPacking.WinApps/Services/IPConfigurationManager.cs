using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DLInventoryPacking.WinApps.Services
{
    public class IPConfigurationManager
    {
        private const string KEY = "IPSetting";
        public IPConfigurationManager()
        {
        }

        public string GetIP()
        {
            return ConfigurationManager.AppSettings[KEY].ToString(); ;
        }

        public void UpdateIP(string newValue)
        {
            //var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            //if (!KeyExists(KEY))
            //{
            //    throw new ArgumentNullException("Key", "<" + KEY + "> does not exist in the configuration. Update failed.");
            //}
            //var appSettingsNode = xmlDoc.SelectSingleNode("configuration/ConfigurationManager");

            //foreach (XmlNode childNode in appSettingsNode)
            //{
            //    if (childNode.Attributes["key"].Value == KEY)
            //        childNode.Attributes["value"].Value = newValue;
            //}
            //xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");
            //xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.AppSettings[KEY] = newValue;
        }

        public bool KeyExists(string strKey)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appsettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    return true;
            }
            return false;
        }

    }
}
