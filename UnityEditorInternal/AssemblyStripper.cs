﻿namespace UnityEditorInternal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEditor;
    using UnityEditor.Utils;

    internal class AssemblyStripper
    {
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<string, string, string> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<RuntimeClassRegistry.MethodDescription, string> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<RuntimeClassRegistry.MethodDescription, string> <>f__am$cache6;
        [CompilerGenerated]
        private static Func<string, bool> <>f__mg$cache0;

        private static bool AddWhiteListsForModules(IEnumerable<string> nativeModules, ref IEnumerable<string> blacklists, string moduleStrippingInformationFolder)
        {
            bool flag = false;
            foreach (string str in nativeModules)
            {
                string moduleWhitelist = GetModuleWhitelist(str, moduleStrippingInformationFolder);
                if (File.Exists(moduleWhitelist) && !Enumerable.Contains<string>(blacklists, moduleWhitelist))
                {
                    string[] second = new string[] { moduleWhitelist };
                    blacklists = Enumerable.Concat<string>(blacklists, second);
                    flag = true;
                }
            }
            return flag;
        }

        internal static void GenerateInternalCallSummaryFile(string icallSummaryPath, string managedAssemblyFolderPath, string strippedDLLPath)
        {
            string exe = Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/InternalCallRegistrationWriter/InternalCallRegistrationWriter.exe");
            string args = string.Format("-assembly=\"{0}\" -output=\"{1}\" -summary=\"{2}\"", Path.Combine(strippedDLLPath, "UnityEngine.dll"), Path.Combine(managedAssemblyFolderPath, "UnityICallRegistration.cpp"), icallSummaryPath);
            Runner.RunManagedProgram(exe, args);
        }

        private static string GetMethodPreserveBlacklistContents(RuntimeClassRegistry rcr)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<linker>");
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = new Func<RuntimeClassRegistry.MethodDescription, string>(null, (IntPtr) <GetMethodPreserveBlacklistContents>m__5);
            }
            IEnumerable<IGrouping<string, RuntimeClassRegistry.MethodDescription>> enumerable = Enumerable.GroupBy<RuntimeClassRegistry.MethodDescription, string>(rcr.GetMethodsToPreserve(), <>f__am$cache5);
            foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> grouping in enumerable)
            {
                builder.AppendLine(string.Format("\t<assembly fullname=\"{0}\">", grouping.Key));
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = new Func<RuntimeClassRegistry.MethodDescription, string>(null, (IntPtr) <GetMethodPreserveBlacklistContents>m__6);
                }
                IEnumerable<IGrouping<string, RuntimeClassRegistry.MethodDescription>> enumerable2 = Enumerable.GroupBy<RuntimeClassRegistry.MethodDescription, string>(grouping, <>f__am$cache6);
                foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> grouping2 in enumerable2)
                {
                    builder.AppendLine(string.Format("\t\t<type fullname=\"{0}\">", grouping2.Key));
                    foreach (RuntimeClassRegistry.MethodDescription description in grouping2)
                    {
                        builder.AppendLine(string.Format("\t\t\t<method name=\"{0}\"/>", description.methodName));
                    }
                    builder.AppendLine("\t\t</type>");
                }
                builder.AppendLine("\t</assembly>");
            }
            builder.AppendLine("</linker>");
            return builder.ToString();
        }

        private static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
        {
            string[] components = new string[] { moduleStrippingInformationFolder, module + ".xml" };
            return Paths.Combine(components);
        }

        private static List<string> GetUserAssemblies(RuntimeClassRegistry rcr, string managedDir)
        {
            <GetUserAssemblies>c__AnonStorey1 storey = new <GetUserAssemblies>c__AnonStorey1 {
                rcr = rcr,
                managedDir = managedDir
            };
            return Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(storey.rcr.GetUserAssemblies(), new Func<string, bool>(storey, (IntPtr) this.<>m__0)), new Func<string, string>(storey, (IntPtr) this.<>m__1)));
        }

        internal static IEnumerable<string> GetUserBlacklistFiles()
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = new Func<string, string>(null, (IntPtr) <GetUserBlacklistFiles>m__4);
            }
            return Enumerable.Select<string, string>(Directory.GetFiles("Assets", "link.xml", SearchOption.AllDirectories), <>f__am$cache4);
        }

        private static bool RunAssemblyLinker(IEnumerable<string> args, out string @out, out string err, string linkerPath, string workingDirectory)
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = new Func<string, string, string>(null, (IntPtr) <RunAssemblyLinker>m__3);
            }
            string str = Enumerable.Aggregate<string>(args, <>f__am$cache3);
            Console.WriteLine("Invoking UnusedByteCodeStripper2 with arguments: " + str);
            Runner.RunManagedProgram(linkerPath, str, workingDirectory, null, null);
            @out = "";
            err = "";
            return true;
        }

        private static void RunAssemblyStripper(string stagingAreaData, IEnumerable assemblies, string managedAssemblyFolderPath, string[] assembliesToStrip, string[] searchDirs, string monoLinkerPath, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr, bool developmentBuild)
        {
            bool flag2;
            bool doStripping = ((rcr != null) && PlayerSettings.stripEngineCode) && platformProvider.supportsEngineStripping;
            IEnumerable<string> first = Il2CppBlacklistPaths;
            if (rcr != null)
            {
                string[] second = new string[] { WriteMethodsToPreserveBlackList(stagingAreaData, rcr), MonoAssemblyStripping.GenerateLinkXmlToPreserveDerivedTypes(stagingAreaData, managedAssemblyFolderPath, rcr) };
                first = Enumerable.Concat<string>(first, second);
            }
            if (!doStripping)
            {
                foreach (string str3 in Directory.GetFiles(platformProvider.moduleStrippingInformationFolder, "*.xml"))
                {
                    string[] textArray2 = new string[] { str3 };
                    first = Enumerable.Concat<string>(first, textArray2);
                }
            }
            string fullPath = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempStrip"));
            do
            {
                string str;
                string str2;
                flag2 = false;
                if (EditorUtility.DisplayCancelableProgressBar("Building Player", "Stripping assemblies", 0f))
                {
                    throw new OperationCanceledException();
                }
                if (!StripAssembliesTo(assembliesToStrip, searchDirs, fullPath, managedAssemblyFolderPath, out str, out str2, monoLinkerPath, platformProvider, first, developmentBuild))
                {
                    object[] objArray1 = new object[] { "Error in stripping assemblies: ", assemblies, ", ", str2 };
                    throw new Exception(string.Concat(objArray1));
                }
                string icallSummaryPath = Path.Combine(managedAssemblyFolderPath, "ICallSummary.txt");
                GenerateInternalCallSummaryFile(icallSummaryPath, managedAssemblyFolderPath, fullPath);
                if (doStripping)
                {
                    HashSet<UnityType> set;
                    HashSet<string> set2;
                    CodeStrippingUtils.GenerateDependencies(fullPath, icallSummaryPath, rcr, doStripping, out set, out set2, platformProvider);
                    flag2 = AddWhiteListsForModules(set2, ref first, platformProvider.moduleStrippingInformationFolder);
                }
            }
            while (flag2);
            string path = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempUnstripped"));
            Directory.CreateDirectory(path);
            foreach (string str7 in Directory.GetFiles(managedAssemblyFolderPath))
            {
                string extension = Path.GetExtension(str7);
                if (string.Equals(extension, ".dll", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".mdb", StringComparison.InvariantCultureIgnoreCase))
                {
                    File.Move(str7, Path.Combine(path, Path.GetFileName(str7)));
                }
            }
            foreach (string str9 in Directory.GetFiles(fullPath))
            {
                File.Move(str9, Path.Combine(managedAssemblyFolderPath, Path.GetFileName(str9)));
            }
            Directory.Delete(fullPath);
        }

        internal static void StripAssemblies(string stagingAreaData, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr, bool developmentBuild)
        {
            string fullPath = Path.GetFullPath(Path.Combine(stagingAreaData, "Managed"));
            List<string> userAssemblies = GetUserAssemblies(rcr, fullPath);
            string[] assembliesToStrip = userAssemblies.ToArray();
            string[] searchDirs = new string[] { fullPath };
            RunAssemblyStripper(stagingAreaData, userAssemblies, fullPath, assembliesToStrip, searchDirs, MonoLinker2Path, platformProvider, rcr, developmentBuild);
        }

        private static bool StripAssembliesTo(string[] assemblies, string[] searchDirs, string outputFolder, string workingDirectory, out string output, out string error, string linkerPath, IIl2CppPlatformProvider platformProvider, IEnumerable<string> additionalBlacklist, bool developmentBuild)
        {
            <StripAssembliesTo>c__AnonStorey0 storey = new <StripAssembliesTo>c__AnonStorey0 {
                workingDirectory = workingDirectory
            };
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<string, bool>(null, (IntPtr) File.Exists);
            }
            additionalBlacklist = Enumerable.Where<string>(Enumerable.Select<string, string>(additionalBlacklist, new Func<string, string>(storey, (IntPtr) this.<>m__0)), <>f__mg$cache0);
            IEnumerable<string> userBlacklistFiles = GetUserBlacklistFiles();
            foreach (string str in userBlacklistFiles)
            {
                Console.WriteLine("UserBlackList: " + str);
            }
            additionalBlacklist = Enumerable.Concat<string>(additionalBlacklist, userBlacklistFiles);
            List<string> args = new List<string> {
                "--api " + PlayerSettings.apiCompatibilityLevel.ToString(),
                "-out \"" + outputFolder + "\"",
                "-l none",
                "-c link",
                "-b " + developmentBuild,
                "-x \"" + GetModuleWhitelist("Core", platformProvider.moduleStrippingInformationFolder) + "\"",
                "-f \"" + Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors") + "\""
            };
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Func<string, string>(null, (IntPtr) <StripAssembliesTo>m__0);
            }
            args.AddRange(Enumerable.Select<string, string>(additionalBlacklist, <>f__am$cache0));
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = new Func<string, string>(null, (IntPtr) <StripAssembliesTo>m__1);
            }
            args.AddRange(Enumerable.Select<string, string>(searchDirs, <>f__am$cache1));
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = new Func<string, string>(null, (IntPtr) <StripAssembliesTo>m__2);
            }
            args.AddRange(Enumerable.Select<string, string>(assemblies, <>f__am$cache2));
            return RunAssemblyLinker(args, out output, out error, linkerPath, storey.workingDirectory);
        }

        private static string WriteMethodsToPreserveBlackList(string stagingAreaData, RuntimeClassRegistry rcr)
        {
            string path = !Path.IsPathRooted(stagingAreaData) ? (Directory.GetCurrentDirectory() + "/") : "";
            path = path + stagingAreaData + "/methods_pointedto_by_uievents.xml";
            File.WriteAllText(path, GetMethodPreserveBlacklistContents(rcr));
            return path;
        }

        private static string BlacklistPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(MonoLinkerPath), "Core.xml");
            }
        }

        private static string[] Il2CppBlacklistPaths
        {
            get
            {
                return new string[] { Path.Combine("..", "platform_native_link.xml") };
            }
        }

        private static string ModulesWhitelistPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(MonoLinkerPath), "ModuleStrippingInformation");
            }
        }

        private static string MonoLinker2Path
        {
            get
            {
                return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/UnusedByteCodeStripper2/UnusedBytecodeStripper2.exe");
            }
        }

        private static string MonoLinkerPath
        {
            get
            {
                return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/UnusedBytecodeStripper.exe");
            }
        }

        [CompilerGenerated]
        private sealed class <GetUserAssemblies>c__AnonStorey1
        {
            internal string managedDir;
            internal RuntimeClassRegistry rcr;

            internal bool <>m__0(string s)
            {
                return this.rcr.IsDLLUsed(s);
            }

            internal string <>m__1(string s)
            {
                return Path.Combine(this.managedDir, s);
            }
        }

        [CompilerGenerated]
        private sealed class <StripAssembliesTo>c__AnonStorey0
        {
            internal string workingDirectory;

            internal string <>m__0(string s)
            {
                return (!Path.IsPathRooted(s) ? Path.Combine(this.workingDirectory, s) : s);
            }
        }
    }
}

