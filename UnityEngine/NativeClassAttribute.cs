﻿namespace UnityEngine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited=false)]
    public sealed class NativeClassAttribute : Attribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private string <QualifiedNativeName>k__BackingField;

        public NativeClassAttribute(string qualifiedCppName)
        {
            this.QualifiedNativeName = qualifiedCppName;
        }

        public string QualifiedNativeName { get; private set; }
    }
}

