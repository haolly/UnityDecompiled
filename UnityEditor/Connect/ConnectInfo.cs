﻿namespace UnityEditor.Connect
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct ConnectInfo
    {
        private int m_Initialized;
        private int m_Ready;
        private int m_Online;
        private int m_LoggedIn;
        private int m_WorkOffline;
        private int m_ShowLoginWindow;
        private int m_Error;
        private string m_LastErrorMsg;
        private int m_Maintenance;
        public bool initialized =>
            (this.m_Initialized != 0);
        public bool ready =>
            (this.m_Ready != 0);
        public bool online =>
            (this.m_Online != 0);
        public bool loggedIn =>
            (this.m_LoggedIn != 0);
        public bool workOffline =>
            (this.m_WorkOffline != 0);
        public bool showLoginWindow =>
            (this.m_ShowLoginWindow != 0);
        public bool error =>
            (this.m_Error != 0);
        public string lastErrorMsg =>
            this.m_LastErrorMsg;
        public bool maintenance =>
            (this.m_Maintenance != 0);
    }
}

