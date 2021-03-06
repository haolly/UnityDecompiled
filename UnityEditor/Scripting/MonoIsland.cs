﻿namespace UnityEditor.Scripting
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEditor;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MonoIsland
    {
        public readonly BuildTarget _target;
        public readonly bool _development_player;
        public readonly bool _editor;
        public readonly string _classlib_profile;
        public readonly string[] _files;
        public readonly string[] _references;
        public readonly string[] _defines;
        public readonly string _output;
        public MonoIsland(BuildTarget target, string classlib_profile, string[] files, string[] references, string[] defines, string output)
        {
            this._target = target;
            this._development_player = false;
            this._editor = false;
            this._classlib_profile = classlib_profile;
            this._files = files;
            this._references = references;
            this._defines = defines;
            this._output = output;
        }

        public string GetExtensionOfSourceFiles() => 
            ((this._files.Length <= 0) ? "NA" : ScriptCompilers.GetExtensionOfSourceFile(this._files[0]));
    }
}

