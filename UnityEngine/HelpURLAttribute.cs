﻿namespace UnityEngine
{
    using System;
    using UnityEngine.Scripting;

    /// <summary>
    /// <para>Provide a custom documentation URL for a class.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false), UsedByNativeCode]
    public sealed class HelpURLAttribute : Attribute
    {
        internal readonly string m_Url;

        /// <summary>
        /// <para>Initialize the HelpURL attribute with a documentation url.</para>
        /// </summary>
        /// <param name="url">The custom documentation URL for this class.</param>
        public HelpURLAttribute(string url)
        {
            this.m_Url = url;
        }

        /// <summary>
        /// <para>The documentation URL specified for this class.</para>
        /// </summary>
        public string URL =>
            this.m_Url;
    }
}

