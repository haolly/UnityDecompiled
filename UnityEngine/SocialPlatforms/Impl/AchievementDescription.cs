﻿namespace UnityEngine.SocialPlatforms.Impl
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class AchievementDescription : IAchievementDescription
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private string <id>k__BackingField;
        private string m_AchievedDescription;
        private bool m_Hidden;
        private Texture2D m_Image;
        private int m_Points;
        private string m_Title;
        private string m_UnachievedDescription;

        public AchievementDescription(string id, string title, Texture2D image, string achievedDescription, string unachievedDescription, bool hidden, int points)
        {
            this.id = id;
            this.m_Title = title;
            this.m_Image = image;
            this.m_AchievedDescription = achievedDescription;
            this.m_UnachievedDescription = unachievedDescription;
            this.m_Hidden = hidden;
            this.m_Points = points;
        }

        public void SetImage(Texture2D image)
        {
            this.m_Image = image;
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { this.id, " - ", this.title, " - ", this.achievedDescription, " - ", this.unachievedDescription, " - ", this.points, " - ", this.hidden };
            return string.Concat(objArray1);
        }

        public string achievedDescription =>
            this.m_AchievedDescription;

        public bool hidden =>
            this.m_Hidden;

        public string id { get; set; }

        public Texture2D image =>
            this.m_Image;

        public int points =>
            this.m_Points;

        public string title =>
            this.m_Title;

        public string unachievedDescription =>
            this.m_UnachievedDescription;
    }
}

