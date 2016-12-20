﻿namespace UnityEditor.Web
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEditor;
    using UnityEditor.Connect;
    using UnityEditor.SceneManagement;

    [InitializeOnLoad]
    internal sealed class EditorProjectAccess
    {
        private const string kCloudEnabled = "CloudEnabled";
        private const string kCloudServiceKey = "CloudServices";

        static EditorProjectAccess()
        {
            JSProxyMgr.GetInstance().AddGlobalObject("unity/project", new EditorProjectAccess());
        }

        public void CloseToolbarWindow()
        {
            CollabToolbarWindow.CloseToolbarWindows();
        }

        public void CloseToolbarWindowImmediately()
        {
            CollabToolbarWindow.CloseToolbarWindowsImmediately();
        }

        public void EnableCloud(bool enable)
        {
            EditorUserSettings.SetConfigValue("CloudServices/CloudEnabled", enable.ToString());
        }

        public void EnterPlayMode()
        {
            EditorApplication.isPlaying = true;
        }

        public string GetBuildTarget()
        {
            return EditorUserBuildSettings.activeBuildTarget.ToString();
        }

        public int GetEditorSkinIndex()
        {
            return EditorGUIUtility.skinIndex;
        }

        public string GetEnvironment()
        {
            return UnityConnect.instance.GetEnvironment();
        }

        public string GetOrganizationID()
        {
            return UnityConnect.instance.projectInfo.organizationId;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern string GetProjectEditorVersion();
        public string GetProjectGUID()
        {
            return UnityConnect.instance.projectInfo.projectGUID;
        }

        public string GetProjectIcon()
        {
            return null;
        }

        public string GetProjectName()
        {
            string projectName = UnityConnect.instance.projectInfo.projectName;
            if (projectName != "")
            {
                return projectName;
            }
            return PlayerSettings.productName;
        }

        public string GetProjectPath()
        {
            return Directory.GetCurrentDirectory();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern string GetRESTServiceURI();
        public string GetUserAccessToken()
        {
            return UnityConnect.instance.GetAccessToken();
        }

        public string GetUserDisplayName()
        {
            return UnityConnect.instance.userInfo.displayName;
        }

        public string GetUserName()
        {
            return UnityConnect.instance.userInfo.userName;
        }

        public string GetUserPrimaryOrganizationId()
        {
            return UnityConnect.instance.userInfo.primaryOrg;
        }

        public void GoToHistory()
        {
            CollabHistoryWindow.ShowHistoryWindow().Focus();
        }

        public bool IsLoggedIn()
        {
            return UnityConnect.instance.loggedIn;
        }

        public bool IsOnline()
        {
            return UnityConnect.instance.online;
        }

        public bool IsPlayMode()
        {
            return EditorApplication.isPlaying;
        }

        public bool IsProjectBound()
        {
            return UnityConnect.instance.projectInfo.projectBound;
        }

        public void OpenLink(string link)
        {
            Help.BrowseURL(link);
        }

        public bool SaveCurrentModifiedScenesIfUserWantsTo()
        {
            return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        public void ShowToolbarDropdown()
        {
            Toolbar.requestShowCollabToolbar = true;
            if (Toolbar.get != null)
            {
                Toolbar.get.Repaint();
            }
        }
    }
}

