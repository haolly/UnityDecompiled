﻿namespace Unity.BindingsGenerator.Core.Attributes
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true)]
    internal class NativeIncludeAttribute : Attribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Header>k__BackingField;

        public string Header { get; set; }
    }
}

