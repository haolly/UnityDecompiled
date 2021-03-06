﻿namespace UnityEditor
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    internal class EditorWindowTitleAttribute : Attribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private string <icon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <title>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <useTypeNameAsIconName>k__BackingField;

        public string icon { get; set; }

        public string title { get; set; }

        public bool useTypeNameAsIconName { get; set; }
    }
}

