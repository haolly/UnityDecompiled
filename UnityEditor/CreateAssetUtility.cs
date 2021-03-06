﻿namespace UnityEditor
{
    using System;
    using System.IO;
    using UnityEditor.ProjectWindowCallback;
    using UnityEngine;

    [Serializable]
    internal class CreateAssetUtility
    {
        [SerializeField]
        private EndNameEditAction m_EndAction;
        [SerializeField]
        private Texture2D m_Icon;
        [SerializeField]
        private int m_InstanceID;
        [SerializeField]
        private string m_Path = "";
        [SerializeField]
        private string m_ResourceFile;

        public bool BeginNewAssetCreation(int instanceID, EndNameEditAction newAssetEndAction, string filePath, Texture2D icon, string newAssetResourceFile)
        {
            string uniquePathNameAtSelectedPath;
            if (!filePath.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
            {
                uniquePathNameAtSelectedPath = AssetDatabase.GetUniquePathNameAtSelectedPath(filePath);
            }
            else
            {
                uniquePathNameAtSelectedPath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            }
            if (!IsPathDataValid(uniquePathNameAtSelectedPath))
            {
                object[] args = new object[] { uniquePathNameAtSelectedPath, filePath };
                Debug.LogErrorFormat("Invalid generated unique path '{0}' (input path '{1}')", args);
                this.Clear();
                return false;
            }
            this.m_InstanceID = instanceID;
            this.m_Path = uniquePathNameAtSelectedPath;
            this.m_Icon = icon;
            this.m_EndAction = newAssetEndAction;
            this.m_ResourceFile = newAssetResourceFile;
            Selection.activeObject = EditorUtility.InstanceIDToObject(instanceID);
            return true;
        }

        public void Clear()
        {
            this.m_EndAction = null;
            this.m_InstanceID = 0;
            this.m_Path = "";
            this.m_Icon = null;
            this.m_ResourceFile = "";
        }

        public void EndNewAssetCreation(string name)
        {
            string pathName = this.folder + "/" + name + this.extension;
            EndNameEditAction endAction = this.m_EndAction;
            int instanceID = this.m_InstanceID;
            string resourceFile = this.m_ResourceFile;
            this.Clear();
            ProjectWindowUtil.EndNameEditAction(endAction, instanceID, pathName, resourceFile);
        }

        public bool IsCreatingNewAsset() => 
            !string.IsNullOrEmpty(this.m_Path);

        private static bool IsPathDataValid(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            return (AssetDatabase.GetMainAssetInstanceID(Path.GetDirectoryName(filePath)) != 0);
        }

        public EndNameEditAction endAction =>
            this.m_EndAction;

        public string extension =>
            Path.GetExtension(this.m_Path);

        public string folder =>
            Path.GetDirectoryName(this.m_Path);

        public Texture2D icon =>
            this.m_Icon;

        public int instanceID =>
            this.m_InstanceID;

        public string originalName =>
            Path.GetFileNameWithoutExtension(this.m_Path);
    }
}

