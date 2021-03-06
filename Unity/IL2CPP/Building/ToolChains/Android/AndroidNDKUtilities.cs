﻿namespace Unity.IL2CPP.Building.ToolChains.Android
{
    using NiceIO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Unity.IL2CPP.Building;
    using Unity.IL2CPP.Common;

    public class AndroidNDKUtilities
    {
        private TargetArchitectureSettings _architectureSettings;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NPath <AndroidNdkRootDir>k__BackingField;
        private const string AndroidAPI = "android-9";
        private const string GccVersion = "4.9";
        private const string GnuStlVersion = "4.9";
        private const string Toolchain = "llvm-3.6";

        public AndroidNDKUtilities(NPath ndkRootPath, Unity.IL2CPP.Building.Architecture architecture)
        {
            if (!ndkRootPath.Exists(""))
            {
                throw new ArgumentException("Android NDK path does not exist: " + ndkRootPath);
            }
            this.AndroidNdkRootDir = ndkRootPath;
            if (architecture is ARMv7Architecture)
            {
                this._architectureSettings = new ARMv7Settings();
            }
            else
            {
                if (!(architecture is x86Architecture))
                {
                    throw new NotSupportedException("Unknown architecture: " + architecture);
                }
                this._architectureSettings = new X86Settings();
            }
        }

        public NPath AndroidNdkRootDir { get; private set; }

        public IEnumerable<string> ArchitectureCompilerFlags =>
            new <>c__Iterator1 { 
                $this=this,
                $PC=-2
            };

        public IEnumerable<string> ArchitectureLinkerFlags =>
            new <>c__Iterator2 { 
                $this=this,
                $PC=-2
            };

        public NPath CCompilerPath
        {
            get
            {
                string[] append = new string[] { "toolchains", "llvm-3.6", "prebuilt", HostPlatform, "bin", "clang" };
                return this.AndroidNdkRootDir.Combine(append);
            }
        }

        public NPath CppCompilerPath
        {
            get
            {
                string[] append = new string[] { "toolchains", "llvm-3.6", "prebuilt", HostPlatform, "bin", "clang++" };
                return this.AndroidNdkRootDir.Combine(append);
            }
        }

        public NPath GccToolchain
        {
            get
            {
                string[] append = new string[] { "toolchains", this._architectureSettings.TCPrefix + "-4.9", "prebuilt", HostPlatform };
                return this.AndroidNdkRootDir.Combine(append);
            }
        }

        public IEnumerable<NPath> GnuStlIncludePaths =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public NPath GnuStlLibrary
        {
            get
            {
                string[] append = new string[] { "libs", this._architectureSettings.ABI };
                return this.GnuStlRoot.Combine(append);
            }
        }

        public NPath GnuStlRoot
        {
            get
            {
                string[] append = new string[] { "sources", "cxx-stl", "gnu-libstdc++", "4.9" };
                return this.AndroidNdkRootDir.Combine(append);
            }
        }

        private static string HostPlatform
        {
            get
            {
                if (PlatformUtils.IsWindows())
                {
                    return (!Environment.Is64BitOperatingSystem ? "windows" : "windows-x86_64");
                }
                if (PlatformUtils.IsLinux())
                {
                    return "linux-x86_64";
                }
                return "darwin-x86_64";
            }
        }

        public NPath LinkerPath =>
            this.CppCompilerPath;

        public NPath ObjCopyPath
        {
            get
            {
                string[] append = new string[] { "bin", this._architectureSettings.BinPrefix + "-objcopy" };
                return this.GccToolchain.Combine(append);
            }
        }

        public string Platform =>
            this._architectureSettings.Platform;

        public NPath SysRoot
        {
            get
            {
                string[] append = new string[] { "platforms", "android-9", "arch-" + this._architectureSettings.Arch };
                return this.AndroidNdkRootDir.Combine(append);
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<NPath>, IEnumerator, IDisposable, IEnumerator<NPath>
        {
            internal NPath $current;
            internal bool $disposing;
            internal int $PC;
            internal AndroidNDKUtilities $this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    {
                        string[] append = new string[] { "include" };
                        this.$current = this.$this.GnuStlRoot.Combine(append);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_00FE;
                    }
                    case 1:
                    {
                        string[] textArray2 = new string[] { "include", "backward" };
                        this.$current = this.$this.GnuStlRoot.Combine(textArray2);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_00FE;
                    }
                    case 2:
                    {
                        string[] textArray3 = new string[] { "libs", this.$this._architectureSettings.ABI, "include" };
                        this.$current = this.$this.GnuStlRoot.Combine(textArray3);
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_00FE;
                    }
                    case 3:
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_00FE:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<NPath> IEnumerable<NPath>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new AndroidNDKUtilities.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<NiceIO.NPath>.GetEnumerator();

            NPath IEnumerator<NPath>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string $current;
            internal bool $disposing;
            internal IEnumerator<string> $locvar0;
            internal int $PC;
            internal AndroidNDKUtilities $this;
            internal string <flag>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this._architectureSettings.CxxFlags.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00BE;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<flag>__0 = this.$locvar0.Current;
                        this.$current = this.<flag>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                this.$PC = -1;
            Label_00BE:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new AndroidNDKUtilities.<>c__Iterator1 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string $current;
            internal bool $disposing;
            internal IEnumerator<string> $locvar0;
            internal int $PC;
            internal AndroidNDKUtilities $this;
            internal string <flag>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this._architectureSettings.LDFlags.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00BE;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<flag>__0 = this.$locvar0.Current;
                        this.$current = this.<flag>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                this.$PC = -1;
            Label_00BE:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new AndroidNDKUtilities.<>c__Iterator2 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

