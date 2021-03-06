﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.XPath;
    using UnityEngine;

    internal class BuildVerifier
    {
        private Dictionary<string, HashSet<string>> m_UnsupportedAssemblies = null;
        private static BuildVerifier ms_Inst = null;

        protected BuildVerifier()
        {
            this.m_UnsupportedAssemblies = new Dictionary<string, HashSet<string>>();
            string uri = Path.Combine(Path.Combine(EditorApplication.applicationContentsPath, "Resources"), "BuildVerification.xml");
            XPathNavigator navigator = new XPathDocument(uri).CreateNavigator();
            navigator.MoveToFirstChild();
            XPathNodeIterator iterator = navigator.SelectChildren("assembly", "");
            while (iterator.MoveNext())
            {
                string attribute = iterator.Current.GetAttribute("name", "");
                if ((attribute == null) || (attribute.Length < 1))
                {
                    throw new ApplicationException($"Failed to load {uri}, <assembly> name attribute is empty");
                }
                string key = iterator.Current.GetAttribute("platform", "");
                if ((key == null) || (key.Length < 1))
                {
                    key = "*";
                }
                if (!this.m_UnsupportedAssemblies.ContainsKey(key))
                {
                    this.m_UnsupportedAssemblies.Add(key, new HashSet<string>());
                }
                this.m_UnsupportedAssemblies[key].Add(attribute);
            }
        }

        protected bool VerifyAssembly(BuildTarget target, string assembly)
        {
            if ((this.m_UnsupportedAssemblies.ContainsKey("*") && this.m_UnsupportedAssemblies["*"].Contains(assembly)) || (this.m_UnsupportedAssemblies.ContainsKey(target.ToString()) && this.m_UnsupportedAssemblies[target.ToString()].Contains(assembly)))
            {
                return false;
            }
            return true;
        }

        public static void VerifyBuild(BuildTarget target, string managedDllFolder)
        {
            if (ms_Inst == null)
            {
                ms_Inst = new BuildVerifier();
            }
            ms_Inst.VerifyBuildInternal(target, managedDllFolder);
        }

        protected void VerifyBuildInternal(BuildTarget target, string managedDllFolder)
        {
            foreach (string str in Directory.GetFiles(managedDllFolder))
            {
                if (str.EndsWith(".dll"))
                {
                    string fileName = Path.GetFileName(str);
                    if (!this.VerifyAssembly(target, fileName))
                    {
                        object[] args = new object[] { fileName, target.ToString() };
                        Debug.LogWarningFormat("{0} assembly is referenced by user code, but is not supported on {1} platform. Various failures might follow.", args);
                    }
                }
            }
        }
    }
}

