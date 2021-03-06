﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEngine;

    internal class AssetServerConfig
    {
        private Dictionary<string, string> fileContents = new Dictionary<string, string>();
        private string fileName = (Application.dataPath + "/../Library/ServerPreferences.plist");
        private static Regex sKeyTag = new Regex("<key>([^<]+)</key>");
        private static Regex sValueTag = new Regex("<string>([^<]+)</string>");

        public AssetServerConfig()
        {
            try
            {
                using (StreamReader reader = new StreamReader(this.fileName))
                {
                    string str;
                    string str2 = ".unkown";
                    while ((str = reader.ReadLine()) != null)
                    {
                        Match match = sKeyTag.Match(str);
                        if (match.Success)
                        {
                            str2 = match.Groups[1].Value;
                        }
                        match = sValueTag.Match(str);
                        if (match.Success)
                        {
                            this.fileContents[str2] = match.Groups[1].Value;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("Could not read asset server configuration: " + exception.Message);
            }
        }

        public string connectionSettings
        {
            get => 
                this.fileContents["Maint Connection Settings"];
            set
            {
                this.fileContents["Maint Connection Settings"] = value;
            }
        }

        public string dbName
        {
            get => 
                this.fileContents["Maint database name"];
            set
            {
                this.fileContents["Maint database name"] = value;
            }
        }

        public int portNumber
        {
            get => 
                int.Parse(this.fileContents["Maint port number"]);
            set
            {
                this.fileContents["Maint port number"] = value.ToString();
            }
        }

        public string projectName
        {
            get => 
                this.fileContents["Maint project name"];
            set
            {
                this.fileContents["Maint project name"] = value;
            }
        }

        public string server
        {
            get => 
                this.fileContents["Maint Server"];
            set
            {
                this.fileContents["Maint Server"] = value;
            }
        }

        public string settingsType
        {
            get => 
                this.fileContents["Maint settings type"];
            set
            {
                this.fileContents["Maint settings type"] = value;
            }
        }

        public float timeout
        {
            get => 
                float.Parse(this.fileContents["Maint Timeout"], CultureInfo.InvariantCulture);
            set
            {
                this.fileContents["Maint Timeout"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string userName
        {
            get => 
                this.fileContents["Maint UserName"];
            set
            {
                this.fileContents["Maint UserName"] = value;
            }
        }
    }
}

