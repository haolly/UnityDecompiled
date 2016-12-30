﻿namespace Unity.IL2CPP.Building.ToolChains.MsvcVersions
{
    using Microsoft.Win32;
    using NiceIO;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Unity.IL2CPP.Building;
    using Unity.IL2CPP.Common;

    public abstract class MsvcInstallation
    {
        private static Dictionary<System.Version, MsvcInstallation> _installations = new Dictionary<System.Version, MsvcInstallation>();
        private readonly bool _use64BitTools;
        [CompilerGenerated]
        private static Func<NPath, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<System.Version, int> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<System.Version, int> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<System.Version, int> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<System.Version, int> <>f__am$cache4;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NPath <SDKDirectory>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Version <Version>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NPath <VisualStudioDirectory>k__BackingField;

        static MsvcInstallation()
        {
            System.Version version = new System.Version(10, 0);
            System.Version version2 = new System.Version(12, 0);
            System.Version version3 = new System.Version(14, 0);
            System.Version key = new System.Version(15, 0);
            NPath visualStudioInstallationFolder = GetVisualStudioInstallationFolder(version);
            NPath visualStudioDir = GetVisualStudioInstallationFolder(version2);
            NPath path3 = GetVisualStudioInstallationFolder(version3);
            if (visualStudioInstallationFolder != null)
            {
                Msvc10Installation installation = new Msvc10Installation(visualStudioInstallationFolder);
                if (installation.HasCppSDK)
                {
                    _installations.Add(version, installation);
                }
            }
            if (visualStudioDir != null)
            {
                Msvc12Installation installation2 = new Msvc12Installation(visualStudioDir);
                if (installation2.HasCppSDK)
                {
                    _installations.Add(version2, installation2);
                }
            }
            if (path3 != null)
            {
                Msvc14Installation installation3 = new Msvc14Installation(path3);
                if (installation3.HasCppSDK)
                {
                    _installations.Add(version3, installation3);
                }
            }
            if (Msvc15Installation.IsInstalled)
            {
                Msvc15Installation installation4 = new Msvc15Installation();
                if (installation4.HasCppSDK)
                {
                    _installations.Add(key, installation4);
                }
            }
        }

        protected MsvcInstallation(System.Version visualStudioVersion)
        {
            this.VisualStudioDirectory = GetVisualStudioInstallationFolder(visualStudioVersion);
            this.Version = visualStudioVersion;
            this._use64BitTools = CanRun64BitProcess();
        }

        protected MsvcInstallation(System.Version visualStudioVersion, NPath visualStudioDir, bool use64BitTools = true)
        {
            this.VisualStudioDirectory = visualStudioDir;
            this.Version = visualStudioVersion;
            this._use64BitTools = use64BitTools && CanRun64BitProcess();
        }

        private static bool CanRun64BitProcess()
        {
            if (IntPtr.Size != 4)
            {
                return true;
            }
            using (Process process = Process.GetCurrentProcess())
            {
                bool flag2;
                return (IsWow64Process(process.Handle, out flag2) && flag2);
            }
        }

        public abstract IEnumerable<NPath> GetIncludeDirectories(Unity.IL2CPP.Building.Architecture architecture);
        public static MsvcInstallation GetLatestInstallationAtLeast(System.Version version)
        {
            <GetLatestInstallationAtLeast>c__AnonStorey0 storey = new <GetLatestInstallationAtLeast>c__AnonStorey0 {
                version = version
            };
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = v => v.Major;
            }
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = v => v.Minor;
            }
            System.Version version2 = _installations.Keys.OrderByDescending<System.Version, int>(<>f__am$cache3).ThenByDescending<System.Version, int>(<>f__am$cache4).Where<System.Version>(new Func<System.Version, bool>(storey.<>m__0)).FirstOrDefault<System.Version>();
            if (version2 == null)
            {
                throw new Exception($"MSVC Installation version {storey.version.Major}.{storey.version.Minor} or later is not installed on current machine!");
            }
            return _installations[version2];
        }

        public static MsvcInstallation GetLatestInstalled()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = k => k.Major;
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = k => k.Minor;
            }
            System.Version version = _installations.Keys.OrderByDescending<System.Version, int>(<>f__am$cache1).ThenByDescending<System.Version, int>(<>f__am$cache2).FirstOrDefault<System.Version>();
            if (version == null)
            {
                throw new Exception("No MSVC installations were found on the machine!");
            }
            return _installations[version];
        }

        public abstract IEnumerable<NPath> GetLibDirectories(Unity.IL2CPP.Building.Architecture architecture, string sdkSubset = null);
        public virtual string GetPathEnvVariable(Unity.IL2CPP.Building.Architecture architecture)
        {
            List<NPath> source = new List<NPath>();
            if (architecture is x86Architecture)
            {
                if (this._use64BitTools)
                {
                    string[] append = new string[] { "VC", "bin", "amd64_x86" };
                    source.Add(this.VisualStudioDirectory.Combine(append));
                    string[] textArray2 = new string[] { "VC", "bin", "amd64" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray2));
                    if (this.SDKDirectory != null)
                    {
                        string[] textArray3 = new string[] { "bin" };
                        string[] textArray4 = new string[] { "x64" };
                        source.Add(this.SDKDirectory.Combine(textArray3).Combine(textArray4));
                    }
                }
                else
                {
                    string[] textArray5 = new string[] { "VC", "bin" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray5));
                }
                if (this.SDKDirectory != null)
                {
                    string[] textArray6 = new string[] { "bin" };
                    string[] textArray7 = new string[] { "x86" };
                    source.Add(this.SDKDirectory.Combine(textArray6).Combine(textArray7));
                }
            }
            else if (architecture is ARMv7Architecture)
            {
                if (this._use64BitTools)
                {
                    string[] textArray8 = new string[] { "VC", "bin", "amd64_arm" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray8));
                    string[] textArray9 = new string[] { "VC", "bin", "amd64" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray9));
                    if (this.SDKDirectory != null)
                    {
                        string[] textArray10 = new string[] { "bin" };
                        string[] textArray11 = new string[] { "x64" };
                        source.Add(this.SDKDirectory.Combine(textArray10).Combine(textArray11));
                    }
                }
                else
                {
                    string[] textArray12 = new string[] { "VC", "bin", "x86_arm" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray12));
                    string[] textArray13 = new string[] { "VC", "bin" };
                    source.Add(this.VisualStudioDirectory.Combine(textArray13));
                }
                if (this.SDKDirectory != null)
                {
                    string[] textArray14 = new string[] { "bin" };
                    string[] textArray15 = new string[] { "x86" };
                    source.Add(this.SDKDirectory.Combine(textArray14).Combine(textArray15));
                }
            }
            else
            {
                if (!(architecture is x64Architecture))
                {
                    throw new NotSupportedException($"'{architecture.Name}' architecture is not supported.");
                }
                string[] textArray16 = new string[] { "VC", "bin", "amd64" };
                source.Add(this.VisualStudioDirectory.Combine(textArray16));
                if (this.SDKDirectory != null)
                {
                    string[] textArray17 = new string[] { "bin" };
                    string[] textArray18 = new string[] { "x64" };
                    source.Add(this.SDKDirectory.Combine(textArray17).Combine(textArray18));
                    string[] textArray19 = new string[] { "bin" };
                    string[] textArray20 = new string[] { "x86" };
                    source.Add(this.SDKDirectory.Combine(textArray19).Combine(textArray20));
                }
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.ToString();
            }
            return source.Select<NPath, string>(<>f__am$cache0).AggregateWith(";");
        }

        public virtual IEnumerable<NPath> GetPlatformMetadataReferences()
        {
            throw new NotSupportedException($"{base.GetType().Name} does not support platform metadata");
        }

        public virtual NPath GetSDKToolPath(string toolName)
        {
            string[] append = new string[] { "bin" };
            NPath path = this.SDKDirectory.Combine(append);
            Unity.IL2CPP.Building.Architecture bestThisMachineCanRun = Unity.IL2CPP.Building.Architecture.BestThisMachineCanRun;
            if (bestThisMachineCanRun is x86Architecture)
            {
                string[] textArray2 = new string[] { "x86", toolName };
                return path.Combine(textArray2);
            }
            if (!(bestThisMachineCanRun is x64Architecture))
            {
                throw new NotSupportedException("Can't find MSVC tool for " + bestThisMachineCanRun);
            }
            string[] textArray3 = new string[] { "x64", toolName };
            return path.Combine(textArray3);
        }

        public virtual NPath GetUnionMetadataDirectory()
        {
            throw new NotSupportedException($"{base.GetType().Name} does not support union metadata");
        }

        protected static NPath GetVisualStudioInstallationFolder(System.Version version)
        {
            if (PlatformUtils.IsWindows())
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\Microsoft\VisualStudio\{version.Major}.{version.Minor}_Config");
                if (key != null)
                {
                    string str = (string) key.GetValue("InstallDir");
                    if (!string.IsNullOrEmpty(str))
                    {
                        return new NPath(str).Parent.Parent;
                    }
                }
            }
            return null;
        }

        public virtual NPath GetVSToolPath(Unity.IL2CPP.Building.Architecture architecture, string toolName)
        {
            string[] append = new string[] { "VC", "bin" };
            NPath path = this.VisualStudioDirectory.Combine(append);
            if (architecture is x86Architecture)
            {
                return (!this._use64BitTools ? path.Combine(new string[] { toolName }) : path.Combine(new string[] { "amd64_x86", toolName }));
            }
            if (architecture is x64Architecture)
            {
                string[] textArray4 = new string[] { "amd64", toolName };
                return path.Combine(textArray4);
            }
            if (!(architecture is ARMv7Architecture))
            {
                throw new NotSupportedException("Can't find MSVC tool for " + architecture);
            }
            return (!this._use64BitTools ? path.Combine(new string[] { "x86_arm", toolName }) : path.Combine(new string[] { "amd64_arm", toolName }));
        }

        public virtual IEnumerable<NPath> GetWindowsMetadataReferences()
        {
            throw new NotSupportedException($"{base.GetType().Name} does not support windows metadata");
        }

        public virtual bool HasMetadataDirectories() => 
            false;

        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

        protected bool HasCppSDK =>
            ((this.SDKDirectory != null) && this.SDKDirectory.Exists(""));

        protected NPath SDKDirectory { get; set; }

        public abstract IEnumerable<Type> SupportedArchitectures { get; }

        public System.Version Version { get; set; }

        protected virtual NPath VisualStudioDirectory { get; set; }

        [CompilerGenerated]
        private sealed class <GetLatestInstallationAtLeast>c__AnonStorey0
        {
            internal Version version;

            internal bool <>m__0(Version v) => 
                (v >= this.version);
        }
    }
}
